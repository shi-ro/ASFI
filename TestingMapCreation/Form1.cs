using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingMapCreation
{
    public partial class Form1 : Form
    {
        Bitmap noise;
        Bitmap blur;
        Bitmap map;
        Gradient gradient = new Gradient();
        List<int> indices = new List<int>(); 
        List<Color> colors = new List<Color>();
        int index = 0;
        public Form1()
        {
            InitializeComponent();
            gradient.SetColor(0, Color.DarkBlue);
            gradient.SetColor(255, Color.White);
            setGradient();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //gradient.background = Color.Black;
            //gradient.SetColor(100, Color.DarkBlue);
            //gradient.SetColor(110, Color.Blue);
            //gradient.SetColor(111, Color.Beige);
            //gradient.SetColor(131, Color.Yellow);
            //gradient.SetColor(132, Color.LightGreen);
            //gradient.SetColor(150, Color.Green);
            //gradient.SetColor(200, Color.DarkGreen);
            //gradient.SetColor(255, Color.White);

            // rainbow                       : 255.0.0.0|255.132.0.35|255.255.0.90|0.255.0.129|0.0.255.192|255.0.255.255|
            // grass feilds and lakes        : 0.0.147.0|0.59.188.78|0.111.255.99|205.171.0.111|243.228.0.121|0.120.28.131|144.193.0.188|74.208.75.255|
            // darker grass feilds and lakes : 0.0.147.0|0.59.188.78|0.111.255.99|205.171.0.111|243.228.0.121|0.66.28.255|31.123.0.174|74.109.75.130|
            // islands and water             : 0.150.0.10|0.62.0.91|94.255.112.0|0.98.255.101|0.19.144.255|
            // mainland and lakes            : 0.150.0.10|0.62.0.147|94.255.112.0|0.98.255.162|0.19.144.255|
            // grey                          : 255.255.255.0|0.0.0.255|

            map = blur.GetMap(gradient);//.ResizeImage(1000,1000);
            pictureBox3.Image = map;

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            saveGradient();
            map.Save(@"C:\Users\usagi\Desktop\map.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            noise = NoiseMap.GenerateNoise((int)numericUpDown5.Value, (int)numericUpDown6.Value, (int)numericUpDown7.Value);
            //blur = noise.ImageBlurFilter(ExtBitmap.BlurType.Median11x11).ImageBlurFilter(ExtBitmap.BlurType.GaussianBlur5x5).ImageBlurFilter(ExtBitmap.BlurType.GaussianBlur5x5);

            blur = noise.ImageBlurFilter(ExtBitmap.BlurType.GaussianBlur5x5).ImageBlurFilter(ExtBitmap.BlurType.GaussianBlur5x5);//.ResizeImage(400,400);

            //apply resolution gradient
            //loadGradient("255.255.255.0|0.0.0.255|");
            
            //blur = blur.GetMap(gradient).ImageBlurFilter(ExtBitmap.BlurType.Median11x11).ImageBlurFilter(ExtBitmap.BlurType.GaussianBlur5x5).ImageBlurFilter(ExtBitmap.BlurType.GaussianBlur5x5).ImageBlurFilter(ExtBitmap.BlurType.GaussianBlur5x5).ResizeImage(blur.Width*2,blur.Height*2); 
            
            pictureBox1.Image = noise;
            ////pictureBox2.Image = blur;
        }

        private void saveGradient()
        {
            int sze = 20;
            Bitmap bmp = gradient.ToBitmap(1);
            for(int e = 0; e < 256; e++)
            {
                Color c = gradient.ColorAt(e);
                Bitmap blk = new Bitmap(sze, sze);
                for(int y = 0; y < sze; y++)
                {
                    for (int x = 0; x < sze; x++)
                    {
                        blk.SetPixel(x, y, c);
                    }
                }
                blk.Save($@"C:\Users\usagi\source\repos\RTS1\RTS1\Content\C\Tiles\M_{e}.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void loadGradient(string gs)
        {
            gradient = new Gradient();
            listBox1.Items.Clear();
            indices.Clear();
            colors.Clear();
            string[] grads = gs.Split('|');
            for(int i = 0; i < grads.Length; i++)
            {
                if(grads[i]!="")
                {
                    string[] dtl = grads[i].Split('.');
                    int R = Int32.Parse(dtl[0]);
                    int G = Int32.Parse(dtl[1]);
                    int B = Int32.Parse(dtl[2]);
                    int I = Int32.Parse(dtl[3]);
                    indices.Add(I);
                    colors.Add(Color.FromArgb(R, G, B));
                    listBox1.Items.Add("-=-=-");
                }
            }
            setGradient();
        }

        private void setGradient()
        {
            string gs = "";
            gradient = new Gradient();
            gradient.background = Color.Black;
            for(int i = 0; i < colors.Count; i++)
            {
                gradient.SetColor(indices[i], colors[i]);
                gs += $"{colors[i].R}.{colors[i].G}.{colors[i].B}.{indices[i]}|";
            }
            richTextBox1.Text = gs;
            pictureBox4.Image = gradient.ToBitmap(100);
            
        }

        private void red_changed(int val)
        {
            colors[index] = Color.FromArgb(val, colors[index].G, colors[index].B);
            setGradient();
        }

        private void green_changed(int val)
        {
            colors[index] = Color.FromArgb(colors[index].R, val, colors[index].B);
            setGradient();
        }

        private void blue_changed(int val)
        {
            colors[index] = Color.FromArgb(colors[index].R, colors[index].G, val);
            setGradient();
        }

        private void index_changed(int val)
        {
            indices[index] = val;
            setGradient();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            red_changed(trackBar1.Value);
            numericUpDown1.Value = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            green_changed(trackBar2.Value);
            numericUpDown2.Value = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            blue_changed(trackBar3.Value);
            numericUpDown3.Value = trackBar3.Value;
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            index_changed(trackBar4.Value);
            numericUpDown4.Value = trackBar4.Value;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            colors.Add(Color.Black);
            indices.Add(0);
            index = colors.Count - 1;
            trackBar1.Value = 0;
            trackBar2.Value = 0;
            trackBar3.Value = 0;
            trackBar4.Value = 0;

            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            listBox1.Items.Add("-=-=-");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            index = i;
            colors[index] = colors[i];
            trackBar1.Value = colors[i].R;
            trackBar2.Value = colors[i].G;
            trackBar3.Value = colors[i].B;
            trackBar4.Value = indices[i];

            numericUpDown1.Value = colors[i].R;
            numericUpDown2.Value = colors[i].G;
            numericUpDown3.Value = colors[i].B;
            numericUpDown4.Value = indices[i];

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            red_changed((int)numericUpDown1.Value);
            trackBar1.Value = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            green_changed((int)numericUpDown2.Value);
            trackBar2.Value = (int)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            blue_changed((int)numericUpDown3.Value);
            trackBar3.Value = (int)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            index_changed((int)numericUpDown4.Value);
            trackBar4.Value = (int)numericUpDown4.Value;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadGradient(richTextBox1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
