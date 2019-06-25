using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public class Position
    {
        public int X;
        public int Y;
        public Position(int X,int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Position(Position pos, Position off)
        {
            X = pos.X + off.X;
            Y = pos.Y + off.Y;
        }

        public Position(Position pos)
        {
            X = pos.X;
            Y = pos.Y;
        }

        public Position CameraPerspective()
        {
            int cax = Program.Game.Camera.X;
            int cay = Program.Game.Camera.Y;
            return new Position(X + cax, Y + cay);
        }

        public Position Average(Position other)
        {
            return new Position((other.X + X) / 2, (other.Y + Y) / 2);
        }

        public double DistanceTo(Position other)
        {
            int dx = Math.Abs(other.X - X);
            int dy = Math.Abs(other.Y - Y);
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
