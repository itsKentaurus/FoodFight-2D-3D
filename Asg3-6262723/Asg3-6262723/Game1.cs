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
        List<Food> _TypeList;
        List<Food> _ThrownList;
        List<Object> _ObjectList;
        Matrix _Projection;
        BoundingBox _Bounds;
        #endregion

        #region CONST
        int _CHEFCOUNT = 4;
        int _HOLECOUNT = 8;
        int _BANANACOUNT = 0;
        int _ORANGECOUNT = 0;
        int _PIZZACOUNT = 0;
        int _WIDTH = 12;
        int _DEPTH = 12;
        int _MINMOVE = 6;
        int _MAXMOVE = 10;
        int _MINSPAWN = 3;
        int _MAXSPAWN = 3;
        int _MINTHROWTIMER = 3;
        int _MAXTHROWTIMER = 7;
        float SPEED = 0.1f;
        float ANGLE = 0.1f;
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
            Random r = new Random();
            bool Overlap = true;

            _Bounds = new BoundingBox(new Vector3(((_WIDTH / 2) + 1) * -2f, -10, (_DEPTH + 1) * -2f), new Vector3((-(_WIDTH / 2) - 1) * -2f, 10, 2f));
            Console.WriteLine(_Bounds);
            #region Hole
            _HoleList = new List<Hole>();
            for (int i = 0; i < _HOLECOUNT; ++i)
            {
                Hole h;

                do
                {
                    h = new Hole(Content.Load<Model>("Models/Hole"),
                    new Vector3(r.Next(-(_WIDTH / 2), _WIDTH / 2) * -2f, 0, r.Next(1, _DEPTH - 1) * -2f),
                    new Vector3(-1f, 0, -1f),
                    new Vector3(1f, 0.5f, 1f),
                    new Chef(Content.Load<Model>("Models/Chef"), new Vector3(-1, 0, -1), new Vector3(1, 3, 1),
                    _MINTHROWTIMER,
                    _MAXTHROWTIMER),
                    _MINMOVE,
                    _MAXMOVE,
                    _MINSPAWN, 
                    _MAXSPAWN);


                    Overlap = false;
                    foreach (Hole node in _HoleList)
                        if (node._Bound.Intersects(h._Bound))
                            Overlap = true;

                } while (Overlap);
                Overlap = true;
                _HoleList.Add(h);
            }
            #endregion

            #region Food
            _FoodList = new List<Food>();
            _FoodList.Add(new Food(Content.Load<Model>("Player/Ship"), new Vector3(0, 0, -10), new Vector3(-0.5f, 0, -0.5f), new Vector3(0.5f, 1, 0.5f)));

            _TypeList = new List<Food>();
            _TypeList.Add(new Food(Content.Load<Model>("Player/Ship"), new Vector3(-0.5f, -10, -0.5f), new Vector3(0.5f, 10, 0.5f)));
            _TypeList.Add(new Food(Content.Load<Model>("Player/Ship"), new Vector3(-0.5f, -10, -0.5f), new Vector3(0.5f, 10, 0.5f)));
            _TypeList.Add(new Food(Content.Load<Model>("Player/Ship"), new Vector3(-0.5f, -10, -0.5f), new Vector3(0.5f, 10, 0.5f)));

            _ThrownList = new List<Food>();
            #endregion

            #region Player
            _Player = new Character(Content.Load<Model>("Models/Player"), new Vector3(-0.5f, 0, -0.5f), new Vector3(0.5f, 3, 0.5f));
            #endregion

            #region Floor
            _ObjectList = new List<Object>();
            for (int i = -(_WIDTH / 2)-1; i < (_WIDTH/2)+1; ++i)
                for (int j = -1; j < _DEPTH+1; ++j)
                    _ObjectList.Add(new Object(Content.Load<Model>("Models/floor"), new Vector3(i*-2f, 0, j*-2f)));

            #endregion
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
            List<Food> Temp = new List<Food>();


            _Player.Update(gameTime);
            _Camera.Update(gameTime, _Player);

            #region Player
            foreach (Food node in _FoodList)
                if (_Player._Bound.Intersects(node._Bound))
                    _Player.PickUp(node);

            foreach (Hole node in _HoleList)
            {
                if (_Player._Bound.Intersects(node._Bound))
                {
                    _Player.UpdatePosition(Vector3.Zero);
                }

                if(node._ChefList.Count > 0 && _Player._Bound.Intersects(node._ChefList[0]._Bound))
                {
                    node.Clear();
                }

            }

            foreach(Food node in _ThrownList)
                if (_Player._Bound.Intersects(node._Bound))
                {
                    Temp.Add(node);
                }

            #endregion

            #region Enemy
            foreach (Hole h in _HoleList)
            {

                if (h._Moving && h._ChefList.Count > 0)
                {
                    bool clear = false;
                    while (!clear)
                    {
                        int HoleNum = r.Next(_HOLECOUNT);
                        if (_HoleList[HoleNum]._ChefList.Count <= 0 && _HoleList[HoleNum] != h)
                        {
                            Chef temp = h._ChefList[0];
                            _HoleList[HoleNum].Summon(h._Position, h._ChefList[0]);
                            h.Clear();
                            clear = true;
                        }
                    }
                }
                h.Update(gameTime);

                foreach (Chef c in h._ChefList)
                {
                    if (c._Throw)
                    {
                        c.Throw();
                        Food f = _TypeList[r.Next(_TypeList.Count())].Clone();
                        f.Thrown(c._Position + new Vector3(0,1.5f,0), c._PlayerDirection);
                        _ThrownList.Add(f);
                    }

                    c.Update(gameTime, _Player._Position);
                }
            }

            foreach (Hole node in _HoleList)
                if (node._StartSpawn)
                    ChefCount++;
                

            for (int i = ChefCount; i < _CHEFCOUNT; ++i)
            {
                int j = 0;
                do
                {
                    j = r.Next(_HoleList.Count);
                }
                while (_HoleList[j]._StartSpawn && _HoleList[j]._ChefList.Count == 1);
                _HoleList[j].Summon();
            }
            #endregion

            #region Food
            foreach (Food node in _ThrownList)
            {
                node.Update(gameTime);
                if (!node._Bound.Intersects(_Bounds))
                    Temp.Add(node);
            }

            foreach (Food node in Temp)
                _ThrownList.Remove(node);

            Temp.Clear();
            #endregion
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Controls();
            if (_Camera.IsThirdPerson())
                Draw(_Player);
            foreach (Food f in _FoodList)
                Draw(f);
            foreach (Food f in _ThrownList)
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
