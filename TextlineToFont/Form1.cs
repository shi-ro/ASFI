using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextlineToFont
{
    public partial class Form1 : Form
    {
        String line = @"C:\Users\usagi\Desktop\string.png";
        Bitmap bmp;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                line = openFileDialog1.FileName;
                textBox1.Text = line;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            line = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Color ignore = Color.FromArgb(0,0,0,0);
            bmp = (Bitmap)Image.FromFile(line);
            int columnIndex = 0;
            bool gettingLetter = false;
            bool hasNotIgnore;
            List<int> starts = new List<int>();
            List<int> ends = new List<int>();
            Console.WriteLine($@"Starting ...");
            Console.WriteLine($@"Bitmap gotten [{bmp.Width},{bmp.Height}].");
            List<Bitmap> bitmaps = new List<Bitmap>();
            while (columnIndex<bmp.Width)
            {
                Console.WriteLine($@"Reading column [{columnIndex}]");
                hasNotIgnore = false;
                for(int i = 0; i < bmp.Height; i++)
                {
                    Color cur = bmp.GetPixel(columnIndex, i);
                    if (cur.A>ignore.A)
                    {
                        //Console.WriteLine($"C v C : [{cur} , {ignore}]");
                        hasNotIgnore = true;
                        break;
                    }
                }
                if (hasNotIgnore)
                {
                    Console.WriteLine($@"Column has other colors");
                    if (!gettingLetter)
                    {
                        Console.WriteLine($@"Starting letter read");
                        gettingLetter = true;
                        starts.Add(columnIndex);
                    }
                } else
                {
                    Console.WriteLine($@"Comun has no other colors");
                    if (gettingLetter)
                    {
                        Console.WriteLine($@"Ending letter read");
                        gettingLetter = false;
                        ends.Add(columnIndex);
                    }
                }
                columnIndex++;
            }
            Console.WriteLine("==================");
            for(int i = 0; i < starts.Count-1; i++)
            {
                Console.Write(starts[i]);
                if (i<starts.Count-2)
                {
                    Console.Write(", ");
                }
            }
            Console.WriteLine(";");
            for (int i = 0; i < ends.Count - 1; i++)
            {
                Console.Write(ends[i]);
                if (i < ends.Count - 2)
                {
                    Console.Write(", ");
                }
            }
            Console.WriteLine(";");
            Console.Write("{ ");
            for(int i = 0; i < starts.Count; i++)
            {
                int st = starts[i];
                int ed = ends[i];
                Console.Write((ed - st));
                if(i<starts.Count-1)
                {
                    Console.Write(", ");
                }
                Bitmap b = new Bitmap(ed - st, bmp.Height);
                for (int c = st; c < ed; c++)
                {
                    for (int p = 0; p < bmp.Height; p++)
                    {
                        Color cur = bmp.GetPixel(c, p);
                        if (cur.A > ignore.A)
                        {
                            b.SetPixel(c-st,p,cur);
                        }
                        else
                        {
                            b.SetPixel(c-st,p,Color.FromArgb(0, 0, 0, 0));
                        }
                    }
                }
                bitmaps.Add(b);
            }
            Console.WriteLine("};");
            for(int i = 0; i < bitmaps.Count; i++)
            {
                Bitmap cur = bitmaps[i];
                cur.Save($@"C:\Users\usagi\source\repos\RTS1\RTS1\Content\C\Text\{i}.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
