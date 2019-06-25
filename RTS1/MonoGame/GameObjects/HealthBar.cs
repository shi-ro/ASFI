using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    public class HealthBar : Sprite
    {
        public new GameObject Parent = null;
        public HealthBarType HealthBarType = HealthBarType.HalfCircle;

        public int Value = 0;
        public int MaxValue = 0;

        private double _followValue = 0;
        private Sprite _overlay = null;
        private Sprite _bar = null;
        private List<Sprite> _radial = new List<Sprite>();

        public HealthBar(string name, Position position, string texture) : base(name, position, texture){}
        public HealthBar(string name, Position position, Animation animation) : base(name, position, animation){}
        public HealthBar(string name, int x, int y, string texture) : base(name, x, y, texture){}
        public HealthBar(string name, int x, int y, Animation animation) : base(name, x, y, animation){}
        public HealthBar(string name, Position position, string texture, Collision colision) : base(name, position, texture, colision){}
        public HealthBar(string name, Position position, Animation animation, Collision colision) : base(name, position, animation, colision){}
        public HealthBar(string name, int x, int y, string texture, Collision colision) : base(name, x, y, texture, colision){}
        public HealthBar(string name, int x, int y, Animation animation, Collision colision) : base(name, x, y, animation, colision){}

        public void LoadHealthBars(int max)
        {
            FollowCamera = true;
            Value = max;
            MaxValue = max;
            _followValue = max;
            if(HealthBarType==HealthBarType.HalfCircle)
            {
                for (double i = -90; i < 90; i++)
                {
                    Sprite sp = new Sprite($"#healthBar_{Name}", new Position(Position), @"C\UI\CircularHealth");
                    sp.FollowCamera = true;
                    sp.Orientation = (float)(i*0.017453f);
                    _radial.Add(sp);
                }
            }
        }

        public override void Update(TimeSpan dt)
        {
            if(Value<_followValue)
            {
                _followValue -= MaxValue/300.0;
                if(_followValue<0)
                {
                    _followValue = 0;
                }
            }else
            {
                _followValue = Value;
            }
            double pc = (Value+0.0) / (MaxValue+0.0);
            double pc2 = (_followValue + 0.0) / (MaxValue + 0.0);
            double full = pc * 180;
            double follow = pc2 * 180;
            //Console.WriteLine(amt);
            if (HealthBarType==HealthBarType.HalfCircle)
            {
                //Position off = new Position(Parent.Width/2, Parent.Height/2);
                Position = new Position(Parent.Position);//,off);
                int i = 0;
                int con = 0;
                foreach (Sprite s in _radial)
                {
                    s.Position = Position;
                    s.Scale = Parent.Scale;
                    if (i>full)
                    {
                        s.LoadTexture($@"C\UI\CircularHealth3");
                        if (i>follow)
                        {
                            s.LoadTexture($@"C\UI\CircularHealth2");
                        }
                    } else
                    {
                        s.LoadTexture(@"C\UI\CircularHealth");
                    }
                    i++;
                }
            }
        }

        public override void Draw(TimeSpan dt)
        {
            foreach (Sprite s in _radial)
            {
                s.Draw(dt);
            }
        }
    }
    public enum HealthBarType
    {
        HalfCircle = 0,
        HorizontalBar = 1,
        VerticalBar = 2,
    }
}
