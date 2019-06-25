using Microsoft.Xna.Framework.Graphics;
using RTS1_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.VisualNovel
{
    public class Charachter
    {
        public int Affinity;
        public int Malice;
        public string Name;
        public Texture2D CurrentSprite;
        public Charachter(string name, int affinity, int malice, string sprite)
        {
            Name = name;
            Affinity = affinity;
            Malice = malice;
            CurrentSprite = DataLoader.AddTexture(sprite);
        }
    }
}
