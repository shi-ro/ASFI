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

namespace MapCreator
{
    public partial class Form1 : Form
    {
        public string Path = @"C:\Users\cream\Desktop\MAP";

        private static Random random = new Random((int)DateTime.Now.Ticks);

        public Dictionary<char, string> TilesT = new Dictionary<char, string>();
        public Dictionary<string, char> Tiles = new Dictionary<string, char>();

        public Dictionary<string, Bitmap> Images = new Dictionary<string, Bitmap>();
        public Dictionary<char, Bitmap> ImagesI = new Dictionary<char, Bitmap>();

        public string Name = "";
        public string Build = "";
        public string Map = "";

        public AutoView view;
        
        public string CurrentTile = "";
        public Bitmap CurrentBitmap;

        public Form1()
        {
            InitializeComponent();
            richTextBox1.Font = new Font(FontFamily.GenericMonospace, 12);
            listBox1.Font = new Font(FontFamily.GenericMonospace, 10);
            textBox3.Text = RandomString(7);
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
        
        private void PrintBuild(object p)
        {
            richTextBox3.Text += $"{p}\n";
        }

        private void PrintOut(object p)
        {
            richTextBox2.Text += $"{p}\n";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // load tiles into listbox
            // exclude overlays for now. 

            string[] files = Directory.GetFiles(Path);
            PrintOut($"Loading tiles ...");
            bool f = false;
            int n = 0;
            for(int i = 0; i < files.Length; i++)
            {
                string str = files[i].Split('\\')[files[i].Split('\\').Length-1];
                if(str.EndsWith(".png"))
                {
                    char ch = (char)(n + 33);
                    string rep = $"[ {ch} ] - " + str.Replace(".png", "");
                    PrintOut($"[ {i + 1}/{files.Length} ] : {rep}");
                    listBox1.Items.Add(rep);
                    Tiles.Add(rep, ch);
                    TilesT.Add(ch, rep);
                    Bitmap bmp = (Bitmap)Image.FromFile($@"{Path}\{str}");
                    Images.Add(rep, bmp);
                    ImagesI.Add(ch, bmp);
                    n++;
                    if (f)
                    {
                        CurrentTile = rep;
                        f = false;
                        CurrentBitmap = bmp;
                        textBox1.Text = CurrentTile;
                        textBox2.Text = Tiles[CurrentTile]+"";
                        pictureBox1.Image = bmp;
                    }
                }
            }
        }
        

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1)
            {
                string sel = $"{listBox1.SelectedItem}";
                CurrentTile = sel;
                char rel = Tiles[sel];
                textBox1.Text = CurrentTile;
                textBox2.Text = ""+rel;
                Bitmap bmp = Images[sel];
                CurrentBitmap = bmp;
                pictureBox1.Image = bmp;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(view==null)
            {
                view = new AutoView(this);
                view.Show();
                button6.Enabled = false;
            }
        }

        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }



        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if(view!=null)
            {
                view.Update();
            }
        }
        
        public List<char> TilesUsed()
        {
            List<char> ret = new List<char>();
            foreach(string s in richTextBox1.Lines)
            {
                foreach(char c in s.ToCharArray())
                {
                    if (!ret.Contains(c)) { ret.Add(c); }
                }
            }
            return ret;
        }

        public void BuildF()
        {
            Build = "";
            foreach(char c in TilesUsed())
            {
                if(TilesT.ContainsKey(c))
                {
                    string s = TilesT[c];
                    Build += $"{c} : {s.Split(' ')[2]}\n";
                }
            }
        }

        public void Save()
        {
            BuildF();
            richTextBox1.SaveFile($@"C:\Users\usagi\source\repos\RTS1\RTS1\Content\C\Maps\{Name}_map.tilemap", RichTextBoxStreamType.PlainText);
            string save = richTextBox1.Text;
            richTextBox1.Text = Build;
            richTextBox1.SaveFile($@"C:\Users\usagi\source\repos\RTS1\RTS1\Content\C\Maps\{Name}_setting.tilesetting", RichTextBoxStreamType.PlainText);
            richTextBox1.Text = save;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BuildF();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BuildF();
            Save();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Name = textBox3.Text == "" ? RandomString(7) : textBox3.Text;
        }
    }
}
