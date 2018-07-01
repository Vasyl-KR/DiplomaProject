using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace IRB_Coder
{
    public partial class Form1 : Form
    {
        static string path = @"D:\polytehnica\Diploma\IRB_Coder\Output.txt";
        static string Text_to_code = "";
        static int[] Selected = new int[] { };
        static List<int[]> IRB = new List<int[]>();
        public Form1()
        {
            InitializeComponent();
            
            IRB.Add(new int[] { 1, 2, 9, 8, 14, 4, 43, 7, 6, 10, 5, 24 });
            IRB.Add(new int[] { 1, 2, 12, 31, 25, 4, 9, 10, 7, 11, 16, 5 });
            IRB.Add(new int[] { 1, 2, 14, 4, 37, 7, 8, 27, 5, 6, 13, 9 });
            IRB.Add(new int[] { 1, 2, 14, 12, 32, 19, 6, 5, 4, 18, 13, 7 });
            IRB.Add(new int[] { 1, 38, 9, 5, 19, 23, 16, 13, 2, 28, 6 });
            IRB.Add(new int[] { 1, 3, 12, 34, 21, 2, 8, 9, 5, 6, 7, 25 });
            IRB.Add(new int[] { 1, 3, 23, 24, 6, 22, 10, 11, 18, 2, 5, 8 });
            IRB.Add(new int[] { 1, 4, 7, 3, 16, 2, 6, 17, 20, 9, 13, 35 });
            IRB.Add(new int[] { 1, 4, 16, 3, 15, 10, 12, 14, 17, 33, 2, 6 });
            IRB.Add(new int[] { 1, 4, 19, 20, 27, 3, 6, 25, 7, 8, 2, 11 });
            IRB.Add(new int[] { 1, 4, 20, 3, 40, 10, 9, 2, 15, 16, 6, 7 });
            IRB.Add(new int[] { 1, 5, 12, 21, 29, 11, 3, 16, 4, 22, 2, 7 });
            IRB.Add(new int[] { 1, 7, 13, 12, 3, 11, 5, 18, 4, 2, 48, 9 });
            IRB.Add(new int[] { 1, 8, 10, 5, 7, 21, 4, 2, 11, 3, 26, 35 });
            IRB.Add(new int[] { 1, 14, 3, 2, 4, 7, 21, 8, 25, 10, 12, 26 });
            IRB.Add(new int[] { 1, 14, 10, 20, 7, 6, 3, 2, 17, 4, 8, 41 });
            IRB.Add(new int[] { 1, 15, 5, 3, 25, 2, 7, 4, 6, 12, 14, 39 });
            IRB.Add(new int[] { 1, 22, 14, 20, 5, 13, 8, 3, 4, 2, 10, 31 });

            foreach (int[] item in IRB)
            {
                string s = "";
                foreach (int i in item)
                {
                    s = s + i + ",";
                }
                comboBox1.Items.Add(s);
            }
            comboBox1.SelectedIndex = 12;
            //Monolite_code(61, IRB[12]);
            //Selected = IRB[12];
            //textBox1.Text = Decoding("001111111100100000000011");

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Selected = IRB[comboBox1.SelectedIndex];
            richTextBox1.Clear();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Text_to_code = textBox1.Text;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Regex pattern = new Regex(@"[\x00-\x7F]");
            if (!pattern.IsMatch(textBox1.Text))
            {
                MessageBox.Show("Допустимі лише символи ASCII");
                button1.Enabled = false;
            }
            else
            {
                richTextBox1.Clear();
                richTextBox1.Text = Coding(textBox1.Text);
                for (int i = 0; i < richTextBox1.TextLength; i = i + Selected.Length)
                {
                    richTextBox1.SelectionStart = i;
                    richTextBox1.SelectionLength = 1;
                    richTextBox1.SelectionColor = Color.Red;
                    File.WriteAllText(path, richTextBox1.Text); 
                }
                
            }
            
        }

        public string Coding(string s)
        {
            string codes = "";
            foreach (char ch in s)
            {
                int Number = ch;
                codes = codes + Monolite_code(Number, Selected);
            }

            return codes;
        }
        public string Monolite_code(int Number, int[] IRB)
        {
            char[] chs = new char[IRB.Length];
            for (int i = 0; i < IRB.Length; i++)
            {
                chs[i] = '0';
            }
   
            int j = 0;
            int temp = 0;
            int count = 0;

            while (temp != Number)
            {
                if (j == IRB.Length)
                {
                    j = 0;
                }
                temp += IRB[j];
                chs[j] = '1';
                j++;
                count++;
                if (temp > Number)
                {
                    for (int i = 0; i < IRB.Length; i++)
                    {
                        chs[i] = '0';
                    }
                    j = j - count +1;
                    if (j < 0)
                        j += 12;
                    count = 0;
                    temp = 0;
                }

            }

            string code = new string(chs);
            return code;
 
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Regex pattern = new Regex(@"[^\x00-\x7F]");
            if (!pattern.IsMatch(textBox1.Text))
            {
                button1.Enabled = true;
            }
            else
            {

                MessageBox.Show("Допустимі лише символи ASCII");
                button1.Enabled = false;
            }
        }
        public string Decoding(string s)
        {
            string decodes = "";
       
            for (int i = 0; i < s.Length; i += Selected.Length)
            {
                string word = s.Substring(i, Selected.Length);
                decodes = decodes + Monolite_decode(word, Selected);
            }
            return decodes;
        }
        public char Monolite_decode(string s, int[] IRB)
        {
            
            int Number = 0;
            int i = 0;
            foreach(char ch in s) 
            {
                if (ch == '1')
                {
                    Number += IRB[i];
                }
                i++;
            }
            char decode = (char) Number;
            return decode;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.Text = Decoding(richTextBox1.Text);
        }
    }
}
