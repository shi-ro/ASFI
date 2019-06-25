using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public enum GameButtonState
    {
        Down = 0,
        Up = 1,
        Hover = 2
    }
    public enum Screen
    {
        Intro = 0,
        Menu = 1,
        CharachterSelect = 2,
        Battle = 3,
        EndGame = 4,
    }
    public enum CollisionType
    {
        None = -1,
        Circle = 0,
        Square = 1
    }
}
