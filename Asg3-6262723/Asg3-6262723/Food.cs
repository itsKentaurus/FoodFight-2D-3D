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
        Pizza,
        Banana,
        Orange
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
        public int _Amount
        {
            get;
            private set;
        }

        public Food(Model Model, Vector3 Position, Vector3 MinVec, Vector3 MaxVec)
            : base(Model, Position, MinVec, MaxVec)
        {
        }
        private Food(Model Model, BoundingBox Bound)
            : base(Model, Bound)
        {
        }
        public void Update(GameTime gameTime)
        {

        }
        public void Thrown(Vector3 Position, Vector3 Velocity)
        {
            _Velocity = Velocity;
            _Position = Position;
        }

        internal Food Clone()
        {
            Food f = new Food(_Model, _Bound);
            f._Type = _Type;
            _Amount--;
            return f;
        }
    }
}
