using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    class SlashEffect : GameObject
    {
        private int _dw = 40;
        private int _dh = 10;
        public SlashEffect(int x, int y, float orientation, float scale, string texture) : base("#slash", new Position(x, y), texture)
        {
            this.Orientation = orientation;
            this.Scale = scale;
        }
        public override void Update(TimeSpan dt)
        {
            if(Enabled)
            {
                Height -= (int)(dt.Milliseconds/15.0);
                Width += (int)(dt.Milliseconds/15.0);
                if (Height < 0)
                {
                    Enabled = false;
                }
            }
        }
    }
}
