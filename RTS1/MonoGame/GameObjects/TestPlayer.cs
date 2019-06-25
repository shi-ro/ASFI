using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace RTS1.MonoGame.GameObjects
{
    public class TestPlayer : Sprite
    {
        public Character Character;
        
        // status booleans
        public bool Flying = false;
        public bool Stunned = false;
        public bool Confused = false;
        public bool Dead = false;
        public bool ActionLocked = false;
        public bool FlipLocked = false;

        public HealthBar HealthBar;

        public int Health = 1000;
        public int MaxHealth = 1000;

        public int Mana = 5;
        public double UltimatePercent = 1.0;
        public int Speed = 4;
        public int JumpStrength = 5;
        public int ShotInterval = 100;

        public int Ability1Cost = 0;
        public int Ability2Cost = 0;
        public int Ability3Cost = 0;

        public int Ability1Cooldown = 0;
        public int Ability2Cooldown = 0;
        public int Ability3Cooldown = 0;

        public Direction FacingDirection = Direction.Right;
        public ControllerState C;
        public PlayerIndex AssociatedPlayer;
        public List<Direction> Limiters = new List<Direction>();
        //public List<Projectile> Bullets = new List<Projectile>();
        public bool Jumping = false;
        public bool MovementAnimated = false;

        public Animation WalkAnimation;
        public Animation JumpAnimation;
        public Animation FallAnimation;
        public Animation IdleAnimation;

        private Movement _currentMovement = Movement.Idle;
        private Movement _lastMovement = Movement.Idle;

        private double _gravityAcceleration = 0;
        private Position _previousPosition;
        private long _lastShotTime = 0;
        private long _lastVibrateTime = 0;
        private long _lastVibrateStarted = 0;
        private long _lastStunTime = 0;
        private int _stunDuration = 0;
        private long _lastConfuseTime = 0;
        private int _confuseDuration = 0;

        private long _lastManaGain = 0;
        private int _manaGainDelay = 1000;

        private bool _vibratingHealth = false;

        private Direction _l = Direction.Left;
        private Direction _r = Direction.Right;
        private Direction _u = Direction.Up;
        private Direction _d = Direction.Down;
        private Direction _lu = Direction.UpLeft;
        private Direction _ld = Direction.DownLeft;
        private Direction _ru = Direction.UpRight;
        private Direction _rd = Direction.DownRight;
        private Direction _n = Direction.None;

        
        public TestPlayer(string name, Position position, string texture) : base(name, position, texture)
        {
        }
        public TestPlayer(string name, Position position, Animation animation) : base(name, position, animation)
        {
        }
        public TestPlayer(string name, int x, int y, string texture) : base(name, x, y, texture)
        {
        }
        public TestPlayer(string name, int x, int y, Animation animation) : base(name, x, y, animation)
        {
        }
        public TestPlayer(string name, Position position, string texture, Collision colision) : base(name, position, texture, colision)
        {
        }
        public TestPlayer(string name, Position position, Animation animation, Collision colision) : base(name, position, animation, colision)
        {
        }
        public TestPlayer(string name, int x, int y, string texture, Collision colision) : base(name, x, y, texture, colision)
        {
        }
        public TestPlayer(string name, int x, int y, Animation animation, Collision colision) : base(name, x, y, animation, colision)
        {
        }

        public void Initialize()
        {
            _previousPosition = Position;

            Name = $"#TRUEPLAYER#{Name}";

            HealthBar = new HealthBar($"#{AssociatedPlayer}_HeathBar", new Position(Position), @"C\UI\CircularHealth");
            HealthBar.Parent = this;

            HealthBar.LoadHealthBars(Health);

            AutoFitCollider = true;
            WrapInCollider(true);
            FollowCamera = true;
        }

        public bool CanMove(Direction d, int di)
        {
            bool r = false;
            int sx = Position.X;
            int sy = Position.Y;
            Position.X += d == _r || d == _rd || d == _ru ? di : _l == d || _ld == d || _lu == d ? -di : 0;
            Position.Y += d == _u || d == _ru || d == _lu ? -di : _d == d || _ld == d || _rd == d ? di : 0;
            if(Program.Game.InsideWall(this)!=null)
            {
                r = true;
            }
            Position.X = sx;
            Position.Y = sy;
            return r;

        }

        public virtual void Update(TimeSpan dt, List<Direction> limiters, ControllerState controller)
        {
            Limiters = limiters;
            C = controller;

            bool canMoveDown = !limiters.Contains(Direction.Down);
            bool canMoveUp = !limiters.Contains(Direction.Up);
            bool canMoveLeft = !limiters.Contains(Direction.Right);
            bool canMoveRight = !limiters.Contains(Direction.Left);
            
            bool cs1 = false;
            bool cs2 = false;
            bool cs3 = false;
            bool cs4 = false;

            if(Confused)
            {
                cs1 = Extensions.RandomBool();
                cs2 = Extensions.RandomBool();
                cs3 = Extensions.RandomBool();
                cs4 = Extensions.RandomBool();
            }

            _currentMovement = Movement.Idle;

            HealthBar.Value = Health;

            #region Vibrating Health
            if(_vibratingHealth)
            {
                if (Extensions.GetMS() - _lastVibrateTime > Health + 200)
                {
                    _lastVibrateTime = Extensions.GetMS();
                    _lastVibrateStarted = Extensions.GetMS();
                    controller.StartVibrate(0.7f, 0.7f);
                }
                if (Extensions.GetMS() - _lastVibrateStarted > 100)
                {
                    controller.StopVibrate();
                }
            }
            #endregion

            #region Satus Updates
            if (Stunned && Extensions.GetMS() - _lastStunTime > _stunDuration)
            {
                Stunned = false;
            }
            if (Confused && Extensions.GetMS() - _lastConfuseTime > _confuseDuration)
            {
                Confused = false;
            }
            if(Extensions.GetMS()-_lastManaGain>_manaGainDelay)
            {
                if(Mana<5)
                {
                    Mana += 1;
                }
            }
            #endregion

            #region Physics
            if (canMoveDown&&!Flying&&!Dead)
            {
                Position.Y += (int)_gravityAcceleration;
                if (Program.Game.InsideWall(this) != null)
                {
                    Position.Y -= (int)_gravityAcceleration;
                    _gravityAcceleration = Program.Game.Gravity * 5;
                    Jumping = false;
                }
                _gravityAcceleration += Program.Game.Gravity;
                _currentMovement = _gravityAcceleration > JumpStrength ? Movement.Fall : Movement.Jump;
            } else
            {
                _gravityAcceleration = 0;
                Jumping = false;
            }
            #endregion

            #region Controller Input
            
            if (controller.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.LeftShoulder))
            {
                Health -= 10;
                if(Health<0)
                {
                    Health = 0;
                }
            } else
            {
                if (Health>MaxHealth)
                {
                    Health = MaxHealth;
                }
            }

            if(!ActionLocked)
            {
                if ((controller.WasButtonPressed(Microsoft.Xna.Framework.Input.Buttons.A)) && !Stunned && !Dead)
                {
                    if (!Jumping)
                    {
                        Jumping = true;
                    }
                }
                if (Jumping && !Flying && !Dead)
                {
                    if (canMoveUp)
                    {
                        Position.Y -= JumpStrength;
                        if (Program.Game.InsideWall(this) != null)
                        {
                            Position.Y += JumpStrength;
                            Jumping = false;
                        }
                    }
                    else
                    {
                        Jumping = false;
                    }
                }

                if (($"{C.LeftThumbstickDirection}".Contains("Right") || cs1) && canMoveRight && !Stunned && !Dead)
                {
                    Position.X += Speed;
                    FacingDirection = Direction.Right;
                    if(!FlipLocked)
                    {
                        FlipHorizontally = false;
                    }
                    if(!canMoveDown)
                    {
                        _currentMovement = Movement.Walk;
                    }
                }
                if (($"{C.LeftThumbstickDirection}".Contains("Left") || cs2) && canMoveLeft && !Stunned && !Dead)
                {
                    Position.X -= Speed;
                    FacingDirection = Direction.Left;
                    if(!FlipLocked)
                    {
                        FlipHorizontally = true;
                    }
                    if (!canMoveDown)
                    {
                        _currentMovement = Movement.Walk;
                    }
                }
                if (Flying)
                {
                    if ($"{C.LeftThumbstickDirection}".Contains("Up") && canMoveUp && !Stunned && !Dead)
                    {
                        Position.Y -= Speed;
                    }
                    if ($"{C.LeftThumbstickDirection}".Contains("Down") && canMoveDown && !Stunned && !Dead)
                    {
                        Position.Y += Speed;
                    }
                }

                if (controller.WasButtonPressed(Microsoft.Xna.Framework.Input.Buttons.B) && !Stunned && !Dead && Mana >= Ability1Cost)
                {
                    Mana -= Ability1Cost;
                    Ability1();
                }

                if (controller.WasButtonPressed(Microsoft.Xna.Framework.Input.Buttons.X) && !Stunned && !Dead && Mana >= Ability2Cost)
                {
                    Mana -= Ability2Cost;
                    Ability2();
                }

                if (controller.WasButtonPressed(Microsoft.Xna.Framework.Input.Buttons.Y) && !Stunned && !Dead && Mana >= Ability3Cost)
                {
                    Mana -= Ability3Cost;
                    Ability3();
                }

                if (controller.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.LeftShoulder) && controller.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.RightShoulder) && UltimatePercent == 1.0 && !Stunned && !Dead)
                {
                    UltimatePercent = 1.0;
                    Ultimate();
                }

                if (controller.RightTriggerPower > 0 && !Stunned && !Dead)
                {
                    var tms = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    if (tms - _lastShotTime > ShotInterval)
                    {
                        Shoot();
                        _lastShotTime = tms;
                    }
                }
            }

            #endregion

            #region Movement Animation

            if (MovementAnimated)
            {
                if (_lastMovement != _currentMovement)
                {
                    //switch to current movement animation
                    switch (_currentMovement)
                    {
                        case Movement.Idle:
                            HardSetAnimation(IdleAnimation);
                            break;
                        case Movement.Fall:
                            HardSetAnimation(FallAnimation);
                            break;
                        case Movement.Jump:
                            HardSetAnimation(JumpAnimation);
                            break;
                        case Movement.Walk:
                            HardSetAnimation(WalkAnimation);
                            break;
                    }
                }
            }

            #endregion

            _previousPosition = Position;
            _lastMovement = _currentMovement;
        }

        public void HardSetAnimation(Animation anim)
        {
            if (anim == null) { return; }
            Animation = null;
            IsAnimated = false;
            AnimationFinished = true;
            LoopAnimation(anim);
        }

        public virtual void Death()
        {

        }

        public virtual void Ability1()
        {

        }

        public virtual void Ability2()
        {

        }

        public virtual void Ability3()
        {

        }

        public virtual void Ultimate()
        {

        }

        public void Stun(int duration)
        {
            if(Stunned)
            {
                Stunned = true;
                _stunDuration += duration;
            } else
            {
                _stunDuration = duration;
                _lastStunTime = Extensions.GetMS();
                Stunned = true;
            }
        }

        public void Confuse(int duration)
        {
            if(Confused)
            {
                Confused = true;
                _confuseDuration += duration;
            } else
            {
                _stunDuration = duration;
                _lastConfuseTime = Extensions.GetMS();
                Confused = true;
            }
        }
        
        public virtual void Shoot()
        {
            Projectile p = new Projectile("#pewpew", new Position(Position.X, Position.Y), @"C\Chars\Red\Slash\h_0_5");
            p.Dir = FacingDirection;
            p.Owner = AssociatedPlayer;
            p.AutoFitCollider = true;
            p.WrapInCollider(false);
            p.FollowCamera = true;
            p.SetDeathTimer(8000);
            p.Owner = AssociatedPlayer;
            Program.Game.Objects.Add(p);
        }
    }
}
