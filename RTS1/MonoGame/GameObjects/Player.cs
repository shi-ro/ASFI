using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    class Player : GameObject
    {
        public Position Position; 
        public PlayerData Data;

        public Player(PlayerData data, int x, int y)
        {
            Data = data;
        }

        public override void Update(TimeSpan dt)
        {
            // Do nothing
        }
    }
}
