using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdamAsmacaOyunu
{
    public partial class Form1 : Form
    {
        public static string AppDir = AppDomain.CurrentDomain.BaseDirectory;
        
        ArrayList kelimeler = new ArrayList();
        ArrayList Eskikelimeler = new ArrayList();
        List<ArrayList> Skorlar = new List<ArrayList>() {new ArrayList(),new ArrayList()};



        // Skor hesaplama Katsayı bilgileri    
        static int HakKatsayı = 4;
        static int ZamanKatsay = 2;   

        // Kayıt konum      
        string SkorKonum = "skor.txt";
        string TxtKonum = "data.txt";
        enum Sinyal
        {
            Hata = 1,
            Uyarı = 2,
            bilgi = 3
        }



        public Form1()
        {
            InitializeComponent();
        }


        int Sayıgüncelle()
        {
            int sayı = kelimeler.Count;
            label2.Text = sayı.ToString();
            return sayı;
        }
        void KelimeYönetim(bool EklensinMi,string kelime)
        {

            if (EklensinMi)
            {
                File.AppendAllText(TxtKonum, kelime.Trim()+ Environment.NewLine);
                kelimeler.Add(textBox1.Text.Trim());
                listBox2.Items.Add(textBox1.Text.Trim());
            }
            else
            {

                for (int i = 0; i < kelimeler.Count; i++)
                {
                    if (kelimeler[i].ToString().Trim() == kelime.Trim())
                    {
                        kelimeler.RemoveAt(i);
                        listBox2.Items.RemoveAt(i);
                    }
                }
                File.WriteAllText(TxtKonum, string.Join(Environment.NewLine, kelimeler.ToArray()));

            }
        }
     
        void HakYenile()
        {

            string a = HakLimit + "/" + HakKalan;
            button7.Text = a;


        }

      
        void SkorYükle()
        {
            listBox1.Items.Clear();
            SkorKonum = AppDir + SkorKonum;
            if (File.Exists(SkorKonum))
            {
                StreamReader file = new System.IO.StreamReader(SkorKonum);

                string satır;
                while ((satır = file.ReadLine()) != null)
                {
                    if (satır.Trim()=="")
                    {
                        continue;
                    }
                    Skorlar[0].Add(satır.Split(new char[] { '|' })[0].ToString());
                    Skorlar[1].Add(satır.Split(new char[] { '|' })[1].ToString());

                }

                file.Close();
                file.Dispose();
            }
            else
            {
                File.CreateText(SkorKonum);
            }

            if (Skorlar[0].Count > 0)
            {
              
                List<ArrayList> sıralı = SkorSırala(Skorlar);


                for (int i = 0; i < sıralı[0].Count; i++)
                {
                    ListboxEkle(sıralı[0][i].ToString(), sıralı[1][i].ToString());
                }


            }

        }
        void SkorEkle(string isim,string skor)
        {
            Skorlar[0].Add(isim);
            Skorlar[1].Add(skor);

            File.AppendAllText(SkorKonum, isim+ "|"+ skor + Environment.NewLine);

            listBox1.Items.Clear();

            if (Skorlar[0].Count > 0)
            {
                List<ArrayList> sıralı = SkorSırala(Skorlar);


                for (int i = 0; i < sıralı[0].Count; i++)
                {
                    ListboxEkle(sıralı[0][i].ToString(), sıralı[1][i].ToString());
                }


            }
        }


         List<ArrayList> SkorSırala( List<ArrayList> _Skorlar)
        {



            List<ArrayList> yedekSkorlar = new List<ArrayList>();
            ArrayList aa = new ArrayList(_Skorlar[0]);
            ArrayList bb = new ArrayList(_Skorlar[1]);
            yedekSkorlar.Add(aa);
            yedekSkorlar.Add(bb);

            
           
            List<ArrayList> SıralıSkorlar = new List<ArrayList>() { new ArrayList(), new ArrayList() };
            int dönsayı = yedekSkorlar[0].Count;
            for (int i = 0; i < dönsayı; i++)
            {
                int max = -1;

                for (int j = 0; j < yedekSkorlar[1].Count; j++)
                {
                    int say = int.Parse(yedekSkorlar[1][j].ToString());
                    if (say > max)
                    {
                        max = say;

                    }


                }

                for (int a = 0; a < yedekSkorlar[1].Count; a++)
                {
                    if (max == int.Parse(yedekSkorlar[1][a].ToString()))
                    {
                        SıralıSkorlar[0].Add(yedekSkorlar[0][a]);
                        SıralıSkorlar[1].Add(yedekSkorlar[1][a]);

                        yedekSkorlar[1].RemoveAt(a);
                        yedekSkorlar[0].RemoveAt(a);
                    }


                }

            }

            return SıralıSkorlar;
           
        }
        void ListboxEkle(string isim,string Skor)
        {
                listBox1.Items.Add(isim + " - " + Skor);
        }
     


        void KelimeYükle()
        {

            TxtKonum = AppDir + TxtKonum;
            if (File.Exists(TxtKonum))
            {
                StreamReader file = new System.IO.StreamReader(TxtKonum,Encoding.Default);

                string satır;
                while ((satır = file.ReadLine()) != null)
                {
                    if (satır.Trim() == "")
                    {
                        continue;
                    }

                    kelimeler.Add(satır);
                    listBox2.Items.Add(satır);
                }

                file.Close();
            }
            else
            {
                File.CreateText(TxtKonum);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Gecis();

            KelimeYükle();
            Sayıgüncelle();
            SkorYükle();
            durum("Unutmayın! Kelime En az 5 en fazla 16 karakter uzunluğa sahip olabilir",Sinyal.bilgi);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length>16)            {

                durum("16 dan uzun kelime girilemez!", Sinyal.Hata);
                return;
            }
            else if (textBox1.Text.Length < 5)
            {
                durum("5 den kısa kelime girilemez!", Sinyal.Hata);
                return;
            }

            foreach (string item in kelimeler)
            {

                if (textBox1.Text.Trim().Equals(item))
                {
                 
                    durum("Böyle bir kelime zaten var!", Sinyal.Hata);
                    return;
                }                
            }
            KelimeYönetim(true, textBox1.Text);
            Sayıgüncelle();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (string item in kelimeler)
            {

                if (textBox1.Text.Trim().Equals(item))
                {

                    KelimeYönetim(false, textBox1.Text);
                    Sayıgüncelle();
                    return;
                }
            }
            durum("Böyle bir kelime bulunamadı!",Sinyal.Hata);



        }

       


        void durum(string msj,Sinyal ss)
        {

            string sonStr = "Hazır!";
            toolStripStatusLabel2.Text = msj;
            Thread Msj=null;

            switch (ss)
            {
                case Sinyal.Hata:
                    Msj = new Thread(a =>
                    {
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.Red; });
                        System.Threading.Thread.Sleep(300);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.Black; });
                        System.Threading.Thread.Sleep(300);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.Red; });
                        System.Threading.Thread.Sleep(300);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.Black; });
                        System.Threading.Thread.Sleep(300);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.Text = sonStr; });
                    });
                    break;
                case Sinyal.Uyarı:
                     Msj = new Thread(a =>
                    {
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.DimGray; });
                        System.Threading.Thread.Sleep(300);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.Black; });
                        System.Threading.Thread.Sleep(300);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.DimGray; });
                        System.Threading.Thread.Sleep(300);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.Black; });
                        System.Threading.Thread.Sleep(300);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.Text = sonStr; });
                    });
                    break;
                case Sinyal.bilgi:

                     Msj = new Thread(a =>
                    {
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.ForestGreen; });
                        System.Threading.Thread.Sleep(500);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.Black; });
                        System.Threading.Thread.Sleep(500);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.ForestGreen; });
                        System.Threading.Thread.Sleep(500);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.Black; });
                        System.Threading.Thread.Sleep(500);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.ForestGreen; });
                        System.Threading.Thread.Sleep(500);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.Black; });
                        System.Threading.Thread.Sleep(500);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.ForestGreen; });
                        System.Threading.Thread.Sleep(500);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.Black; });
                        System.Threading.Thread.Sleep(500);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.ForestGreen; });
                        System.Threading.Thread.Sleep(500);
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.ForeColor = Color.Black; });
                        statusStrip1.Invoke((MethodInvoker)delegate { toolStripStatusLabel2.Text = sonStr; });

                    });
                    break;
            }


                    Msj.Start();
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
            {

            }
            else
            {

            }

        }

        string kelimeCache = "";
        int kelimeCachetur = 0;
        int KelimeEskimi(string kelime)
        {
            //0 false-1 true -2 error
            
            if (kelimeCache == kelime)
            {
                kelimeCachetur++;
                if (kelimeCachetur>kelimeler.Count)
                {
                    return 2;
                }
            }
            else
            {
                kelimeCachetur = 0;
                kelimeCache = kelime;
            }
           


            foreach (string item in Eskikelimeler)
            {
                if (kelime==item)
                {

                    return 1;
                }

            }
            
            return 0;           
        }


        string KelimeTut(int uzunluk = 0)
        {

            string tutulan = "";
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            if (uzunluk == 0)//RandomSeç
            {
                int drm=0;

                do
                {
                    int say = rnd.Next(0, Sayıgüncelle());
                    tutulan = kelimeler[say].ToString();
                    drm = KelimeEskimi(tutulan);
                    if (drm == 2)
                    {
                        durum("Bütün Kelimeler Kullanılmış! Listeyi güncelleyin", Sinyal.Hata);
                        return "-1";
                    }
                } while (drm == 1);
            }
            else
            {
                ArrayList HedefKelimeler = new ArrayList();

                foreach (string item in kelimeler)
                {
                    if (item.Length==uzunluk && KelimeEskimi(item)==0)
                    {
                        HedefKelimeler.Add(item);
                    }
                }

                if (HedefKelimeler.Count==0)
                {
                    durum("Hedef Uzunlukta Kelime Bulunamadı veya daha önce kullanıldı!", Sinyal.Hata);
                    return "-1";
                }

                int say = rnd.Next(0,HedefKelimeler.Count-1);
                tutulan = HedefKelimeler[say].ToString();
                
            }
            return tutulan;
        }

        Stopwatch Zaman;
        int MaxZaman;
        int HakLimit;
        int HakKalan;
        string TutulanKelimem;
        static int HarfBaşıSaniye = 10;
        private void button3_Click(object sender, EventArgs e)
        {

            if (Sayıgüncelle()<1)
            {
                durum("Oynamak için Kelime Yetersiz!", Sinyal.Hata);
                return;
            }

           
           

            if (radioButton1.Checked)
            {
                TutulanKelimem = KelimeTut((int)numericUpDown1.Value);
            }
            else
            {
                TutulanKelimem = KelimeTut();
            }
            if (TutulanKelimem == "-1")
            {
                return;
            }

            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();
            button3.Enabled = false;
            button5.Enabled = true;
            boşluk = false;
            MaxZaman = HarfBaşıSaniye * TutulanKelimem.Length;
            Bilinen = 0;
            HarfNesneEkle(TutulanKelimem);
            Zaman = Stopwatch.StartNew();
            Sayac.Start();
            HakKalan = TutulanKelimem.Length*2;
            HakLimit = HakKalan;
       


        }
 
        int SkorHesapla(string Kelime)
        {

            float skor = Kelime.Length*10;
            float oran1=1, oran2 = 1;
            oran1 = ((HakKatsayı * HakKalan) / HakLimit);

            oran2 =(Zaman.ElapsedMilliseconds / 1000);
            oran2 = (MaxZaman - oran2);
            oran2 = (oran2 / MaxZaman);
            oran2 = ((MaxZaman - oran2) / MaxZaman);


            oran2 = ZamanKatsay * oran2;
            skor = (skor * oran1);
            skor = (skor * oran2);

            return (int)skor;
        }


        void HarfNesneEkle(string kelime)
        {
            string[] harfler = new string[kelime.Length];
            ArrayList hObj = new ArrayList();


            for (int i = 0; i< kelime.Length; i++)
            {

                harfler[i] = kelime.Substring(i, 1);
                hObj.Add(new harf(harfler[i], i+1));              
                flowLayoutPanel1.Controls.Add((harf)hObj[i]);
            }


           


        }

        private void button4_Click(object sender, EventArgs e)
        {
            HarfNesneEkle("efskfsejhfseoıjh");
            SkorYükle();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 1)
            {
                textBox2.Text = textBox2.Text.Substring(0, 1) ;
            }
            textBox2.Text= textBox2.Text.ToUpper();
        }
        ArrayList DenenmişHarfler = new ArrayList();
        int Bilinen = 0;
        bool boşluk = false;
        private void button5_Click(object sender, EventArgs e)
        {
            string Tbox= textBox2.Text.ToLower();

            bool Bildimi = false;
            if (TutulanKelimem.Contains(Tbox))
            {
                for (int i = 0; i < TutulanKelimem.Length; i++)
                {
                    harf a = (harf)flowLayoutPanel1.Controls[i];
                    string tutulan = TutulanKelimem.Substring(i, 1);
                    if (tutulan == Tbox)
                    {                       
                        a.HarfGöster();
                        if (a.renk != Color.GreenYellow)
                        {
                            a.renk = Color.GreenYellow;
                            Bilinen++;
                        }
                        Bildimi= true;                       
                    }
                    else if (!boşluk && tutulan==" ")
                    {
                        Bilinen++;
                    }
                    
                }
            }
            boşluk = true;
            string harfler = String.Join("", DenenmişHarfler.ToArray());
            if (!harfler.Contains(Tbox))
            {
                Label lbl = new Label();
                lbl.Text = Tbox.ToUpper();
                lbl.Font = new Font(textBox2.Font.FontFamily, textBox2.Font.Size,FontStyle.Strikeout);
                lbl.AutoSize = true;

                

                DenenmişHarfler.Add(Tbox);
                if (!Bildimi)
                {
                    HakKalan--;
                   
                }
                else
                {
                    lbl.Font = new Font(textBox2.Font.FontFamily, textBox2.Font.Size, FontStyle.Bold);
                    lbl.ForeColor = Color.ForestGreen;
                }
                    

                flowLayoutPanel2.Controls.Add(lbl);
            }
            else if (flowLayoutPanel2.Controls.Count>0 && !Bildimi)
            {
                foreach (Label lbl in flowLayoutPanel2.Controls)
                {
                    if (lbl.Text.Equals(Tbox,StringComparison.OrdinalIgnoreCase))
                    {

                        Thread uyarı = new Thread(a=> {
                            lbl.ForeColor = Color.Red;
                            Thread.Sleep(80);
                            lbl.ForeColor = Color.Black;
                            Thread.Sleep(80);
                            lbl.ForeColor = Color.Red;
                            Thread.Sleep(80);
                            lbl.ForeColor = Color.Black;
                            Thread.Sleep(80);
                            lbl.ForeColor = Color.Red;
                            Thread.Sleep(80);
                            lbl.ForeColor = Color.Black;
                        });
                        uyarı.Start();

                    }


                }


            }


            if (HakKalan==0)
            {

                MessageBox.Show("Bilemediniz! Doğru Cevap:" + TutulanKelimem);
                button3.Enabled = true;
                button5.Enabled = false;
                Zaman.Stop();
                Sayac.Stop();
            }
            else if (Bilinen == TutulanKelimem.Length)
            {
                SkorEkle msg = new SkorEkle();
                string skor = SkorHesapla(TutulanKelimem).ToString();
                msg.label1.Text = string.Format(msg.label1.Text, skor);
                Zaman.Stop();
                Sayac.Stop();
                msg.ShowDialog();
                if (msg.isim!=null)               
                    SkorEkle(msg.isim, skor);
               

                button3.Enabled = true;
                button5.Enabled = false;
              
            }


          

        }

        private void Sayac_Tick(object sender, EventArgs e)
        {

            HakYenile();
            TimeSpan çıkan = TimeSpan.FromSeconds(MaxZaman);
                    
            DateTime dateTime = DateTime.Today.Add(çıkan);
            dateTime= dateTime.AddMilliseconds(-(Zaman.ElapsedMilliseconds));

            button9.Text= dateTime.ToString("mm:ss");

            if (MaxZaman*1000<Zaman.ElapsedMilliseconds)
            {
                Zaman.Stop();
                Sayac.Stop();
                MessageBox.Show("Bilemediniz! Doğru Cevap:" + TutulanKelimem);
                button3.Enabled = true;
                button5.Enabled = false;
               

            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listBox2.SelectedIndex<0)
            {
                textBox1.Text = listBox2.Items[listBox2.SelectedIndex].ToString();                               
            }
        }
        bool gecis;
        float gecisYedek = -1;
        int minYedek = -1;
        void Gecis()
        {

            TableLayoutColumnStyleCollection styles = tableLayoutPanel1.ColumnStyles;
            
            int x = (listBox1.Size.Width + listBox2.Size.Width);
            int _x = this.Location.X + (listBox1.Size.Width + listBox2.Size.Width);

            if (gecisYedek ==-1)
            {
                gecisYedek = styles[2].Width;
                minYedek= this.MinimumSize.Width;

            }


            if (gecis)
            {
                this.MinimumSize = new Size(minYedek, this.MinimumSize.Height);
                groupBox4.Visible = true;
                groupBox6.Visible = true;
                styles[2].Width = gecisYedek;
                styles[1].Width = gecisYedek;
                button4.Text = "<<";
            }
            else
            {
                button4.Text = ">>";
                this.MinimumSize = new Size(minYedek-x, this.MinimumSize.Height);
                groupBox4.Visible = false;
                groupBox6.Visible = false;
                this.Size = new Size(this.Size.Width- x, this.Size.Height);
                styles[2].Width = 0;
                styles[1].Width = 0;
            }
           



            gecis = !gecis;
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
              Gecis();
            
           
            //return;
            //foreach (RowStyle style in styles)
            //{
            //    if (style.SizeType == SizeType.Absolute)
            //    {
            //        style.SizeType = SizeType.AutoSize;
            //    }
            //    else if (style.SizeType == SizeType.AutoSize)
            //    {
            //        style.SizeType = SizeType.Percent;

            //        // Set the row height to be a percentage  
            //        // of the TableLayoutPanel control's height.  
            //        style.Height = 33;
            //    }
            //    else
            //    {

            //        // Set the row height to 50 pixels.  
            //        style.SizeType = SizeType.Absolute;
            //        style.Height = 50;
            //    }
            //}
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }
    }
}
