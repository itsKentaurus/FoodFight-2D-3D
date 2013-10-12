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

namespace Asg26262732
{
    public class Food
    {
        #region Fields
        public Vector2 _Position
        {
            get;
            private set;
        }
        public Texture2D[] _Textures
        {
            get;
            private set;
        }
        public Texture2D _Texture
        {
            get;
            private set;
        }
        public Rectangle _Rectangle
        {
            get;
            private set;
        }
        public Rectangle _sourceRectangle
        {
            get;
            private set;
        }
        public int _Amount
        {
            get;
            private set;
        }
        public FoodStatus _Status
        {
            get;
            private set;
        }
        public int _FoodDamage
        {
            get;
            private set;
        }
        public bool isIceCream
        {
            get;
            private set;
        }
        public int _Total
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public void Initialize(Vector2 Position, Texture2D Texture, int Total)
        {
            _Position = Position;
            _Texture = Texture;
            _Textures = null;
            _Amount = 0;
            _Total = Total;
            _FoodDamage = 0;
            isIceCream = true;
            _sourceRectangle = new Rectangle(0,0, 400, (int)_Texture.Height);
            _Rectangle = new Rectangle((int)_Position.X, (int)_Position.Y, 25, (int)_Texture.Height);
        }
        public void Initialize(Vector2 Position, Texture2D[] Texture, Texture2D singleTexture, int Amount, int Damage)
        {
            _Position = Position;
            _Textures = Texture;
            _Texture = singleTexture;
            _Amount = Amount;
            _FoodDamage = Damage;
            isIceCream = false;
            _sourceRectangle = new Rectangle();
            _Rectangle = new Rectangle((int)_Position.X, (int)_Position.Y, (int)_Textures[Amount].Width, (int)_Textures[Amount].Height);
        }
        public void SetPosition(Vector2 v)
        {
            _Position = v;
            if (_Amount != 0)
                _Rectangle = new Rectangle((int)_Position.X, (int)_Position.Y, (int)_Textures[_Amount].Width, (int)_Textures[_Amount].Height);
            else
                _Rectangle = new Rectangle((int)_Position.X, (int)_Position.Y, 25, (int)_Texture.Height);
        }
        public bool ReduceOne()
        {
            if (isIceCream)
            {
                if (++_Amount < _Total)
                {
                    _Rectangle = new Rectangle((int)_Position.X+25, (int)_Position.Y+30, 25, (int)((_Texture.Height/100)*10));
                    _sourceRectangle = new Rectangle(_Amount * 400, 0, 400, (int)_Texture.Height);
                    return false;
                }
            }
            return true;
        }
        public bool HasFood()
        {
            return _Amount > 0;
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            if (isIceCream)
                sb.Draw(_Texture, _Position, _sourceRectangle, Color.White, 0, new Vector2(_sourceRectangle.Width / 2 - _sourceRectangle.Width, _sourceRectangle.Height / 2 - _sourceRectangle.Height), 0.15f, SpriteEffects.None, 0f);
            else
                sb.Draw(_Textures[_Amount], _Position, Color.White);
            sb.End();
        }
        #endregion

        #region Enemy Methods
        public void EnemyInitialize(Texture2D singleTexture, int Amount, int Damage)
        {
            _Texture = singleTexture;
            _Amount = Amount;
            _FoodDamage = Damage;
        }
        #endregion

        internal Food Clone()
        {
            Food f = new Food();
            f._Texture = _Texture;
            f._Status = FoodStatus.Held;
            f._FoodDamage = _FoodDamage;
            _Amount--;

            return f;

        }

        internal Food EnemyClone()
        {
            Food f = new Food();
            f._Texture = _Texture;
            f._FoodDamage = _FoodDamage;
            f._Status = FoodStatus.Thrown;
            _Amount--;

            return f;

        }
    }
}
