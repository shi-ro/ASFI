using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public class Sprite : GameObject
    {
        public GameObject Parent = null;
        public Position Offset = new Position(0,0);
        public int ChildScaleMode = 0;

        public Sprite(string name, int x, int y, string texture) : base(name, new Position(x,y), texture) { }
        public Sprite(string name, int x, int y, Animation animation) : base(name, new Position(x, y), animation) { }
        public Sprite(string name, Position position, string texture) : base(name, position, texture) { }
        public Sprite(string name, Position position, Animation animation) : base(name, position, animation) { }

        public Sprite(string name, int x, int y, string texture, Collision colision) : base(name, new Position(x, y), texture, colision) { }
        public Sprite(string name, int x, int y, Animation animation, Collision colision) : base(name, new Position(x, y), animation, colision) { }
        public Sprite(string name, Position position, string texture, Collision colision) : base(name, position, texture, colision) { }
        public Sprite(string name, Position position, Animation animation, Collision colision) : base(name, position, animation, colision) { }

        public override void Update(TimeSpan dt)
        {
            if(Parent!=null)
            {
                double w = Parent.Width * Parent.Scale;
                double h = Parent.Height * Parent.Scale;
                int ofx = (int)(Offset.X * Parent.Scale);
                int ofy = (int)(Offset.Y * Parent.Scale);
                //ofx -= ofx > 0 ? (int)(2 * Parent.Scale + cw) : ofx;
                //ofy -= ofy > 0 ? (int)(2 * Parent.Scale + ch) : ofy;
                //ofx += ofx < 0 ? (int)(2 * Parent.Scale + cw) : ofx;
                //ofy += ofy < 0 ? (int)(2 * Parent.Scale + ch) : ofy;
                ColliderOffset = new Position((int)(ColliderOffset.X * Parent.Scale), (int)(ColliderOffset.Y * Parent.Scale));
                Position = new Position(Parent.Position.X+ofx, Parent.Position.Y+ofy);
                switch (ChildScaleMode)
                {
                    case 1:
                        Width = (int)w;
                        Collision.Width = (int)w;
                        Height = (int)(2*Parent.Scale);
                        Collision.Height = (int)(2 * Parent.Scale);
                        break;
                    case 2:
                        Height = (int)h;
                        Collision.Height = (int)h;
                        Width = (int)(2 * Parent.Scale);
                        Collision.Width = (int)(2 * Parent.Scale);
                        break;
                    case 3:
                        Width = (int)w;
                        Collision.Width = (int)w;
                        Height = (int)h;
                        Collision.Height = (int)h;
                        break;
                }
            }
        }
    }
}
