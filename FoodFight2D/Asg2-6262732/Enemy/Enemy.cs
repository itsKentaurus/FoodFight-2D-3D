#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Asg2_6262732
{
    public class Enemy
    {
        #region Fields
        public Vector2 _Position
        {
            get;
            private set;
        }
        public Texture2D _Texture
        {
            get;
            private set;
        }
        public EnemyStatus _Status
        {
            get;
            set;
        }
        public Rectangle _Rectangle
        {
            get;
            private set;
        }
        public float _Timer
        {
            get;
            private set;
        }
        public bool _HorizontalCheck
        {
            get;
            private set;
        }
        public bool _VerticalCheck
        {
            get;
            private set;
        }
        public Food[] _FoodList
        {
            get;
            private set;
        }
        public ThrownFood _Food
        {
            get;
            private set;
        }
        public List<ThrownFood> _FoodThrown { get; private set; }
        public List<ThrownFood> temp { get; private set; }
        public float timer
        {
            get;
            private set;
        }
        public float reset
        {
            get;
            private set;
        }
        #endregion

        #region Method
        public void Initialize(Vector2 _Position, Texture2D _Texture)
        {
            this._Position = _Position;
            this._Texture = _Texture;
            _Rectangle = new Rectangle((int)_Position.X, (int)_Position.Y, (int)_Texture.Width, (int)_Texture.Height);
        }
        public void SetPosition(Vector2 _Position)
        {
            this._Position = _Position;
            _Rectangle = new Rectangle((int)_Position.X, (int)_Position.Y, (int)_Texture.Width, (int)_Texture.Height);
        }
        public void Update(ref Character _Main, int height, GameTime gt, Rectangle _Bound)
        {
            Random r = new Random();
            UpdateRectangle();
            Vector2 vel = Vector2.Zero;
            if (_Status == EnemyStatus.Spawning)
                vel = new Vector2(0, -1);


            if (height > (int)_Position.Y + (int)_Texture.Height-20)
                _Status = EnemyStatus.Alive;

            timer += (float)gt.ElapsedGameTime.TotalSeconds;
            if (_Status == EnemyStatus.Alive)
            {
                if (timer > reset)
                {
                    Shoot(ref _Main, _Bound);
                    timer -= reset;
                    reset = r.Next(2);
                    reset += 0.5f;
                }
/*                if (_Position.X > _Main._Position.X)
                    vel.X = -1.5f;
                else if (_Position.X < _Main._Position.X)
                    vel.X = 1.5f;
                else
                    vel.X = 0;

                if (_Position.Y > _Main._Position.Y)
                    vel.Y = -1.5f;
                else if (_Position.Y < _Main._Position.Y)
                    vel.Y = 1.5f;
                else
                    vel.Y = 0;
 */
            }
            _Position += vel;
            AnimateFood();
        }
        private void Shoot(ref Character _Main, Rectangle _Bound)
        {
            Random r = new Random();
            _Food = new ThrownFood(_FoodList[r.Next(3)].EnemyClone(), _Bound, 6.5f);

            if ((_Rectangle.Top < _Main._Rec.Top && _Main._Rec.Top < _Rectangle.Bottom)
                || (_Rectangle.Top < _Main._Rec.Bottom && _Main._Rec.Bottom < _Rectangle.Bottom))
            {
                if (_Rectangle.Right < _Main._Rec.Left)
                    _Food.SetDirection(Direction.East);
                else
                    _Food.SetDirection(Direction.West);
            }
            else if ((_Rectangle.Left < _Main._Rec.Left && _Main._Rec.Left < _Rectangle.Right)
                || (_Rectangle.Left < _Main._Rec.Right && _Main._Rec.Right < _Rectangle.Right))
            {
                if (_Rectangle.Top < _Main._Rec.Bottom)
                    _Food.SetDirection(Direction.South);
                else
                    _Food.SetDirection(Direction.North);
            }
            else if (_Rectangle.Bottom < _Main._Rec.Top && _Rectangle.Right < _Main._Rec.Left)
                _Food.SetDirection(Direction.SouthEast);
            else if (_Rectangle.Top > _Main._Rec.Bottom && _Rectangle.Right < _Main._Rec.Left)
                _Food.SetDirection(Direction.NorthEast);
            else if (_Rectangle.Bottom < _Main._Rec.Top && _Rectangle.Left > _Main._Rec.Right)
                _Food.SetDirection(Direction.SouthWest);
            else if (_Rectangle.Top > _Main._Rec.Bottom && _Rectangle.Left > _Main._Rec.Right)
                _Food.SetDirection(Direction.NorthWest);

            _Food.SetPosition(_Position);
            _FoodThrown.Add(_Food);
        }
        private void AnimateFood()
        {
            foreach (ThrownFood node in _FoodThrown)
            {
                node.Update();
                if (node.Outside())
                    temp.Add(node);
            }
            foreach (ThrownFood node in temp)
                _FoodThrown.Remove(node);

        }
        public void UpdateRectangle()
        {
            _Rectangle = new Rectangle((int)_Position.X, (int)_Position.Y, (int)_Texture.Width, (int)_Texture.Height);
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_Texture, _Position, Color.White);
            foreach (ThrownFood node in _FoodThrown)
                node.Draw(sb);
        }
        #endregion

        internal Enemy Clone(Food[] _F)
        {
            Enemy e = new Enemy();

            e._Position = _Position;
            e._Texture = _Texture;
            e._FoodList = _F;
            e.temp = new List<ThrownFood>();
            e._FoodThrown = new List<ThrownFood>();
            e.reset = 0;
            e._Status = EnemyStatus.Spawning;

            return e;
        }
    }
}
