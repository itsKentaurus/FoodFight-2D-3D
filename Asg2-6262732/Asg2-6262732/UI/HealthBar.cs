using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Asg2_6262732
{
    class HealthBar
    {
        #region Fields
        public Texture2D _Texture
        {
            get;
            private set;
        }
        public Vector2 _Position
        {
            get;
            private set;
        }
        public int _CurrentDamage
        {
            get;
            private set;
        }
        public float alpha
        {
            get;
            private set;
        }
        public int _MaxHealth
        {
            get;
            private set;
        }
        public int _Height
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public void Initialize(Texture2D Texture, Vector2 Position, int MaxHealth, int height)
        {
            _Texture = Texture;
            _Position = Position;
            _CurrentDamage = 11;
            _MaxHealth = MaxHealth + 11;
            alpha = 0;
        }

        public void Update()
        {
            _CurrentDamage = (int)MathHelper.Clamp(_CurrentDamage, 11, _MaxHealth);
        }
        public void Reset()
        {
            _CurrentDamage = 11;
        }
        public void DecreaseHp(int Num)
        {
            _CurrentDamage += Num;
        }
        public void IncreaseHp(int Num)
        {
            _CurrentDamage -= Num;
            Console.WriteLine(Num);
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Begin();

            //Draw the negative space for the health bar
            sb.Draw(_Texture, new Rectangle((int)_Position.X, (int)_Position.Y, 44, _Texture.Width), new Rectangle(0, 45, _Texture.Width, 44), Color.Red);

            //Draw the current health level based on the current Health
            sb.Draw(_Texture, new Rectangle((int)_Position.X, (int)_Position.Y, 44, (int)(_Texture.Width * ((double)_CurrentDamage / _MaxHealth))), new Rectangle(0, 45, _Texture.Width, 44), Color.Black);

            sb.End();
        }
        #endregion
    }
}
