using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    class YellowPlayer : TestPlayer
    {
        private bool _poweredUp = false;

        private bool _ability1Active = false;
        private int _ability1State = 0;
        private Direction _ability1Direction = Direction.Left;
        private int _ability1Damage = 100;
        private List<TestPlayer> _ability1Hits = new List<TestPlayer>(); 
        private int _ability1StunDuration = 1000;

        private bool _ability2Active = false;
        private int _ability2State = 0;
        private long _ability2ChargeStartedTime = 0;
        private long _ability2ChargeEndedTime = 0;
        private int _ability2ChargeTimeMultiplyer = 1;
        private long _ability2ChargeTime = 0;
        private int _ability2TravelDistance = 50;
        private int _ability2TravelStartX = 0;
        private int _ability2Damage = 100;
        private int _ability2BonusDamage = 200;
        private int _ability2TraveledDistance = 0;
        private int _ability2DustDelta = 50;
        private long _ability2LastDust = 0;
        private Direction _ability2Direction = Direction.Left;

        private bool _ability3Active = false;
        private long _ability3LastCall = 0;
        private int _ability3Duration = 200;
        private int _ability3State = 0;

        private Sprite _stunPunch;
        private Sprite _chargePunch;
        private Sprite _chargePunch2;
        private Sprite _chargePunch3;
        private Sprite _chargePunch4;
        private Sprite _chargedPunch;

        private Animation _stunPunchAnimation = new Animation("#stunpch1",new[] { @"C\Chars\Yellow\StunPunch\s_1_0", @"C\Chars\Yellow\StunPunch\s_1_1", @"C\Chars\Yellow\StunPunch\s_1_2", @"C\Chars\Yellow\StunPunch\s_1_3" }, 50);
        private Animation _chargeBallAnimation = new Animation("#chargeBall1", new[] { @"C\Chars\Yellow\ChargePunch\pc_0_0", @"C\Chars\Yellow\ChargePunch\pc_0_1" }, 300);
        private Animation _chargeIntoAnimation = new Animation("#chargeIn0to1", new[] { $@"C\Chars\Yellow\ChargePunch\pt_0_0", $@"C\Chars\Yellow\ChargePunch\pt_0_1", $@"C\Chars\Yellow\ChargePunch\pt_0_2" }, 50);
        private Animation _chargeDurAnimation = new Animation("#chargeDur1", new[] { $@"C\Chars\Yellow\ChargePunch\pt_1", $@"C\Chars\Yellow\ChargePunch\pt_2", $@"C\Chars\Yellow\ChargePunch\pt_3", $@"C\Chars\Yellow\ChargePunch\pt_4" }, 50);
        
        public YellowPlayer(string name, Position pos, string texture) : base(name, pos, texture)
        {
        }

        public override void Ability1()
        {
            if(!_ability1Active)
            {
                _ability1Active = true;
                _stunPunch = new Sprite("#sunpch",Position, @"C\Chars\Yellow\StunPunch\s_0_1");
                _stunPunch.AutoFitCollider = true;
                _stunPunch.FollowCamera = true;
                _ability1Direction = FacingDirection;
                ActionLocked = true;
                FlipLocked = true;
                _stunPunch.WrapInCollider(false);
                if(FacingDirection==Direction.Left)
                {
                    _stunPunch.FlipHorizontally = true;
                }
                Program.Game.Objects.Add(_stunPunch);
                _stunPunch.SingleAnimation(_stunPunchAnimation);
            }
        }

        public override void Ability2()
        {
            if(!_ability2Active)
            {
                _ability2Active = true;
                _ability2State = 0;
                _ability2ChargeStartedTime = Extensions.GetMS();
                _chargePunch = new Sprite("#chargepart",Position, @"C\Chars\Yellow\ChargePunch\pc_0_1");
                _chargePunch.FollowCamera = true;
                _chargePunch.Scale = 0.8f;
                _chargePunch.LoopAnimation(_chargeBallAnimation);
                Program.Game.Objects.Add(_chargePunch);
                ActionLocked = true;
                FlipLocked = true;
                _ability2Direction = FacingDirection;
            }
        }

        public override void Ability3()
        {
            if(!_ability3Active)
            {
                ActionLocked = true;
                FlipLocked = true;
                _ability3Active = true;
                _ability3State = 0;
                _ability3LastCall = Extensions.GetMS();
                // start counterattack prep animation
            }
        }
        
        public override void Ultimate()
        {

        }

        public override void Shoot()
        {

        }

        public override void Update(TimeSpan dt, List<Direction> limiters, ControllerState controller)
        {
            base.Update(dt, limiters, controller);

            bool canMoveDown = !limiters.Contains(Direction.Down);
            bool canMoveUp = !limiters.Contains(Direction.Up);
            bool canMoveLeft = !limiters.Contains(Direction.Right);
            bool canMoveRight = !limiters.Contains(Direction.Left);

            long ms = Extensions.GetMS();

            if (_ability1Active)
            {
                FlipHorizontally = _stunPunch.FlipHorizontally;
                _stunPunch.Position = Position;
                List<TestPlayer> tl = Program.Game.TouchingPlayers(_stunPunch);
                if(tl.Count>0)
                {
                    foreach (TestPlayer t in tl)
                    {
                        if (t != this && !_ability1Hits.Contains(t))
                        {
                            _ability1Hits.Add(t);
                            t.Stun(_ability1StunDuration);
                            t.Health -= _ability1Damage;
                        }
                    }
                }
                if (_stunPunch.Animation.name== "#stunpch1"&&_stunPunch.AnimationFinished)
                {
                    Program.Game.Objects.Remove(_stunPunch);
                    _stunPunch = null;
                    _ability1Active = false;
                    FlipLocked = false;
                    ActionLocked = false;
                }
                else if(_stunPunch != null && _stunPunch.Animation.name == "#stunpch1"&& Program.Game.InsideWall(this) == null)
                {
                    Position.X -= _ability1Direction == Direction.Right ? 1 : -1;
                }
                if (_stunPunch!=null && _stunPunch.Animation.name == "#stunpch1" && Program.Game.InsideWall(this) != null)
                {
                    Position.X += _ability1Direction == Direction.Right ? 1 : -1;
                }
            }

            if(_ability2Active)
            {
                if(controller.WasButtonReleased(Microsoft.Xna.Framework.Input.Buttons.X)&&_ability2State==0)
                {
                    _ability2ChargeEndedTime = ms;
                    _ability2State = 1;
                    _ability2ChargeTime = _ability2ChargeEndedTime - _ability2ChargeStartedTime;
                }
                switch (_ability2State)
                {
                    case 0:
                        long diff = ms - _ability2ChargeStartedTime;
                        _chargePunch.Position = Position;
                        if (diff>2500)
                        {
                            _chargePunch.Scale = 1f;
                            _chargePunch.Animation.FrameRate = 150;
                            _chargePunch.Orientation += 3.0f;
                            break;
                        }
                        if(diff>1000)
                        {
                            _chargePunch.Scale = 0.8f;
                            _chargePunch.Animation.FrameRate = 200;
                            _chargePunch.Orientation += 1.2f;
                            break;
                        }
                        if(diff>500)
                        {
                            _chargePunch.Scale = 0.6f;
                            _chargePunch.Animation.FrameRate = 250;
                            _chargePunch.Orientation += 0.6f;
                            break;
                        }
                        if(diff>0)
                        {
                            _chargePunch.Scale = 0.4f;
                            _chargePunch.Animation.FrameRate = 300;
                            _chargePunch.Orientation += 0.1f;
                            break;
                        }
                        break;
                    case 1:
                        _ability2ChargeTimeMultiplyer = _ability2ChargeTime > 2500 ? 4 : _ability2ChargeTime > 1000 ? 3 : _ability2ChargeTime > 500 ? 2 : 1;
                        _ability2State = 2;
                        _chargedPunch = new Sprite("#chpsprite", Position, $@"C\Chars\Yellow\ChargePunch\pt_0_0");
                        _chargedPunch.FollowCamera = true;
                        _chargedPunch.Scale = 1;
                        _chargedPunch.FlipHorizontally = FlipHorizontally;
                        Program.Game.Objects.Add(_chargedPunch);
                        _chargedPunch.SingleAnimation(_chargeIntoAnimation);
                        break;

                    case 2:
                        Program.Game.Objects.Remove(_chargePunch);
                        if (_chargedPunch.Animation.name == "#chargeInto1" && _chargedPunch.AnimationFinished)
                        {
                            _chargedPunch.LoopAnimation(_chargeDurAnimation);
                            _ability2State = 3;
                        }
                        else if (_chargedPunch.Animation.name == "#chargeInto1" && !_chargedPunch.AnimationFinished && (canMoveRight && _ability2Direction == Direction.Right) || (canMoveLeft && _ability2Direction == Direction.Left)) // was CanMove(_ability2Direction, 3)
                        {
                            Position.X += _ability2Direction == Direction.Right ? 3 : -3;
                            if(!canMoveDown&&ms-_ability2LastDust>_ability2DustDelta)
                            {
                                Dust(Position.X, Position.Y - 10);
                                _ability2LastDust = ms;
                            }
                            _chargedPunch.Position = Position;
                        }
                        break;

                    case 3:
                        if ((canMoveRight && _ability2Direction == Direction.Right) || (canMoveLeft && _ability2Direction == Direction.Left)) // was inside wall check 
                        {
                            Position.X += _ability2Direction == Direction.Right ? 10 : -10;
                            _chargedPunch.Position = Position;
                        } else
                        {
                            Position.X -= _ability2Direction == Direction.Right ? 10 : -10;
                            _chargedPunch.Position = Position;
                            _ability2State = 4;
                        }

                        break;
                    case 4:
                        _ability2Active = false;
                        _ability2State = -1;
                        Program.Game.Objects.Remove(_chargedPunch);
                        _chargedPunch = null;
                        ActionLocked = false;
                        FlipLocked = false;
                        _ability2TraveledDistance = 0;
                        break;
                }
            }

            if(_ability3Active)
            {
                switch(_ability3State)
                {
                    case 0:
                        if (ms - _ability3LastCall > _ability3Duration)
                        {
                            _ability3State = 0;
                            _ability3Active = false;
                            FlipLocked = false;
                            ActionLocked = false;
                        }

                        //if projectile hits during counter attack state
                        if(false)
                        {
                            //sparkle and counterattack animation
                        }
                        break;
                }
            }
        }

        private void Dust(int x, int y)
        {
            ScaleDownFadeOut s = new ScaleDownFadeOut("#particledust", new Position(x, y), $@"C\Chars\Yellow\dust_{Extensions.R.Next(0, 4)}");
            s.ScaleDownSpeed = -0.1f;
            Program.Game.Objects.Add(s);
        }
    }
}
