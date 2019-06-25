using RTS1.MonoGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    class EightDirectionNode
    {
        public EightDirectionNode Up = null;
        public EightDirectionNode UpRight = null;
        public EightDirectionNode Right = null;
        public EightDirectionNode DownRight = null;
        public EightDirectionNode Down = null;
        public EightDirectionNode DownLeft = null;
        public EightDirectionNode Left = null;
        public EightDirectionNode UpLeft = null;
        public string Name = "";
        public EightDirectionNode(string name)
        {
            Name = name;
        }
        public void Set(EightDirectionNode up, EightDirectionNode upright, EightDirectionNode right, EightDirectionNode downright, EightDirectionNode down, EightDirectionNode downleft, EightDirectionNode left, EightDirectionNode upleft)
        {
            Up = up;
            UpRight = upright;
            Right = right;
            DownRight = downright;
            Down = down;
            DownLeft = downleft;
            Left = left;
            UpLeft = upleft;
        }
        public EightDirectionNode Get(Direction direction)
        {
            switch(direction)
            {
                case Direction.Up:
                    return Up;
                case Direction.UpRight:
                    return UpRight;
                case Direction.Right:
                    return Right;
                case Direction.DownRight:
                    return DownRight;
                case Direction.Down:
                    return Down;
                case Direction.DownLeft:
                    return DownLeft;
                case Direction.Left:
                    return Left;
                case Direction.UpLeft:
                    return UpLeft;
                case Direction.None:
                    return this;
            }
            return null;
        }
    }
}
