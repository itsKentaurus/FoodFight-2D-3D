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

        public Hole(Model Model, Vector3 Position, Vector3 MinVec, Vector3 MaxVec, Chef Chef)
            : base(Model, Position, MinVec, MaxVec)
        {
            _ChefList = new List<Chef>();
            _Chef = Chef;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Summon()
        {
            Chef c = _Chef.Clone(_Position + new Vector3(0,1.5f,0));
            _ChefList.Add(c);
        }
    }
}
