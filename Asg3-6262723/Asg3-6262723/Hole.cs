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

namespace Asg3_6262723
{
    class Hole : Object
    {
        #region Fields
        public List<Chef> _ChefList
        {
            get;
            private set;
        }
        public Chef _Chef
        {
            get;
            private set;
        }
        public bool _Moving
        {
            get;
            private set;
        }
        public float _Elapse
        {
            get;
            private set;
        }
        public float _Interval
        {
            get;
            private set;
        }
        Random r = new Random();
        public int _MinMove
        {
            get;
            private set;
        }
        public int _MaxMove
        {
            get;
            private set;
        }
        public int _RespawnTimer
        {
            get;
            private set;
        }
        public int _MinSpawn
        {
            get;
            private set;
        }
        public int _MaxSpawn
        {
            get;
            private set;
        }
        public int _ElapseTimer
        {
            get;
            private set;
        }
        public bool _StartSpawn
        {
            get;
            private set;
        }
        
        #endregion

        #region Methods
        public Hole(Model Model, Vector3 Position, Vector3 MinVec, Vector3 MaxVec, Chef Chef, int MinMove, int MaxMove, int MinSpawn, int MaxSpawn)
            : base(Model, Position, MinVec, MaxVec)
        {
            _MinMove = MinMove;
            _MaxMove = MaxMove;
            _MinSpawn = MinSpawn;
            _MaxSpawn = MaxSpawn;
            _Elapse = 0;
            _ChefList = new List<Chef>();
            _Moving = false;
            _Chef = Chef;
            _World = Matrix.CreateRotationX((float)-(Math.PI / 2)) * (Matrix.CreateTranslation(_Position));
            _Interval = r.Next(_MinMove, _MaxMove);
            _RespawnTimer = r.Next(_MinSpawn, _MaxSpawn);
            _StartSpawn = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!_Moving && _ChefList.Count > 0)
            {
                _Elapse += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_Elapse > _Interval)
                {
                    _Moving = true;
                    Random r = new Random();
                    _Elapse = 0;
                    _Interval = r.Next(_MinMove, _MaxMove);
                }
            }

            if (_ChefList.Count == 0 && _StartSpawn)
            {
                _Elapse += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_Elapse > _RespawnTimer)
                {
                    Chef c = _Chef.Clone(_Position);
                    _ChefList.Add(c);
                    _RespawnTimer = r.Next(_MinSpawn, _MaxSpawn);
                }
            }

            if (_ChefList.Count > 0 && _ChefList[0]._Strength <= 0)
                Clear();
        }

        public void Summon()
        {
            _StartSpawn = true;
        }
        public void Summon(Vector3 Position, Chef Chef)
        {
            Chef c = _Chef.Clone(Position, Chef);
            c.Move(_Position);
            _ChefList.Add(c);
            _StartSpawn = true;
        }
        public void Clear()
        {
            _Elapse = 0;
            _Moving = false;
            _StartSpawn = false;
            _ChefList.Clear();
        }
        #endregion
    }
}
