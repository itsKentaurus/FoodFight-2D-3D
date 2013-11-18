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
        public FoodType _FoodType
        {
            get;
            private set;
        }
        public bool _HoldingFood
        {
            get;
            private set;
        }
        private int _Amount;
        private int _LastDirection;
        #endregion

        #region Methods
        public Character(Model Model, Vector3 MinVec, Vector3 MaxVec, int Strength)
            : base(Model, MinVec, MaxVec)
        {
            _HoldingFood = false;
            _Strength = Strength;
        }
        public void SetStrength(int Health)
        {
            _Strength = Health;
        }
        public void Update(GameTime gameTime)
        {
            _World = Matrix.CreateRotationX((float) -Math.PI / 2) *  (Matrix.CreateRotationY(_Angle) * Matrix.CreateTranslation(_Position));
            base.UpdatePosition();
        }
        public void UpdatePosition(Vector3 Position)
        {
            _Position = Position;
        }
        public void Move(int Direction, float Speed)
        {
            _Velocity = Direction * new Vector3(Speed * (float)Math.Sin(_Angle), 0, Speed * (float)Math.Cos(_Angle));
            _LastDirection = Direction;
            _Position += _Velocity;
        }

        public void Turn(int Direction, float Angle)
        {
            _Angle += Direction * 0.1f;
        }

        public void PickUp(FoodType Food,int  Amount)
        {
            if (!_HoldingFood)
            {
                _FoodType = Food;
                _HoldingFood = true;
                _Amount = Amount;
            }
        }
        public FoodType Shoot(float Speed)
        {
            if (_HoldingFood)
            {
                _HoldingFood = false;
                _Velocity = _LastDirection * new Vector3(Speed * (float)Math.Sin(_Angle), 0, Speed * (float)Math.Cos(_Angle));
                return _FoodType;
            }
            return FoodType.None;
        }
        public void Eat()
        {
            if (_HoldingFood)
            {
                _HoldingFood = !_HoldingFood;
                GainHealth(_Amount);
            }
        }
        #endregion
    }
}
