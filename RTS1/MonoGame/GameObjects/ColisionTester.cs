using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    class ColisionTester : Sprite
    {
        public ColisionTester(string name, Position position, string texture) : base(name, position, texture)
        {
        }

        public ColisionTester(string name, Position position, Animation animation) : base(name, position, animation)
        {
        }

        public ColisionTester(string name, int x, int y, string texture) : base(name, x, y, texture)
        {
        }

        public ColisionTester(string name, int x, int y, Animation animation) : base(name, x, y, animation)
        {
        }

        public ColisionTester(string name, Position position, string texture, Collision colision) : base(name, position, texture, colision)
        {
        }

        public ColisionTester(string name, Position position, Animation animation, Collision colision) : base(name, position, animation, colision)
        {
        }

        public ColisionTester(string name, int x, int y, string texture, Collision colision) : base(name, x, y, texture, colision)
        {
        }

        public ColisionTester(string name, int x, int y, Animation animation, Collision colision) : base(name, x, y, animation, colision)
        {
        }
    }
}
