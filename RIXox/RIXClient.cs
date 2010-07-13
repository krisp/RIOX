﻿using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace RIXox
{
    public class RIXClient
    {
        public event ObjectReceivedEventHandler ObjectReceivedEvent;

        private TcpClient _client;      

        public RIXClient(String hostname, int port)
        {
            try
            {
                _client = new TcpClient(hostname, port);     
                Thread t = new Thread(ClientThread);

            }
            catch (Exception e)
            {                
                throw new Exception("Could not connect to RIXServer", e);
            }            
        }

        public void SendCommand(RIXCommand command)
        {
            NetworkStream ns = _client.GetStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ns, command);
            ns.Flush();
        }

        private void ClientThread(object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;
            NetworkStream ns = client.GetStream();
            BinaryFormatter bf = new BinaryFormatter();

            if(ns.DataAvailable)
            {
                Object o = bf.Deserialize(ns);
                ns.Flush();
                ObjectReceivedEvent(this, new ObjectReceivedEventArgs(o));
            }
        }

        public delegate void ObjectReceivedEventHandler(object o, ObjectReceivedEventArgs e);
        public class ObjectReceivedEventArgs : EventArgs
        {
            public Object DataObject { get; set;}
            public ObjectReceivedEventArgs(Object dataObject)
            {
                DataObject = dataObject;
            }
        }
    }
}