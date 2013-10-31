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
        private Chef(Chef Chef, Vector3 Position)
            : base(Chef._Model, Position, Chef._Bound.Min, Chef._Bound.Max)
        {
            _Direction = new Vector3(0, 0, -1);
        }
        public Chef(Model Model, Vector3 MinVec, Vector3 MaxVec)
            : base(Model, MinVec, MaxVec)
        {
            _Direction = new Vector3(0, 0, -1);
        }

        public Chef(Model Model, Vector3 Position, Vector3 MinVec, Vector3 MaxVec)
            : base(Model,Position, MinVec, MaxVec)
        {
            _Direction = new Vector3(0, 0, -1);
        }

        public void Update(GameTime gameTime, Vector3 PlayerPos)
        {
            Aim(PlayerPos);
            _World = Matrix.CreateRotationY(_Angle) * Matrix.CreateTranslation(_Position);
        }

        private void Aim(Vector3 PlayerPos)
        {

            Vector3 PlayerDirection = Vector3.Normalize( PlayerPos - _Position);
            Vector3 MyDirection = Vector3.Normalize(_Direction);
            float a = (float)Math.Sqrt((double)Math.Pow((double)PlayerDirection.X, 2.0) + Math.Pow((double)PlayerDirection.Y, 2.0) + Math.Pow((double)PlayerDirection.Z, 2.0));
            float b = (float)Math.Sqrt((double)Math.Pow((double)_Direction.X, 2.0) + Math.Pow((double)_Direction.Y, 2.0) + Math.Pow((double)_Direction.Z, 2.0));
            float dot = Vector3.Dot(PlayerDirection, _Direction);

            _Angle = (float) Math.Acos( dot / (a*b));
            if (PlayerDirection.X > 0)
                _Angle *= -1;

        }

        internal Chef Clone(Vector3 Position)
        {
            Chef c = new Chef(this, Position);
            return c;
        }
    }
}
