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
    public class UI
    {
        #region Fields
        List<SceneNode> _tileList;
        public int BoxWidth
        {
            get;
            private set;
        }
        public int BoxHeight
        {
            get;
            private set;
        }
        public Texture2D VerticalBar
        {
            get;
            private set;
        }
        public Texture2D HorizontalBar
        {
            get;
            private set;
        }

        public Rectangle _Bounds
        {
            get;
            private set;
        }
        #endregion

        public void Initialize(ContentManager Content, GraphicsDeviceManager graphics, int BoxWidthPercentage, int BoxHeightPercentage)
        {
            #region Wall
            HorizontalBar = Content.Load<Texture2D>("Images/Container/HorizontalBar");
            VerticalBar = Content.Load<Texture2D>("Images/Container/VerticalBar");
            int h = graphics.PreferredBackBufferHeight;
            int w = graphics.PreferredBackBufferWidth;
            _tileList = new List<SceneNode>();
            BoxWidth = (w / 100) * BoxWidthPercentage;  // 1228.8
            BoxHeight = (h / 100) * BoxHeightPercentage; // 576
            for (int i = (w - BoxWidth) / 2; i < ((w - BoxWidth) / 2) + BoxWidth; i++)
            {   // TOP BAR
                SceneNode n = new SceneNode(HorizontalBar, new Vector2(HorizontalBar.Width * i, (h - BoxHeight) / 2));
                _tileList.Add(n);
            }
            for (int i = (h - BoxHeight) / 2 + HorizontalBar.Height; i < (h - BoxHeight) / 2 + HorizontalBar.Height + BoxHeight; i++)
            {   // LEFT BAR
                SceneNode n = new SceneNode(VerticalBar, new Vector2((w - BoxWidth) / 2, VerticalBar.Height * i));
                _tileList.Add(n);
            }
            for (int i = (w - BoxWidth) / 2; i < ((w - BoxWidth) / 2) + BoxWidth; i++)
            {   // BOTTOM BAR
                SceneNode n = new SceneNode(HorizontalBar, new Vector2(HorizontalBar.Width * i,
                                                                (h - BoxHeight) / 2 + HorizontalBar.Height + BoxHeight));
                _tileList.Add(n);
            }
            for (int i = (h - BoxHeight) / 2 + HorizontalBar.Height; i < (h - BoxHeight) / 2 + HorizontalBar.Height + BoxHeight; i++)
            {   // RIGHT BAR
                SceneNode n = new SceneNode(VerticalBar, new Vector2(((w - BoxWidth) / 2) + BoxWidth - VerticalBar.Width,
                                                                VerticalBar.Height * i));
                _tileList.Add(n);
            }
            _Bounds = new Rectangle((graphics.PreferredBackBufferWidth - BoxWidth) / 2 + VerticalBar.Width,
                        (graphics.PreferredBackBufferHeight - BoxHeight) / 2 + HorizontalBar.Height,
                        BoxWidth,
                        BoxHeight);
            #endregion

            #region Text
            #endregion
        }
        public void Update()
        {
        }
        public void Draw(SpriteBatch sb)
        {
            foreach (SceneNode node in _tileList)
                node.Draw(sb);
        }
    }
}
