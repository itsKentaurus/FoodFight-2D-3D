using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asg26262732
{
    class Button
    {
        Texture2D texture;
        Texture2D[] textures;
        Vector2 position;
        Rectangle rectangle;
        int _Initial;
        int _Count;

        Color _color = new Color(255, 255, 255, 255);

        public Vector2 size;

        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            texture = newTexture;

            //ScreenW = 800, ScreenH = 600
            //ImgW = 100, ImgH = 20
            size = new Vector2(graphics.Viewport.Width / 8, graphics.Viewport.Height / 30);
            _Initial = _Count = -1;

        }
        public Button(Texture2D[] newTexture, GraphicsDevice graphics, int count)
        {
            textures = newTexture;
            _Initial = 0;
            _Count = count;
            size = new Vector2(graphics.Viewport.Width / 8, graphics.Viewport.Height / 30);

        }

        bool down;
        public bool isClicked;

        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                if (_color.A == 255)
                    down = false;

                if (_color.A == 0)
                    down = true;

                if (down)
                    _color.A += 3;
                else
                    _color.A -= 3;

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;
                    _Initial = (++_Initial % _Count);
                }
            }
            else if (_color.A < 255)
            {
                _color.A += 3;
                isClicked = false;
            }
        }

        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Begin();
            if (_Count == -1)
                spritebatch.Draw(texture, rectangle, _color);
            else
                spritebatch.Draw(textures[_Initial], rectangle, _color);

            spritebatch.End();
        }
    }
}
