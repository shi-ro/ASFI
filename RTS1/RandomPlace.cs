using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTS1
{
    public partial class RandomPlace : Form
    {
        public string home = ""; 
        public List<Bitmap> items = new List<Bitmap>();
        
        public RandomPlace()
        {
            InitializeComponent();
        }

        private void RandomPlace_Load(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                items.Add(new Bitmap(Image.FromFile(openFileDialog1.FileName)));
                listBox1.Items.Add(openFileDialog1.FileName);
            }
        }

        private void save(Bitmap bmp, string name)
        {
            if (home == "") { return; }
            bmp.Save($@"{home}\{name}.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void button4_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                home = folderBrowserDialog1.SelectedPath;
                textBox1.Text = home;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Random R = new Random();
            int W = (int)numericUpDown1.Value;
            int H = (int)numericUpDown2.Value;
            Bitmap cur = new Bitmap(W, H);
            for(int i = 0; i < (int)numericUpDown3.Value; i ++)
            {
                cur = cur.AddToTopRandom(items[R.Next(items.Count-1)]);
            }
            pictureBox1.Image = cur;
        }
    }
}
