using Microsoft.Xna.Framework.Graphics;
using RTS1_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    class WaveObject : GameObject
    {
        private double _waveMove = 0;
        private int _waveWidth = 0;
        private double _waveSpeed = 0;
        private int _waveHeight = 0;
        private double _waveDelta = 0;
        private Texture _singleTex;
        private int _waveNum = 0;
        private List<Sprite> _waves = new List<Sprite>();

        public bool WaveColisionEnabled = false;
        
        public WaveObject(string name, Position position, int waves, int width, int height, double movementSpeed) : base(name, position, @"C\Tiles\empty")
        {
            _waveNum = waves;
            _waveDelta = (double)width / waves;
            _waveSpeed = movementSpeed;
            _waveHeight = height;
            _waveWidth = width;
        }

        public void CreateWave(string waveTexture, double offset = 0)
        {
            _waves.Clear();
            double cx = 0;
            _waveMove += offset;
            for (int i = 0; i < _waveNum; i ++)
            {
                Sprite wave = new Sprite("#wavenorm", new Position((int)cx + Position.X, (int)(Math.Sin((cx+_waveMove)*5.0f)*5.0f*_waveHeight)+ Position.Y),waveTexture);
                cx += _waveDelta;
                if(WaveColisionEnabled)
                {
                }
                _waves.Add(wave);
            }
        }

        public void CreateWave(string[] textures, double offset = 0)
        {
            _waves.Clear();
            double cx = 0;
            _waveMove += offset;
            for (int i = 0; i < textures.Length; i++)
            {
                Sprite wave = new Sprite("#wavespec", new Position((int)cx + Position.X, (int)(Math.Sin((cx + _waveMove) * 5.0f) * 5.0f * _waveHeight) + Position.Y), textures[i]);
                cx += _waveDelta;
                if (WaveColisionEnabled)
                {
                }
                _waves.Add(wave);
            }
        }

        public override void Draw(TimeSpan dt)
        {
            foreach(Sprite s in _waves)
            {
                s.Draw(dt);
            }
        }

        public override void Update(TimeSpan dt)
        {
            double cx = 0;
            _waveMove += _waveSpeed;
            for(int i = 0; i < _waves.Count(); i ++)
            {
                Sprite cur = _waves[i];
                cur.Position = new Position((int)cx + Position.X, (int)(Math.Sin((cx + _waveMove) * 5.0f) * 5.0f * _waveHeight) + Position.Y);
                cx += _waveDelta;
            }
        }
    }
}
