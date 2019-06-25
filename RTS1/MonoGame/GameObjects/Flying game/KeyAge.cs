using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects.Flying_game
{
    class KeyAge
    {
        public double time;
        public Keys key;
        public KeyAge(Keys key, TimeSpan time)
        {
            this.key = key;
            this.time = time.TotalMilliseconds;
        }
    }
}
