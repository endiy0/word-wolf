using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Server_test
{
    public partial class Form2 : Form
    {
        private Form1 form1;
        public Form2(Form1 form)
        {
            InitializeComponent();
            form1 = form;
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox1.Text != "")
            {
                if (!textBox1.Text.Contains(','))
                {
                    listBox1.Items.Add(textBox1.Text);
                    textBox1.Text = "";
                }
                else
                {
                    MessageBox.Show("단어에는 쉼표(,)가 포함될 수 없습니다");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (!textBox1.Text.Contains(','))
                {
                    listBox1.Items.Add(textBox1.Text);
                    textBox1.Text = "";
                }
                else
                {
                    MessageBox.Show("단어에는 쉼표(,)가 포함될 수 없습니다");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var item in listBox1.Items)
            {
                foreach (var item1 in listBox1.Items)
                {
                    if (item != item1)
                    {
                        form1.DataAdd(item.ToString(), item1.ToString());
                    }
                }
            }
            listBox1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSV File(*.csv)|*.csv";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog1.FileName, false, Encoding.GetEncoding("utf-8")))
                {
                    file.WriteLine("단어");
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        file.WriteLine($"{listBox1.Items[i]}");
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CSV File(*.csv)|*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader sr = new StreamReader(openFileDialog1.FileName);
                    string line = sr.ReadLine();
                    if (line != "단어")
                        throw new Exception("오류");
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        listBox1.Items.Add(line);
                    }
                }
                catch
                {
                    MessageBox.Show("이 프로그램과 맞지 않는 형식입니다.");
                }
            }
        }
    }
}
