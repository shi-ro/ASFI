using Microsoft.Xna.Framework.Input;
using RTS1.MonoGame.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects.Flying_game
{
    public abstract class Flying_Character : GameObject
    {
        private double _x = 0.0;
        private double _y = 0.0;
        private int _speed = 3;
        private double _turn = 10;
        private KeyboardState _prev;
        private KeyAge _up = new KeyAge(Keys.Up, TimeSpan.Zero);
        private KeyAge _down = new KeyAge(Keys.Down, TimeSpan.Zero);
        private KeyAge _left = new KeyAge(Keys.Left, TimeSpan.Zero);
        private KeyAge _right = new KeyAge(Keys.Right, TimeSpan.Zero);
        private KeyAge _a = null;
        private KeyAge _s = null;
        private KeyAge _d = null;
        private KeyAge _f = null;
        private Direction _direction = Direction.None;
        private TimeSpan _pdt = TimeSpan.Zero;
        private double _degrees = 0;
        private double _rotation = 0;
        private List<GameObject> effects = new List<GameObject>();
        private double MaxHealth = 100;
        private double MaxMana1 = 100;
        private double MaxMana2 = 100;
        private double MaxSpecial = 100;

        public double Health = 0;
        public double Mana1 = 0;
        public double Mana2 = 0;
        public double Special = 0;
        public int RepeatA = -1;
        public int RepeatS = -1;
        public int RepeatD = -1;
        public int RepeatF = -1;
        public bool CanAct = true;
        public Flying_Character Target;

        public Flying_Character(string name, int x, int y, string texture) : base(name, new Position(x, y), texture)
        {
            _x = x;
            _y = y;
            Health = MaxHealth;
            Mana1 = MaxMana1;
            Mana2 = MaxMana2;
            Special = 0;
        }

        public static double AngleDifference(double angle1, double angle2)
        {
            double diff = (angle2 - angle1 + 180) % 360 - 180;
            return diff < -180 ? diff + 360 : diff;
        }

        public override void Draw(TimeSpan dt)
        {
            
            base.Draw(dt);
            if (effects.Count > 0)
            {
                foreach (GameObject o in effects)
                {
                    o.Draw(dt);
                }
            }
        }

        public abstract void WeakAttack();

        public abstract void RangedAttack();

        public abstract void StrongAttack();

        public abstract void SpecialAttack();

        public override void Update(TimeSpan dt)
        {
            List<GameObject> _todispose = new List<GameObject>();
            if(effects.Count>0)
            {
                foreach(GameObject o in effects)
                {
                    o.Update(dt);
                    if(o.Disposed())
                    {
                        _todispose.Add(o);
                    }
                }
            }
            foreach(GameObject o in _todispose)
            {
                effects.Remove(o);
            }
            _todispose.Clear();

            if (!CanAct)
            {
                _prev = Program.Keyboard;
                _pdt = Program.Gt.TotalGameTime;
                return;
            }

            // Weak Attack
            if (Program.Keyboard.IsKeyDown(Keys.A))
            {
               if(_a==null)
               {
                    _a = new KeyAge(Keys.A, Program.Gt.TotalGameTime);
                    WeakAttack();
                }
               else
               {
                    if(Program.Gt.TotalGameTime.TotalMilliseconds-_a.time>RepeatA)
                    {
                        WeakAttack();
                        _a.time = Program.Gt.TotalGameTime.TotalMilliseconds;
                    }
               }
            } else
            {
                _a = null;
            }

            // Ranged Attack
            if (Program.Keyboard.IsKeyDown(Keys.S))
            {
                if (_s == null)
                {
                    _s = new KeyAge(Keys.S, Program.Gt.TotalGameTime);
                    RangedAttack();
                }
                else
                {
                    if (Program.Gt.TotalGameTime.TotalMilliseconds - _s.time > RepeatS)
                    {
                        RangedAttack();
                        _s.time = Program.Gt.TotalGameTime.TotalMilliseconds;
                    }
                }
            }
            else
            {
                _s = null;
            }

            // Strong Attak
            if (Program.Keyboard.IsKeyDown(Keys.D))
            {
                if (_d == null)
                {
                    _d = new KeyAge(Keys.D, Program.Gt.TotalGameTime);
                    StrongAttack();
                }
                else
                {
                    if (Program.Gt.TotalGameTime.TotalMilliseconds - _d.time > RepeatD)
                    {
                        StrongAttack();
                        _d.time = Program.Gt.TotalGameTime.TotalMilliseconds;
                    }
                }
            }
            else
            {
                _d = null;
            }

            // Special Attack
            if (Program.Keyboard.IsKeyDown(Keys.F))
            {
                if (_f == null)
                {
                    _f = new KeyAge(Keys.F, Program.Gt.TotalGameTime);
                    SpecialAttack();
                }
            }
            else
            {
                _f = null;
            }

            // Player Movement
            if (Program.Keyboard.IsKeyDown(Keys.LeftShift))
            {
                _speed = 6;
            }
            else
            {
                _speed = 3;
            }

            if (!Program.Keyboard.IsKeyDown(Keys.Up)) { _up.time = 0; }
            if (!Program.Keyboard.IsKeyDown(Keys.Down)) { _down.time = 0; }
            if (!Program.Keyboard.IsKeyDown(Keys.Left)) { _left.time = 0; }
            if (!Program.Keyboard.IsKeyDown(Keys.Right)) { _right.time = 0; }

            if (_prev!=null)
            {
                if(!_prev.IsKeyDown(Keys.Up) && Program.Keyboard.IsKeyDown(Keys.Up)) { _up = new KeyAge(Keys.Up, _pdt); }
                if (!_prev.IsKeyDown(Keys.Down) && Program.Keyboard.IsKeyDown(Keys.Down)) { _down = new KeyAge(Keys.Down, _pdt); }
                if (!_prev.IsKeyDown(Keys.Left) && Program.Keyboard.IsKeyDown(Keys.Left)) { _left = new KeyAge(Keys.Left, _pdt); }
                if (!_prev.IsKeyDown(Keys.Right) && Program.Keyboard.IsKeyDown(Keys.Right)) { _right = new KeyAge(Keys.Right, _pdt); }
            }

            bool U = false;
            bool D = false;
            bool L = false;
            bool R = false;

            if (Program.Keyboard.IsKeyDown(Keys.Up) && _up.time > _down.time ) { U = true; }
            if (Program.Keyboard.IsKeyDown(Keys.Down) && _down.time > _up.time ) { D = true; }
            if (Program.Keyboard.IsKeyDown(Keys.Left) && _left.time > _right.time ) { L = true; }
            if (Program.Keyboard.IsKeyDown(Keys.Right) && _right.time> _left.time ) { R = true; }

            if (U && L) { _direction = Direction.UpLeft; }
            else if (U && R) { _direction = Direction.UpRight; }
            else if (D && R) { _direction = Direction.DownRight; }
            else if (D && L) { _direction = Direction.DownLeft; }
            else if (U) {  _direction = Direction.Up; }
            else if (D) { _direction = Direction.Down; }
            else if (L) {  _direction = Direction.Left; }
            else if (R) { _direction = Direction.Right; }

            if (!U && !D && !L && !R) { _degrees = 180; }

            //dd = 45;
            //c = _degrees > dd + 180 ? _degrees - 360 : _degrees;
            //if (_degrees < dd) { _degrees += _turn; }
            //if (_degrees >= dd) { _degrees -= _turn; }

            int dd = 0;
            switch (_direction)
            {
                case Direction.Up:
                    dd = 0;
                    break;
                case Direction.UpRight:
                    dd = 45;
                    break;
                case Direction.Right:
                    dd = 90;
                    break;
                case Direction.DownRight:
                    dd = 135;
                    break;
                case Direction.Down:
                    dd = 180;
                    break;
                case Direction.DownLeft:
                    dd = 225;
                    break;
                case Direction.Left:
                    dd = 270;
                    break;
                case Direction.UpLeft:
                    dd = 315;
                    break;
            }

            _degrees += AngleDifference(dd, _degrees + 180) > 0?-_turn:_turn;
            _rotation = _degrees + 180;
            if (!U && !D && !L && !R)
            {
                _rotation = 0;
            } else
            {
                double dy = Math.Cos(Math.PI * _rotation / 180.0);
                double dx = Math.Sin(Math.PI * _rotation / 180.0);
                _x += dx * _speed;
                _y -= dy * _speed;
                Position.X = (int)_x;
                Position.Y = (int)_y;
            }

            Orientation = (float)((_rotation) / 180 * Math.PI);

            _prev = Program.Keyboard;
            _pdt = Program.Gt.TotalGameTime;
        }
    }
}
