using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    class Button : GameObject
    {
        // Up >> Hover >> Down >> Hover >> Up (what happens)

        private Func<Button,int> _enterDown = EnterDown;
        private Func<Button, int> _enterUp = EnterUp;
        private Func<Button, int> _enterHover = EnterHover;
        
        public bool Clicked = false;

        public Button(string name, int x, int y, string texture) : base(name, new Position(x, y), texture) { }
        public Button(string name, int x, int y, Animation animation) : base(name, new Position(x, y), animation) { }
        public Button(string name, Position position, string texture) : base(name, position, texture) { }
        public Button(string name, Position position, Animation animation) : base(name, position, animation) { }

        public GameButtonState State = GameButtonState.Up;

        public void EnteringHover()
        {
            _enterHover(this);
        }
        public void EnteringUp()
        {
            _enterUp(this);
        }
        public void EnteringDown()
        {
            _enterDown(this);
        }

        private static int EnterHover(Button b)
        {
            b.Scale = 0.95f;
            return 0;
        }
        private static int EnterUp(Button b)
        {
            b.Scale = 1.0f;
            return 0;
        }
        private static int EnterDown(Button b)
        {
            b.Scale = 0.8f;
            return 0;
        }

        public override void Update(TimeSpan dt)
        {
            MouseState mouse = Program.Mouse;
            Position mpos = new Position(mouse.X, mouse.Y);
            if (Contains(mpos) && State == GameButtonState.Up) 
            {
                State = GameButtonState.Hover;
                Clicked = false;
                EnteringHover();
            }
            if (Contains(mpos) && mouse.LeftButton == ButtonState.Pressed && State == GameButtonState.Hover) 
            {
                State = GameButtonState.Down;
                Clicked = false;
                EnteringDown();
            }
            if (Contains(mpos) && mouse.LeftButton == ButtonState.Released && State == GameButtonState.Down) 
            {
                State = GameButtonState.Hover;
                Clicked = true;
                EnteringHover();
            }
            if (!Contains(mpos) && State != GameButtonState.Up)
            {
                State = GameButtonState.Up;
                Clicked = false;
                EnteringUp();
            }
        }
    }
}
