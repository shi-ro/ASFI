using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame.Extended.MonogameTilesetImporter
{
    public class TilesetData
    {
        public string Data;
        public string Name;
        public Dictionary<char, string> Tiles = new Dictionary<char, string>();
        public TilesetData(string name, string data, char split)
        {
            Name = name;
            Data = data;
            string[] lines = this.Data.Split(split);
            foreach (string line in lines)
            {
                string[] sp = line.Replace(" : ", ":").Split(':');
                Tiles.Add(sp[0].ToCharArray()[0], sp[1]);
            }
        }
        internal TilesetData(string data, char split)
        {
            Data = data;
            string[] lines = this.Data.Split(split);
            foreach(string line in lines)
            {
                string[] sp = line.Replace(" : ", ":").Split(':');
                Tiles.Add(sp[0].ToCharArray()[0], sp[1]);
            }
        }
    }
}
