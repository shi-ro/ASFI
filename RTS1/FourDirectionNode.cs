using RTS1.MonoGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public class FourDirectionNode
    {
        public long LastUpdate = 0;
        public FourDirectionNode Up = null;
        public FourDirectionNode Down = null;
        public FourDirectionNode Left = null; 
        public FourDirectionNode Right = null;
        public string Name = "";
        public FourDirectionNode(string name)
        {
            Name = name;
        }
        public void Set(FourDirectionNode up, FourDirectionNode down, FourDirectionNode left, FourDirectionNode right)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }

        public KeyValuePair<FourDirectionNode, bool> MeaningfulGet(Direction direction)
        {
            FourDirectionNode ret = null;
            bool meaningful = false;
            switch (direction)
            {
                case Direction.Down:
                    ret = Down == null ? this : Down;
                    meaningful = Down != null;
                    break;
                case Direction.Up:
                    ret = Up == null ? this : Up;
                    meaningful = Up != null;
                    break;
                case Direction.Left:
                    ret = Left == null ? this : Left;
                    meaningful = Left != null;
                    break;
                case Direction.Right:
                    ret = Right == null ? this : Right;
                    meaningful = Right != null;
                    break;
                case Direction.None:
                    ret = this;
                    break;
            }
            return new KeyValuePair<FourDirectionNode, bool>(ret,meaningful);
        }

        public FourDirectionNode Get(Direction direction)
        {
            return MeaningfulGet(direction).Key;
        }
    }
}
