using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects.Testing
{
    class SimpleMovement : GameObject
    {
        private int _start = 0;
        public SimpleMovement(int x, int y, string texture) : base("#simplemovement",new Position(x, y), texture)
        {
            this.Scale = 0.1f;
            this.UseBlur = true;
        }
        public override void Update(TimeSpan dt)
        {
            if(Program.Keyboard.IsKeyDown(Keys.Enter))
            {
                this.Width += 300;
                this.Height -= 100;
            }
            if(Program.Keyboard.IsKeyDown(Keys.RightShift))
            {
                this.Position.X -= 1;
            }
        }
    }
}
