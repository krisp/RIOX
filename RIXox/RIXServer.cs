using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Soap;

namespace RIXox
{
    public class RIXServer
    {
        public event CommandEventHandler CommandEvent;
       
        public int ClientCount { get { return _clients.Count; } }
        public bool IsStarted { get; private set; }
        public Object DataObject { get; set; }
        public int ObjectUpdateInterval { get; set; }
        public bool SendUpdatesAtInterval { get; set; }

        private Thread _listenThread;
        private TcpListener _listener;
        private ArrayList _clients;
        private bool _stopThreads;
        private int _clientIdNext;
        private Timer _objectUpdateTimer;

        public RIXServer(Object dataObject, IPAddress bindAddress, int port)
        {
            _clients = new ArrayList();
            _clientIdNext = 0;
            DataObject = dataObject;
            ObjectUpdateInterval = 500; // initially 500ms
            SendUpdatesAtInterval = true;

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
                _listenThread = new Thread(ListenThread);
                _listenThread.Start();
                if(SendUpdatesAtInterval)
                    _objectUpdateTimer = new Timer(TTick, null, 0, ObjectUpdateInterval);
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
            _listener.Stop();
            foreach(ClientHandle ch in _clients)
            {
                try
                {
                    ch.TcpClient.Close();
                }
                catch(Exception e)
                {
                    throw new Exception(e.Message, e);
                }
            }
        }
        #endregion

        #region Control Functions
        public void SendObjectUpdate()
        {
             if(_clients.Count == 0)
                 return;

             foreach (ClientHandle ch in _clients)
             {
                 if (ch.IsDead)
                     continue;
                 SendObjectToClientHandle(ch);
             }
        }
        #endregion

        #region Threads
        private void ListenThread()
        {
            while(!_stopThreads)
            {
                try
                {
                    TcpClient c = _listener.AcceptTcpClient();
                    Console.WriteLine("Client connected: " + c.Client.RemoteEndPoint); 
                    Thread t = new Thread(ClientThread);
                    ClientHandle ch = new ClientHandle(t, c, _clientIdNext++);
                    t.Start(ch);
                    SendObjectToClientHandle(ch);
                    _clients.Add(ch);                    
                }
                catch (SocketException se)
                {
                    if(se.ErrorCode == 10004)
                        return;                    
                    throw new Exception(se.Message, se);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message, e);
                }
            }
        }

        private void SendObjectToClientHandle(ClientHandle ch)
        {
            if(DataObject == null)
            {
                throw new Exception("Attempted SendObjectToClientHandle with no DataObject");
            }

            if(ch.TcpClient.Connected == false)
            {
                // client is disconnected, remove it from the list
                Console.WriteLine("Client is disconnected, marking for removal.");
                ch.IsDead = true;
                return;
            }

            Console.WriteLine("SendObjectToClientHandle: " + ch.TcpClient.Client.RemoteEndPoint + " (" + ch.ClientId + ")");
            Console.WriteLine(DataObject);
            SoapFormatter sf = new SoapFormatter();
            try
            {
                sf.Serialize(ch.TcpClient.GetStream(), DataObject);
                ch.TcpClient.GetStream().Flush();                                   
            }
            catch (System.IO.IOException)
            {
                // IO/socket exceptions mean that the intended client is probably dead, so lets remove
                // the client from our list.
                ch.IsDead = true;
            }
            catch (SocketException)
            {
                ch.IsDead = true;
            }
            catch(Exception e)
            {
                throw new Exception("Error sending object to client: "+ e.Message, e);   
            }
        }

        private void TTick(object o)
        {
            foreach(ClientHandle ch in _clients)
            {
                SendObjectToClientHandle(ch);
            }
        }

        private void ClientThread(object o)
        {
            ClientHandle ch = (ClientHandle) o;
            SoapFormatter sf = new SoapFormatter();
            NetworkStream ns = ch.TcpClient.GetStream();
            
            while(!_stopThreads)
            {
                try
                {
                    if (ns.DataAvailable)
                    {
                        RIXCommand r = (RIXCommand)sf.Deserialize(ns);
                        ns.Flush();
                        CommandEventArgs cea = new CommandEventArgs(r.Command, r.Data);
                        CommandEvent(this, cea);
                    }
                }
                catch (ObjectDisposedException)
                {
                    // do nothing
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message, e);
                }
            }

            ch.TcpClient.Close();
            ch.Thread.Abort();
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
            public int ClientId { get; private set; }
            public bool IsDead { get; set; }
            public ClientHandle(Thread t, TcpClient c, int id)
            {
                Thread = t;
                TcpClient = c;
                ClientId = id;
                IsDead = false;
            }
            public ClientHandle() { IsDead = false; }
        }        
#endregion
    }
}
