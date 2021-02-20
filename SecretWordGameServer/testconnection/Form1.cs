using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testconnection
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream nStream;
        BinaryWriter sw;
        BinaryReader sr;
        IPAddress local_address;
        Task startRecieveTask;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client = new TcpClient();
            byte[] ip_byte = { 127, 0, 0, 1 };
            local_address = new IPAddress(ip_byte);
            client.Connect(local_address, 2000);
            nStream = client.GetStream();
            sr = new BinaryReader(nStream);
            sw = new BinaryWriter(nStream);
            startRecieveTask = new Task(RecieveMessage);
            startRecieveTask.Start();

        }

        private void RecieveMessage()
        {
            string msg;
            while (true)
            {

                if ((msg = sr.ReadString()) != null)
                {

                    if (msg == "no")
                    {
                        MessageBox.Show("refused");
                        sr.Close();
                        sw.Close();
                        nStream.Close();
                        break;
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show(msg, "info", MessageBoxButtons.OKCancel);
                        if (result == DialogResult.OK) sw.Write("yes");
                        else
                        {
                            sw.Write("no");
                            sr.Close();
                            sw.Close();
                            nStream.Close();
                            break;
                        }

                    }
                }
                   
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
