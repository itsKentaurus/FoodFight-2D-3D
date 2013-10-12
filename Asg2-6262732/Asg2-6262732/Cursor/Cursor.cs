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
    class Cursor
    {
        #region Fields
        public Vector2 _Position;
        public Texture2D _Texture
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public void Initialize(Texture2D Texture)
        {
            _Position = new Vector2(0, 0);
            _Texture = Texture;
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(_Texture, _Position, new Rectangle(0, 0, (int)_Texture.Width, (int)_Texture.Height), Color.White, 0, new Vector2(15, 15), 0.075f, SpriteEffects.None, 0f);
            sb.End();
        }
        #endregion
    }
}
