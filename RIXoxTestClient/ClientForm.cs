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

namespace RIXoxTestClient
{
    public partial class ClientForm : Form
    {
        private RadioData _radio;
        private RIOXClient _client;

        public ClientForm()
        {
            InitializeComponent();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _client = new RIOXClient(typeof(RadioData),"localhost", 1234);
            _client.ObjectReceivedEvent += ClientObjectReceivedEvent;
        }

        void ClientObjectReceivedEvent(object o, RIOXClient.ObjectReceivedEventArgs e)
        {
            _radio = (RadioData) e.DataObject;
            Console.WriteLine("Object received: " + _radio);
            Console.WriteLine("Preamp: " + _radio.Custom["preamp"]);
            UpdateForm();
        }

        void UpdateForm()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) delegate
                                           {
                                               txtMode.Text = _radio.Mode;
                                               txtVFOA.Text = _radio.vfoa;
                                               txtVFOB.Text = _radio.vfob;
                                               cbMox.Checked = _radio.mox;
                                           });
            }
            else
            {
                txtMode.Text = _radio.Mode;
                txtVFOA.Text = _radio.vfoa;
                txtVFOB.Text = _radio.vfob;
                cbMox.Checked = _radio.mox;                
            }
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(_client != null)
                _client.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _client.SendCommand(new RIOXCommand("VFOA", txtVFOA.Text));
        }
    }
}
