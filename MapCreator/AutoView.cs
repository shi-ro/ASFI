using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapCreator
{
    public partial class AutoView : Form
    {
        public Form1 P;

        public AutoView( Form1 parent)
        {
            P = parent;
            FormClosed += new FormClosedEventHandler(Form1_FormClosed);
            InitializeComponent();
        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            P.view = null;
            P.button6.Enabled = true;
        }

        private void AutoView_Load(object sender, EventArgs e)
        {

        }

        public void Update()
        {
            string[] lines = P.richTextBox1.Lines;
            int w = 0;
            int h = lines.Length;
            foreach(string line in lines)
            {
                if(line.Length>w)
                {
                    w = line.Length;
                }
            }
            char[][] chars = new char[h][];
            for(int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                char[] well = new char[w];
                int b = 0;
                foreach(char c in line.ToCharArray())
                {
                    well[b] = c;
                    b++;
                }
                chars[i] = well; 
            }
            if(w<=0&&h<=0)
            {
                return;
            }
            Bitmap draw = new Bitmap(w * 20, h * 20);
            for (int he = 0; he < h; he++)
            {
                for (int wd = 0; wd < w; wd++)
                {
                    char ch = chars[he][wd];
                    if(!P.ImagesI.ContainsKey(ch))
                    {
                        continue;
                    }
                    Bitmap tile = P.ImagesI[ch];
                    int dx = wd * 20;
                    int dy = he * 20;
                    if (tile == null) { }
                    for(int y = 0; y < 20; y ++)
                    {
                        for(int x = 0; x < 20; x++)
                        {
                            draw.SetPixel(dx + x, dy + y, tile.GetPixel(x, y));
                        }
                    }
                }
            }
            try
            {
                pictureBox1.Image = draw;
            } catch
            {

            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
