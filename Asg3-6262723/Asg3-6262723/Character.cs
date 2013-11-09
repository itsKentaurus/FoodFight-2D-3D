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
    public class Character : Object
    {
        #region Fields
        public Vector3 _Velocity
        {
            get;
            private set;
        }
        public float _Angle
        {
            get;
            private set;
        }
        public List<Food> _FoodList
        {
            get;
            private set;
        }
        public Food _Food
        {
            get;
            private set;
        }
        public bool _HoldingFood
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public Character(Model Model, Vector3 MinVec, Vector3 MaxVec)
            : base(Model, MinVec, MaxVec)
        {
            _HoldingFood = false;
            _FoodList = new List<Food>();
        }

        public void Update(GameTime gameTime)
        {
            _World = Matrix.CreateRotationX((float) -Math.PI / 2) *  (Matrix.CreateRotationY(_Angle) * Matrix.CreateTranslation(_Position));
            base.UpdatePosition(_Position);
        }

        public void Move(int Direction, float Speed)
        {
            _Velocity = Direction * new Vector3(Speed * (float)Math.Sin(_Angle), 0, Speed * (float)Math.Cos(_Angle));
            _Position += _Velocity;
        }

        public void Turn(int Direction, float Angle)
        {
            _Angle += Direction * 0.1f;
        }

        public void PickUp(Food Food)
        {
            if (!_HoldingFood)
                _Food = Food.Clone();
        }
        public void Shoot()
        {
            if (_HoldingFood)
            {
                _Food.Thrown(_Position, _Velocity);
                _FoodList.Add(_Food);
            }
        }
        public void Eat()
        {
            if (_HoldingFood)
            {
                _HoldingFood = !_HoldingFood;
            }
        }
        #endregion
    }
}
