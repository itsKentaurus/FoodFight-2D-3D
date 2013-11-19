#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Asg3_6262723
{
    public enum FoodType
    {
        Pizza = 0,
        Orange = 1,
        Banana = 2,
        None
    }

    public class Food : Object
    {
        public Vector3 _Velocity
        {
            get;
            private set;
        }
        public FoodType _Type
        {
            get;
            private set;
        }
        private float _Elapse;
        float y = 0.05f;
        float Scale = 1f;

        public Food(Model Model, Vector3 MinVec, Vector3 MaxVec, int Strength)
            : base(Model, MinVec, MaxVec)
        {
            _Strength = Strength;
            _World = Matrix.CreateRotationX((float)-(Math.PI / 2)) * Matrix.CreateTranslation(_Position);
        }

        public Food(Model Model, Vector3 Position, Vector3 MinVec, Vector3 MaxVec, FoodType Type)
            : base(Model, Position, MinVec, MaxVec)
        {
            _Type = Type;
            _World = Matrix.CreateRotationX((float)-(Math.PI / 2)) * Matrix.CreateTranslation(_Position);
        }
        private Food(Food Food)
            : base(Food._Model, Food._Bound)
        {
            _Type = Food._Type;
            _Strength = Food._Strength;
            _World = Matrix.CreateRotationX((float)-(Math.PI / 2)) * Matrix.CreateTranslation(_Position);
        }
        public void Update(GameTime gameTime)
        {
            if (_Type != FoodType.None)
            {
                _Elapse += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_Elapse > 0.05f)
                    y -= 0.001f;
                _Position += _Velocity + new Vector3(0, y, 0);
                _World = Matrix.CreateRotationX((float)-(Math.PI / 2)) * Matrix.CreateTranslation(_Position);
            }
            else
            {
                Scale -= 0.00065f;
                _World = Matrix.CreateScale(Scale) *  Matrix.CreateRotationX((float)-(Math.PI / 2)) * Matrix.CreateTranslation(_Position);
            }
            base.UpdatePosition();
        }

        public void Thrown(Vector3 Position, Vector3 Velocity)
        {
            _Velocity = Velocity/10;
            _Position = Position;
            base.UpdatePosition();
        }

        internal Food Clone()
        {
            Food f = new Food(this);
            return f;
        }
    }
}
