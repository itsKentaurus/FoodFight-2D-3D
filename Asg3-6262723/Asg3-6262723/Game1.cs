#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

#endregion

namespace Asg3_6262723
{
    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver,
        Clear,
        Pause
    }
    public class Game1 : Game
    {
        #region Pre-Defined Class
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont sf;
        #endregion

        Engine3D _3DEngine;
        Text _Timer;
        Text _Health;
        List<Text> _TextList;
        GameState _CurrentGameState;
        Button _Play;
        KeyboardState _PreviousKey;
        KeyboardState _CurrentKey;
        bool GameSet;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            _CurrentGameState = GameState.MainMenu;
            _3DEngine = new Engine3D();
            _3DEngine.Initialize(Content);
            GameSet = false;
            #region Text
            _TextList = new List<Text>();
            _TextList.Add(new Text("Health: ", new Vector2(graphics.PreferredBackBufferWidth * 0.70f, graphics.PreferredBackBufferHeight * 0.75f)));
            _TextList.Add(new Text("Time Remaining: ", new Vector2(graphics.PreferredBackBufferWidth * 0.56f, graphics.PreferredBackBufferHeight * 0.8f)));
            _TextList.Add(new Text("sec ", new Vector2(graphics.PreferredBackBufferWidth * 0.85f, graphics.PreferredBackBufferHeight * 0.8f)));
            _Health = new Text("0", new Vector2(graphics.PreferredBackBufferWidth * 0.81f, graphics.PreferredBackBufferHeight * 0.75f));
            _Timer = new Text("30", new Vector2(graphics.PreferredBackBufferWidth * 0.81f, graphics.PreferredBackBufferHeight * 0.8f));
            _TextList.Add(_Health);
            _TextList.Add(_Timer);
            #endregion
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sf = Content.Load<SpriteFont>("font");
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
//            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
//                Exit();
            MouseState mouse = Mouse.GetState();
            _PreviousKey = _CurrentKey;
            _CurrentKey = Keyboard.GetState();
            
            switch (_CurrentGameState)
            {
                case GameState.MainMenu:
                    if (!GameSet)
                    {
                        _3DEngine.Setup();
                        _Health.SetText(Convert.ToString(_3DEngine._Player._Strength));
                        _Timer.SetText(Convert.ToString(30));
                        GameSet = true;
                    }


                    if (_PreviousKey.IsKeyDown(Keys.Q) && _CurrentKey.IsKeyUp(Keys.Q))
                        Exit();
                    if (_PreviousKey.IsKeyDown(Keys.Space) && _CurrentKey.IsKeyUp(Keys.Space))
                        _CurrentGameState = GameState.Playing;
                    break;

                case GameState.Playing:
                    _CurrentGameState = _3DEngine.Update(gameTime, _Health, _Timer);
                    if (_CurrentGameState == GameState.Playing && _PreviousKey.IsKeyDown(Keys.Escape) && _CurrentKey.IsKeyUp(Keys.Escape))
                        _CurrentGameState = GameState.Pause;
                    break;

                case GameState.Pause:
                    if (_PreviousKey.IsKeyDown(Keys.Escape) && _CurrentKey.IsKeyUp(Keys.Escape))
                        _CurrentGameState = GameState.Playing;
                    if (_PreviousKey.IsKeyDown(Keys.Q) && _CurrentKey.IsKeyUp(Keys.Q))
                        Exit();
                    break;

                case GameState.GameOver:
                case GameState.Clear:
                    if (GameSet)
                    {
                        _3DEngine.Reset();
                        GameSet = false;
                    }
                    if (_PreviousKey.IsKeyDown(Keys.Escape) && _CurrentKey.IsKeyUp(Keys.Escape))
                        _CurrentGameState = GameState.MainMenu;
                    break;
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (_CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Menus/Screen/Menu"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.End();
                    break;
                case GameState.Playing:
                    #region Playing
            BlendState OriginalBlend = graphics.GraphicsDevice.BlendState;
            DepthStencilState OriginalStencil = graphics.GraphicsDevice.DepthStencilState;
            SamplerState OriginalSampler = graphics.GraphicsDevice.SamplerStates[0];

            graphics.GraphicsDevice.BlendState = BlendState.Opaque;
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            _3DEngine.Draw();

            graphics.GraphicsDevice.BlendState = OriginalBlend;
            graphics.GraphicsDevice.DepthStencilState = OriginalStencil;
            graphics.GraphicsDevice.SamplerStates[0] = OriginalSampler;

            foreach(Text t in _TextList)
                t.Draw(spriteBatch, sf, Color.Black);

            #endregion
                    break;

                case GameState.Pause:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Menus/Screen/Pause"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.End();
                    break;

                case GameState.GameOver:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Menus/Screen/GameOver"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.End();
                    break;
                case GameState.Clear:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Menus/Screen/Clear"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
