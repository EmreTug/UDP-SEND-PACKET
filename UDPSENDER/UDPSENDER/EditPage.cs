using UDPSENDER.Models;
using System;
using System.IO;
using System.Windows.Forms;

namespace UDPSENDER
{
    public partial class EditPage : Form
    {
        public EditPage(MainPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
        }
        /*
         form açılırken eski veriler yüklenir ve gerekli yerde arayüzde gösterilir.
         */
        MainPage mainPage;
        private void editPage_Load(object sender, EventArgs e)
        {
            var item = StaticClass.Values.Packets[StaticClass.packetindeks].Values[StaticClass.valueindeks];
            textBox1.Text = item.Name;
            textBox2.Text = item.Type;
            if (item.ValueValue.Bool != null)
                textBox3.Text = item.ValueValue.Bool.ToString();
            else if (item.ValueValue.String != null)
                textBox3.Text = item.ValueValue.String;
            else if (item.ValueValue.Integer != null)
                textBox3.Text = item.ValueValue.Integer.ToString();
        }
        /*
         veriler üzerinde yapılan değişiklikler kaydedilir.
         */
        
        private void button1_Click(object sender, EventArgs e)
        {

            mainPage.updateValue(textBox1.Text,textBox3.Text);
            this.Close();

        }

       
    }
}
