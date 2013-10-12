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
    class SceneNode
    {
        private Texture2D texture;
        private Vector2 worldPosition;
        private bool _Visible;
        public Vector2 Position
        {
            get { return worldPosition; }
            set { worldPosition = value; }
        }

        public SceneNode(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.worldPosition = position;
            this._Visible = true;
        }

        public void GoInvis()
        {
            this._Visible = false;
        }
        public void GoVisible()
        {
            this._Visible = true;
        }
        public void Draw(SpriteBatch renderer)
        {
            if (!_Visible) return;
            renderer.Begin();
            renderer.Draw(texture, worldPosition, Color.White);
            renderer.End();
        }
    }
}
