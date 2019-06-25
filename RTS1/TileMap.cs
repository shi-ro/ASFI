using Microsoft.Xna.Framework.Graphics;
using RTS1.MonoGame.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public class TileMap : GameObject
    {
        public int TileSize = 20;
        private bool first = false;
        public DirectionalCollisionObject[,] array = new DirectionalCollisionObject[1,1];
        
        public TileMap(string name, int w, int h, string[] objects, int initialHeight)
        {
            Tag = "#map";
            Name = name;
            int[,] map = NoiseMap.GetNotSmoothedNoise(w, h, initialHeight+1);
            map = map.MeanBlur(5);
            map = map.ChangeVariance(objects.Length-1);
            array = new DirectionalCollisionObject[h, w];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    array[y, x] = new DirectionalCollisionObject($"#|{map[y,x]}|WALLtile@{y}-{x}", new Position(x * TileSize, y * TileSize), objects[map[y, x]]);
                    array[y, x].AutoFitCollider = true;
                    array[y, x].WrapInCollider(true);
                }
            }
        }

        public TileMap(string name, string[,] array) : base()
        {
            Tag = "#map";
            Name = name;
            this.array = new DirectionalCollisionObject[array.GetLength(0), array.GetLength(1)];
            for(int y = 0; y < array.GetLength(0); y++)
            {
                for(int x = 0; x < array.GetLength(1); x++)
                {
                    this.array[y, x] = new DirectionalCollisionObject($"#WALL|{array[y, x]}|WALLtile@{y}-{x}", new Position(x * TileSize, y * TileSize), array[y, x]);
                    this.array[y, x].Tag = array[y, x];
                    this.array[y, x].AutoFitCollider = true;
                    this.array[y, x].WrapInCollider(true);
                }
            }
        }

        public void AddSubCollidersToObjectsList()
        {
            for (int y = 0; y < array.GetLength(0); y++)
            {
                for (int x = 0; x < array.GetLength(1); x++)
                {
                    if(!array[y, x].Tag.EndsWith(@"empty"))
                    {
                        array[y, x].AddToObjects();
                    }
                }
            }
        }

        public override void Update(TimeSpan dt)
        {
            
            for (int y = 0; y < array.GetLength(0); y++)
            {
                for (int x = 0; x < array.GetLength(1); x++)
                {
                    var cur = array[y, x];
                    if(!cur.Enabled)
                    {
                        DirectionalCollisionObject d = new DirectionalCollisionObject($"DeadTile", new Position(x * TileSize, y * TileSize), @"C\Tiles\empty");
                        d.Tag = "empty";
                        array[y, x] = d;
                    }
                    if (Scale != 1)
                    {
                        cur.Position = new Position((int)(x * TileSize * Scale), (int)(y * TileSize * Scale));
                        cur.Scale = Scale;
                    }
                }
            }
        }

        public override void Draw(TimeSpan dt)
        {
            for (int y = 0; y < array.GetLength(0); y++)
            {
                for (int x = 0; x < array.GetLength(1); x++)
                {
                    GameObject cur = array[y, x];
                    cur.Draw(dt);
                }
            }
        }

        public void Draw(TimeSpan dt, Position p)
        {
            int ax = p.X;
            int ay = p.Y;
            for (int y = 0; y < array.GetLength(0); y++)
            {
                for (int x = 0; x < array.GetLength(1); x++)
                {
                    GameObject cur = array[y, x];
                    Position prev = cur.Position;
                    cur.Orientation = Orientation;
                    cur.Position = new Position(ax + cur.Position.X, ay + cur.Position.Y);
                    cur.Draw(dt);
                    cur.Position = prev;
                }
            }
        }
    }
}
