using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.GameObjects
{
    class RedPlayer : TestPlayer
    {
        private bool _trueTransform = false;
        private int _maliciousIntent = 1;
        private Vector2 _rightStickDelta = new Vector2(0, 0);

        private bool _ability1Active = false;
        private int _ability1State = -1;
        private Position _ability1StartPosition = null;
        private List<TestPlayer> _ability1Hits = new List<TestPlayer>();

        private bool _ability2Active = false;
        private TestPlayer _ability2Target = null;

        private bool _ability3Active = false;
        private TestPlayer _ability3Target = null;
        private int _ability3ShotInterval = 100;
        private int _ability3Shots = 5;
        private int _ability3ShotsFired = 0;
        private long _ability3LastShot = 0;
        private Direction _ability3Direction = Direction.Left;

        private int _transformState = -1;
        private long _initialTransform = 0;
        private long _secondTransform = 0;
        private long _thirdTranform = 0;

        private int _transformState0ChannelTime = 2000;
        private int _transformState1ChannelTime = 4000;
        private int _transformState2BaseChannelTime = 30000;
        private int _transformState2ChannelTime = 30000;

        private long _transformLastBatShot = 0;
        private int _transformBatsShot = 7;
        private int _transformBatShotDelta = 500;
        private int _transformBatShotDamage = 10;
        private int _transformBatMinSpeed = 5;
        private int _transformBatMaxSpeed = 15;

        private Texture save;

        private Sprite _darkBall;
        private Sprite _chargeWave;
        private Sprite _darkWave;
        private Sprite _darkOverlay;
        private Sprite _returnWave;
        private Sprite _slashLine;
        private Sprite _knifeAttack = null;

        private bool _knife2Attack = false;

        //private Animation _knifeMovement = new Animation("#knifeMovement", new[] { @"C\Chars\Red\Slash\s_1_0", @"C\Chars\Red\Slash\s_3_0", @"C\Chars\Red\Slash\s_3_1"}, 50);
        private Animation _knifeAnimation = new Animation("#sepcialKnifeAnimation", new[] { @"C\Chars\Red\Slash\s_4_0", @"C\Chars\Red\Slash\s_4_1", @"C\Chars\Red\Slash\s_4_2", @"C\Chars\Red\Slash\s_4_2" }, 50);
        private Animation _knifeAnimation2 = new Animation("#sepcialKnifeAnimation2", new[] { @"C\Chars\Red\Slash\s_6_0", @"C\Chars\Red\Slash\s_6_1", @"C\Chars\Red\Slash\s_6_2", @"C\Chars\Red\Slash\s_6_2" }, 50);
        private Animation _prepareForNotTrueSlash = new Animation("#prepareNtDash", new[] { @"C\Chars\Red\Slash\s_1_0", @"C\Chars\Red\Slash\s_1_1", @"C\Chars\Red\Slash\s_1_2", @"C\Chars\Red\Slash\s_1_3", @"C\Chars\Red\Slash\s_1_4", }, 50);
        private Animation _trueAnimation = new Animation("#bloodlord", new[] { @"C\Chars\Red\t_0_0", @"C\Chars\Red\t_0_1", @"C\Chars\Red\t_0_2" }, 100);
        private Animation _intoBlobAnimation = new Animation("#toBlob", new[] { @"C\Chars\Red\Reform\r_0_0", @"C\Chars\Red\Reform\r_0_1", @"C\Chars\Red\Reform\r_0_2", @"C\Chars\Red\Reform\r_0_3", @"C\Chars\Red\Reform\r_0_4" }, 100);
        private Animation _outofBlobAnimation = new Animation("#fromBlob", new[] { @"C\Chars\Red\Reform\r_0_4", @"C\Chars\Red\Reform\r_0_3", @"C\Chars\Red\Reform\r_0_2", @"C\Chars\Red\Reform\r_0_1", @"C\Chars\Red\Reform\r_0_0" }, 100);

        private float _darkWaveInitialSize = 0.1f;
        private float _darkWaveExpansionRate = 0.5f;

        public RedPlayer(string name, Position pos, string texture) : base(name, pos, texture)
        {
            _maliciousIntent = 1;
            _trueTransform = false;
        } 

        public override void Death()
        {
            base.Death();
            _maliciousIntent = 1;
            _trueTransform = false;
            _transformState = -1;
        }

        public override void Ability1()
        {
            if(!_ability1Active)
            {
                _ability1State = 0;
                _ability1Active = true;
                ActionLocked = true;
                _ability1Hits.Clear();
                if(!_trueTransform)
                {
                    SingleAnimation(_prepareForNotTrueSlash);
                }
                Flying = true;
            }
        }
        
        public override void Ability2()
        {
            TestPlayer s = Program.Game.ClocestCharachterInRange(this, 200);
            if (!_ability2Active&&s!=null)
            {
                _ability2Target = s;
                CutInAnimate(_intoBlobAnimation);
                _ability2Active = true;
                ActionLocked = true;
                Flying = true;
            }
        }

        public override void Ability3()
        {
            if (!_ability3Active)
            {
                _ability3ShotsFired = 0;
                _ability3Active = true;
                _ability3LastShot = Extensions.GetMS();
                _ability3Direction = FacingDirection;
                FlipLocked = true;
            }
        }

        public override void Ultimate()
        {
            if(_transformState==-1)
            {
                _transformState = 0;
                _initialTransform = Extensions.GetMS();
                ActionLocked = true;
                Flying = true;
                save = Texture;
                Program.BlurOrigin = Position;
                Program.BlurStrength = 1;
                Program.Blur = true;
                LoopAnimation(_trueAnimation);

                _darkBall = new Sprite("#chargeElement", Position, @"C\Chars\Red\b_0");
                _darkBall.Scale = 0.1f;
                _darkBall.FollowCamera = true;
                _darkBall.LoopAnimation(new Animation("#darkBall", new[] { @"C\Chars\Red\b_0", @"C\Chars\Red\b_1", @"C\Chars\Red\b_2" }, 100));
                _chargeWave = new Sprite("#chargesprite", Position, @"C\Chars\Red\True\chargewave");
                _chargeWave.Scale = 1.5f;
                _chargeWave.FollowCamera = true;
                Program.Game.Objects.Add(_darkBall);
                Program.Game.Ownerships.Add(new OwnershipObject(_darkBall, "RedPlayer"));
                Program.Game.Objects.Add(_chargeWave);
                Program.Game.Ownerships.Add(new OwnershipObject(_chargeWave, "RedPlayer"));
            }
        }

        public override void Shoot()
        {
            if (_trueTransform)
            {
                Projectile p = new Projectile("#pewpew", new Position(Position.X, Position.Y), $@"C\Chars\Red\True\b_{Extensions.R.Next(0,6)}");
                p.Dir = Direction.None;
                p.Owner = AssociatedPlayer;
                p.AutoFitCollider = true;
                p.WrapInCollider(false);
                p.FollowCamera = true;
                p.SetDeathTimer(8000);
                p.Owner = AssociatedPlayer;
                if (_rightStickDelta.X != 0)
                {
                    p.DX = (int)(_rightStickDelta.X*10);
                } else
                {
                    p.DX = FacingDirection == Direction.Right ? 10 : -10;
                }
                if(_rightStickDelta.Y != 0)
                {
                    p.DY = (int)(_rightStickDelta.Y*10);
                }
                Program.Game.Objects.Add(p);
            }
            else
            {
                if(_knifeAttack==null)
                {
                    _knifeAttack = new Sprite("#knifeAttack", Position, @"C\Chars\Red\Slash\s_0_0");
                    if (_knife2Attack)
                    {
                        LoadTexture(@"C\Chars\Red\Slash\s_3_3");
                    } else
                    {
                        LoadTexture(@"C\Chars\Red\Slash\s_3_1");
                    }
                    _knifeAttack.FollowCamera = true;
                    _knifeAttack.Height = 25;
                    _knifeAttack.Width = 25;
                    
                    Projectile p = new Projectile("#pewpew", new Position(Position.X+(FacingDirection==Direction.Left?-10:10), Position.Y+0), $@"C\Chars\Red\Slash\{(_knife2Attack ? "s_5_2" : "s_5_1")}");
                    p.Dir = FacingDirection;
                    p.Owner = AssociatedPlayer;
                    p.AutoFitCollider = true;
                    p.WrapInCollider(false);
                    p.FollowCamera = true;
                    p.SetDeathTimer(1000);
                    p.DX = _knife2Attack ? 5 : 3;
                    p.Owner = AssociatedPlayer;
                    Program.Game.Objects.Add(p);
                    if (FacingDirection == Direction.Left)
                    {
                        p.FlipHorizontally = true;
                        _knifeAttack.FlipHorizontally = true;
                    }
                    else
                    {
                        p.FlipHorizontally = false;
                        _knifeAttack.FlipHorizontally = false;
                    }
                    if (_knife2Attack)
                    {
                        _knifeAttack.SingleAnimation(_knifeAnimation2);
                    }
                    else
                    {
                        _knifeAttack.SingleAnimation(_knifeAnimation);
                    }
                    _knife2Attack = !_knife2Attack;
                    Program.Game.Objects.Add(_knifeAttack);
                }
            }
        }

        private void SlashHit(Position pos, int num, int shake)
        {
            if(num<=1)
            {
                SlashEffect sf = new SlashEffect(Extensions.Shake(pos.X, shake), Extensions.Shake(pos.Y, shake), Extensions.R.Next(0, 359), (float)((Extensions.R.NextDouble() * 2) + 1), $@"C\Chars\Red\Slash\ss_0_0");
                sf.Scale = 1;
                sf.FollowCamera = true;
                Program.Game.Objects.Add(sf);
                return;
            }
            for(int i = 0; i < Extensions.R.Next(1,num+2); i++)
            {
                SlashEffect sf = new SlashEffect(Extensions.Shake(pos.X, shake), Extensions.Shake(pos.Y, shake), Extensions.R.Next(0, 359),(float)((Extensions.R.NextDouble() * 2) + 1), $@"C\Chars\Red\Slash\ss_0_0");
                sf.Scale = 1;
                sf.FollowCamera = true;
                Program.Game.Objects.Add(sf);
            }
        }

        private void Blood(Position pos, Direction dir)
        {
            int ldx = dir == Direction.Right ? 1 : -5;
            int rdx = dir == Direction.Left ? 1 : 5;
            for (int i = 0; i < Extensions.R.Next(10, 100); i++)
            {
                GravityParticle g = new GravityParticle("#part", new Position(Extensions.Shake(pos.X, 5), Extensions.Shake(pos.Y, 5)), $@"C\Chars\Red\Blood\b_{Extensions.R.Next(0, 5)}");
                g.Scale = (float)((Extensions.R.NextDouble() + 0.2) / 2.0f);
                g.Dx = Extensions.R.Next(ldx, rdx);
                g.Dy = Extensions.R.Next(-3, 1);
                g.SetDeathTimer(3000);
                g.FollowCamera = true;
                Program.Game.Objects.Add(g);
            }
        }

        public override void Update(TimeSpan dt, List<Direction> limiters, ControllerState controller)
        {
            base.Update(dt, limiters, controller);

            long ms = Extensions.GetMS();

            _rightStickDelta = controller.RightThumbstickDelta;
            
            if(_knifeAttack!=null)
            {
                FlipHorizontally = _knifeAttack.FlipHorizontally;
                _knifeAttack.Position = Position;
                if(_knifeAttack.Animation.name.Contains("#sepcialKnifeAnimation") && _knifeAttack.AnimationFinished)
                {
                    Program.Game.Objects.Remove(_knifeAttack);
                    
                    LoadTexture(@"C\Chars\Red\w_0_1");
                    _knifeAttack = null;
                }
            }

            if(_ability1Active)
            {
                switch(_ability1State)
                {
                    case 0:
                        if (Animation.name == "#prepareNtDash" && AnimationFinished)
                        {
                            Flying = true;
                            Animation = null;
                            IsAnimated = false;
                            AnimationFinished = true;
                            _ability1State = 1;
                            LoadTexture(@"C\Chars\Red\Slash\s_1_4");
                            _slashLine = new Sprite("#slashline", new Position(Position), $@"C\Chars\Red\Slash\s_2_3");
                            _slashLine.Width = 0;
                            _slashLine.Height = 10;
                            _slashLine.AutoFitCollider = true;
                            _slashLine.WrapInCollider(false);
                            _slashLine.FollowCamera = true;
                            Program.Game.Objects.Add(_slashLine);
                            _ability1StartPosition = new Position(Position);
                        }
                        break;
                    case 1:
                        List<TestPlayer> tl = Program.Game.TouchingPlayers(_slashLine);
                        //TestPlayer t = (TestPlayer)Program.Game.TouchingPlayer(_slashLine);
                        if (tl.Count>0)
                        {   
                            foreach(TestPlayer t in tl)
                            {
                                if(t!=this&& !_ability1Hits.Contains(t))
                                {
                                    _ability1Hits.Add(t);
                                    SlashHit(new Position(t.Position), 1, 0);
                                    Blood(new Position(t.Position), FacingDirection == Direction.Left ? Direction.Right : Direction.Left);
                                    t.Health -= 200;
                                }
                            }
                        }
                        if(FacingDirection==Direction.Right)
                        {
                            _slashLine.FlipHorizontally = false;
                            _slashLine.Width = Math.Abs(-_ability1StartPosition.X + Position.X + 20);
                            _slashLine.AutoFitCollider = true;
                            _slashLine.WrapInCollider(false);
                            Position.X += 20;
                        } else
                        {
                            _slashLine.FlipHorizontally = true;
                            _slashLine.Width = Math.Abs(_ability1StartPosition.X - Position.X - 20);
                            _slashLine.AutoFitCollider = true;
                            _slashLine.WrapInCollider(false);
                            Position.X -= 20;
                        }
                        if (Program.Game.InsideWall(this) != null)
                        {
                            Position.X += FacingDirection == Direction.Right ? -20 : 20;
                            _ability1State = 2;
                        }
                        _slashLine.Position = new Position(Extensions.AveragePosition(_ability1StartPosition, Position));
                        if (Math.Abs(Position.X - _ability1StartPosition.X) > 100)
                        {
                            _ability1State = 2;
                        }
                        break;
                    case 2:
                        _slashLine.Height -= 2;
                        if(_slashLine.Height<=0)
                        {
                            _ability1State = -1;
                            _ability1Active = false;
                            ActionLocked = false;
                            if(!_trueTransform)
                            {
                                Flying = false;
                            }
                            if (_trueTransform)
                            {
                                LoopAnimation(_trueAnimation);
                            }
                            else
                            {
                                LoadTexture(@"C\Chars\Red\w_0_1");
                            }
                            Program.Game.Objects.Remove(_slashLine);
                            _slashLine = null;
                            _ability1Hits.Clear();
                        }
                        break;
                }
            }

            if(_ability2Active)
            {
                if(Animation.name=="#toBlob"&&Animation.CurrentFrame==4)
                {
                    Position = new Position(_ability2Target.Position);
                    Animation = null;
                    IsAnimated = false;
                    AnimationFinished = true;
                    CutInAnimate(_outofBlobAnimation);
                }
                if(Animation.name=="#fromBlob"&&Animation.CurrentFrame==4)
                {
                    Animation = null;
                    IsAnimated = false;
                    AnimationFinished = true;
                    if (_trueTransform)
                    {
                        LoopAnimation(_trueAnimation);
                    } else
                    {
                        LoadTexture(@"C\Chars\Red\w_0_1");
                    }
                    ActionLocked = false;
                    if (!_trueTransform)
                    {
                        Flying = false;
                    }
                    _ability2Active = false;
                }
            }

            if(_ability3Active)
            {
                if(ms-_ability3LastShot>_ability3ShotInterval)
                {
                    Projectile p = new Projectile("#pewpew", new Position(Position.X + (FacingDirection == Direction.Left ? -10 : 10), Position.Y + 0), $@"C\Chars\Red\CursedDagger\d_0_0");
                    p.Dir = _ability3Direction;
                    p.Owner = AssociatedPlayer;
                    if(_ability3Direction == Direction.Left)
                    {
                        p.FlipHorizontally = true;
                    }
                    p.Scale = 0.7f;
                    p.AutoFitCollider = true;
                    p.WrapInCollider(false);
                    p.FollowCamera = true;
                    p.SetDeathTimer(8000);
                    p.DX = 10;
                    p.Owner = AssociatedPlayer;
                    Program.Game.Objects.Add(p);
                    _ability3ShotsFired += 1;
                    _ability3LastShot = ms;
                }
                if(_ability3ShotsFired>=_ability3Shots)
                {
                    _ability3Active = false;
                    FlipLocked = false;
                }
            }

            switch(_transformState)
            {
                case 0:       
                    _darkBall.Scale += 0.01f;
                    _chargeWave.Scale -= 0.01f;
                    Program.BlurStrength += 0.1f;
                    if(_chargeWave.Scale<0.01f)
                    {
                        _chargeWave.Scale = 0.01f;
                    }

                    if (ms-_initialTransform>_transformState0ChannelTime)
                    {
                        _transformState = 1;
                        _darkWave = new Sprite("#damagedarkwave", Position, @"C\Chars\Red\True\burstwave");
                        _darkWave.FollowCamera = true;
                        Program.Game.Overlay.Add(_darkWave);
                        Program.Game.Objects.Remove(_chargeWave);
                        _darkOverlay = null;
                        _darkWave.Scale = _darkWaveInitialSize;
                        _transformLastBatShot = ms;
                        _secondTransform = ms;
                    }
                    break;
                case 1:

                    // fire darkwave, and bats
                    if(_darkWave!=null)
                    {
                        Program.BlurStrength -= 1f;
                        if(Program.BlurStrength<0)
                        {
                            Program.BlurStrength = 0;
                            Program.Blur = false;
                        }
                        _darkWave.Scale += _darkWaveExpansionRate;
                        if (_darkWave.Scale>14)
                        {
                            _darkOverlay = new Sprite("#darkoverlay", new Position(400, 300), @"C\Chars\Red\True\overlay");
                            Program.Game.Overlay.Add(_darkOverlay);
                            Program.Game.Overlay.Remove(_darkWave);
                            Program.Game.Objects.Remove(_darkBall);
                            _darkWave = null;
                            Program.Blur = false;
                        }
                    }
                    if(ms-_secondTransform>_transformState1ChannelTime)
                    {
                        _transformState = 2;
                        _secondTransform = ms;
                        _transformState2ChannelTime = _transformState2BaseChannelTime * _maliciousIntent;
                        _maliciousIntent = 1;
                        _trueTransform = true;
                        ActionLocked = false;
                    }
                    if(ms-_transformLastBatShot>_transformBatShotDelta)
                    {
                        for(float i = 0; i < 2*Math.PI; i += (float)(2*Math.PI)/(_transformBatsShot))
                        {
                            Projectile p = new Projectile("#pewpew", new Position(Position.X+(int)(Math.Cos(i)*15), Position.Y+ (int)(Math.Sin(i) * 15)), $@"C\Chars\Red\Bats\b_{Extensions.R.Next(0, 2)}_{Extensions.R.Next(0, 5)}");
                            p.Damage = _transformBatShotDamage;
                            p.Dir = Direction.None;
                            p.Owner = AssociatedPlayer;
                            p.AutoFitCollider = true;
                            p.WrapInCollider(false);
                            p.FollowCamera = true;
                            p.SetDeathTimer(8000);
                            p.Scale = 2;
                            p.Owner = AssociatedPlayer;
                            double mult = Extensions.R.Next(_transformBatMinSpeed, _transformBatMaxSpeed);
                            p.DX = (int)(Math.Cos(i)*mult);
                            p.DY = (int)(Math.Sin(i)*mult);
                            p.Orientation = (float)(i*180.0f/Math.PI);
                            Program.Game.Objects.Add(p);
                        }
                        _transformLastBatShot = ms;
                    }
                    break;
                case 2:
                    if(ms-_secondTransform>_transformState2ChannelTime)
                    {
                        ActionLocked = true;
                        _returnWave = new Sprite("#returnwave", new Position(Position.X, Position.Y), @"C\Chars\Red\True\reversewave");
                        _returnWave.Scale = 14;
                        _returnWave.FollowCamera = true;
                        Program.Game.Overlay.Add(_returnWave);
                        Program.Game.Overlay.Remove(_darkOverlay);
                        _transformState = 3;
                    }
                    break;
                case 3:
                    if(_returnWave!=null)
                    {
                        _returnWave.Scale -= _darkWaveExpansionRate;
                        if(_returnWave.Scale<0)
                        {
                            Program.Game.Overlay.Remove(_returnWave);
                            ActionLocked = false;
                            Flying = false;
                            _maliciousIntent = 1;
                            _transformState = -1;
                            _trueTransform = false;
                            Animation = null;
                            IsAnimated = false;
                            AnimationFinished = true;
                            LoadTexture(@"C\Chars\Red\w_0_1");
                        }
                    }
                    break;
            }

        }
    }
}
