using UDPSENDER.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace UDPSENDER
{
    public partial class NewMemberPage : Form
    {
        public NewMemberPage(MainPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
           
        }
        /*
         formdan gelen değerler seçilen pakede value olarak eklenir.
         */
        MainPage mainPage;
        private void button1_Click(object sender, EventArgs e)
        {

            mainPage.addNewValue(textBox1.Text, comboBox1.SelectedItem.ToString(), textBox3.Text);
            this.Close();
           


        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /*
         değişken tipi sınırlandırılır.
         */
        private void newMemberPage_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("int32");
            comboBox1.Items.Add("String");
            comboBox1.Items.Add("bool");

        }
    }
}
