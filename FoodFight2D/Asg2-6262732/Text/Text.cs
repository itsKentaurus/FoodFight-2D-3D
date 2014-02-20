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
    public class Text
    {
        #region Fields
        public String _Text
        {
            get;
            private set;
        }
        public Vector2 _Position
        {
            get;
            private set;
        }
        #endregion

        #region Method
        public Text()
        {
            // Ment to be Empty
        }
        public Text(String text, Vector2 position)
        {
            _Text = text;
            _Position = position;
        }
        public void SetText(String text)
        {
            _Text = text;
        }
        public void SetText(Vector2 position, String text)
        {
            _Position = position;
            _Text = text;
        }
        public String GetText()
        {
            return _Text;
        }
        public void Draw(SpriteBatch sb, SpriteFont font, Color color)
        {
            sb.Begin();
            sb.DrawString(font, _Text, _Position, color);
            sb.End();
        }
        #endregion
    }
}