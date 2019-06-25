using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RTS1_Data;
using RTS1_Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS0618 // Type or member is obsolete

namespace RTS1
{
    public abstract class GameObject
    {
        //public float Layer = 0f;
        //public int Orientation = 0;
        public bool Enabled = true;
        public bool FollowCamera = false;
        public string Name = "#default";
        public string Tag = "";
        public Position Position;
        public float Layer = 0;
        public float Orientation = 0;
        public int Opacity = 255;
        public float Scale = 1f;
        public int Width = 0;
        public int Height = 0;
        public Texture2D Texture { get; set; }
        public bool IsAnimated { get; set; }
        public Collision Collision = null;
        public bool AutoFitCollider = false;
        public bool UseBlur;
        public Position ColliderOffset = new Position(0, 0);
        public Animation Animation;

        public bool FlipHorizontally = false;
        public bool DrawWithTint = false;
        public Color Tint = Color.White;
        
        private long _deathCallTime;
        private int _deathTime;
        private bool _deathTimerSet = false;
        private TimeSpan _lastUpdate;
        private Animation _stashedAnimation = null;
        private Texture2D _stashedFrame = null;
        private bool _disposed = false;
        private bool _disposable = true;

        private int LeftPadding = 2;
        private int RightPadding = 2;
        private int TopPadding = 2;
        private int BottomPadding = 2;
        
        public bool AnimationFinished = true;
        public AnimationType AnimationType = AnimationType.None;

        public GameObject() { }
        public GameObject(string name, Position position, string texture)
        {
            this.Name = name;
            this.Position = position;
            LoadTexture(texture);
            WrapInCollider();
        }
        public GameObject(string name, Position position, Animation animation)
        {
            this.Name = name;
            this.Position = position;
            Animation = animation;
            IsAnimated = true;
            AnimationType = AnimationType.Loop;
            WrapInCollider();
        }
        public GameObject(string name, Position position, string texture, Collision colision)
        {
            this.Name = name;
            this.Collision = colision;
            this.Position = position;
            LoadTexture(texture);
            WrapInCollider();
        }
        public GameObject(string name, Position position, Animation animation, Collision colision)
        {
            this.Name = name;
            this.Collision = colision;
            this.Position = position;
            Animation = animation;
            IsAnimated = true;
            AnimationType = AnimationType.Loop;
            WrapInCollider();

        }
        
        public void SetDeathTimer(int ms)
        {
            if(!_deathTimerSet)
            {
                _deathTimerSet = true;
                _deathCallTime = Extensions.GetMS();
                _deathTime = ms;
            }
        }

        public void WrapInCollider()
        {
            if(AutoFitCollider)
            {
                Collision = new Collision(Texture == null ? 20 : Texture.Width, Texture == null ? 20 : Texture.Height);
            }
        }

        public void WrapInCollider(bool fixOffset)
        {
            if (!AutoFitCollider) { return; }
            if (fixOffset)
            {
                ColliderOffset = new Position((int)(-Texture.Width / 2.0f), (int)(-Texture.Height / 2.0f));
            }
            WrapInCollider();
        }

        public void FixColliderOffset()
        {
            ColliderOffset = new Position((int)(-Texture.Width / 2.0f), (int)(-Texture.Height / 2.0f));
        }

        public void SetColor(Color target)
        {
            int dR = Color.Red.R - target.R;
            int dG = Color.Red.G - target.G;
            int dB = Color.Red.B - target.B;
            Color[] data = new Color[Texture.Width * Texture.Height];
            Texture.GetData(data);
            for(int i = 0; i < data.Length; i++)
            {
                Color cur = data[i];
                if (cur.R != cur.G || cur.G != cur.B || cur.R != cur.B)
                {
                    data[i] = new Color(cur.R + dR, cur.G + dG, cur.B + dB);
                } 
            }
            Texture.SetData(data);
        }

        public void CutInAnimate(Animation animation)
        {
            if (IsAnimated)
            {
                AnimationType = AnimationType.CutIn;
                _stashedAnimation = Animation;
                animation.Reset();
                Animation = animation;
                AnimationFinished = false;
            }
            else
            {
                SingleAnimation(animation);
            }
        }

