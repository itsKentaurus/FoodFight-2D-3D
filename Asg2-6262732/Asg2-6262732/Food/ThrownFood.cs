using System;
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
    public class ThrownFood
    {
        #region Fields
        private Vector2 _Position;
        private Vector2 _Velocity;
        public float _Speed
        {
            get;
            private set;
        }

        public Texture2D _Texture
        {
            get;
            private set;
        }

        public FoodStatus _Status
        {
            get;
            set;
        }
        public Rectangle _Rectangle
        {
            get;
            private set;
        }

        public Rectangle _Bounds
        {
            get;
            private set;
        }
        public int _FoodDamage
        {
            get;
            private set;
        }
        Vector2 _INITAL_POSITION = new Vector2(1000, 1000);
        #endregion


        public ThrownFood(Food f, Rectangle r, float Speed)
        {
            _Position = _INITAL_POSITION;
            _Texture = f._Texture;
            _Status = f._Status;
            _FoodDamage = f._FoodDamage;
            _Speed = Speed;
            _Bounds = r;
            UpdateRectangle();
        }

        public void SetPosition(Vector2 Position)
        {
            _Position = Position;
        }
        public void SetDirection(Direction direction)
        {
            if (direction == Direction.North)
                _Velocity = new Vector2(0, -_Speed);
            if (direction == Direction.South)
                _Velocity = new Vector2(0, _Speed);
            if (direction == Direction.East)
                _Velocity = new Vector2(_Speed, 0);
            if (direction == Direction.West)
                _Velocity = new Vector2(-_Speed, 0);

            if (direction == Direction.NorthEast)
                _Velocity = new Vector2(_Speed, -_Speed);
            if (direction == Direction.NorthWest)
                _Velocity = new Vector2(-_Speed, -_Speed);
            if (direction == Direction.SouthEast)
                _Velocity = new Vector2(_Speed, _Speed);
            if (direction == Direction.SouthWest)
                _Velocity = new Vector2(-_Speed, _Speed);
        }

        public void Update()
        {
            if (_Status == FoodStatus.Thrown) {
                _Position += _Velocity;
                UpdateRectangle();
            }
        }
        private void UpdateRectangle()
        {
            _Rectangle = new Rectangle((int)_Position.X, (int)_Position.Y, (int)_Texture.Width, (int)_Texture.Height);
        }
        public bool Outside()
        {
            return !_Rectangle.Intersects(_Bounds);
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_Texture, _Position, Color.White);
        }
    }
}
