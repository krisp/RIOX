/* This file is part of RIOX.
   RIOX is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   MiniDeluxe is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with RIOX.  If not, see <http://www.gnu.org/licenses/>.
   
   RIOX is Copyright (C) 2010 by K1FSY
*/

using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Soap;
using System.Threading;

namespace RIOX
{
    public class RIOXClient
    {
        public event ObjectReceivedEventHandler ObjectReceivedEvent;

        public bool IsConnected { 
            get 
            { 
                if(_client != null) 
                    return _client.Connected;
                return false; 
            } 
        }

        public Type DataType { get; set; }

        private TcpClient _client;
        private Thread _clientThread;
        private bool _stopThreads;

        public RIOXClient(Type dataType, String hostname, int port)
        {
            try
            {
                _client = new TcpClient(hostname, port);     
                _clientThread = new Thread(ClientThread);
                _clientThread.Start(_client);
                DataType = dataType;
            }
            catch (Exception e)
            {                
                throw new Exception("Could not connect to RIOXServer", e);
            }            
        }

        public void SendCommand(RIOXCommand command)
        {
            NetworkStream ns = _client.GetStream();
            SoapFormatter sf = new SoapFormatter();
            sf.Serialize(ns, command);
            ns.Flush();
        }
                
        private void ClientThread(object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;
            NetworkStream ns = client.GetStream();
            SoapFormatter sf = new SoapFormatter();
            
            while (!_stopThreads)
            {
                try
                {
                    if (ns.DataAvailable)
                    {
                        Object o = sf.Deserialize(ns);
                        if(o.GetType() != DataType)
                        {
                            throw new Exception("Object received does not match DataType");
                        }
                        ns.Flush();
                        ObjectReceivedEvent(this, new ObjectReceivedEventArgs(o));
                    }                
                }
                catch (Exception e)
                {                    
                    throw new Exception("Object deserialization error",e);
                }
                Thread.Sleep(1);                
            }
        }

        public void Close()
        {
            _stopThreads = true;
            _client.Close();
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
