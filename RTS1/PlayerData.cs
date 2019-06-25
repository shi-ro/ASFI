using RTS1_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public class PlayerData
    {
        private Dictionary<string, string[]> _frames = new Dictionary<string, string[]>();
        public string Name;
        public int Speed;
        public int Health;
        private bool _framesLoaded = false;
        private bool _animationsLoaded = false;

        public PlayerData(string name, int speed, int health, Dictionary<string,string[]> frames)
        {
            Name = name;
            Speed = speed;
            Health = health;
        }

        public void LoadAllFrames()
        {
            if(_framesLoaded)
            {
                return;
            }

            foreach (KeyValuePair<string, string[]> e in _frames)
            {
                string[] f = e.Value;
                for(int i = 0; i < f.Length; i++)
                {
                    DataLoader.AddTexture(f[i]);
                }
            }
            _framesLoaded = true;
        }
        public void LoadAllAnimations()
        {
            if(_animationsLoaded)
            {
                return;
            }
            foreach (KeyValuePair<string, string[]> e in _frames)
            {
                DataLoader.CreateAnimation(e.Key, e.Value, 41);
            }
            _animationsLoaded = true;
        }
    }
}
