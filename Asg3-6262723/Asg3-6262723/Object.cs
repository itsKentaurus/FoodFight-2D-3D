using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asg3_6262723
{
    public class Object
    {
        #region Fields
        public Vector3 _Position
        {
            get;
            protected set;
        }
        public Matrix _World
        {
            get;
            protected set;
        }
        public Model _Model
        {
            get;
            private set;
        }
        public BoundingBox _Bound
        {
            get;
            private set;
        }
        public Vector3 _Min
        {
            get;
            private set;
        }
        public Vector3 _Max
        {
            get;
            private set;
        }
        public int _Strength
        {
            get;
            protected set;
        }
        #endregion

        public Object(Model Model, Vector3 Position)
        {
            _Model = Model;
            _World = Matrix.CreateRotationX((float)-(Math.PI / 2)) * Matrix.CreateTranslation(Position);

        }
        public Object(Model Model, BoundingBox Bound)
        {
            _Model = Model;
            _Bound = Bound;
        }
        public Object(Model Model, Vector3 MinVec, Vector3 MaxVec)
        {
            _Bound = new BoundingBox(MinVec, MaxVec);
            _Min = MinVec;
            _Max = MaxVec;
            _Model = Model;
            _World = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        }
        public void UpdatePosition()
        {
            _Bound = new BoundingBox(_Min + _Position, _Max + _Position);

        }
        public Object(Model Model, Vector3 Position, Vector3 MinVec, Vector3 MaxVec)
        {
            _Bound = new BoundingBox(MinVec + Position, MaxVec + Position);
            _Min = MinVec;
            _Max = MaxVec;
            _Model = Model;
            _Position = Position;
            _World = Matrix.CreateTranslation(Position);
        }
        public void SetPosition(Vector3 Position)
        {
            _Position = Position;
        }
        public void ReduceHealth(int Amount)
        {
            _Strength -= Amount;
        }
        public void GainHealth(int Amount)
        {
            _Strength += Amount;
        }
    }
}
