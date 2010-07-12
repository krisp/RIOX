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
        public int ClientCount { get { return _clients.Count; } }
        public bool IsStarted { get; private set; }

        private TcpListener _listener;
        private ArrayList _clients;
        private bool _stopThreads;
        
        public RIXServer(IPAddress bindAddress, int port)
        {
            try
            {
                _listener = new TcpListener(bindAddress, port);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to create RIX server: " + e.Message,e);
            }
        }

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

        private void ListenThread()
        {
            while(!_stopThreads)
            {
                TcpClient c = _listener.AcceptTcpClient();                
                Thread t = new Thread(new ParameterizedThreadStart(ClientThread));
                ClientHandle ch = new ClientHandle(t,c);
                t.Start(ch);
                _clients.Add(ch);
            }
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
    }
}
