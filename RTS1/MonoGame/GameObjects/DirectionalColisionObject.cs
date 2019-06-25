using RTS1_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    public class DirectionalCollisionObject : GameObject
    {
        private int _internalBoxSize = 2;
        private int _boxStickout = 1;
        private int _health = DataLoader.LOW_HEALTH;
        private Sprite top;
        private Sprite bot;
        private Sprite lft;
        private Sprite rit;
        
        public DirectionalCollisionObject(string name, int x, int y, string texture) : base(name, new Position(x, y), texture) { Init(); }
        public DirectionalCollisionObject(string name, int x, int y, Animation animation) : base(name, new Position(x, y), animation) { Init(); }
        public DirectionalCollisionObject(string name, Position position, string texture) : base(name, position, texture) { Init(); }
        public DirectionalCollisionObject(string name, Position position, Animation animation) : base(name, position, animation) { Init(); }
        public DirectionalCollisionObject(string name, int x, int y, string texture, Collision colision) : base(name, new Position(x, y), texture, colision) { Init(); }
        public DirectionalCollisionObject(string name, int x, int y, Animation animation, Collision colision) : base(name, new Position(x, y), animation, colision) { Init(); }
        public DirectionalCollisionObject(string name, Position position, string texture, Collision colision) : base(name, position, texture, colision) { Init(); }
        public DirectionalCollisionObject(string name, Position position, Animation animation, Collision colision) : base(name, position, animation, colision) { Init(); }

        private void Init()
        {
            FollowCamera = true;
            int w = Width;
            int h = Height;
            int pw = w / 2;
            int ph = h / 2;
            int x = Position.X;
            int y = Position.Y;
            top = new Sprite("#drawnCollider", new Position(x, y - ph - _boxStickout),@"C\Tiles\red");
            bot = new Sprite("#drawnCollider", new Position(x, y + ph + _boxStickout), @"C\Tiles\green");
            lft = new Sprite("#drawnCollider", new Position(x + pw + _boxStickout, y), @"C\Tiles\blue");
            rit = new Sprite("#drawnCollider", new Position(x - pw - _boxStickout, y), @"C\Tiles\yellow");
            top.Offset = new Position(0, 0 - ph - _boxStickout);
            bot.Offset = new Position(0, ph + _boxStickout);
            rit.Offset = new Position(0 - pw - _boxStickout, 0);
            lft.Offset = new Position(pw + _boxStickout, 0);
            top.Tag = "#topCollider";
            bot.Tag = "#botCollider";
            lft.Tag = "#lftCollider";
            rit.Tag = "#ritCollider";
            top.Parent = this;
            bot.Parent = this;
            rit.Parent = this;
            lft.Parent = this;
            top.FollowCamera = true;
            bot.FollowCamera = true;
            lft.FollowCamera = true;
            rit.FollowCamera = true;
            top.Width = w;
            top.Height = _internalBoxSize;
            top.Collision = new Collision(top.Width, top.Height);
            bot.Width = w;
            bot.Height = _internalBoxSize;
            bot.Collision = new Collision(bot.Width, bot.Height);
            rit.Width = _internalBoxSize;
            rit.Height = h;
            rit.Collision = new Collision(rit.Width, rit.Height);
            lft.Width = _internalBoxSize;
            lft.Height = h;
            lft.Collision = new Collision(lft.Width, lft.Height);
            top.ChildScaleMode = 1;
            bot.ChildScaleMode = 1;
            rit.ChildScaleMode = 2;
            lft.ChildScaleMode = 2;
            //top.FixColliderOffset();
            //bot.FixColliderOffset();
            //lft.FixColliderOffset();
            //rit.FixColliderOffset();
            top.ColliderOffset.X -= pw;
            bot.ColliderOffset.X -= pw;
            bot.ColliderOffset.Y -= 1;
            lft.ColliderOffset.Y -= ph;
            rit.ColliderOffset.Y -= ph;
            AdjustHealthToTile();
        }
        
        private void AdjustHealthToTile()
        {
            try
            {
                string name = Name.Split('|')[1].Split('\\')[2];
                switch (name)
                {
                    case "spike":
                        _health = DataLoader.MEDIUM_HEALTH;
                        break;
                    case "rock1":
                        _health = DataLoader.MEDIUM_HEALTH;
                        break;
                    case "spk3":
                        _health = DataLoader.MEDIUM_HEALTH;
                        break;
                }
            } catch { }
            
        }

        public void AddToObjects()
        {
            Program.Game.Objects.Add(top);
            Program.Game.Objects.Add(lft);
            Program.Game.Objects.Add(bot);
            Program.Game.Objects.Add(rit);
        }

        public void DealDamage(int a)
        {
            _health -= a;
            //spawn block hit animation
            if(_health<0)
            {
                //spawn block death animation
                Enabled = false;
                top.Enabled = false;
                bot.Enabled = false;
                lft.Enabled = false;
                rit.Enabled = false;
            }
        }

        public override void Update(TimeSpan dt)
        {
            // Do nothing
        }
    }
}
