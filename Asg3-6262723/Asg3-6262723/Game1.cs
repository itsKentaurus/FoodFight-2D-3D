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
    public class Game1 : Game
    {
        #region Pre-Defined Class
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        #endregion

        #region Defined Class
        Character _Player;
        Camera _Camera;
        #endregion

        #region Fields
        List<Hole> _HoleList;
        List<Food> _FoodList;
        List<Object> _ObjectList;
        public Matrix _Projection
        {
            get;
            private set;
        }
        #endregion

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
            _Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 600);
            _Camera = new Camera();
            
            _HoleList = new List<Hole>();
            for (int i = 0; i < 1; ++i)
                _HoleList.Add(new Hole(Content.Load<Model>("Player/Ship"), 
                    new Vector3(0, -1.5f, -10), 
                    new Vector3(-0.5f, 0, -0.5f), 
                    new Vector3(0.5f, 1, 0.5f), 
                    new Chef(Content.Load<Model>("Player/Ship"), new Vector3(-0.5f, 0, -0.5f), new Vector3(0.5f, 1, 0.5f))));

            _FoodList = new List<Food>();
//            _FoodList.Add(new Food(Content.Load<Model>("Player/Ship"), new Vector3(0, 0, -10), new Vector3(-0.5f, 0, -0.5f), new Vector3(0.5f, 1, 0.5f)));
            _Player = new Character(Content.Load<Model>("Models/Player"), new Vector3(-0.5f, 0, -0.5f), new Vector3(0.5f, 1, 0.5f));
            _ObjectList = new List<Object>();
            _ObjectList.Add(new Object(Content.Load<Model>("Models/floor"), Vector3.Zero, Vector3.Zero,Vector3.Zero));
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            int ChefCount = 0;
            Random r = new Random();


            _Player.Update(gameTime);
            _Camera.Update(gameTime, _Player);

            foreach (Food node in _FoodList)
                if (_Player._Bound.Intersects(node._Bound))
                    _Player.PickUp(node);

            foreach (Hole h in _HoleList)
            {
                foreach (Chef c in h._ChefList)
                {
                    c.Update(gameTime, _Player._Position);
                }
                ChefCount += h._ChefList.Count;
            }

            for (int i = ChefCount; i < 1; ++i)
            {
                int j = 0;
                do
                {
                    j = r.Next(_HoleList.Count);
                }
                while (_HoleList[j]._ChefList.Count == 1);
                _HoleList[j].Summon();
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Controls();

            Draw(_Player);
            foreach (Food f in _FoodList)
                Draw(f);
            foreach (Object o in _ObjectList)
                Draw(o);
            foreach (Hole h in _HoleList)
            {
                Draw(h);
                foreach (Chef c in h._ChefList)
                    Draw(c);
            }


            base.Draw(gameTime);
        }
        private void Controls()
        {
            KeyboardState ks = Keyboard.GetState();
            Keys[] keys = ks.GetPressedKeys();
            const float SPEED = 0.1f;
            const float ANGLE = 0.1f;
            foreach (Keys key in keys)
            {
                switch (key)
                {
                    case Keys.W:
                        _Player.Move(-1, SPEED);
                        break;
                    case Keys.D:
                        _Player.Turn(-1, ANGLE);
                        break;
                    case Keys.S:
                        _Player.Move(1, SPEED);
                        break;
                    case Keys.A:
                        _Player.Turn(1, ANGLE);
                        break;
                    case Keys.Space:
                        _Player.Shoot();
                        break;
                    case Keys.E:
                        _Player.Eat();
                        break;
                    case Keys.D1:
                        _Camera.ToggleFirst();
                        break;
                    case Keys.D3:
                        _Camera.ToggleThird();
                        break;
                }
            }
        }

        public void Draw(Object Object)
        {
            foreach (ModelMesh mesh in Object._Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = Object._World;
                    effect.View = _Camera._Position;
                    effect.Projection = _Projection;
                }

                mesh.Draw();
            }
        }
    }
}
