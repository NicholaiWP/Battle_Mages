using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class WaveController : Component
    {
        private GameScene scene;
        private int wave;

        public WaveController(GameObject gameObject, GameScene scene) : base(gameObject)
        {
            this.scene = scene;
            scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(50, 50), EnemyType.Ranged, true));
            /* scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(100, 50), EnemyType.CloseRange));
             scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(150, 70), EnemyType.Ranged));
             scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(10, 20), EnemyType.CloseRange));
             scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(-10, 30), EnemyType.Ranged));
             scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(250, -50), EnemyType.Ranged));*/
        }

        public void NewWave()
        {
            scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(50, 50), EnemyType.Ranged, true));
            scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(100, 50), EnemyType.CloseRange, true));
            /*scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(50, 50), EnemyType.Ranged));
            scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(100, 50), EnemyType.CloseRange));
            scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(150, 70), EnemyType.Ranged));
            scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(10, 20), EnemyType.CloseRange));
            scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(-10, 30), EnemyType.Ranged));
            scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(250, -50), EnemyType.Ranged));
            */
        }
    }
}