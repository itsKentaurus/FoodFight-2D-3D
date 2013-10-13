﻿#region Using Statements
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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace Asg2_6262732
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Fields

        #region Pre-Defined Class
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        #endregion

        #region Defined Class
        Character _kathy;
        UI _UI;
        Food _IceCreamCone;
        HealthBar _HealthBar;   
        Text _Points;
        Text _LevelAnnounce;
        Audio _Audio;
        Cursor _Mouse;
        #endregion

        #region Update
        List<Food> _foodList;
        List<Hole> _holeList;
        List<Text> _TextList;
        #endregion

        #region Menu
        GameState _CurrentGameState = GameState.MainMenu;
        GameState _PreviouysGameState = GameState.Options;
        Texture2D Menu;
        Texture2D _Credits;
        Texture2D _GameOver;
        Button btnPlay;
        Button _Exit;
        Button _Resume;
        Button _Hotkeys;
        Button _Credit;
        Button _BackToStart;
        Button _FullScreen;
        KeyboardState _CurrentKey;
        KeyboardState _PreviousKey;
        #endregion

        #region Game Set
        bool _Set = false;
        int level = 1;
        #endregion

        #region Timer
        float elapse;
        #endregion

        #endregion

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            #region UI
            _UI = new UI();
            _UI.Initialize(Content, graphics, 96, 90);
            #endregion

            #region Kathy
            _kathy = new Character(Content.Load<Texture2D>("Images/Character/Kathy"), _UI._Bounds, 5, 200);
            #endregion

            #region Holes
            _holeList = new List<Hole>();
            #endregion

            #region Food
            _foodList = new List<Food>();
            #endregion

            #region Menus
            graphics.ApplyChanges();
            
            Menu = Content.Load<Texture2D>("Images/Menus/Screen/Menu");
            _Credits = Content.Load<Texture2D>("Images/Menus/Screen/Credits");
            _GameOver = Content.Load<Texture2D>("Images/Menus/Screen/GameOver");
            btnPlay = new Button(Content.Load<Texture2D>("Images/Menus/Button/Button"), graphics.GraphicsDevice);
            _Exit = new Button(Content.Load<Texture2D>("Images/Menus/Button/Exit"), graphics.GraphicsDevice);


            _Resume = new Button(Content.Load<Texture2D>("Images/Menus/Button/Resume"), graphics.GraphicsDevice);
            _Hotkeys = new Button(Content.Load<Texture2D>("Images/Menus/Button/Hotkeys"), graphics.GraphicsDevice);
            _Credit = new Button(Content.Load<Texture2D>("Images/Menus/Button/Credits"), graphics.GraphicsDevice);
            _BackToStart = new Button(Content.Load<Texture2D>("Images/Menus/Button/BackToStart"), graphics.GraphicsDevice);
            Texture2D[] t = new Texture2D[2];
            t[0] = Content.Load<Texture2D>("Images/Menus/Screen/FullScreenOff");
            t[1] = Content.Load<Texture2D>("Images/Menus/Screen/FullScreenOn");

            _FullScreen = new Button(t, graphics.GraphicsDevice, 2);
            #endregion

            #region Audio
            _Audio = new Audio();
            _Audio.Initialize();
            #endregion

            #region Text
            _TextList = new List<Text>();
            _Points = new Text("000", new Vector2(55, 20));
            _TextList.Add(_Points);
            #endregion

            #region HealthBar
            _HealthBar = new HealthBar();
            _HealthBar.Initialize(Content.Load<Texture2D>("Images/HealthBar/HealthBar"), new Vector2(graphics.PreferredBackBufferWidth - 55, 0) ,_kathy.GetHealth(), _UI.BoxHeight);
            #endregion

            #region Ice Cream
            _IceCreamCone = new Food();
            #endregion
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("font");
//            graphics.IsFullScreen = _FullScreenToggle;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            _PreviousKey = _CurrentKey;
            _CurrentKey = Keyboard.GetState();

            switch (_CurrentGameState)
            {
                #region Main Menu
                case GameState.MainMenu:
                    elapse = 0;
                    FirstTimeSetup(7 - level, 7 + level);
                    _Audio.StartGame();
                    _HealthBar.Reset();
                    _PreviouysGameState = GameState.MainMenu;
                    btnPlay.SetPosition(new Vector2((graphics.PreferredBackBufferWidth / 100 ) * 70, (graphics.PreferredBackBufferHeight / 100 ) * 70));
                    _FullScreen.SetPosition(new Vector2((graphics.PreferredBackBufferWidth / 100) * 70, (graphics.PreferredBackBufferHeight / 100) * 75));
                    _Hotkeys.SetPosition(new Vector2((graphics.PreferredBackBufferWidth / 100) * 70, (graphics.PreferredBackBufferHeight / 100) * 80));
                    _Credit.SetPosition(new Vector2((graphics.PreferredBackBufferWidth / 100) * 70, (graphics.PreferredBackBufferHeight / 100) * 85));
                    _Exit.SetPosition(new Vector2((graphics.PreferredBackBufferWidth / 100) * 70, (graphics.PreferredBackBufferHeight / 100) * 90));
                    if (btnPlay.isClicked == true)
                    {
                        _CurrentGameState = GameState.Playing;
                        btnPlay.isClicked = false;
                    }
                    if (_FullScreen.isClicked == true){
                        graphics.ToggleFullScreen();
                        graphics.ApplyChanges();
                    }
                    if (_Exit.isClicked == true)
                        Exit();
                    if (_Hotkeys.isClicked == true)
                        _CurrentGameState = GameState.Hotkeys;
                    if (_Credit.isClicked == true)
                        _CurrentGameState = GameState.Credits;
                    _FullScreen.Update(mouse);
                    btnPlay.Update(mouse);
                    _Hotkeys.Update(mouse);
                    _Credit.Update(mouse);
                    _Exit.Update(mouse);
                    break;
                #endregion

                #region Counting
                case GameState.Counting:
                    _Audio.StartVictory();
                    if (_Audio._Victory.IsPlaying) return;
                    elapse += (float) gameTime.ElapsedGameTime.TotalSeconds;
                    if ( elapse > 1 && _foodList.Count > 0)
                    {
                        Food f = _foodList[0];
                        if (f._FoodDamage == 10)
                            _Points.SetText(Convert.ToString(Convert.ToInt32(_Points.GetText()) + 200 * f._Amount));
                        if (f._FoodDamage == 7)
                            _Points.SetText(Convert.ToString(Convert.ToInt32(_Points.GetText()) + 100 * f._Amount));
                        if (f._FoodDamage == 5)
                            _Points.SetText(Convert.ToString(Convert.ToInt32(_Points.GetText()) + 50 * f._Amount));
                        _foodList.Remove(f);
                        elapse -= 1;
                        _Audio.Play("Ping");
                    }
                    else if (_foodList.Count <= 0)
                    {
                        level++;
                        elapse = 0;
                        _CurrentGameState = GameState.NextLevel;
                        _Audio.StartNextLevel();
                   }
                break;
                #endregion

                #region Next Level
                case GameState.NextLevel:
                _LevelAnnounce.SetText("Level " + level);
                if (!_Audio._NextLevel.IsPlaying)
                {
                    Reset();
                    elapse = 0;
                    _CurrentGameState = GameState.Playing;
                    FirstTimeSetup(7 - level, 7 + level);
                    _Audio.ResetSounds();
                }
                break;
                #endregion

                #region Playing
                case GameState.Playing:
                    #region Declarations
                    _PreviouysGameState = GameState.Playing;
                    _Resume.SetPosition(new Vector2((graphics.PreferredBackBufferWidth / 100) * 45, (graphics.PreferredBackBufferHeight / 100) * 35));
                    int eCount = 0;
                    Random r = new Random();
                    List<Food> empty = new List<Food>();
                    #endregion

                    if (_PreviousKey.IsKeyDown(Keys.Escape) && _CurrentKey.IsKeyUp(Keys.Escape))
                        _CurrentGameState = GameState.Options;

                    #region Kathy Update
                    _kathy.Update();
                    if ((_PreviousKey.IsKeyDown(Keys.Space) && _CurrentKey.IsKeyUp(Keys.Space)) && _kathy._hasThrownFood)
                    {
                        _kathy.Shoot();
                        _Audio.Play("Throw");
                    }
                    if (_PreviousKey.IsKeyDown(Keys.E) && _CurrentKey.IsKeyUp(Keys.E) && _kathy._hasThrownFood &&
                        _HealthBar._CurrentDamage != 11)
                        _HealthBar.IncreaseHp(_kathy.EatFood());
                    #endregion

                    #region Timer Update
                    elapse += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    float pass = 30 /24;
                    if (elapse > pass)
                    {
                        elapse -= pass;
                        if (_IceCreamCone.ReduceOne())
                            _CurrentGameState = GameState.GameOver;
                    }
                    #endregion

                    #region Food Update
                    foreach (Food food in _foodList)
                        _kathy.PickUp(Keyboard.GetState(), food);

                    foreach (Food food in _foodList)
                        if (!food.HasFood())
                            empty.Add(food);
                    foreach (Food node in empty)
                        _foodList.Remove(node);
                    #endregion

                    #region Hole Update

                    foreach (Hole node in _holeList)
                    {
                        node.Update();
                        foreach (Enemy e in node._Enemy)
                        {
                            e.Update(ref _kathy, (int)node._Position.Y, gameTime, _UI._Bounds);
                            foreach (ThrownFood to in _kathy._FoodThrown)
                                if (e._Rectangle.Intersects(to._Rectangle))
                                {
                                    node.deadEnemy.Add(e);
                                    _kathy.temp.Add(to);
                                    _Points.SetText(Convert.ToString(Convert.ToInt32(_Points.GetText()) + 100));
                                    _Audio.Play("Hit");
                                }

                            foreach(ThrownFood from in e._FoodThrown)
                                if (_kathy._Rec.Intersects(from._Rectangle))
                                {
                                    e.temp.Add(from);
                                    _HealthBar.DecreaseHp(from._FoodDamage);
                                }
                        }
                        eCount += node._Enemy.Count;
                    }

                   for (int i = eCount; i < 3 + level; ++i)
                    {
                        _holeList[r.Next(_holeList.Count)].SpawnEnemy();
                        _Audio.Play("Summon");
                    }
                    #endregion

                    #region Health Update
                    _HealthBar.Update();
                    if (_HealthBar._MaxHealth == _HealthBar._CurrentDamage)
                        _CurrentGameState = GameState.GameOver;
                    #endregion

                    #region Ice Cream Collision
                    if (_IceCreamCone._Rectangle.Intersects(_kathy._Rec))
                        _CurrentGameState = GameState.Counting;
                    #endregion

                    break;

                #endregion

                #region Options
                case GameState.Options:
                    _PreviouysGameState = GameState.Options;
                    if (_Resume.isClicked == true || (_PreviousKey.IsKeyDown(Keys.Escape) == true && _CurrentKey.IsKeyUp(Keys.Escape) == true))
                        _CurrentGameState = GameState.Playing;
                    if (_Hotkeys.isClicked == true)
                        _CurrentGameState = GameState.Hotkeys;
                    if (_Exit.isClicked == true)
                        Exit();
                    _Resume.Update(mouse);
                    _Hotkeys.SetPosition(new Vector2((graphics.PreferredBackBufferWidth / 100) * 45, (graphics.PreferredBackBufferHeight / 100) * 40));
                    _Exit.SetPosition(new Vector2((graphics.PreferredBackBufferWidth / 100) * 45, (graphics.PreferredBackBufferHeight / 100) * 45));
                    _Hotkeys.Update(mouse);
                    _Exit.Update(mouse);
                    break;
                #endregion

                #region Credits
                case GameState.Credits:
                    if (_PreviousKey.IsKeyDown(Keys.Escape) && _CurrentKey.IsKeyUp(Keys.Escape))
                    {
                        _CurrentGameState = _PreviouysGameState;
                        _PreviouysGameState = GameState.Empty;
                    }
                    if (_BackToStart.isClicked == true)
                        _CurrentGameState = _PreviouysGameState;
                    _BackToStart.SetPosition(new Vector2((graphics.PreferredBackBufferWidth / 100) * 45, (graphics.PreferredBackBufferHeight / 100) * 50));
                    _BackToStart.Update(mouse);
                    break;
                #endregion

                #region Hotkeys
                case GameState.Hotkeys:
                    if (_BackToStart.isClicked == true)
                        _CurrentGameState = _PreviouysGameState;
                    if (_PreviousKey.IsKeyDown(Keys.Escape)&& _CurrentKey.IsKeyUp(Keys.Escape))
                    {
                        _CurrentGameState = _PreviouysGameState;
                        _PreviouysGameState = GameState.Empty;
                    }
                    _BackToStart.SetPosition(new Vector2((graphics.PreferredBackBufferWidth / 100) * 45, (graphics.PreferredBackBufferHeight / 100) * 50));
                    _BackToStart.Update(mouse);
                    break;
                #endregion

                #region Game Over
                case GameState.GameOver:
                    Reset();
                    _BackToStart.SetPosition(new Vector2((graphics.PreferredBackBufferWidth / 100) * 45, (graphics.PreferredBackBufferHeight / 100) * 45));
                    _Exit.SetPosition(new Vector2((graphics.PreferredBackBufferWidth / 100) * 45, (graphics.PreferredBackBufferHeight / 100) * 50));
                    _Exit.Update(mouse);
                    _BackToStart.Update(mouse);
                    _Audio.StartGameOver();
                    if (_Exit.isClicked == true)
                        Exit();
                    if (_BackToStart.isClicked == true)
                    {
                        _CurrentGameState = GameState.MainMenu;
                        _BackToStart.isClicked = false;
                    }
                    break;
                #endregion
            }

            #region Audio
            _Audio.Update();
            #endregion

            #region Mouse
            _Mouse._Position.X = Mouse.GetState().X;
            _Mouse._Position.Y = Mouse.GetState().Y;
            if (_Mouse._Position.X < 0 || _Mouse._Position.Y < 0 || _Mouse._Position.X > graphics.PreferredBackBufferWidth || _Mouse._Position.Y > graphics.PreferredBackBufferHeight)
                IsMouseVisible = true;
            else
                IsMouseVisible = false;
            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (_CurrentGameState)
            {
                #region Main Menu
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Menu, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.End();
                    btnPlay.Draw(spriteBatch);
                    _Hotkeys.Draw(spriteBatch);
                    _Credit.Draw(spriteBatch);
                    _Exit.Draw(spriteBatch);
                    _FullScreen.Draw(spriteBatch);
                    _Mouse.Draw(spriteBatch);
                    break;
                #endregion
                #region Options
                case GameState.Options:
                    _Resume.Draw(spriteBatch);
                    _Hotkeys.Draw(spriteBatch);
                   _Exit.Draw(spriteBatch);
                    _Mouse.Draw(spriteBatch);
                    break;
               #endregion

                #region Counting
                case GameState.Counting:
                    _UI.Draw(spriteBatch);
                    foreach (Food node in _foodList)
                        node.Draw(spriteBatch);
                    foreach (Text node in _TextList)
                        node.Draw(spriteBatch, spriteFont, Color.White);
                    _IceCreamCone.Draw(spriteBatch);
                    _HealthBar.Draw(spriteBatch);
                    _kathy.Draw(spriteBatch);
                break;
                #endregion

                #region Next Level
                case GameState.NextLevel:
                _LevelAnnounce.Draw(spriteBatch, spriteFont, Color.White);
                break;
                #endregion

                #region Playing
                case GameState.Playing:
                    _UI.Draw(spriteBatch);
                    _IceCreamCone.Draw(spriteBatch);
                    foreach (Food node in _foodList)
                        node.Draw(spriteBatch);
                    foreach (Hole node in _holeList)
                        node.Draw(spriteBatch);
                    foreach (Text node in _TextList)
                       node.Draw(spriteBatch, spriteFont, Color.White);
                    _HealthBar.Draw(spriteBatch);
                    _kathy.Draw(spriteBatch);
                    break;
                #endregion

                #region Credits
                case GameState.Credits:
                    spriteBatch.Begin();
                    spriteBatch.Draw(_Credits, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.End();
                    _BackToStart.Draw(spriteBatch);
                    _Mouse.Draw(spriteBatch);
                    break;
                #endregion

                #region Hotkeys
                case GameState.Hotkeys:
                    _BackToStart.Draw(spriteBatch);
                    _Mouse.Draw(spriteBatch);
                    break;
                #endregion

                #region GameOver
                case GameState.GameOver:
                   spriteBatch.Begin();
                    spriteBatch.Draw(_GameOver, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.End();
                    _Exit.Draw(spriteBatch);
                    _BackToStart.Draw(spriteBatch);
                    _Mouse.Draw(spriteBatch);
                    break;
               #endregion
            }



            base.Draw(gameTime);
        }
        private void FirstTimeSetup(int NumFood, int NumHoles)
        {
            if (_Set) return;
            _Set = true;


            _kathy.SetPosition(new Vector2(_UI._Bounds.Right - 50, ((_UI._Bounds.Top + _UI._Bounds.Bottom) / 2) - 25));
            /*  Set Ground Banana Texture */
            Texture2D[] Banana = new Texture2D[7];
            Banana[0] = Content.Load<Texture2D>("Images/General/Pile");
            Banana[1] = Content.Load<Texture2D>("Images/Food/PileBanana/PileBanana1");
            Banana[2] = Content.Load<Texture2D>("Images/Food/PileBanana/PileBanana2");
            Banana[3] = Content.Load<Texture2D>("Images/Food/PileBanana/PileBanana3");
            Banana[4] = Content.Load<Texture2D>("Images/Food/PileBanana/PileBanana4");
            Banana[5] = Content.Load<Texture2D>("Images/Food/PileBanana/PileBanana5");
            Banana[6] = Content.Load<Texture2D>("Images/Food/PileBanana/PileBanana6");
            /*  Set Ground Pizza Texture */
            Texture2D[] Pizza = new Texture2D[7];
           Pizza[0] = Content.Load<Texture2D>("Images/General/Pile");
           Pizza[1] = Content.Load<Texture2D>("Images/Food/PilePizza/PilePizza1");
           Pizza[2] = Content.Load<Texture2D>("Images/Food/PilePizza/PilePizza2");
           Pizza[3] = Content.Load<Texture2D>("Images/Food/PilePizza/PilePizza3");
           Pizza[4] = Content.Load<Texture2D>("Images/Food/PilePizza/PilePizza4");
           Pizza[5] = Content.Load<Texture2D>("Images/Food/PilePizza/PilePizza5");
           Pizza[6] = Content.Load<Texture2D>("Images/Food/PilePizza/PilePizza6");
            /*  Set Ground Orange Texture */
            Texture2D[] Orange = new Texture2D[7];
           Orange[0] = Content.Load<Texture2D>("Images/General/Pile");
           Orange[1] = Content.Load<Texture2D>("Images/Food/PileOrange/PileOrange1");
           Orange[2] = Content.Load<Texture2D>("Images/Food/PileOrange/PileOrange2");
           Orange[3] = Content.Load<Texture2D>("Images/Food/PileOrange/PileOrange3");
           Orange[4] = Content.Load<Texture2D>("Images/Food/PileOrange/PileOrange4");
           Orange[5] = Content.Load<Texture2D>("Images/Food/PileOrange/PileOrange5");
           Orange[6] = Content.Load<Texture2D>("Images/Food/PileOrange/PileOrange6");

            Random rand = new Random();
            List<Texture2D[]> _FOOD = new List<Texture2D[]>();
            _FOOD.Add(Orange);
            _FOOD.Add(Pizza);
            _FOOD.Add(Banana);
            List<Texture2D> _SingleFOOD = new List<Texture2D>();
            _SingleFOOD.Add(Content.Load<Texture2D>("Images/Food/ThrownObject/Orange"));
            _SingleFOOD.Add(Content.Load<Texture2D>("Images/Food/ThrownObject/Pizza"));
            _SingleFOOD.Add(Content.Load<Texture2D>("Images/Food/ThrownObject/banana"));
           int[] health = new int[3];
            health[0] = 5;
            health[1] = 10;
            health[2] = 7;
            for (int i = 0; i < NumFood; ++i)
            {
                Vector2 spawn = new Vector2(rand.Next(_UI.BoxWidth - 70) + 20, rand.Next(_UI.BoxHeight - 70) + 10) +
                    new Vector2((graphics.PreferredBackBufferWidth - _UI.BoxWidth) / 2, (graphics.PreferredBackBufferHeight - _UI.BoxHeight) / 2 + _UI.HorizontalBar.Height);
                Food f = new Food();
                int q = rand.Next(3);
                f.Initialize(spawn,
                    _FOOD[q],
                    _SingleFOOD[q],
                    rand.Next(5) + 2,
                    health[q]);
               for (int j = 0; j < _foodList.Count; ++j )
                {
                    while (_foodList[j]._Rectangle.Intersects(f._Rectangle))
                    {
                        spawn = new Vector2(rand.Next(_UI.BoxWidth - 70) + 20, rand.Next(_UI.BoxHeight - 70) + 10) +
                        new Vector2((graphics.PreferredBackBufferWidth - _UI.BoxWidth) / 2, (graphics.PreferredBackBufferHeight - _UI.BoxHeight) / 2 + _UI.HorizontalBar.Height);
                        f.SetPosition(spawn);
                        j = 0;
                    }
               }
                _foodList.Add(f);
            }

            Food[] ef = new Food[3];
            ef[0] = new Food();
            ef[1] = new Food();
            ef[2] = new Food();
            ef[0].EnemyInitialize(_SingleFOOD[0], 256, 5);
            ef[1].EnemyInitialize(_SingleFOOD[1], 256, 10);
            ef[2].EnemyInitialize(_SingleFOOD[2], 256, 7);
            for (int i = 0; i < NumHoles; ++i)
            {
                Vector2 spawn = new Vector2(rand.Next(_UI.BoxWidth - 70) + 20, rand.Next(_UI.BoxHeight - 70) + 10) +
                    new Vector2((graphics.PreferredBackBufferWidth - _UI.BoxWidth) / 2, (graphics.PreferredBackBufferHeight - _UI.BoxHeight) / 2 + _UI.HorizontalBar.Height);
                Hole hole = new Hole();
               hole.Initialize(spawn,
                    Content.Load<Texture2D>("Images/General/Hole"),
                    Content.Load<Texture2D>("Images/Character/Chef"),
                    ef);
                bool clear = false;
                while (!clear)
                {
                    clear = true;
                    for (int j = 0; j < _foodList.Count; ++j)
                   {
                        while (_foodList[j]._Rectangle.Intersects(hole._Rectangle))
                        {
                            spawn = new Vector2(rand.Next(_UI.BoxWidth - 70) + 20, rand.Next(_UI.BoxHeight - 70) + 10) +
                            new Vector2((graphics.PreferredBackBufferWidth - _UI.BoxWidth) / 2, (graphics.PreferredBackBufferHeight - _UI.BoxHeight) / 2 + _UI.HorizontalBar.Height);
                            hole.SetPosition(spawn);
                            j = 0;
                        }
                   }
                    for (int j = 0; j < _holeList.Count; ++j)
                    {
                        while (_holeList[j]._Rectangle.Intersects(hole._Rectangle))
                        {
                            spawn = new Vector2(rand.Next(_UI.BoxWidth - 70) + 20, rand.Next(_UI.BoxHeight - 70) + 10) +
                            new Vector2((graphics.PreferredBackBufferWidth - _UI.BoxWidth) / 2, (graphics.PreferredBackBufferHeight - _UI.BoxHeight) / 2 + _UI.HorizontalBar.Height);
                            hole.SetPosition(spawn);
                            j = 0;
                            clear = false;
                        }
                       if (!clear)
                            break;
                    }
                }
                _holeList.Add(hole);
            }
            Texture2D IceCreamCone = Content.Load<Texture2D>("Images/Food/IceCreamCone/IceCream");
            _IceCreamCone.Initialize(new Vector2(_UI._Bounds.Left, rand.Next(_UI.BoxHeight) + _UI._Bounds.Top), IceCreamCone, 24);
            /*  Mouse   */
            _Mouse = new Cursor();
            _Mouse.Initialize(Content.Load<Texture2D>("Images/Cursor/Cursor"));

            /*  Level Display   */
            _LevelAnnounce = new Text("", new Vector2(graphics.PreferredBackBufferWidth/2, graphics.PreferredBackBufferHeight/2));

        }

        public void Reset()
        {
            if (!_Set) return;
            _Set = false;
           foreach (Hole node in _holeList)
            {
                node.Update();
                foreach (Enemy e in node._Enemy)
                {
                    foreach (ThrownFood to in _kathy._FoodThrown)
                        _kathy.temp.Add(to);
                    foreach (ThrownFood from in e._FoodThrown)
                        e.temp.Add(from);
                    node.deadEnemy.Add(e);
                }
           }
            List<Food> empty = new List<Food>();
            List<Hole> emp = new List<Hole>();
            foreach (Hole node in _holeList)
               emp.Add(node);
            foreach (Hole node in emp)
                _holeList.Remove(node);
            foreach (Food food in _foodList)
                if (food.HasFood())
                    empty.Add(food);
            foreach (Food node in empty)
                _foodList.Remove(node);

        }
    }
}
