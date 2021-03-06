﻿/* This file is part of RIOX.
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
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Soap;

namespace RIOX
{
    public class RIOXServer
    {
        public event CommandEventHandler CommandEvent;
        public event ClientEventHandler ClientConnectedEvent;
        public event ClientEventHandler ClientDisconnectedEvent;
       
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

        // Constructor. Requires a serializable dataObject, and ip address to bind to, and a port to listen on.
        public RIOXServer(Object dataObject, IPAddress bindAddress, int port)
        {
            // init the client list
            _clients = new ArrayList();
            // set the starting client id
            _clientIdNext = 0;
            // set stuff up
            DataObject = dataObject;            
            ObjectUpdateInterval = 500; // initially 500ms
            SendUpdatesAtInterval = true;

            try
            {
                // create the listener
                _listener = new TcpListener(bindAddress, port);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to create RIOX server: " + e.Message, e);
            }            
        }

        #region Methods
        // Start the server. Start the ListenThread. Start the automatic update timer if enabled
        public void Start()
        {
            try
            {
                // make sure threads marked to run
                _stopThreads = false;
                // start the TcpListener
                _listener.Start();
                // Create the ListenThread
                _listenThread = new Thread(ListenThread);
                // Start the ListenThread
                _listenThread.Start();
                // Start the update timer if it is enabled
                if(SendUpdatesAtInterval)
                    _objectUpdateTimer = new Timer(TTick, null, 0, ObjectUpdateInterval);
                // mark our status as started
                IsStarted = true;
                // create the client garbage collection timer. it purges dead clients from the client list.
            }
            catch (Exception e)
            {                
                throw new Exception("Error starting server: " + e.Message, e);
            }
        }
        
        // Stop everything.
        public void Close()
        {   
            // tell the threads to stop. all the while loops loop around this bool.
            _stopThreads = true;            
            // set our status
            IsStarted = false;
            // stop the TcpListener
            _listener.Stop();

            // Stop each client
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
        
        // Send out an object to all of the clients
        public void SendObjectUpdate()
        {
            // dont do anything if we have no clients
            if(_clients.Count == 0)
                return;
            
            // iterate the list of clients
            foreach (ClientHandle ch in _clients)
            {
                // if it is marked dead, skip it.
                if (ch.IsDead)
                    continue;
                // send the object to the client handle
                SendObjectToClientHandle(ch);
            }
            ClientGC();
        }
        #endregion

        #region Threads
        
        // This is the "Server." ListenThread waits for connections and initial client handling.
        // It spins off ClientThreads for every connection.
        private void ListenThread()
        {
            while(!_stopThreads)
            {
                try
                {
                    // This is a blocking operation. When a client connects, it is passed to c.
                    TcpClient c = _listener.AcceptTcpClient();
                    Console.WriteLine("Client connected: " + c.Client.RemoteEndPoint); 
                    // Create a new client thread for this new client
                    Thread t = new Thread(ClientThread);
                    // Create a client handle to store the thread and the client, and give it the next id.
                    ClientHandle ch = new ClientHandle(t, c, _clientIdNext++);
                    if(ClientConnectedEvent !=  null)
                        ClientConnectedEvent(this, new ClientEventArgs(c, ch.ClientId));
                    // start the client thread, and pass the client handle to the new thread.
                    t.Start(ch);
                    // send the latest object to the client
                    SendObjectToClientHandle(ch);
                    // add the client to the list of clients
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
                Thread.Sleep(1);
            }
        }

        // send an object to the client handle
        private void SendObjectToClientHandle(ClientHandle ch)
        {
            // Panic if the DataObject is null
            if(DataObject == null)
            {
                throw new Exception("Attempted SendObjectToClientHandle with no DataObject");
            }

            // mark the client as dead if it disconnected.
            if(ch.TcpClient.Connected == false)
            {
                // client is disconnected, mark for removal
                Console.WriteLine("Client is disconnected, marking for removal.");
                // TODO: Fire an event for the disconnected client
                if(ClientDisconnectedEvent != null)                
                    ClientDisconnectedEvent(this, new ClientEventArgs(ch.TcpClient, ch.ClientId));                
                ch.IsDead = true;
                return;
            }

            Console.WriteLine("SendObjectToClientHandle: " + ch.TcpClient.Client.RemoteEndPoint + " (" + ch.ClientId + ")");
            Console.WriteLine(DataObject);

            // Create the formatter
            SoapFormatter sf = new SoapFormatter();
            try
            {
                // Serialize the object into the TcpClient stream. This sends the object to the client.
                sf.Serialize(ch.TcpClient.GetStream(), DataObject);
                // Flush the stream
                ch.TcpClient.GetStream().Flush();
            }
            catch (System.IO.IOException)
            {
                // IO/socket exceptions mean that the intended client is probably dead, so lets remove
                // the client from our list.
                if (ClientDisconnectedEvent != null)
                    ClientDisconnectedEvent(this, new ClientEventArgs(ch.TcpClient, ch.ClientId));
                ch.IsDead = true;
            }
            catch (SocketException)
            {
                if (ClientDisconnectedEvent != null)
                    ClientDisconnectedEvent(this, new ClientEventArgs(ch.TcpClient, ch.ClientId));
                ch.IsDead = true;
            }
            catch (System.Xml.XmlException xmle)
            {
                Console.WriteLine("SendObjectToClientHandle: EXCEPTION: " + xmle.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Error sending object to client: " + e.Message, e);
            }
        }

        // Automatic update ticker
        private void TTick(object o)
        {
            foreach(ClientHandle ch in _clients)
            {
                SendObjectToClientHandle(ch);
            }
            ClientGC();
        }

        private void ClientGC()
        {
            for(int i = _clients.Count - 1; i >= 0; i--)
            {
                ClientHandle o = (ClientHandle)_clients[i];
                if(o.IsDead)
                    _clients.Remove(o);                
            }
        }

        // ClientThread is the thread that listens for traffic sent by the client to the server
        private void ClientThread(object o)
        {
            // here is the client handle, which contains everything we need to know about.
            ClientHandle ch = (ClientHandle) o;
            SoapFormatter sf = new SoapFormatter();
            // This is the network stream for the client
            NetworkStream ns = ch.TcpClient.GetStream();
            
            // run until told to stop
            while(!_stopThreads)
            {
                try
                {
                    // check if there is data 
                    if (ns.DataAvailable)
                    {
                        // Data from the client must be a RIOXCommand.
                        Object d = sf.Deserialize(ns);
                        if(d.GetType() != typeof(RIOXCommand))
                        {
                            ch.TcpClient.Close();
                            ch.Thread.Abort();
                            throw new Exception("Received non-command data from client, connection closed.");                            
                        }

                        RIOXCommand r = (RIOXCommand) d;
                        ns.Flush();
                        // check if the command is an internal ping and discard
                        // this is used by the client to test if it is connected
                        if (r.Command.Equals("__PING"))
                            continue;
                        // fire the new object event
                        CommandEventArgs cea = new CommandEventArgs(r.Command, r.Data, ch.ClientId );
                        CommandEvent(this, cea);
                    }
                }
                catch (ObjectDisposedException)
                {
                    if (ClientDisconnectedEvent != null)
                        ClientDisconnectedEvent(this, new ClientEventArgs(ch.TcpClient, ch.ClientId));  
                    ch.IsDead = true;
                }
                catch (System.Xml.XmlException xmle)
                {
                    Console.WriteLine("ClientThread: EXCEPTION: " + xmle.Message);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message, e);
                }
                Thread.Sleep(1);
            }

            // clean up now that the thread is closing
            ch.TcpClient.Close();
            ch.Thread.Abort();
        }
        #endregion

        #region Event declarations

        public delegate void ClientEventHandler(object sender, ClientEventArgs e);
        public delegate void CommandEventHandler(object sender, CommandEventArgs e);

        public class ClientEventArgs : EventArgs
        {
            public TcpClient TcpClient { get; set; }            
            public int ClientId { get; set; }

            public ClientEventArgs(TcpClient client, int clientId)
            {
                TcpClient = client;                
                ClientId = clientId;
            }

            public ClientEventArgs() { }
        }

        public class CommandEventArgs : EventArgs
        {
            public String Command { get; set; }
            public String Data { get; set; }
            public int ClientId { get; set; }

            public CommandEventArgs(String command, String data, int clientId)
            {
                Command = command;
                Data = data;
                ClientId = clientId;
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