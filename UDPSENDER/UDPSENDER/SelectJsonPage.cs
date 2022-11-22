using Newtonsoft.Json;
using UDPSENDER.Models;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Windows.Forms;

namespace UDPSENDER
{

    public partial class SelectJsonPage : Form
    {
        public SelectJsonPage()
        {
            InitializeComponent();
        }
        /*
            butona tıklandığı zaman json belgesini okuyup staticclassdaki modeli doldorur 
           daha sonra başlangıç sayfasını gizler ve main sayfasını açar.  
            */
        private void button1_Click(object sender, EventArgs e)
        {
           
       
            LoadJson();
            this.Hide();
            MainPage page = new MainPage();
            page.Show();

        }

          /*
         kullanıcının dosya seçmesi için openfiledialog kullanılmıştır.
        seçilen dosyanın dosya yolu üzerinden okunması sağlanır ve okunan json valuemodel modeline eşitlenir.
        Eşitleme için deserializeobject kullanılır.
         */
        public void LoadJson()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); 
            string file = "";
            if (result == DialogResult.OK)
            {
                file = openFileDialog1.FileName;

            }
            StaticClass.url = file;
            try
            {
                using (StreamReader r = new StreamReader(StaticClass.url))
                {

                    string json = r.ReadToEnd();
                    StaticClass.Values = JsonConvert.DeserializeObject<ValueModel>(json);
                    r.Close();
                }
            }
            catch (Exception)
            {
                LoadJson();
                
            }
          
        }



    }
}
