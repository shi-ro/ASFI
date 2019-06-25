using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    public class Text : GameObject
    {
        public string Line = "";
        private string _prevLine = "";

        private string[] list = new string[] { @"`", @"1", @"2", @"3", @"4", @"5", @"6", @"7", @"8", @"9", @"0", @"-", @"=", @"~", @"!", @"@", @"#", @"$", @"%", @"^", @"&", @"*", @"(", @")", @"_", @"+", @"q", @"w", @"e", @"r", @"t", @"y", @"u", @"i", @"o", @"p", @"a", @"s", @"d", @"f", @"g", @"h", @"j", @"k", @"l", @"z", @"x", @"c", @"v", @"b", @"n", @"m", @"Q", @"W", @"E", @"R", @"T", @"Y", @"U", @"I", @"O", @"P", @"A", @"S", @"D", @"F", @"G", @"H", @"J", @"K", @"L", @"Z", @"X", @"C", @"V", @"B", @"N", @"M", @"[", @"]", @"\", @"{", @"}", @"|", @";", @"'", @":", @"""", @",", @".", @"/", @"<", @">", @"?" };
        private int[] lengths = new int[] { 3, 3, 6, 6, 7, 6, 6, 6, 6, 6, 6, 7, 6, 5, 1, 9, 8, 7, 11, 5, 8, 7, 3, 3, 8, 7, 6, 9, 7, 4, 5, 6, 6, 1, 7, 6, 7, 7, 6, 5, 6, 6, 3, 6, 1, 7, 7, 6, 7, 6, 6, 9, 9, 11, 5, 6, 7, 7, 7, 3, 9, 6, 9, 7, 8, 6, 9, 7, 7, 7, 5, 8, 7, 9, 9, 7, 7, 9, 2, 2, 5, 3, 3, 1, 3, 3, 2, 6, 3, 2, 5, 6, 6, 7 };

        private List<RTS1.Sprite> sprites = new List<RTS1.Sprite>();
        public Text(int x, int y, string line) : base("#text",new Position(x,y), "C\\Tiles\\empty")
        {
            _prevLine = line + "|";
            Line = line;
        }
        public override void Draw(TimeSpan dt)
        {
            base.Draw(dt);
            foreach(var s in sprites)
            {
                s.Draw(dt);
            }
        }

        private void LoadSprites()
        {
            sprites.Clear();
            int cl = 8;
            int x = Position.X;
            int y = Position.Y;
            char[] ln = Line.ToCharArray();
            for(int i = 0; i < ln.Length; i++)
            {
                char c = ln[i];
                switch(c)
                {
                    
                    case '\n':
                        x = Position.X;
                        y += 19;
                        break;
                    case ' ':
                        cl = 6;
                        break;
                    default:
                        int idx = Array.IndexOf(list, $"{c}");
                        sprites.Add(new RTS1.Sprite("#let",new Position(x, y),$@"C\Text\{idx}"));
                        cl = lengths[idx] + 1;
                        break;
                }
                x += cl;
            }
        }

        public override void Update(TimeSpan dt)
        {
            if(_prevLine!=Line)
            {
                LoadSprites();
                _prevLine = Line;
            }

        }
    }
}
