#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Asg3_6262723
{
    class Chef : Object
    {

        public float _Angle
        {
            get;
            private set;
        }
        public Vector3 _Direction
        {
            get;
            private set;
        }
        Vector3 _Velocity;

        public Vector3 _Destination
        {
            get;
            private set;
        }
        public bool _Throw
        {
            get;
            private set;
        }
        public int _ThrowTimer
        {
            get;
            private set;
        }
        public int _MinThrowTimer
        {
            get;
            private set;
        }
        public int _MaxThrowTimer
        {
            get;
            private set;
        }
        Random r = new Random();
        float _Elapse;
        public Vector3 _PlayerDirection
        {
            get;
            private set;
        }

        private Chef(Chef Chef, Vector3 Position)
            : base(Chef._Model, Position, Chef._Bound.Min, Chef._Bound.Max)
        {
            _Direction = new Vector3(0, 0, -1);
            _Destination = _Position;
            _Velocity = Vector3.Zero;
        }
        public Chef(Model Model, Vector3 MinVec, Vector3 MaxVec, int MinThrowTimer, int MaxThrowTimer)
            : base(Model, MinVec, MaxVec)
        {
            _Direction = new Vector3(0, 0, -1);
            _Destination = _Position;
            _Velocity = Vector3.Zero;
            _MinThrowTimer = MinThrowTimer;
            _MaxThrowTimer = MaxThrowTimer;
            _ThrowTimer = r.Next(_MinThrowTimer, _MaxThrowTimer);
            _Throw = false;
        }

        public Chef(Model Model, Vector3 Position, Vector3 MinVec, Vector3 MaxVec, int MinThrowTimer, int MaxThrowTimer)
            : base(Model,Position, MinVec, MaxVec)
        {
            _Direction = new Vector3(0, 0, -1);
            _Destination = _Position;
            _Velocity = Vector3.Zero;
            _MinThrowTimer = MinThrowTimer;
            _MaxThrowTimer = MaxThrowTimer;
            _ThrowTimer = r.Next(_MinThrowTimer, _MaxThrowTimer);
            _Throw = false;
        }

        public void Update(GameTime gameTime, Vector3 PlayerPos)
        {
            Aim(PlayerPos);
            if (Vector3.Distance(_Destination, _Position) >= 0.1)
                _Position += _Velocity / 10;
            else
                _Destination = _Position;

            _World = Matrix.CreateRotationX((float)-(Math.PI / 2)) * (Matrix.CreateRotationY(_Angle + (float)Math.PI) * Matrix.CreateTranslation(_Position));

            if (!_Throw)
            {
                _Elapse += (float) gameTime.ElapsedGameTime.TotalSeconds;

                if (_Elapse > _ThrowTimer)
                {
                    _Throw = true;
                }
            }

            base.UpdatePosition();
        }

        public void Throw()
        {
            _ThrowTimer = 3;
            _Throw = false;
            _Elapse = 0;
        }

        private void Aim(Vector3 PlayerPos)
        {

            _PlayerDirection = Vector3.Normalize( PlayerPos - _Position);
            Vector3 MyDirection = Vector3.Normalize(_Direction);
            float a = (float)Math.Sqrt((double)Math.Pow((double)_PlayerDirection.X, 2.0) + Math.Pow((double)_PlayerDirection.Y, 2.0) + Math.Pow((double)_PlayerDirection.Z, 2.0));
            float b = (float)Math.Sqrt((double)Math.Pow((double)_Direction.X, 2.0) + Math.Pow((double)_Direction.Y, 2.0) + Math.Pow((double)_Direction.Z, 2.0));
            float dot = Vector3.Dot(_PlayerDirection, _Direction);

            _Angle = (float) Math.Acos( dot / (a*b));
            if (_PlayerDirection.X > 0)
                _Angle *= -1;

        }

        public void Move(Vector3 Destination)
        {
            _Destination = Destination;
            _Velocity = Vector3.Normalize(Destination - _Position);
        }

        internal Chef Clone(Vector3 Position)
        {
            Chef c = new Chef(this, Position);
            return c;
        }
        internal Chef Clone(Vector3 Position, Chef Chef)
        {
            Chef c = new Chef(this, Position);
            //To Do Fill in
            return c;
        }
    }
}
