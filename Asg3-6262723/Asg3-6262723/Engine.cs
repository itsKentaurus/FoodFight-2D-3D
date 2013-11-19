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
    public class Engine3D
    {

        #region Defined Class
        public Character _Player
        {
            get;
            private set;
        }
        Camera _Camera;
        Food _IceCream;
        float _Elapse;
        ContentManager Content;
        Audio _Audio;
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
        int _HOLECOUNT = 9;
        int _FOODCOUNT = 10;
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
        int _PlayerStrength = 150;
        int _ChefStrength = 10;
        int _PizzaStrength = 10;
        int _BananaStrength = 7;
        int _OrangeStrength = 5;
        #endregion
        public Engine3D(ref Audio Audio)
        {
            _Audio = Audio;
        }

        public void Initialize(ContentManager Content)
        {
            this.Content = Content;
            _Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 600);
            _Bounds = new BoundingBox(new Vector3(((_WIDTH / 2) + 1) * -2f, -10, (_DEPTH + 1) * -2f), new Vector3((-(_WIDTH / 2) - 1) * -2f, 10, 2f));
            _Camera = new Camera();
            _FoodList = new List<Food>();
            _HoleList = new List<Hole>();
            _ObjectList = new List<Object>();
            _TypeList = new List<Food>();

            #region Ice Cream
            _IceCream = new Food(Content.Load<Model>("Models/IceCream"), Vector3.Zero, new Vector3(-0.5f, 0, -0.5f), new Vector3(0.5f, 1, 0.5f), FoodType.None);
            #endregion
            #region Player
            _Player = new Character(Content.Load<Model>("Models/Player"), new Vector3(-0.5f, 0, -0.5f), new Vector3(0.5f, 3, 0.5f), _PlayerStrength);
            #endregion

        }

        public void Setup()
        {
            #region Necessary To Load
            Random r = new Random();
            bool Overlap = true;

            Model[] m = new Model[3];
            m[0] = Content.Load<Model>("Models/Pizza");
            m[1] = Content.Load<Model>("Models/Orange");
            m[2] = Content.Load<Model>("Models/Banana");
            FoodType[] ft = new FoodType[3];
            ft[0] = FoodType.Pizza;
            ft[1] = FoodType.Orange;
            ft[2] = FoodType.Banana;
            #endregion
            #region Hole
            for (int i = 0; i < _HOLECOUNT; ++i)
            {
                Hole h;

                do
                {
                    h = new Hole(Content.Load<Model>("Models/Hole"),
                    new Vector3(r.Next(-(_WIDTH / 2), _WIDTH / 2) * -2f, 0, r.Next(1, _DEPTH - 1) * -2f),
                    new Vector3(-1f, 0, -1f),
                    new Vector3(1f, 0.5f, 1f),
                    new Chef(Content.Load<Model>("Models/Chef"), new Vector3(-0.5f, 0, -0.5f), new Vector3(0.5f, 3, 0.5f),
                    _MINTHROWTIMER,
                    _MAXTHROWTIMER,
                    _ChefStrength),
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
            for (int i = 0; i < _FOODCOUNT; ++i)
            {
                Food f;
                do
                {
                    int num = r.Next(3);
                    f = new Food(m[num],
                        new Vector3(r.Next(-(_WIDTH / 2), _WIDTH / 2) * -2f, 0, r.Next(1, _DEPTH - 1) * -2f),
                        new Vector3(-0.5f, 0, -0.5f),
                        new Vector3(0.5f, 1, 0.5f),
                        ft[num]);

                    Overlap = false;
                    foreach (Hole node in _HoleList)
                        if (node._Bound.Intersects(f._Bound))
                            Overlap = true;
                    foreach (Food node in _FoodList)
                        if (node._Bound.Intersects(f._Bound))
                            Overlap = true;

                } while (Overlap);
                Overlap = true;
                _FoodList.Add(f);
            }

            _TypeList.Add(new Food(m[0], new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), _PizzaStrength));
            _TypeList.Add(new Food(m[1], new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), _OrangeStrength));
            _TypeList.Add(new Food(m[2], new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), _BananaStrength));

            _ThrownList = new List<Food>();
            #endregion
            #region Floor
            for (int i = -(_WIDTH / 2) - 1; i < (_WIDTH / 2) + 1; ++i)
                for (int j = -1; j < _DEPTH + 1; ++j)
                    _ObjectList.Add(new Object(Content.Load<Model>("Models/floor"), new Vector3(i * -2f, 0, j * -2f)));

            #endregion

            _IceCream.SetPosition(new Vector3(r.Next(-(_WIDTH / 2), (_WIDTH / 2) + 1) * -2, 0, (_DEPTH) * -2));
            _Player.SetPosition(Vector3.Zero);
            _Player.SetStrength(_PlayerStrength);
        }

        public void Reset()
        {
            _HoleList.Clear();
            _FoodList.Clear();
            _TypeList.Clear();
            _ThrownList.Clear();
            _ObjectList.Clear();

        }

        public GameState Update(GameTime gameTime, Text Health, Text Timer)
        {
            Controls();

            int ChefCount = 0;
            Random r = new Random();
            List<Food> Temp = new List<Food>();

            _Player.Update(gameTime);
            _Camera.Update(gameTime, _Player);
            _IceCream.Update(gameTime);

            Health.SetText(Convert.ToString(Convert.ToInt32(_Player._Strength)));
            _Elapse += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_Elapse > 1000f)
            {
                Timer.SetText(Convert.ToString(Convert.ToInt32(Timer.GetText()) - 1));
                _Elapse -= 1000;
            }

            #region Player
            foreach (Food node in _FoodList)
                if (!_Player._HoldingFood && _Player._Bound.Intersects(node._Bound))
                {
                    _Player.PickUp(node._Type, node._Strength);
                    Temp.Add(node);
                }
            foreach (Food node in Temp)
                _FoodList.Remove(node);
            Temp.Clear();


            foreach (Hole node in _HoleList)
            {
                if (_Player._Bound.Intersects(node._Bound))
                {
                    _Player.UpdatePosition(Vector3.Zero);
                    _Player.ReduceHealth(50);
                }

                if (node._ChefList.Count > 0 && _Player._Bound.Intersects(node._ChefList[0]._Bound))
                {
                    _Player.ReduceHealth(20);
                    node.Clear();
                }
            }

            foreach (Food node in _ThrownList)
                if (_Player._Bound.Intersects(node._Bound))
                {
                    _Player.ReduceHealth(node._Strength);
                    Temp.Add(node);
                }

            if (_Player._Bound.Intersects(_IceCream._Bound))
            {
                return GameState.Clear;
            }

            if (Convert.ToInt32(Health.GetText()) <= 0)
                return GameState.GameOver;

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
                            Chef temp = h._ChefList[0].Clone(h._Position);
                            _HoleList[HoleNum].Summon(temp);
                            h.Clear();
                            clear = true;
                        }
                    }
                }

                foreach (Chef c in h._ChefList)
                {
                    if (c._Throw)
                    {
                        c.Throw();
                        Food f = _TypeList[r.Next(_TypeList.Count())].Clone();
                        Vector3 p = c._Position + new Vector3(0, 1.5f, 0) + c._PlayerDirection;
                        Vector3 v = c._PlayerDirection * (c._Strength / 5);
                        f.Thrown(p, v);
                        _ThrownList.Add(f);
                    }
                    foreach (Food node in _ThrownList)
                    {
                        if (!c._Moving && c._Bound.Intersects(node._Bound))
                        {
                            c.ReduceHealth(node._Strength);
                            Temp.Add(node);
                            _Audio.Play("Enemy", "Doh");
                        }
                        if (c._Moving && c._Bound.Intersects(node._Bound))
                        {
                            c.ReduceHealth(_ChefStrength);
                            Temp.Add(node);
                        }
                    }
                    c.Update(gameTime, _Player._Position);
                }
                h.Update(gameTime, _Audio);
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

            return GameState.Playing;
        }

        public void Draw()
        {
            if (_Camera.IsThirdPerson())
                Draw(_Player);

            Draw(_IceCream);
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
                        if (_Player._FoodType != FoodType.None)
                        {
                            int num = 0;
                            if (_Player._FoodType == FoodType.Pizza)
                                num = 0;
                            if (_Player._FoodType == FoodType.Banana)
                                num = 2;
                            if (_Player._FoodType == FoodType.Orange)
                                num = 1;

                            _Player.Shoot(SPEED);
                            Food f = _TypeList[num].Clone();
                            f.Thrown(_Player._Position + new Vector3(0, 1.5f, 0) + _Player._Velocity * 10, _Player._Velocity * 12);
                            _ThrownList.Add(f);
                            _Audio.Play("Kathy", "Thrown");
                        }
                        break;
                    case Keys.E:
                        if (_Player._FoodType != FoodType.None)
                        {
                            int num = 0;
                            if (_Player._FoodType == FoodType.Pizza)
                                num = 0;
                            if (_Player._FoodType == FoodType.Banana)
                                num = 2;
                            if (_Player._FoodType == FoodType.Orange)
                                num = 1;

                            Food f = _TypeList[num].Clone();
                            Console.WriteLine(f._Strength);
                            _Player.Eat(f._Strength);
                        }
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
