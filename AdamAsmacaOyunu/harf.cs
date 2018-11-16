using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdamAsmacaOyunu
{
    public partial class harf : UserControl
    {

       public string Harf { get; set; }
        public int No { get; set; }
        Color _renk=SystemColors.ButtonHighlight;
        public Color renk
        {
            set
            {
                _renk = value;
                button2.FlatAppearance.BorderColor = _renk;
            }
            get
            {
                return _renk;
            }


        }
        public void HarfGöster()
        {
            textBox1.Text = Harf;

        }
        public harf(string hh,int nn)
        {
            InitializeComponent();
            Harf = hh;
            if (hh==" ")
            {
                textBox1.Text = " ";
                textBox1.Enabled = false;
                renk = Color.ForestGreen;
            }
            No = nn;
           // textBox1.Text =Harf;
            label1.Text = No.ToString();
        }

        private void harf_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length>1)
            {
                textBox1.Text = textBox1.Text.Substring(0, 1);
            }

          

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
