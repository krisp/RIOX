using System;
using System.Windows.Forms;
using RIXox;

namespace RIXoxTestClient
{
    public partial class ClientForm : Form
    {
        private RadioData _radio;
        private RIXClient _client;

        public ClientForm()
        {
            InitializeComponent();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _client = new RIXClient(typeof(RadioData),"localhost", 1234);
            _client.ObjectReceivedEvent += ClientObjectReceivedEvent;
        }

        void ClientObjectReceivedEvent(object o, RIXClient.ObjectReceivedEventArgs e)
        {
            _radio = (RadioData) e.DataObject;
            Console.WriteLine("Object received: " + _radio);
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
            _client.SendCommand(new RIXCommand("VFOA", txtVFOA.Text));
        }
    }
}
