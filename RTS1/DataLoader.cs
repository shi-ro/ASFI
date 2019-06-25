using Microsoft.Xna.Framework.Graphics;
using Monogame.Extended.MonogameTilesetImporter;
using MonoGame.Extended.MonogameMapImporter;
using RTS1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1_Data
{
    public static class DataLoader
    {
        public const int LOW_HEALTH = 100;
        public const int MEDIUM_HEALTH = 500;
        public const int HIGH_HEALTH = 2000;
        public const int TOP_HEALTH = Int32.MaxValue;

        private static List<Animation> ANIMATIONS = new List<Animation>();
        private static Dictionary<string, Texture2D> TEXTURES = new Dictionary<string, Texture2D>();
        private static Dictionary<string, TileMap> TILEMAPS = new Dictionary<string, TileMap>();
        private static Dictionary<string, TilemapData> TILEMAPDATAS = new Dictionary<string, TilemapData>();
        private static Dictionary<string, TilesetData> TILESETDATAS = new Dictionary<string, TilesetData>();
        private static Dictionary<string, PlayerData> PLAYERDATAS = new Dictionary<string, PlayerData>();

        public static void Initialize()
        {
            InitAnimations();
        }

        private static void InitAnimations()
        {
            
        }
        private static void InitPlayerDatas()
        {

        }
        
        public static Texture2D AddTexture(string name)
        {
            if(TEXTURES.ContainsKey(name))
            {
                return TEXTURES[name];
            }
            Texture2D load = Program.Content.Load<Texture2D>(name);
            TEXTURES.Add(name, load);
            return load;
        }
        public static Texture2D GetTexture(string name)
        {
            return TEXTURES[name];
        }

        public static TilemapData AddTilemapData(string name)
        {
            if(TILEMAPDATAS.ContainsKey(name))
            {
                return TILEMAPDATAS[name];
            }
            TilemapData load = Program.Content.Load<TilemapData>(name);
            TILEMAPDATAS.Add(name, load);
            return load;
        }
        public static TilemapData GetTilemapData(string name)
        {
            return TILEMAPDATAS[name];
        }

        public static TilesetData AddTilesetData(string name)
        {
            if(TILESETDATAS.ContainsKey(name))
            {
                return TILESETDATAS[name];
            }
            TilesetData load = Program.Content.Load<TilesetData>(name);
            TILESETDATAS.Add(name, load);
            return load;
        }
        public static TilesetData GetTilesetData(string name)
        {
            return TILESETDATAS[name];
        }

        public static TileMap AddTilemap(string data, string setting)
        {
            return AddTilemap(AddTilemapData(data),AddTilesetData(setting));
        }
        public static TileMap AddTilemap(TilemapData data, TilesetData setting)
        {
            if(TILEMAPS.ContainsKey(data.Name))
            {
                return TILEMAPS[data.Name];
            }
            string[,] tiles = new string[data.Map.GetLength(0), data.Map.GetLength(1)];
            for (int y = 0; y < data.Map.GetLength(0); y++)
            {
                for(int x = 0; x < data.Map.GetLength(1); x++)
                {
                    if(setting.Tiles.ContainsKey(data.Map[y, x]))
                    {
                        tiles[y, x] = $@"C\Tiles\{setting.Tiles[data.Map[y, x]]}";
                    } else
                    {
                        tiles[y, x] = @"C\Tiles\empty";
                    }
                }
            }
            TileMap ret = new TileMap(data.Name, tiles);
            TILEMAPS.Add(data.Name, ret);
            return ret;
        }
        public static TileMap GetTilemap(string name)
        {
            return TILEMAPS[name];
        }

        public static PlayerData AddPlayerData(string name, int speed, int health, Dictionary<string,string[]> animations)
        {
            if(PLAYERDATAS.ContainsKey(name))
            {
                return PLAYERDATAS[name];
            }
            PlayerData load = new PlayerData(name, speed, health, animations);
            PLAYERDATAS.Add(name,load);
            return load;
        }
        public static PlayerData GetPlayerData(string name)
        {
            return PLAYERDATAS[name];
        }

        public static Animation GetAnimation(string name)
        {
            foreach(Animation a in ANIMATIONS)
            {
                if(a.name==name)
                {
                    return a.CopyOf();
                }
            }
            return null;
        }
        public static void CreateAnimation(string name, string[] frames, int frameRate)
        {
            ANIMATIONS.Add(new Animation(name, frames, frameRate));
        }
    }
}
