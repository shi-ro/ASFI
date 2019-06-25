using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    class GravityParticle : Sprite
    {
        public int Dx = 0;
        public double Dy = 0;
        
        public GravityParticle(string name, Position position, string texture) : base(name, position, texture)
        {
        }

        public GravityParticle(string name, Position position, Animation animation) : base(name, position, animation)
        {
        }

        public GravityParticle(string name, int x, int y, string texture) : base(name, x, y, texture)
        {
        }

        public GravityParticle(string name, int x, int y, Animation animation) : base(name, x, y, animation)
        {
        }

        public GravityParticle(string name, Position position, string texture, Collision colision) : base(name, position, texture, colision)
        {
        }

        public GravityParticle(string name, Position position, Animation animation, Collision colision) : base(name, position, animation, colision)
        {
        }

        public GravityParticle(string name, int x, int y, string texture, Collision colision) : base(name, x, y, texture, colision)
        {
        }

        public GravityParticle(string name, int x, int y, Animation animation, Collision colision) : base(name, x, y, animation, colision)
        {
        }

        public override void Update(TimeSpan dt)
        {
            if (!Enabled)
            {
                return;
            }
            Position.X = (Position.X + Dx*dt.Milliseconds/15);
            Position.Y = (int)(Position.Y + Dy);
            Dy += Program.Game.Gravity;
        }
    }
}
