using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Asg3_6262723
{
    class Camera
    {
        #region Fields
        public Matrix _Position
        {
            get;
            private set;
        }
        public int _VisionToggle
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public Camera()
        {
            _Position = Matrix.CreateLookAt(new Vector3(10, 5, 10), Vector3.Zero, Vector3.UnitY);
            _VisionToggle = 3;
        }
        public void Update(GameTime gameTime, Character Player)
        {
            if (_VisionToggle == 3)
                _Position = Matrix.CreateLookAt(new Vector3(10 * (float)Math.Cos(0) * (float)Math.Sin(Player._Angle),
                                            5,
                                            10 * (float)Math.Cos(Player._Angle)) + Player._Position, Player._Position, Vector3.UnitY);
            if (_VisionToggle == 1)
                _Position = Matrix.CreateLookAt(Player._Position, Player._Position + new Vector3(-10 * (float)Math.Cos(0) * (float)Math.Sin(Player._Angle),
                                            -0,
                                            -10 * (float)Math.Cos(Player._Angle)), Vector3.UnitY);
        }
        public void ToggleFirst()
        {
            _VisionToggle = 1;
        }
        public void ToggleThird()
        {
            _VisionToggle = 3;
        }
        #endregion
    }
}
