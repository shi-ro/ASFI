using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    class FadeInFadeOut : GameObject
    {
        public bool ActionFinished = false;

        public int FadeInTime = 254;
        public int FadeStayTime = 46;
        public int FadeOutTime = 254;

        private int _subop = 0;

        public FadeInFadeOut(string name, int x, int y, string texture) : base(name, new Position(x, y), texture) { Opacity = 0; }
        public FadeInFadeOut(string name, int x, int y, Animation animation) : base(name, new Position(x, y), animation) { Opacity = 0; }
        public FadeInFadeOut(string name, Position position, string texture) : base(name, position, texture) { Opacity = 0; }
        public FadeInFadeOut(string name, Position position, Animation animation) : base(name, position, animation) { Opacity = 0; }

        public FadeInFadeOut(string name, int x, int y, string texture, Collision colision) : base(name, new Position(x, y), texture, colision) { Opacity = 0; }
        public FadeInFadeOut(string name, int x, int y, Animation animation, Collision colision) : base(name, new Position(x, y), animation, colision) { Opacity = 0; }
        public FadeInFadeOut(string name, Position position, string texture, Collision colision) : base(name, position, texture, colision) { Opacity = 0; }
        public FadeInFadeOut(string name, Position position, Animation animation, Collision colision) : base(name, position, animation, colision) { Opacity = 0; }

        public override void Update(TimeSpan dt)
        {
            if(!ActionFinished)
            {
                int delta = dt.Milliseconds / 10;
                if (_subop < 254)
                {
                    Opacity += delta;
                }
                if (_subop > 300)
                {
                    Opacity -= delta;
                }
                if (_subop > 554 || (_subop > 300 && Opacity<=0))
                {
                    ActionFinished = true;
                    Opacity = 0;
                }
                _subop += delta;
            }
        }
    }
}
