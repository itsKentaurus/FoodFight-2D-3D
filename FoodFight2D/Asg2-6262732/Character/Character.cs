﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Asg2_6262732
{
    public class Character
    {
        #region Fields
        public Vector2 _Position;
        public Texture2D _Texture{ get; private set; }
        public bool[] _Direction{ get; private set; }
        public float _Speed{ get; private set; }
        public ThrownFood _Food{ get; private set; }
        public bool _hasThrownFood{ get; private set; }
        public Direction _LastDirection{ get; private set; }
        public List<ThrownFood> _FoodThrown{ get; private set; }
        public Rectangle _Rec { get; private set; }
        public Rectangle _Bound { get; private set; }
        public int _CurrentHealth { get; private set; }
        public List<ThrownFood> temp;
        #endregion

        #region Declarations
        public Character(Texture2D Texture, Rectangle r, int Speed, int CurrentHealth)
        {
            _Texture = Texture;
            _Direction = new bool[4];
            _Bound = r;
            _Speed = Speed;
            _hasThrownFood = false;
            _Rec = new Rectangle((int)_Position.X, (int)_Position.Y, (int)_Texture.Width, (int)_Texture.Height);
            _CurrentHealth = CurrentHealth;
            _FoodThrown  = new List<ThrownFood>();
            temp = new List<ThrownFood>();

        }
        public void SetPosition(Vector2 Position)
        {
            _Position = Position;
        }
        #endregion

        #region Others
        public int GetHealth()
        {
            return _CurrentHealth;
        }
        public void PickUp(KeyboardState ks, Food obj)
        {
            if (obj.HasFood() &&
                ks.IsKeyUp(Keys.Space) &&
                _Rec.Intersects(obj._Rectangle) &&
                !_hasThrownFood)
            {
                _hasThrownFood = !_hasThrownFood;
               _Food = new ThrownFood(obj.Clone(), _Bound, 6.5f);
            }

        }

        public int EatFood()
        {
            _hasThrownFood = !_hasThrownFood;
            return _Food._FoodDamage;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            foreach (ThrownFood node in _FoodThrown)
                node.Draw(sb);
            sb.Draw(_Texture, _Position, Color.White);
            sb.End();
        }
#endregion

        #region Update
        public void Update()
        {
            KeyboardState ks = Keyboard.GetState();
            MovementControl(ks);
            ShootingControl(ks);
            Moving();
            UpdateRectangle();
            AnimateFood();
        }
        private void AnimateFood()
        {
            
            foreach (ThrownFood node in _FoodThrown) {
                node.Update();
                if (node.Outside())
                    temp.Add(node);
            }
            foreach (ThrownFood node in temp)
                _FoodThrown.Remove(node);

        }

        private void UpdateRectangle()
        {
            _Rec = new Rectangle((int)_Position.X, (int)_Position.Y, (int)_Texture.Width, (int)_Texture.Height);
        }
        private void MovementControl(KeyboardState ks)
        {
            Vector2 LeftThumb = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;

            if (ks.IsKeyDown(Keys.Up) || -LeftThumb.Y > 0) _Direction[0] = true;
            else                            _Direction[0] = false;
            if (ks.IsKeyDown(Keys.Down) || -LeftThumb.Y < 0) _Direction[1] = true;
            else                            _Direction[1] = false;
            if (ks.IsKeyDown(Keys.Left) || LeftThumb.X < 0) _Direction[2] = true;
            else                            _Direction[2] = false;
            if (ks.IsKeyDown(Keys.Right) || LeftThumb.X > 0) _Direction[3] = true;
            else                            _Direction[3] = false;

        }
        private void ShootingControl(KeyboardState ks)
        {
            if (_Direction[0] && _Direction[2])
                _LastDirection = Direction.NorthWest;
            else if (_Direction[0] && _Direction[3])
                _LastDirection = Direction.NorthEast;
            else if (_Direction[1] && _Direction[2])
                _LastDirection = Direction.SouthWest;
            else if (_Direction[1] && _Direction[3])
                _LastDirection = Direction.SouthEast;
            else if (_Direction[0])
                _LastDirection = Direction.North;
            else if (_Direction[1])
                _LastDirection = Direction.South;
            else if (_Direction[3])
                _LastDirection = Direction.East;
            else if (_Direction[2])
                _LastDirection = Direction.West;


        }
        public void Shoot()
        {
            if (!_hasThrownFood) return;
            _Food._Status = FoodStatus.Thrown;
            _FoodThrown.Add(_Food);
            _hasThrownFood = !_hasThrownFood;
            _Food.SetPosition(new Vector2(_Position.X + _Texture.Width / 8, _Position.Y + _Texture.Height / 4));
            _Food.SetDirection(_LastDirection);
        }
        private void Moving()
        {
            if (_Position.Y > _Bound.Top  && _Direction[0])          // TOP WALL
                _Position.Y += -_Speed;

            if (_Position.Y + _Texture.Height < _Bound.Bottom && _Direction[1])  // BOTTOM WALL
                _Position.Y += _Speed;

            if (_Position.X > _Bound.Left && _Direction[2])          // LEFT WALL
                _Position.X += -_Speed;

            if (_Position.X + _Texture.Width < _Bound.Right && _Direction[3])          // RIGHT WALL
                _Position.X += _Speed;
        }
        #endregion
    }
}
