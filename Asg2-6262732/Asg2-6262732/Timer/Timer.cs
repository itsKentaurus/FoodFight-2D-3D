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
    public class Timer
    {
        #region Fields
        public int TIMER_START
        {
            get;
            private set;
        }
        public int _TimerCounter
        {
            get;
            private set;
        }
        public float _Elapse
        {
            get;
            private set;
        }
        public int _Lenght
        {
            get;
            private set;
        }

        #endregion

        #region Methods
        public void Initialize(int Height, int length)
        {
            TIMER_START = Height;
            _Elapse = 0;
            _TimerCounter = 1;
            _Lenght = length;
        }
        public void Update(GameTime gameTime)
        {
            _Elapse += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_Elapse >= TIMER_START / _Lenght / 19 && TIMER_START - _TimerCounter >= 0)
            {
                _Elapse -= 53;
                _TimerCounter++;
            }
        }
        #endregion
    }
}
