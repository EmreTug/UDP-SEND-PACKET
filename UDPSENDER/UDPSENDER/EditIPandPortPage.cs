using UDPSENDER.Models;
using System;
using System.IO;
using System.Windows.Forms;

namespace UDPSENDER
{
    public partial class EditIPandPortPage : Form
    {
        public EditIPandPortPage(MainPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
        }
        /*
         form yüklenirken eski veriker ekranda gösterilmek üzere yüklenir.
         */
        MainPage mainPage;
        private void editIPandPortPage_Load(object sender, EventArgs e)
        {
            textBox2.Text = StaticClass.Values.Ip;
            textBox3.Text = StaticClass.Values.Port.ToString();

        }
        /*
         yeni veriler kaydedilir.
         */
        private void button1_Click(object sender, EventArgs e)
        {
            mainPage.updatePort(textBox2.Text, textBox3.Text);
            this.Close();
        }

      
    }
}
