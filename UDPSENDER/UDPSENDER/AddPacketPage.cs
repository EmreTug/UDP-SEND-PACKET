using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace UDPSENDER
{
    public partial class AddPacketPage : Form
    {
        public AddPacketPage(MainPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
        }
        /*
         girilen paket isminde yeni bir paket oluşturulur.
         */
        MainPage mainPage;
        private void button1_Click(object sender, EventArgs e)
        {

            mainPage.addNewPacket(textBox1.Text);
            this.Close();
        }

     
    }
}
