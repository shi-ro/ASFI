using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    class ScaleDownFadeOut : Sprite
    {
        public int FadeSpeed = 5;
        public float ScaleDownSpeed = 0.1f;
        public int Spin = 0;
        public ScaleDownFadeOut(string name, Position position, string texture) : base(name, position, texture)
        {
        }

        public ScaleDownFadeOut(string name, Position position, Animation animation) : base(name, position, animation)
        {
        }

        public ScaleDownFadeOut(string name, int x, int y, string texture) : base(name, x, y, texture)
        {
        }

        public ScaleDownFadeOut(string name, int x, int y, Animation animation) : base(name, x, y, animation)
        {
        }

        public ScaleDownFadeOut(string name, Position position, string texture, Collision colision) : base(name, position, texture, colision)
        {
        }

        public ScaleDownFadeOut(string name, Position position, Animation animation, Collision colision) : base(name, position, animation, colision)
        {
        }

        public ScaleDownFadeOut(string name, int x, int y, string texture, Collision colision) : base(name, x, y, texture, colision)
        {
        }

        public ScaleDownFadeOut(string name, int x, int y, Animation animation, Collision colision) : base(name, x, y, animation, colision)
        {
        }

        public override void Update(TimeSpan dt)
        {
            if (!Enabled)
            {
                return;
            }
            Opacity -= FadeSpeed;
            Scale -= ScaleDownSpeed;
            Orientation += Spin;
            if(Opacity<0)
            {
                Opacity = 0;
                Enabled = false;
            }
            if(Scale < 0)
            {
                Scale = 0;
                Enabled = false;
            }
        }
    }
}
