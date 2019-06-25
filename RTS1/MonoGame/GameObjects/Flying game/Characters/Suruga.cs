using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects.Flying_game.Characters
{
    class Suruga : Flying_Character
    {
        public Suruga(int x, int y, string texture) : base("#SURUGA", x, y, texture)
        {
            RepeatA = 100;
            RepeatS = 1000;
            RepeatD = 5000;
        }

        public override void WeakAttack()
        {
            Console.WriteLine("Weak");
        }

        public override void StrongAttack()
        {
            Console.WriteLine("Strong");
        }

        public override void RangedAttack()
        {
            Console.WriteLine("Ranged");
        }

        public override void SpecialAttack()
        {
            Console.WriteLine("Special");
        }


    }
}
