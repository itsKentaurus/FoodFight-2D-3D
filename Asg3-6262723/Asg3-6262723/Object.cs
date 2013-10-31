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
        public Object(Model Model, BoundingBox Bound)
        {
            _Model = Model;
            _Bound = Bound;
        }
        public Object(Model Model, Vector3 MinVec, Vector3 MaxVec)
        {
            _Bound = new BoundingBox(MinVec, MaxVec);
            _Model = Model;
            _World = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        }
        public void UpdatePosition(Vector3 Position)
        {
            _Bound = new BoundingBox(_Bound.Min + Position, _Bound.Max + Position);
        }
        public Object(Model Model, Vector3 Position, Vector3 MinVec, Vector3 MaxVec)
        {
            _Bound = new BoundingBox(MinVec, MaxVec);
            _Model = Model;
            _World = Matrix.CreateTranslation(Position);
            _Position = Position;
        }
    }
}
