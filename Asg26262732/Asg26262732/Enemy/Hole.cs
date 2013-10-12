using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Asg26262732
{
    public class Hole
    {
        #region Fields
        public Vector2 _Position
        {
            get;
            private set;
        }
        public int _EnemyCount
        {
            get;
            private set;
        }
        public Rectangle _Rectangle
        {
            get;
            private set;
        }
        public Texture2D _Texture
        {
            get;
            private set;
        }
        public List<Enemy> _Enemy
        {
            get;
            private set;
        }
        public Enemy _SingleEnemy
        {
            get;
            private set;
        }
        public Food[] _Food
        {
            get;
            private set;
        }
        public List<Enemy> deadEnemy;
        #endregion

        #region Method
        public void Initialize(Vector2 _Position, Texture2D _Hole, Texture2D Enemy, Food[] _Food)
        {
            this._Food = _Food;
            this._Position = _Position;
            this._Texture = _Hole;
            this._Rectangle = new Rectangle((int)_Position.X, (int)_Position.Y, (int)_Texture.Width, (int)_Texture.Height);
            this._SingleEnemy = new Enemy();
            this._SingleEnemy.Initialize(_Position, Enemy);
            _Enemy = new List<Enemy>();
            deadEnemy = new List<Enemy>();
        }
        public void SetPosition(Vector2 v)
        {
            this._SingleEnemy.SetPosition(v);
            _Position = v;
            this._Rectangle = new Rectangle((int)_Position.X, (int)_Position.Y, (int)_Texture.Width, (int)_Texture.Height);
        }
        public void Update()
        {

            this._Rectangle = new Rectangle((int)_Position.X, (int)_Position.Y, (int)_Texture.Width, (int)_Texture.Height);

            foreach (Enemy e in deadEnemy)
                _Enemy.Remove(e);
        }
        public void SpawnEnemy()
        {
            if ((_Enemy.Count > 0 && _Enemy[_Enemy.Count-1]._Status != EnemyStatus.Spawning) || _Enemy.Count == 0)
                _Enemy.Add(_SingleEnemy.Clone(_Food));
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(_Texture, _Position, Color.White);
            foreach (Enemy e in _Enemy)
                e.Draw(sb);
            sb.End();
        }
        #endregion

    }
}
