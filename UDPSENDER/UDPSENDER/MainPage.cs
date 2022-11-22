using UDPSENDER.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDPSENDER
{
    public partial class MainPage : Form
    {
        public MainPage()
        {

            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        /*
         sayfa yüklenirken yeni bir therad içerisinde dinleme işlemi başlar
         staticclass içerisindeki modelde var olan ip, port ve diğer veriler arayüzde gerekli yerlere eklenir.
        Listede en son seçili olan itemin tekrar seçili olması için selectedindex ayarlanır.
         */

        private void mainPage_Load(object sender, EventArgs e1)
        {
            if (StaticClass.PeriodicModels.Count > 0)
            {
                foreach (var item in StaticClass.PeriodicModels)
                {


                    listBox3.Items.Add(StaticClass.Values.Packets.FirstOrDefault(a => a.id == item.id).PacketName);

                }
            }
            foreach (var item in StaticClass.Values.Packets)
            {
                listBox1.Items.Add(item.PacketName);


            }
            textBox2.Text = StaticClass.Values.Ip;
            textBox3.Text = StaticClass.Values.Port.ToString();
            listBox1.SelectedIndex = StaticClass.packetindeks;


        }
        /*
        gönderilecek veri alınır ve byte arraye çevirilir.
        gönderilecek veri id ile başlar ve sonra sırasıyla devam eder.
        int 4 byte 32 bit
        string 50 byte 400 bit
        bool 1 byte 8 bit
        daha sonra udp ile gönderilir.
         */

        int counter = 0;
        public void sendPeriodic(Packet packets)
        {


            var item = packets.Values;



            byte[] sendbyte = new byte[0];

            sendbyte = addByteList(sendbyte, BitConverter.GetBytes(packets.id));
            for (int i = 0; i < item.Count; i++)
            {
                if (item[i].ValueValue.Bool != null)
                {

                    sendbyte = addByteList(sendbyte, BitConverter.GetBytes(item[i].ValueValue.Bool.Value));

                }

                else if (item[i].ValueValue.Integer != null)
                {
                    byte[] addByte = BitConverter.GetBytes(item[i].ValueValue.Integer.Value);

                    sendbyte = addByteList(sendbyte, addByte);

                }
                else if (item[i].ValueValue.String != null)
                {
                    char[] values = new char[50];
                    item[i].ValueValue.String.CopyTo(0, values, 0, item[i].ValueValue.String.Length);
                    byte[] bytes = Encoding.ASCII.GetBytes(values);
                    sendbyte = addByteList(sendbyte, bytes);
                }
            }






            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);
            IPAddress serverAddr = IPAddress.Parse(StaticClass.Values.Ip);
            IPEndPoint endPoint = new IPEndPoint(serverAddr, Convert.ToInt32(StaticClass.Values.Port));


            counter++;
            sock.SendTo(sendbyte, endPoint);
            if (sw.ElapsedMilliseconds > 3000)
            {
                sw.Stop();
            }

        }
        public void send()
        {

            int index = listBox1.SelectedIndex;
            var item = StaticClass.Values.Packets[index].Values;




            byte[] sendbyte = new byte[0];
            sendbyte = addByteList(sendbyte, BitConverter.GetBytes(StaticClass.Values.Packets[index].id));
            for (int i = 0; i < item.Count; i++)
            {
                if (item[i].ValueValue.Bool != null)
                {

                    sendbyte = addByteList(sendbyte, BitConverter.GetBytes(item[i].ValueValue.Bool.Value));

                }

                else if (item[i].ValueValue.Integer != null)
                {
                    byte[] addByte = BitConverter.GetBytes(item[i].ValueValue.Integer.Value);

                    sendbyte = addByteList(sendbyte, addByte);

                }
                else if (item[i].ValueValue.String != null)
                {
                    char[] values = new char[50];
                    item[i].ValueValue.String.CopyTo(0, values, 0, item[i].ValueValue.String.Length);
                    byte[] bytes = Encoding.ASCII.GetBytes(values);
                    sendbyte = addByteList(sendbyte, bytes);
                }
            }






            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);
            IPAddress serverAddr = IPAddress.Parse(StaticClass.Values.Ip);
            IPEndPoint endPoint = new IPEndPoint(serverAddr, Convert.ToInt32(StaticClass.Values.Port));



            sock.SendTo(sendbyte, endPoint);


        }
        public void task()
        {


            System.Timers.Timer tmr = new System.Timers.Timer();

            tmr.AutoReset = true;
            tmr.Interval = Convert.ToInt32((1000 / Convert.ToDouble(textBox1.Text))) - 5;

            StaticClass.PeriodicModels.Add(new PeriodicModel { id = StaticClass.Values.Packets[StaticClass.packetindeks].id, timer = tmr, packetName = StaticClass.Values.Packets[StaticClass.packetindeks].PacketName });
            foreach (var item in StaticClass.PeriodicModels)
            {
                if (item.timer.Enabled == false)
                {

                    item.timer.Elapsed += (sender, args) => sendPeriodic(StaticClass.Values.Packets.FirstOrDefault(a => a.id == item.id));

                    sw = Stopwatch.StartNew();
                    item.timer.Start();
                }
            }
            listBox3.Items.Add(StaticClass.Values.Packets[StaticClass.packetindeks].PacketName);
        }
        Stopwatch sw;

        private void send_button(object sender, EventArgs e)
        {


            if (listBox1.SelectedIndex != -1)
            {
                if (checkBox1.Checked)
                {


                    if (Int32.TryParse(textBox1.Text, out int value))
                    {

                        if (value > 50 || value < 1)
                        {
                            MessageBox.Show("Lütfen 1 ile 50 arasıında tamsayı değeri giriniz");

                        }
                        else
                        {
                            if (!StaticClass.PeriodicModels.Any(a => a.id == StaticClass.Values.Packets[StaticClass.packetindeks].id))
                            {
                                try
                                {
                                    button1.Enabled = false;

                                    task();
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lütfen 1 ile 50 arasıında tamsayı değeri giriniz");
                    }


                }
                else
                {
                    send();
                }

            }
        }

        public struct UdpState
        {
            public UdpClient u;
            public IPEndPoint e;
        }


        /*
         byte dizisi olarak gelen verinin ilk 4 elemanı alınır ve id bulunur.
        idye göre json içerisinden veri sıralamasına bakılır.
        sıralamaya göre verilen byte dizisinden normal haline çevirilir.
         */


        /*
         mesajlar asenkron şekilde dinlenmeye başlanır.
         */

        /*
         paketlerden biri şeçildiği anda paket içerisindeki veriler arayüzde gerekli kısma eklenir.
         */
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
         if(StaticClass.PeriodicModels.Any(a=>a.id== StaticClass.Values.Packets[listBox1.SelectedIndex].id))
            {
                button1.Enabled = false;
                button2.Visible = false;
                StaticClass.packetindeks = listBox1.SelectedIndex;
                listBox2.Items.Clear();
                int index = listBox1.SelectedIndex;
                if (StaticClass.Values.Packets[index].Values != null)
                {
                    foreach (var item in StaticClass.Values.Packets[index].Values)
                    {
                        if (item.ValueValue.String != null)
                        {

                            listBox2.Items.Add(item.Name + "---------->" + item.ValueValue.String);

                        }
                        else if (item.ValueValue.Bool != null)
                        {
                            listBox2.Items.Add(item.Name + "---------->" + item.ValueValue.Bool.ToString());

                        }
                        else if (item.ValueValue.Integer != null)
                        {
                            listBox2.Items.Add(item.Name + "---------->" + item.ValueValue.Integer.ToString());

                        }
                    }
                }
            }
            else
            {
                button1.Enabled = true;

                button2.Visible = false;
                StaticClass.packetindeks = listBox1.SelectedIndex;
                listBox2.Items.Clear();
                int index = listBox1.SelectedIndex;
                if (StaticClass.Values.Packets[index].Values != null)
                {
                    foreach (var item in StaticClass.Values.Packets[index].Values)
                    {
                        if (item.ValueValue.String != null)
                        {

                            listBox2.Items.Add(item.Name + "---------->" + item.ValueValue.String);

                        }
                        else if (item.ValueValue.Bool != null)
                        {
                            listBox2.Items.Add(item.Name + "---------->" + item.ValueValue.Bool.ToString());

                        }
                        else if (item.ValueValue.Integer != null)
                        {
                            listBox2.Items.Add(item.Name + "---------->" + item.ValueValue.Integer.ToString());

                        }
                    }
                }
            }
        }

        private void editValue(object sender, EventArgs e)
        {
            StaticClass.packetindeks = listBox1.SelectedIndex;
            StaticClass.valueindeks = listBox2.SelectedIndex;
            EditPage edit = new EditPage(this);
            edit.Show();

        

        }
        public void updateValue(string name,string value)
        {
        
            StaticClass.Values.Packets[StaticClass.packetindeks].Values[StaticClass.valueindeks].Name =name;

            if (StaticClass.Values.Packets[StaticClass.packetindeks].Values[StaticClass.valueindeks].ValueValue.Bool != null)
            {
                StaticClass.Values.Packets[StaticClass.packetindeks].Values[StaticClass.valueindeks].ValueValue = Convert.ToBoolean(value);
                listBox2.Items[listBox2.SelectedIndex] = name + "---------->" + value;

            }
            else if (StaticClass.Values.Packets[StaticClass.packetindeks].Values[StaticClass.valueindeks].ValueValue.String != null)
            {
                StaticClass.Values.Packets[StaticClass.packetindeks].Values[StaticClass.valueindeks].ValueValue = value;
                listBox2.Items[listBox2.SelectedIndex] = name + "---------->" + value;

            }


            else if (StaticClass.Values.Packets[StaticClass.packetindeks].Values[StaticClass.valueindeks].ValueValue.Integer != null)
            {
                StaticClass.Values.Packets[StaticClass.packetindeks].Values[StaticClass.valueindeks].ValueValue = Convert.ToInt32(value);
                listBox2.Items[listBox2.SelectedIndex] = name + "---------->" + value;

            }

            using (StreamWriter r = new StreamWriter(StaticClass.url))
            {
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(StaticClass.Values, Newtonsoft.Json.Formatting.Indented);
                r.Write(output);
                r.Close();
                
            }


        }
        private void addPacket(object sender, EventArgs e)
        {
            AddPacketPage page = new AddPacketPage(this);
            page.Show();

           

        }
        public void addNewPacket(string packetName)
        {
            if (!StaticClass.Values.Packets.Any(a => a.PacketName == packetName))
            {

                StaticClass.Values.Packets.Add(new Packet { PacketName = packetName, Values = new List<Value>(), id = StaticClass.Values.Packets.Last().id + 1 });
                using (StreamWriter r = new StreamWriter(StaticClass.url))
                {
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(StaticClass.Values, Newtonsoft.Json.Formatting.Indented);
                    r.Write(output);
                    r.Close();

                }
                listBox1.Items.Add(packetName);

            }
            else
            {
                MessageBox.Show("Aynı paket ismine sahip başka bir paket var");
            }
        }
        private void addValue(object sender, EventArgs e)
        {
            NewMemberPage page = new NewMemberPage(this);
            page.Show();

           

        }
        public void addNewValue(string name,string type,string value)
        {
            if (!StaticClass.Values.Packets[StaticClass.packetindeks].Values.Any(a => a.Name == name))
            {

                int value1;
                bool value2;
                if (Int32.TryParse(value, out value1))
                    StaticClass.Values.Packets[StaticClass.packetindeks].Values.Add(new Value { Name = name, Type = type, ValueValue = value1 });
                
                else if (bool.TryParse(value, out value2))
                    StaticClass.Values.Packets[StaticClass.packetindeks].Values.Add(new Value { Name = name, Type = type, ValueValue = value2 });
                else
                    StaticClass.Values.Packets[StaticClass.packetindeks].Values.Add(new Value { Name = name, Type = type, ValueValue = value });


                using (StreamWriter r = new StreamWriter(StaticClass.url))
                {
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(StaticClass.Values, Newtonsoft.Json.Formatting.Indented);
                    r.Write(output);
                    r.Close();

                }
                listBox2.Items.Add(name + "---------->" + value);



            }
            else
            {
                MessageBox.Show("Aynı değişken ismine sahip başka bir değişken var");
            }
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            StaticClass.valueindeks = listBox2.SelectedIndex;

            button2.Visible = true;

        }

        private void editPort(object sender, EventArgs e)
        {
            EditIPandPortPage page = new EditIPandPortPage(this);
            page.Show();
          
        }
        public void updatePort(string ip,string port)
        {
            StaticClass.Values.Ip = ip;
            StaticClass.Values.Port = Convert.ToInt32(port);
            using (StreamWriter r = new StreamWriter(StaticClass.url))
            {
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(StaticClass.Values, Newtonsoft.Json.Formatting.Indented);
                r.Write(output);
                r.Close();


            }
            textBox2.Text = ip;
            textBox3.Text = port;
        }
        /*
         gelen ilk parametredeki diziye ikinci parametredeki dizi eklenir.
         */
        public byte[] addByteList(byte[] a, byte[] b)
        {
            int startlenght = a.Length;
            int lenght = a.Length + b.Length;
            int j = 0;
            byte[] result = new byte[lenght];
            for (int i = 0; i < startlenght; i++)
            {
                result[i] = a[i];
            }
            for (int i = startlenght; i < lenght; i++)
            {

                result[i] = b[j];
                j++;
            }
            return result;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Do_Checked();
        }
        private void Do_Checked()
        {
            textBox1.Visible = checkBox1.Checked;
            label6.Visible = checkBox1.Checked;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                var index = listBox3.SelectedIndex;
                if (Int32.TryParse(textBox4.Text, out int value))
                {

                    if (value > 50 || value < 1)
                    {
                        MessageBox.Show("Lütfen 1 ile 50 arasıında tamsayı değeri giriniz");

                    }
                    else
                    {
                        StaticClass.PeriodicModels[index].timer.Interval = Convert.ToInt32((1000 / Convert.ToDouble(textBox4.Text))) - 5;

                    }
                }
                else
                {
                    MessageBox.Show("Lütfen 1 ile 50 arasıında tamsayı değeri giriniz");
                }


            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.Items.Count > 0 && listBox3.SelectedIndex == -1)
            {
                listBox3.SelectedIndex = 0;

            }
            if (listBox3.Items.Count > 0)
            {
                var index = listBox3.SelectedIndex;

                textBox4.Text = (Convert.ToInt32(1000 / (StaticClass.PeriodicModels[index].timer.Interval+5))).ToString();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                var index = listBox3.SelectedIndex;
                StaticClass.PeriodicModels[index].timer.Stop();
                StaticClass.PeriodicModels.RemoveAt(index);
                listBox3.Items.RemoveAt(index);
            }


        }

    }
}
