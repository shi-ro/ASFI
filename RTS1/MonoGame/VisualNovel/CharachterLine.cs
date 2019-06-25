using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.VisualNovel
{
    class CharachterLine
    {
        public Charachter Charachter;
        public string Line;
        public CharachterLine(Charachter charachter, string line)
        {
            Charachter = charachter;
            Line = line;
        }
    }
}
