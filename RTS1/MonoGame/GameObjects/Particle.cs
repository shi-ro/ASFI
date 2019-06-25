using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    class Particle : GameObject
    {
        private double _dx = 0;
        private double _dy = 0;
        private double _gt = 0;
        public Particle(Position pos, int dx, int dy, string texture) : base("#particle", new Position(pos), texture)
        {
            double vx = Math.Abs(dx / 3.0f);
            double vy = Math.Abs(dy / 3.0f);
            _dx = dx + Extensions.R.Next(-(int)vx,(int)vx);
            _dy = dy + Extensions.R.Next(-(int)vy, (int)vy);
            Position = pos;
        }
        public override void Update(TimeSpan dt)
        {
            Position.X += (int)_dx;
            Position.Y += (int)(_dy+_gt);
            _gt += Program.Game.Gravity;
        }
    }
}
