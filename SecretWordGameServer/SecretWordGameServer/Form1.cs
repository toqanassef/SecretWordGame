using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace SecretWordGameServer
{
    public partial class Form1 : Form
    {
        TcpListener server;
        Task listen;
        Task readMessage;
        public Socket connection;
        public NetworkStream nstream;
        public BinaryWriter writer;
        public BinaryReader reader;
        List<string> words;
        public String SelectedWord;

        public ComboBox categry_comboBox { get { return comboBox1; } }
        public Form1()
        {
            InitializeComponent();                         
            byte[] ip = new byte[] { 127, 0, 0, 1 };
            IPAddress publicAddress = new IPAddress(ip);
            server = new TcpListener(publicAddress, 2000);           
            listen = new Task(StartListen);
            words = new List<string>();
            
        }

        //[elm1, elm2, elm3, ......]  word-cat-level push --> []
        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader reader = File.OpenText(@"C:\Users\MoHaLegenD\Desktop\SecretWordGameServer\categories.txt");
            string input;
            while ((input = reader.ReadLine()) != null)
            {
                comboBox1.Items.Add(input.Split(' ')[0]);
            }
            reader.Close();
            comboBox1.SelectedIndex = 0;      
            comboBox2.SelectedItem = "1";

            listen.Start(); //start listening to client request
           
        }

        private void StartListen()
        {
            while (true)
            {
                server.Start();
                connection = server.AcceptSocket();
                nstream = new NetworkStream(connection);
                writer = new BinaryWriter(nstream);
                reader = new BinaryReader(nstream);
                DialogResult dresult = MessageBox.Show("Client want to start a game!", "Game Request", 
                                                           MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dresult == DialogResult.OK)
                {
                  
                    writer.Write("Category: " + comboBox1.SelectedItem + ", Level: " + comboBox2.SelectedItem);
                    readMessage = new Task(GetMessageFromClient);
                    readMessage.Start();

                }
                else
                {
                    writer.Write("no");
                    writer.Close();
                    reader.Close();
                    nstream.Close();
                    connection.Close();
                }
            }
        }

        private void GetMessageFromClient()
        {
            string msg;
            while (true)
            {
                if ((msg = reader.ReadString()) != null)
                {
                    if (msg == "yes")
                    {
                 
                        words.Clear();
                        StreamReader reader2 = File.OpenText(@"C:\Users\MoHaLegenD\Desktop\SecretWordGameServer\words.txt");
                        string input;
                        
                        while ((input = reader2.ReadLine()) != null)
                        {
                            if (int.Parse(input.Split('-')[1]) == comboBox1.SelectedIndex && input.Split('-')[2] == comboBox2.SelectedItem.ToString())
                            {                          
                                words.Add(input.Split('-')[0]);
                            }

                        }
                        
                        reader2.Close();
                        Random r = new Random();
                        int index = r.Next(words.Count);
                        SelectedWord = words[index];
        
                        game g = new game(this);
                        g.ShowDialog();

                    }
                    else MessageBox.Show("no");
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void soreToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
