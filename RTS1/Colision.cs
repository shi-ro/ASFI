using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public class Collision
    {
        public CollisionType colisionType = CollisionType.None;
        public int Width;
        public int Height;
        public int radius;
        public Collision(int W, int H)
        {
            Width = W;
            Height = H;
            colisionType = CollisionType.Square;
        }
        public Collision(int R)
        {
            radius = R;
            colisionType = CollisionType.Circle;
        }
    }
}
