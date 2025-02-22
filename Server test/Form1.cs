using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server_test
{
    public partial class Form1 : Form
    {
        static TcpListener server;
        static List<Client> clients;
        Thread T;
        List<Thread> Tt;
        static bool isServerRun;
        static bool isClosing;
        static bool isGameStarted;
        static List<Client> wolfs;
        static List<Client> innocents;
        static List<string> vote;
        public Form1()
        {
            InitializeComponent();
            clients = new List<Client>();
            isServerRun = false;
            T = new Thread(() => ServerLoop(1111));
            Tt = new List<Thread>();
            button2.Enabled = false;
            button3.Enabled = false;
            button7.Enabled = false;
            isClosing = false;
            label2.Text = "로컬 IP주소:\n" + GetLocalIPAddress() + "\n외부 IP주소:\n" + GetExternalIPAddress();
            button4.Enabled = false;
            button5.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int port) && 0 < port && port < 100000)
            {

                T = new Thread(() => ServerLoop(port));
                T.IsBackground = true;
                T.Start();
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = false;
                isGameStarted = false;
                isServerRun = true;
                listBox1.Items.Add("Server started");
            }
            else
            {
                MessageBox.Show("포트는 1에서 99999 사이의 정수를 입력해 주세요");
            }
        }
        /*
        입력 코드
        0:채팅
        1:연결종료
        2:번호 지정(서버=>클라이언트)
        3:닉네임 전송(클라이언트=>서버)
        4:접속한 클라이언트 이름
        5:접속 종료한 클라이언트 이름
        6:워드울프 단어 전송(시작 사인, 서버=>클라이언트)
        7:워드울프 투표(닉네임 전송, 7⧫현재투표자⧫이전투표자(없을 경우 ""), 클라이언트=>서버)
        8:워드울프 종료사인(서버=>클라이언트)
        9:새로운 투표(서버=>클라이언트)
         */
        //Split 문자 : ⧫
        //송신 Check 문자 : ◊

        public void Delay(int ms)
        {
            DateTime dateTimeNow = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime dateTimeAdd = dateTimeNow.Add(duration);
            while (dateTimeAdd >= dateTimeNow)
            {
                System.Windows.Forms.Application.DoEvents();
                dateTimeNow = DateTime.Now;
            }
            return;
        }

        //TODO:단어편집툴과 단어 불러오기, 단어 내보내기 만들기

        //Thread func
        void ServerLoop(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            isServerRun = true;

            int count = 0;
            byte[] buffer;

            while (true)
            {
                try
                {
                    clients.Add(new Client(server.AcceptTcpClient(), count));
                    Invoke(new Action(() => listBox2.Items.Add(clients[clients.Count - 1].nickname)));
                    count++;

                    Tt.Add(new Thread(() => ClientCheck(clients.Count - 1, count)));
                    Delay(100);
                    clients[clients.Count - 1].client.GetStream().Write(Encoding.UTF8.GetBytes($"2⧫{count}◊"));
                    Tt[Tt.Count - 1].IsBackground = true;
                    Tt[Tt.Count - 1].Start();
                }
                catch (Exception ex)
                {
                    break;
                }
            }
        }

        void ClientCheck(int clientrealnumber, int clientn)
        {
            Client client = clients[clientrealnumber];
            NetworkStream stream = clients[clientrealnumber].client.GetStream();
            byte[] buffer = new byte[102400];
            buffer[102399] = 255;
            bool error = false;
            string msg = "";
            while (isServerRun)
            {
                try
                {
                    buffer = new byte[102400];
                    if (msg != "")
                    {
                        buffer = Encoding.UTF8.GetBytes(msg);
                    }
                    while (true)
                    {
                        byte[] data = new byte[256];
                        int bytesRead = stream.Read(data, 0, data.Length);
                        if (bytesRead == 0)
                            break;
                        data = data.Where(x => x != 0).ToArray();
                        if (buffer.Length == 102400) buffer = data;
                        else buffer = buffer.Concat(data).ToArray();

                        msg = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                        if (msg.Contains('◊')) break;
                    }
                    if (Encoding.UTF8.GetString(buffer, 0, buffer.Length).Split("◊").Length == 1)
                        msg = "";
                    else msg = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Split("◊")[1];
                    string[] message = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Split("◊")[0].Split('⧫');
                    if (message[0] == "0")
                    {
                        Invoke(new Action(() => listBox1.Items.Add(message[1])));

                        foreach (var c in clients)
                        {
                            if (c != client)
                            {
                                NetworkStream cStream = c.client.GetStream();
                                byte[] responseBytes = Encoding.UTF8.GetBytes("0⧫" + message[1] + '◊');
                                cStream.Write(responseBytes, 0, responseBytes.Length);
                            }
                        }
                    }
                    else if (message[0] == "1")
                    {


                        Invoke(new Action(() => listBox1.Items.Add($"{client.nickname} disconnected...")));
                        Invoke(new Action(() => listBox2.Items.Remove(client.nickname)));
                        foreach (var c in clients)
                        {
                            NetworkStream cStream = c.client.GetStream();
                            byte[] responseBytes = buffer;
                            if (c != client)
                            {
                                cStream.Write(Encoding.UTF8.GetBytes($"0⧫{client.nickname} disconnected...◊"));
                                cStream.Flush();
                                Delay(100);
                                cStream.Write(Encoding.UTF8.GetBytes($"5⧫{client.nickname}◊"));
                                cStream.Flush();
                            }

                        }
                        break;
                    }
                    else if (message[0] == "3")
                    {
                        foreach (var c in clients)
                        {
                            if (c.nickname == message[1])
                            {
                                string nickname = "";
                                foreach (var c2 in clients)
                                {
                                    if (c2 != client) nickname += c2.nickname + ", ";
                                }
                                client.client.GetStream().Write(Encoding.UTF8.GetBytes("1⧫닉네임은 다음과 같을 수 없습니다:" + nickname + '◊'));
                                clients.Remove(client);
                                Invoke(new Action(() => listBox2.Items.Remove(client.nickname)));
                                int b = 0;
                                error = true;
                                int a = 10 / b;
                            }
                        }
                        clients.Remove(client);
                        Invoke(new Action(() => listBox2.Items.Remove(client.nickname)));
                        client.nickname = message[1];
                        foreach (var c in clients)
                        {
                            client.client.GetStream().Write(Encoding.UTF8.GetBytes("4⧫" + c.nickname + '◊'));
                            Delay(100);
                        }
                        clients.Add(client);
                        foreach (var c in clients)
                        {
                            c.client.GetStream().Write(Encoding.UTF8.GetBytes("4⧫" + client.nickname + '◊'));
                        }
                        Invoke(new Action(() => listBox2.Items.Add(client.nickname)));
                        Invoke(new Action(() => listBox1.Items.Add($"{message[1]} joined")));
                        buffer = Encoding.UTF8.GetBytes($"0⧫{client.nickname} joined◊");
                        foreach (var c in clients)
                        {
                            NetworkStream s = c.client.GetStream();
                            s.Write(buffer, 0, buffer.Length);
                        }
                    }
                    else if (message[0] == "7")
                    {
                        if (message[2] == "")
                        {
                            vote.Add(message[1]);
                            foreach (var c in clients)
                            {
                                c.client.GetStream().Write(Encoding.UTF8.GetBytes($"0⧫{clients.Count}명중 {vote.Count}명이 투표했습니다.◊"));
                            }
                        }
                        else
                        {
                            vote.Add(message[1]);
                            vote.Remove(message[2]);
                        }
                        if (vote.Count >= clients.Count)
                        {
                            foreach (var w in wolfs)
                            {
                                if (!w.client.Connected) wolfs.Remove(w);
                            }
                            foreach (var i in innocents)
                            {
                                if (!i.client.Connected) wolfs.Remove(i);
                            }
                            Dictionary<string, int> vote_counter = new Dictionary<string, int>();

                            foreach (var w in wolfs)
                            {
                                vote_counter.Add(w.nickname, 0);
                            }
                            foreach (var i in innocents)
                            {
                                vote_counter.Add(i.nickname, 0);
                            }
                            foreach (var v in vote)
                            {
                                vote_counter[v]++;
                            }
                            int n = 0;
                            List<string> s = new List<string>();
                            foreach (var v in vote_counter)
                            {
                                if (n < v.Value)
                                {
                                    s = new List<string>();
                                    s.Add(v.Key);
                                    n = v.Value;
                                }
                                else if (n == v.Value)
                                {
                                    s.Add(v.Key);
                                }

                            }
                            foreach (var c in clients)
                            {
                                c.client.GetStream().Write(Encoding.UTF8.GetBytes("9⧫◊"));
                            }
                            if (s.Count > 1)
                            {
                                foreach (var c in clients)
                                {
                                    c.client.GetStream().Write(Encoding.UTF8.GetBytes("0⧫" + string.Join(", ", s) + $"님들이 {n}표로 동표입니다.◊"));
                                }
                                Invoke(new Action(() => listBox1.Items.Add(string.Join(", ", s) + $"님들이 {n}표로 동표입니다.")));
                            }
                            else
                            {
                                bool isWolf = false;
                                foreach (var w in wolfs)
                                {
                                    if (w.nickname == s[0])
                                    {
                                        isWolf = true;
                                        wolfs.Remove(w);
                                        break;
                                    }
                                }
                                if (isWolf)
                                {
                                    foreach (var c in clients)
                                    {
                                        Delay(100);
                                        c.client.GetStream().Write(Encoding.UTF8.GetBytes($"0⧫투표로 {s[0]}님이 선정되었습니다. {s[0]}님은 늑대였습니다.◊"));
                                        Delay(100);
                                        c.client.GetStream().Write(Encoding.UTF8.GetBytes($"0⧫늑대가 {wolfs.Count}명 남았습니다◊"));
                                    }
                                    Invoke(new Action(() => listBox1.Items.Add($"투표로 {s[0]}님이 선정되었습니다. {s[0]}님은 늑대였습니다.")));
                                    Invoke(new Action(() => listBox1.Items.Add($"늑대가 {wolfs.Count}명 남았습니다")));
                                }
                                else
                                {
                                    foreach (var c in clients)
                                    {
                                        Delay(100);
                                        c.client.GetStream().Write(Encoding.UTF8.GetBytes($"0⧫투표로 {s[0]}님이 선정되었습니다. {s[0]}님은 늑대가 아닙니다.◊"));
                                    }
                                    Invoke(new Action(() => listBox1.Items.Add($"투표로 {s[0]}님이 선정되었습니다. {s[0]}님은 늑대가 아닙니다.")));
                                }
                                Delay(100);
                                if (wolfs.Count == 0)
                                {
                                    foreach (var c in clients)
                                    {
                                        c.client.GetStream().Write(Encoding.UTF8.GetBytes($"0⧫늑대가 모두 사라졌습니다. 게임을 종료합니다◊"));
                                    }
                                    Invoke(new Action(() =>
                                    {
                                        button4.Enabled = true;
                                        button5.Enabled = false;
                                        listBox1.Items.Add($"늑대가 모두 사라졌습니다. 게임을 종료합니다");
                                    }));
                                    Delay(100);
                                    foreach (var c in clients)
                                    {
                                        c.client.GetStream().Write(Encoding.UTF8.GetBytes($"8⧫◊"));
                                    }
                                    isGameStarted = false;
                                }
                            }
                            vote = new List<string>();
                        }
                    }
                    Invoke(new Action(() => listBox1.TopIndex = listBox1.Items.Count - 1));
                }
                catch (Exception e)
                {


                    break;
                }
            }
            //if (!isClosing && !error)
            //{
            //    foreach (var c in clients)
            //    {
            //        if (c != clients[clientrealnumber])
            //        {
            //            NetworkStream cStream = c.client.GetStream();
            //            byte[] responseBytes = buffer;
            //            cStream.Write(Encoding.UTF8.GetBytes($"{clients[clientrealnumber].nickname} disconnected..."));
            //        }
            //    }
            //    Invoke(new Action(() => listBox1.Items.Add($"{clients[clientrealnumber].nickname} disconnected...")));
            //}
            client.client.Close();
            if (!isClosing)
            {
                Invoke(new Action(() => listBox1.Items.Remove(client.nickname)));
                clients.Remove(client);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;
            foreach (var c in clients)
            {
                NetworkStream n = c.client.GetStream();
                n.Write(Encoding.UTF8.GetBytes("1⧫◊"));
                c.client.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var c in clients)
            {
                c.client.GetStream().Write(Encoding.UTF8.GetBytes("1⧫◊"));
                c.client.Close();
            }
            button2.Enabled = false;
            button1.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            isGameStarted = false;
            isServerRun = false;
            listBox1.Items.Add("Server stopped");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            server.Stop();
            listBox2.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Contains('⧫') && !textBox1.Text.Contains('◊'))
            {
                if (textBox1.Text != "")
                {
                    foreach (var c in clients)
                    {
                        c.client.GetStream().Write(Encoding.UTF8.GetBytes("0⧫" + "Server:" + textBox2.Text + '◊'));
                    }
                    listBox1.Items.Add("Server:" + textBox2.Text);
                    textBox2.Text = "";
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
                else
                {
                    MessageBox.Show("문자는 공백이면 안됩니다.");
                }
            }
            else
            {
                MessageBox.Show("채팅에 다음 문자는 포함되면 안됩니다: ⧫, ◊");
            }
        }

        static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "로컬 IP 주소를 찾을 수 없습니다.";
        }

        static string GetExternalIPAddress()
        {
            using (WebClient client = new WebClient())
            {
                string response = client.DownloadString("https://api.ipify.org");
                return response;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }
        static Thread Ttt;
        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            button5.Enabled = true;
            isGameStarted = true;
            Ttt = new Thread(Game);
            Ttt.IsBackground = true;
            Ttt.Start();
        }

        int WolfCount(int people)
        {
            if (people >= 3 && people <= 4)
            {
                return 1;
            }
            else if (people >= 5 && people <= 6)
            {
                Random r = new Random(Convert.ToInt32(DateTime.Now.Ticks % 1000));
                return r.Next(1, 3);
            }
            else if (people >= 7 && people <= 9)
            {
                return 2;
            }
            else if (people == 10)
            {
                Random r = new Random(Convert.ToInt32(DateTime.Now.Ticks % 1000));
                return r.Next(2, 4);
            }
            return 0;
        }

        void Game()
        {
            int count = 0;
            Invoke(() => count = listBox3.Items.Count);
            if (count > 0)
            {
                Random r = new Random(Convert.ToInt32(DateTime.Now.Ticks % 1000));
                int word = r.Next(0, count);

                if (clients.Count >= 3 && clients.Count <= 10)
                {
                    int wolfcount = WolfCount(clients.Count);

                    HashSet<int> pickedNumbers = new HashSet<int>();
                    while (pickedNumbers.Count < wolfcount)
                    {
                        int number = r.Next(0, clients.Count);
                        pickedNumbers.Add(number);
                    }
                    List<int> wolf = new List<int>(pickedNumbers);
                    wolfs = new List<Client>();
                    innocents = new List<Client>();
                    string wolf_word = "";
                    string innocent_word = "";
                    Invoke(new Action(() => innocent_word = listBox3.Items[word].ToString()));
                    Invoke(new Action(() => wolf_word = listBox4.Items[word].ToString()));
                    foreach (var c in clients)
                    {
                        c.client.GetStream().Write(Encoding.UTF8.GetBytes($"0⧫게임을 시작합니다◊"));
                    }
                    Invoke(new Action(() => listBox1.Items.Add("게임을 시작합니다")));
                    Invoke(new Action(() => listBox1.TopIndex = listBox1.Items.Count - 1));
                    Delay(1000);
                    foreach (var c in clients)
                    {
                        c.client.GetStream().Write(Encoding.UTF8.GetBytes($"0⧫3◊"));
                    }
                    Invoke(new Action(() => listBox1.Items.Add("3")));
                    Invoke(new Action(() => listBox1.TopIndex = listBox1.Items.Count - 1));
                    Delay(1000);
                    foreach (var c in clients)
                    {
                        c.client.GetStream().Write(Encoding.UTF8.GetBytes($"0⧫2◊"));
                    }
                    Invoke(new Action(() => listBox1.Items.Add("2")));
                    Invoke(new Action(() => listBox1.TopIndex = listBox1.Items.Count - 1));
                    Delay(1000);
                    foreach (var c in clients)
                    {
                        c.client.GetStream().Write(Encoding.UTF8.GetBytes($"0⧫1◊"));
                    }
                    Invoke(new Action(() => listBox1.Items.Add("1")));
                    Invoke(new Action(() => listBox1.TopIndex = listBox1.Items.Count - 1));
                    Delay(1000);
                    foreach (int number in wolf)
                    {
                        wolfs.Add(clients[number]);
                        clients[number].client.GetStream().Write(Encoding.UTF8.GetBytes("6⧫" + wolf_word + "◊"));
                    }
                    foreach (var client in clients)
                    {
                        if (!wolfs.Contains(client))
                        {
                            innocents.Add(client);
                            client.client.GetStream().Write(Encoding.UTF8.GetBytes("6⧫" + innocent_word + "◊"));
                        }
                    }
                    Delay(100);
                    foreach (var c in clients)
                    {
                        c.client.GetStream().Write(Encoding.UTF8.GetBytes($"0⧫단어가 전달되었습니다◊"));
                    }
                    Invoke(new Action(() => listBox1.Items.Add("단어가 전달되었습니다")));
                    Invoke(new Action(() => listBox1.TopIndex = listBox1.Items.Count - 1));
                    vote = new List<string>();
                }
                else
                {
                    MessageBox.Show("사람은 3명 이상 10명 이하여야 하며, 권장하는 플레이 인원은 4명 이상입니다.");
                    Invoke(new Action(() =>
                    {
                        button4.Enabled = true;
                        button5.Enabled = false;
                    }));
                }
            }
            else
            {
                MessageBox.Show("단어 목록을 추가해 주세요");
                Invoke(new Action(() =>
                {
                    button4.Enabled = true;
                    button5.Enabled = false;
                }));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            button5.Enabled = false;
            isGameStarted = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox3.Items.RemoveAt(listBox3.SelectedIndex);
            listBox4.Items.RemoveAt(listBox4.SelectedIndex);
            button7.Enabled = false;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && isServerRun)
            {
                if (!textBox1.Text.Contains('⧫') && !textBox1.Text.Contains('◊'))
                {
                    if (textBox1.Text != "")
                    {
                        foreach (var c in clients)
                        {
                            c.client.GetStream().Write(Encoding.UTF8.GetBytes("0⧫" + "Server:" + textBox2.Text + '◊'));
                        }
                        listBox1.Items.Add("Server:" + textBox2.Text);
                        textBox2.Text = "";
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                    }
                    else
                    {
                        MessageBox.Show("문자는 공백이면 안됩니다.");
                    }
                }
                else
                {
                    MessageBox.Show("채팅에 다음 문자는 포함되면 안됩니다: ⧫, ◊");
                }
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox3.Text != "" && textBox4.Text != "")
            {
                listBox3.Items.Add(textBox3.Text);
                listBox4.Items.Add(textBox4.Text);
                textBox3.Text = "";
                textBox4.Text = "";
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox3.Text != "" && textBox4.Text != "")
            {
                if (!textBox3.Text.Contains(',') && !textBox4.Text.Contains(','))
                {
                    listBox3.Items.Add(textBox3.Text);
                    listBox4.Items.Add(textBox4.Text);
                    textBox3.Text = "";
                    textBox4.Text = "";
                }
                else
                {
                    MessageBox.Show("단어에는 쉼표(,)가 포함될 수 없습니다");
                }
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                listBox4.SelectedIndexChanged -= listBox4_SelectedIndexChanged;
                listBox4.SelectedIndex = listBox3.SelectedIndex;
                listBox4.SelectedIndexChanged += listBox4_SelectedIndexChanged;
                button7.Enabled = true;
            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex != -1)
            {
                listBox3.SelectedIndexChanged -= listBox3_SelectedIndexChanged;
                listBox3.SelectedIndex = listBox4.SelectedIndex;
                listBox3.SelectedIndexChanged += listBox3_SelectedIndexChanged;
                button7.Enabled = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "")
            {
                listBox3.Items.Add(textBox3.Text);
                listBox4.Items.Add(textBox4.Text);
                textBox3.Text = "";
                textBox4.Text = "";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            listBox4.Items.Clear();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CSV File(*.csv)|*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader sr = new StreamReader(openFileDialog1.FileName);
                    string line = sr.ReadLine();
                    if (line != "시민 단어, 늑대 단어")
                        throw new Exception("오류");
                    string[] data = line.Split(',');
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        data = line.Split(',');
                        listBox3.Items.Add(data[0]);
                        listBox4.Items.Add(data[1]);
                    }
                }
                catch
                {
                    MessageBox.Show("이 프로그램과 맞지 않는 형식입니다.");
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSV File(*.csv)|*.csv";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog1.FileName, false, Encoding.GetEncoding("utf-8")))
                {
                    file.WriteLine("시민 단어, 늑대 단어");
                    for (int i = 0; i < listBox3.Items.Count; i++)
                    {
                        file.WriteLine($"{listBox3.Items[i]},{listBox4.Items[i]}");
                    }
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2(this);
            form.Show();
        }

        public void DataAdd(string data1, string data2)
        {
            listBox3.Items.Add(data1);
            listBox4.Items.Add(data2);
        }
    }
    class Client
    {
        public TcpClient client;
        public string nickname;
        public bool isWolf;

        public Client(TcpClient client, int n)
        {
            this.client = client;
            nickname = "Client" + n.ToString();
            isWolf = false;
        }
        public Client(TcpClient client, string str)
        {
            this.client = client;
            nickname = str;
            isWolf = false;
        }
    }
}
