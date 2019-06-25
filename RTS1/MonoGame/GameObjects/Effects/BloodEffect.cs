using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects.Effects
{
    class BloodEffect : GameObject
    {
        private List<GameObject> _particles = new List<GameObject>();
        private double _acdg = 0;
        private Position _delta;
        private int _shuff;
        public BloodEffect(int x, int y, int dx, int dy, int shuffle) : base("#effect-blood", new Position(x, y), @"C\Tiles\empty")
        {
            _delta = new Position(dx, dy);
            _shuff = shuffle;
        }

        public void Act(int amt)
        {
            string[] arts = {"b_0","b_1","b_2","b_3","b_4","b_5"};
            for(int i = 0; i < amt; i++)
            {
                GameObject g = new Sprite("#particle", Position, $@"C\Chars\Red\Blood\{arts.RandomIndex()}");
                g.Scale = (float)Extensions.R.NextDouble();
                g.SetDeathTimer(Extensions.R.Next(1000,2000));
                g.DeltaMovement = new Position(_delta.X + Extensions.R.Next(-_shuff, _shuff), _delta.Y + Extensions.R.Next(-_shuff, _shuff));
            }
        }

        public override void Update(TimeSpan dt)
        {
            foreach(GameObject g in _particles)
            {
                g.Position.X += g.DeltaMovement.X;
                g.Position.Y += g.DeltaMovement.Y + (int)_acdg;
                _acdg += Program.Game.Gravity;
                g.Update(dt);
            }
        }

        public override void Draw(TimeSpan dt)
        {
            base.Draw(dt);
            foreach (GameObject g in _particles)
            {
                g.Draw(dt);
            }
        }
    }
}
