using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecretWordGameServer
{
    public partial class game : Form
    {
        //Form1 obj;
        string word;
        string form_word;
        Socket connection;
        NetworkStream nstream;
        BinaryWriter writer;
        BinaryReader reader;
        public game(Form1 obj)
        {
            InitializeComponent();

            this.connection = obj.connection;
            this.nstream = obj.nstream;
            this.writer = obj.writer;
            this.reader = obj.reader;
            this.word = obj.SelectedWord;
            form_word = "";
            label2.Text = obj.categry_comboBox.SelectedItem.ToString();
            
        }

        private void game_Load(object sender, EventArgs e)
        {
            label1.Text = "";
            for (int i = 0; i < word.Length; i++) label1.Text += "_";
            
           
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            
            if (e.Button == MouseButtons.Left)
            {

                char letter = char.Parse(btn.Text.ToLower());
                int index = word.IndexOf(letter);

                if (index != -1)
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (word[i] == letter)
                        {
                            form_word += btn.Text;

                        }
                        else form_word += label1.Text[i];
                    }

                    label1.Text = form_word;
                    form_word = "";
                    btn.Enabled = false;
                }
                else
                {
                    panel1.Enabled = false;
                }
                
               
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            writer.Close();
            reader.Close();
            nstream.Close();
            connection.Close();
            this.Close();
        }

     
    }
}
