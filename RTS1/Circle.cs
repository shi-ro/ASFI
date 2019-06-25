using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public class Circle
    {
        public Position position;
        public int R;
        public Circle(int x, int y, int R)
        {
            this.R = R;
            position = new Position(x, y);
        }

        public bool Contains(int x, int y)
        {
            int dx = Math.Abs(position.X - x);
            int dy = Math.Abs(position.Y - y);
            if (R + 10 < dy) { return false; }
            if (R + 10 < dx) { return false; }
            return R >= Math.Sqrt(dx * dx + dy * dy);
        }

        public bool Intersects(Circle circle)
        {
            int dis = R + circle.R;
            int dx = Math.Abs(position.X - circle.position.X);
            int dy = Math.Abs(position.Y - circle.position.Y);
            if (dis > dy + 10) { return false; }
            if (dis > dx + 10) { return false; }
            double tot = Math.Sqrt(dx * dx + dy * dy);
            return dis <= tot;
        }

        public bool Intersects(Rectangle rect)
        {
            int cdX = Math.Abs(position.X - rect.X);
            int cdY = Math.Abs(position.Y - rect.Y);

            if (cdX > (rect.Width / 2 + R)) { return false; }
            if (cdY > (rect.Height / 2 + R)) { return false; }

            if (cdX <= (rect.Width / 2)) { return true; }
            if (cdY <= (rect.Height / 2)) { return true; }

            double cdSq = ((cdX - rect.Width / 2)* (cdX - rect.Width / 2)) + ((cdY - rect.Height / 2)* (cdY - rect.Height / 2));

            return (cdSq <= (R * R));
        }
    }
}
