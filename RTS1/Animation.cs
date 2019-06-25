using Microsoft.Xna.Framework.Graphics;
using RTS1_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public class Animation
    {
        public string[] textures;
        public List<Texture2D> Frames = new List<Texture2D>();
        public string name;
        public int FrameRate;
        public int CurrentFrame = 0;
        
        public Animation(string name, string[] textures, int frameRate)
        {
            this.name = name;
            this.textures = textures;
            this.FrameRate = frameRate;
            
            foreach(string texture in textures)
            {
                DataLoader.AddTexture(texture);
                Console.WriteLine($"[ *C ] <-- {texture} : {name}");
                Frames.Add(DataLoader.GetTexture(texture));
            }
        }
        
        public void Reset()
        {
            CurrentFrame = 0;
        }

        public Animation CopyOf()
        {
            return new Animation(name, textures, FrameRate);
        }
    }
}
