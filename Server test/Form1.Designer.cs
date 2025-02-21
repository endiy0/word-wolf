namespace Server_test
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            textBox1 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            listBox1 = new ListBox();
            textBox2 = new TextBox();
            button3 = new Button();
            label2 = new Label();
            groupBox1 = new GroupBox();
            listBox2 = new ListBox();
            button4 = new Button();
            button5 = new Button();
            listBox3 = new ListBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            button6 = new Button();
            button7 = new Button();
            listBox4 = new ListBox();
            label3 = new Label();
            label4 = new Label();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(588, 9);
            label1.Name = "label1";
            label1.Size = new Size(62, 32);
            label1.TabIndex = 0;
            label1.Text = "포트";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(588, 44);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(200, 39);
            textBox1.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(794, 40);
            button1.Name = "button1";
            button1.Size = new Size(200, 46);
            button1.TabIndex = 2;
            button1.Text = "서버 시작";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(794, 92);
            button2.Name = "button2";
            button2.Size = new Size(200, 46);
            button2.TabIndex = 3;
            button2.Text = "서버 종료";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(570, 612);
            listBox1.TabIndex = 4;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(12, 634);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(414, 39);
            textBox2.TabIndex = 5;
            textBox2.KeyDown += textBox2_KeyDown;
            // 
            // button3
            // 
            button3.Location = new Point(432, 630);
            button3.Name = "button3";
            button3.Size = new Size(150, 46);
            button3.TabIndex = 6;
            button3.Text = "전송";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(588, 99);
            label2.Name = "label2";
            label2.Size = new Size(141, 128);
            label2.TabIndex = 7;
            label2.Text = "로컬 IP주소:\r\n0.0.0.0\r\n외부 IP주소:\r\n0.0.0.0";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(listBox2);
            groupBox1.Location = new Point(594, 275);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(400, 401);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "접속자";
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.Location = new Point(6, 38);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(388, 356);
            listBox2.TabIndex = 0;
            // 
            // button4
            // 
            button4.Location = new Point(794, 144);
            button4.Name = "button4";
            button4.Size = new Size(200, 46);
            button4.TabIndex = 9;
            button4.Text = "게임 시작";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(794, 196);
            button5.Name = "button5";
            button5.Size = new Size(200, 46);
            button5.TabIndex = 10;
            button5.Text = "게임 종료";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // listBox3
            // 
            listBox3.FormattingEnabled = true;
            listBox3.Location = new Point(1000, 44);
            listBox3.Name = "listBox3";
            listBox3.Size = new Size(294, 516);
            listBox3.TabIndex = 11;
            listBox3.SelectedIndexChanged += listBox3_SelectedIndexChanged;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(1000, 570);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(144, 39);
            textBox3.TabIndex = 12;
            textBox3.KeyDown += textBox3_KeyDown;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(1150, 570);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(144, 39);
            textBox4.TabIndex = 13;
            textBox4.KeyDown += textBox4_KeyDown;
            // 
            // button6
            // 
            button6.Location = new Point(1300, 566);
            button6.Name = "button6";
            button6.Size = new Size(142, 46);
            button6.TabIndex = 14;
            button6.Text = "추가";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new Point(1448, 566);
            button7.Name = "button7";
            button7.Size = new Size(138, 46);
            button7.TabIndex = 15;
            button7.Text = "제거";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // listBox4
            // 
            listBox4.FormattingEnabled = true;
            listBox4.Location = new Point(1300, 44);
            listBox4.Name = "listBox4";
            listBox4.Size = new Size(286, 516);
            listBox4.TabIndex = 16;
            listBox4.SelectedIndexChanged += listBox4_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(1000, 9);
            label3.Name = "label3";
            label3.Size = new Size(117, 32);
            label3.TabIndex = 17;
            label3.Text = "시민 단어";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(1300, 9);
            label4.Name = "label4";
            label4.Size = new Size(117, 32);
            label4.TabIndex = 18;
            label4.Text = "늑대 단어";
            // 
            // button8
            // 
            button8.Location = new Point(1000, 618);
            button8.Name = "button8";
            button8.Size = new Size(196, 58);
            button8.TabIndex = 19;
            button8.Text = "단어 전체 제거";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Location = new Point(1202, 618);
            button9.Name = "button9";
            button9.Size = new Size(196, 58);
            button9.TabIndex = 20;
            button9.Text = "단어 불러오기";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button10
            // 
            button10.Location = new Point(1404, 618);
            button10.Name = "button10";
            button10.Size = new Size(182, 58);
            button10.TabIndex = 21;
            button10.Text = "단어 내보내기";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1598, 692);
            Controls.Add(button10);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(listBox4);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(listBox3);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(groupBox1);
            Controls.Add(label2);
            Controls.Add(button3);
            Controls.Add(textBox2);
            Controls.Add(listBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            KeyPreview = true;
            Name = "Form1";
            Text = "Word Wolf Server";
            FormClosing += Form1_FormClosing;
            KeyDown += Form1_KeyDown;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Button button1;
        private Button button2;
        private ListBox listBox1;
        private TextBox textBox2;
        private Button button3;
        private Label label2;
        private GroupBox groupBox1;
        private ListBox listBox2;
        private Button button4;
        private Button button5;
        private ListBox listBox3;
        private TextBox textBox3;
        private TextBox textBox4;
        private Button button6;
        private Button button7;
        private ListBox listBox4;
        private Label label3;
        private Label label4;
        private Button button8;
        private Button button9;
        private Button button10;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
    }
}
