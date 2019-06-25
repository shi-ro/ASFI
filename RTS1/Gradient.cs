using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public class Gradient
    {
        public System.Drawing.Color[] colors = new System.Drawing.Color[256];
        public bool[] ee = new bool[256];
        public Gradient() { }
        public System.Drawing.Color background = System.Drawing.Color.Black;
        public bool changed = true;
        public Bitmap saved = null;
        public System.Drawing.Color ColorAt(int idx)
        {
            Bitmap b = ToBitmap(1);
            return b.GetPixel(0, idx);
        }
        
        public Bitmap ToBitmap(int w)
        {
            int h = 256;
            int start = -1;
            int end = -1;
            Bitmap ret = new Bitmap(w, h);
            for (int i = 0; i < ret.Height; i++)
            {
                ret.SetRow(i, background);
            }
            for (int i = 0; i < ee.Length; i++)
            {
                if (ee[i] && start == -1)
                {
                    start = i;
                }
                else if (ee[i] && start != -1 && end == -1)
                {
                    end = i;
                }
                if (end != -1 && start != -1)
                {
                    int dist = end - start;
                    Color S = colors[start];
                    Color E = colors[end];
                    Color RZ = Color.Black;

                    double DR = (E.R - S.R + 0.0f) / dist;
                    double DG = (E.G - S.G + 0.0f) / dist;
                    double DB = (E.B - S.B + 0.0f) / dist;

                    double CR = S.R;
                    double CG = S.G;
                    double CB = S.B;

                    for (int e = start; e < end; e++)
                    {
                        CR += DR;
                        CG += DG;
                        CB += DB;
                        RZ = Color.FromArgb(255, (int)CR, (int)CG, (int)CB);
                        ret.SetRow(e, RZ);
                    }

                    start = end;
                    end = -1;
                }
            }
            if (changed)
            {
                saved = ret;
                changed = false;
                return ret;
            }
            else
            {
                return saved;
            }
        }

        public void SetColor(int idx, System.Drawing.Color c)
        {
            colors[idx] = c;
            ee[idx] = true;
            changed = true;
        }
    }
}