        public void SingleAnimation(Animation animation)
        {
            AnimationType = AnimationType.Single;
            _stashedFrame = Texture;
            IsAnimated = true;
            animation.Reset();
            Animation = animation;
            AnimationFinished = false;
        }

        public void LoopAnimation(Animation animation)
        {
            IsAnimated = true;
            AnimationType = AnimationType.Loop;
            Animation = animation;
        }

        public void LoopAnimation(Animation animation, int loops)
        {
            AnimationType = AnimationType.LimitedLoop;
        }

        public abstract void Update(TimeSpan dt);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Position.ToVector2(), Color.White);
            spriteBatch.End();
        }
        
        public virtual void Draw(TimeSpan dt)
        {
            if(_deathTimerSet)
            {
                if(Extensions.GetMS()-_deathCallTime>_deathTime)
                {
                    Enabled = false;
                }
            }
            if (IsAnimated && Timing.DrawTiming.Elapsed - _lastUpdate > TimeSpan.FromMilliseconds(Animation.FrameRate))
            {
                Texture = Animation.Frames[Animation.CurrentFrame];
                _lastUpdate = Timing.DrawTiming.Elapsed;
                Animation.CurrentFrame += 1;
                Animation.CurrentFrame %= Animation.Frames.Count;
                if(AnimationType==AnimationType.CutIn&&Animation.CurrentFrame==0&&_stashedAnimation!=null)
                {
                    Animation = _stashedAnimation;
                    _stashedAnimation = null;
                    AnimationFinished = true;
                }
                if(AnimationType==AnimationType.Single&&Animation.CurrentFrame==0)
                {
                    Texture = _stashedFrame;
                    IsAnimated = false;
                    AnimationType = AnimationType.None;
                    AnimationFinished = true;
                }
            }
            if(Texture!=null)
            {
                int cax = 0;
                int cay = 0;
                if (FollowCamera)
                {
                    cax = Program.Game.Camera.X;
                    cay = Program.Game.Camera.Y;
                }
                if (UseBlur && Program.Blur)
                {
                    double dix = Position.X - Program.BlurOrigin.X;
                    double diy = Position.Y - Program.BlurOrigin.Y;
                    double hyp = Math.Sqrt(dix * dix + diy * diy);
                    double dy = diy / hyp * Program.BlurStrength;
                    double dx = dix / hyp * Program.BlurStrength;
                    int prevOpacity = Opacity;
                    Opacity = Program.BlurOpacity;
                    double cx = 0;
                    double cy = 0;
                    while(Opacity>0)
                    {
                        if (FlipHorizontally)
                        {
                            Program.Game.SpriteBatch?.Draw(Texture, new Rectangle((int)(Position.X + cx + cax), (int)(Position.Y + cy + cay), (int)(Width * Scale), (int)(Height * Scale)), null, Tint * (Opacity / 255.0f), Orientation, new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f), SpriteEffects.FlipHorizontally, Layer);
                        }
                        else
                        {
                            Program.Game.SpriteBatch?.Draw(Texture, new Rectangle((int)(Position.X + cx + cax), (int)(Position.Y + cy + cay), (int)(Width * Scale), (int)(Height * Scale)), null, Tint * (Opacity / 255.0f), Orientation, new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f), SpriteEffects.None, Layer);
                        }
                        Opacity -= 25;
                        cx += dx;
                        cy += dy;
                    }
                    Opacity = prevOpacity;
                }
                else
                {
                    if(FlipHorizontally)
                    {
                        Program.Game.SpriteBatch?.Draw(Texture, new Rectangle(Position.X + cax, Position.Y + cay, (int)(Width * Scale), (int)(Height * Scale)), null, Tint * (Opacity / 255.0f), Orientation, new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f), SpriteEffects.FlipHorizontally, Layer);

                    }
                    else
                    {
                        Program.Game.SpriteBatch?.Draw(Texture, new Rectangle(Position.X + cax, Position.Y + cay, (int)(Width * Scale), (int)(Height * Scale)), null, Tint * (Opacity / 255.0f), Orientation, new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f), SpriteEffects.None, Layer);
                    }
                }
                //Program.Game.SpriteBatch?.Draw(Texture, new Rectangle(position.X, position.Y, Texture.Width, Texture.Height), null, Color.White, Orientation, new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f), SpriteEffects.None, Layer);
                //Program.Game.SpriteBatch?.Draw(Texture, position.ToVector2());
                WrapInCollider();
            } else
            {
                Console.WriteLine("Detected null texture, skipping set.");
            }
        }

        public void LoadTexture(string texture)
        {
            DataLoader.AddTexture(texture);
            Texture = DataLoader.GetTexture(texture);
            Width = Texture.Width;
            Height = Texture.Height;
        }

        public DisposeStatus Dispose()
        {
            if(_disposable)
            {
                _disposed = true;
                return DisposeStatus.Succeded;
            } else
            {
                return DisposeStatus.Failed;
            }
        }

        public bool Disposed()
        {
            return _disposed;
        }
        
        public Circle GetColisionCircle()
        {
            int cax = 0;
            int cay = 0;
            if (FollowCamera)
            {
                cax = Program.Game.Camera.X;
                cay = Program.Game.Camera.Y;
            }
            if (Collision.colisionType == CollisionType.Circle)
            {
                return new Circle(Position.X + ColliderOffset.X+cax, Position.Y + ColliderOffset.Y+cay, Collision.radius);
            }
            else
            {
                Console.WriteLine("Cannot get circle collider of a rectangle");
                return new Circle(0, 0, 0);
            }
        }

        public Rectangle GetColisionRectangle()
        {
            int cax = 0;
            int cay = 0;
            if (FollowCamera)
            {
                cax = Program.Game.Camera.X;
                cay = Program.Game.Camera.Y;
            }
            if (Collision.colisionType == CollisionType.Square)
            {
                return new Rectangle(Position.X + ColliderOffset.X+cax, Position.Y + ColliderOffset.Y+cay, Collision.Width, Collision.Height);
            } else
            {
                Console.WriteLine("Cannot get rectangle collider of a circle");
                return new Rectangle(0,0,0,0);
            }
        }
        
        public bool CollidesWith(GameObject other)
        {
            if(Collision!=null&&other.Collision!=null)
            {
                if(Collision.colisionType == CollisionType.Circle)
                {
                    Circle col = GetColisionCircle();
                    if(other.Collision.colisionType == CollisionType.Circle)
                    {
                        Circle oth = other.GetColisionCircle();
                        return col.Intersects(oth);
                    }
                    else if (other.Collision.colisionType == CollisionType.Square)
                    {
                        Rectangle oth = other.GetColisionRectangle();
                        return col.Intersects(oth);
                    }
                } 
                else if (Collision.colisionType == CollisionType.Square)
                {
                    Rectangle col = GetColisionRectangle();
                    if (other.Collision.colisionType == CollisionType.Circle)
                    {
                        Circle oth = other.GetColisionCircle();
                        return oth.Intersects(col);
                    }
                    else if (other.Collision.colisionType == CollisionType.Square)
                    {
                        Rectangle oth = other.GetColisionRectangle();
                        return col.Intersects(oth);
                    }
                }
            }
            else
            {
                Console.WriteLine($"[{Name}] Cannot collide with [{other.Name}]");
            }
            return false;
        }

        public bool Contains(Position point)
        {
            if(Collision!=null)
            {
                if(Collision.colisionType == CollisionType.Circle)
                {
                    Circle col = GetColisionCircle();
                    return col.Contains(point.X, point.Y);
                } 
                else if (Collision.colisionType == CollisionType.Square)
                {
                    Rectangle col = GetColisionRectangle();
                    return col.Contains(point.X, point.Y);
                }
            } else
            {
                Console.WriteLine("Cannot collide with something that does not have a collider.");
            }
            return false;
        }
    }
}
