using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Extended.MonogameMapImporter
{
    public class TilemapData
    {
        public string Name;
        public char[,] Map = new char[1, 1];
        public string Data;
        public TilemapData(string name, string data, char split)
        {
            Name = name;
            Data = data;
            string[] lines = Data.Split(split);
            Map = new char[lines.Length, lines[0].Length];
            int y = 0;
            int x = 0;
            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    Map[y, x] = c;
                    x++;
                }
                y++;
                x = 0;
            }
        }
        internal TilemapData(string data, char split)
        {
            Data = data;
            string[] lines = Data.Split(split);
            Map = new char[lines.Length,lines[0].Length];
            int y = 0;
            int x = 0;
            foreach(string line in lines)
            {
                foreach(char c in line)
                {
                    Map[y, x] = c;
                    x++;
                }
                y++;
                x = 0;
            }
        }
    }
}
