using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace RIXox
{
    public class RIXServer
    {
        public event CommandEventHandler CommandEvent;
        public event ObjectPollEventHandler ObjectPollEvent;
        public int ClientCount { get { return _clients.Count; } }
        public bool IsStarted { get; private set; }
        public Object DataObject { get; set; }
        public int ObjectPollInterval { get; set; }

        private Object _lastDataObject;
        private TcpListener _listener;
        private ArrayList _clients;
        private bool _stopThreads;
        private Timer _objectPollTimer;

        public RIXServer(Object DataObject, IPAddress bindAddress, int port)
        {
            _clients = new ArrayList();
            ObjectPollInterval = 500;
            _objectPollTimer = new Timer(ObjectPollTick, null, 0, ObjectPollInterval);

            try
            {
                _listener = new TcpListener(bindAddress, port);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to create RIX server: " + e.Message, e);
            }
        }

        #region Methods
        public void Start()
        {
            try
            {
                _stopThreads = false;
                _listener.Start();
                Thread t = new Thread(ListenThread);
                t.Start();
                IsStarted = true;
            }
            catch (Exception e)
            {                
                throw new Exception("Error starting server: " + e.Message, e);
            }
        }

        public void Close()
        {
            _stopThreads = true;
            IsStarted = false;
            foreach(ClientHandle ch in _clients)
            {
                try
                {
                    ch.TcpClient.Close();
                    ch.Thread.Abort();                    
                }
                catch(Exception e)
                {
                    throw new Exception(e.Message, e);
                }
            }
        }
        #endregion

        #region Control Functions
        private void SendObjectUpdate()
        {
            if (!DataObject.Equals(_lastDataObject))
            {
                foreach (ClientHandle ch in _clients)
                {
                    SendObjectToClientHandle(ch);
                }
            }
            _lastDataObject = DataObject;
        }
        #endregion

        #region Threads
        private void ListenThread()
        {
            while(!_stopThreads)
            {
                TcpClient c = _listener.AcceptTcpClient();                
                Thread t = new Thread(ClientThread);
                ClientHandle ch = new ClientHandle(t,c);
                t.Start(ch);
                SendObjectToClientHandle(ch);
                _clients.Add(ch);
            }
        }

        private void SendObjectToClientHandle(ClientHandle ch)
        {
            if(DataObject == null)
            {
                throw new Exception("Attempted SendObjectToClientHandle with no DataObject");
            }

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ch.TcpClient.GetStream(), DataObject);
            ch.TcpClient.GetStream().Flush();
        }

        private void ClientThread(object o)
        {
            ClientHandle ch = (ClientHandle) o;
            BinaryFormatter bf = new BinaryFormatter();
            NetworkStream ns = ch.TcpClient.GetStream();
            
            while(!_stopThreads)
            {
                if(ns.DataAvailable)
                {
                    RIXCommand r = (RIXCommand) bf.Deserialize(ns);
                    ns.Flush();
                    CommandEventArgs cea = new CommandEventArgs(r.Command, r.Data);
                    CommandEvent(this, cea);
                }
            }

            ch.TcpClient.Close();
            ch.Thread.Abort();
        }

        private void ObjectPollTick(object o)
        {
            // remind the program to update the object
            ObjectPollEvent(this, new EventArgs());
            // send out the updates
            SendObjectUpdate();
        }
        #endregion

        #region Event declarations
        public delegate void CommandEventHandler(object sender, CommandEventArgs e);
        public class CommandEventArgs : EventArgs
        {
            public String Command { get; set; }
            public String Data { get; set; }

            public CommandEventArgs(String Command, String Data)
            {
                this.Command = Command;
                this.Data = Data;
            }

            public CommandEventArgs() { }
        }

        private class ClientHandle
        {
            public Thread Thread { get; private set; }
            public TcpClient TcpClient { get; private set; }
            public ClientHandle(Thread t, TcpClient c)
            {
                Thread = t;
                TcpClient = c;
            }
            public ClientHandle() { }
        }

        public delegate void ObjectPollEventHandler(object sender, EventArgs e);        
#endregion
    }
}
