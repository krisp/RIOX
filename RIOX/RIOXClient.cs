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
        // Event is fired every time an object is received
        public event ObjectReceivedEventHandler ObjectReceivedEvent;

        // Expose TCPClient.Connected
        public bool IsConnected { 
            get 
            { 
                if(_client != null) 
                    return _client.Connected;
                return false; 
            } 
        }

        // typeof(DataObject)
        public Type DataType { get; set; }

        private TcpClient _client;
        private Thread _clientThread;
        private bool _stopThreads;

        // Constructor requires the dataType, hostname, and port of the server
        public RIOXClient(Type dataType, String hostname, int port)
        {
            try
            {
                // create the client
                _client = new TcpClient(hostname, port);
                // create the client thread
                _clientThread = new Thread(ClientThread);
                // start the client thread
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
            // Get the client's network stream and send
            // the RIOXCommand
            NetworkStream ns = _client.GetStream();
            SoapFormatter sf = new SoapFormatter();
            sf.Serialize(ns, command);
            ns.Flush();
        }
                
        private void ClientThread(object tcpClient)
        {
            // TcpClient is passed to the thread on start
            TcpClient client = (TcpClient)tcpClient;
            // Get the network stream from the client
            NetworkStream ns = client.GetStream();
            // Create the SoapFormatter for the object
            SoapFormatter sf = new SoapFormatter();
            
            // Loop until told to stop
            while (!_stopThreads)
            {
                try
                {
                    // Check if there is any available data on the network stream
                    if (ns.DataAvailable)
                    {
                        // Receive the data and deserialize it into an object
                        Object o = sf.Deserialize(ns);
                        // Check if the object type is the correct type
                        if(o.GetType() != DataType)
                        {
                            throw new Exception("Object received does not match DataType");
                        }
                        ns.Flush();
                        // Fire ObjectReceivedEvent, which should be captured in the client app
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
