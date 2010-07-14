﻿using System;
using System.Windows.Forms;
using RIXox;
using RIXoxTestClient;

namespace RIXoxTestServer
{
    public partial class ServerForm : Form
    {
        private RIXServer _server;
        private RadioData _radio;
        private Timer _t;

        public ServerForm()
        {
            InitializeComponent();
            _radio = new RadioData();
            
            _t = new Timer {Interval = 250};
            _t.Tick += TTick;
            //_t.Start();            
            
        }
        
        void TTick(object sender, EventArgs e)
        {
            _radio.vfoa = txtVFOA.Text;
            _radio.vfob = txtVFOB.Text;
            _radio.Mode = txtMode.Text;
            _radio.mox = cbMox.Checked;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            _server = new RIXServer(_radio, System.Net.IPAddress.Loopback, 1234);            
            _server.CommandEvent += _server_CommandEvent;
            _radio.PropertyChanged += RadioPropertyChanged;
            _server.SendUpdatesAtInterval = false;
            _server.Start();
            TTick(null,null);
            Console.WriteLine("Server started");
        }

        void RadioPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine("RadioPropertyChanged: " + e.PropertyName);
            _server.SendObjectUpdate();
        }

        void _server_CommandEvent(object sender, RIXServer.CommandEventArgs e)
        {
            // command received
            Console.WriteLine("CommandEvent fired");
        }

        private void txtVFOA_TextChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("Text changed");
            _radio.vfoa = txtVFOA.Text;
        }

        private void txtVFOB_TextChanged(object sender, EventArgs e)
        {
            _radio.vfob = txtVFOB.Text;
        }

        private void txtMode_TextChanged(object sender, EventArgs e)
        {
            _radio.Mode = txtMode.Text;
        }

        private void cbMox_CheckedChanged(object sender, EventArgs e)
        {
            _radio.mox = cbMox.Checked;
        }

        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.WriteLine("Closed -- stopping server");
            if(_server != null)
                _server.Close();
        }
    }
}