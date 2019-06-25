using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    class Projectile : Sprite
    {
        public PlayerIndex Owner;
        public int Damage = 100;
        public Direction Dir = Direction.Right;
        public bool DamageWalls = true;

        public int DX = 6;
        public int DY = 0;
        

        public Projectile(string name, Position position, string texture) : base(name, position, texture)
        {
        }

        public Projectile(string name, Position position, Animation animation) : base(name, position, animation)
        {
        }

        public Projectile(string name, int x, int y, string texture) : base(name, x, y, texture)
        {
        }

        public Projectile(string name, int x, int y, Animation animation) : base(name, x, y, animation)
        {
        }

        public Projectile(string name, Position position, string texture, Collision colision) : base(name, position, texture, colision)
        {
        }

        public Projectile(string name, Position position, Animation animation, Collision colision) : base(name, position, animation, colision)
        {
        }

        public Projectile(string name, int x, int y, string texture, Collision colision) : base(name, x, y, texture, colision)
        {
        }

        public Projectile(string name, int x, int y, Animation animation, Collision colision) : base(name, x, y, animation, colision)
        {
        }

        public void BulletHitWall(GameObject tile)
        {
            DirectionalCollisionObject d = (DirectionalCollisionObject)tile;
            if(DamageWalls)
            {
                d.DealDamage(Damage);
            }
            Enabled = false;
        }

        public void BulletHitPlayer(GameObject player)
        {
            TestPlayer p = (TestPlayer)player;
            p.Health -= Damage;
            if (p.Health<0)
            {
                p.Health = 0;
            }
            Enabled = false;
        }

        public override void Update(TimeSpan dt)
        {
            if(!Enabled)
            {
                return;
            }
            GameObject tile = Program.Game.InsideWall(this);
            GameObject player = Program.Game.TouchingPlayer(this);
            if (tile!=null)
            {
                BulletHitWall(tile);
            }
            if (player!=null&&($"{((TestPlayer)player).AssociatedPlayer}"!=$"{Owner}"))
            {
                BulletHitPlayer(player);
            }
            if(Enabled)
            {
                if(Dir==Direction.None)
                {
                    Position.X += DX;
                    Position.Y += DY;
                }
                if(Dir==Direction.Right)
                {
                    Position.X += DX;
                }
                if(Dir==Direction.Left)
                {
                    Position.X -= DX;
                }
            }
        }
    }
}
