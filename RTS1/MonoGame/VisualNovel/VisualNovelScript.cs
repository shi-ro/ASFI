using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.VisualNovel
{
    class VisualNovelScript
    {
        public int CurrentLine = 0;
        private List<CharachterLine> _charachterLines = new List<CharachterLine>();
        public CharachterLine NextLine()
        {
            if(CurrentLine<_charachterLines.Count)
            {
                return _charachterLines[CurrentLine];
            }
            return new CharachterLine(null, "");
        } 
    }
}
