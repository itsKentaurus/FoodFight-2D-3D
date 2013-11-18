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


        public Food(Model Model, Vector3 MinVec, Vector3 MaxVec, int Strength)
            : base(Model, MinVec, MaxVec)
        {
            _Strength = Strength;
        }

        public Food(Model Model, Vector3 Position, Vector3 MinVec, Vector3 MaxVec, FoodType Type)
            : base(Model, Position, MinVec, MaxVec)
        {
            _Type = Type;
        }
        private Food(Food Food)
            : base(Food._Model, Food._Bound)
        {
            _Type = Food._Type;
            _Strength = Food._Strength;
        }
        public void Update(GameTime gameTime)
        {
            _Position += _Velocity;
            _World = Matrix.CreateRotationX((float)-(Math.PI / 2)) * Matrix.CreateTranslation(_Position);
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
