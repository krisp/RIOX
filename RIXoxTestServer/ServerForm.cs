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
using System.Windows.Forms;
using RIOX;
using RIXoxTestClient;

namespace RIXoxTestServer
{
    public partial class ServerForm : Form
    {
        private RIOXServer _server;
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
            _radio.Custom["preamp"] = "on";
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            _server = new RIOXServer(_radio, System.Net.IPAddress.Loopback, 1234);            
            _server.CommandEvent += ServerCommandEvent;
            _radio.PropertyChanged += RadioPropertyChanged;
            _radio.Custom = new System.Collections.Hashtable();
            _radio.Custom.Add("preamp", "off");
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

        void ServerCommandEvent(object sender, RIOXServer.CommandEventArgs e)
        {
            // command received
            Console.WriteLine("CommandEvent fired");
            if (e.Command == "VFOA")
                Invoke((MethodInvoker)delegate { txtVFOA.Text = e.Data; });
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
