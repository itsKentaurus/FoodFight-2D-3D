using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asg3_6262723
{
    class EnemyHealth : Object
    {
        private Chef _Chef;
        private int _ChefHealth;
        public EnemyHealth(Model Model, Vector3 Position, ref Chef Chef, int ChefHealth)
            : base(Model, Position)
        {
            _Chef = Chef;
            _ChefHealth = ChefHealth;
        }

        public void Update()
        {
            Size = _Chef._Strength / _ChefHealth;
            _Position = _Chef._Position;
        }
    }
}
