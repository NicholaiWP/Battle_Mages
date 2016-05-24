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
            scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(50, 20), new Golem(null)));
        }

        public void NewWave()
        {
            scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(25, 30), new Golem(null)));
            scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(-20, -30), new Orb(null)));
            scene.AddObject(ObjectBuilder.BuildEnemy(new Vector2(120, 90), new Slime(null)));
        }
    }
}