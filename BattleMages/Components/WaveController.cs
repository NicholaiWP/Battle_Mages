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

        private Dictionary<int, Wave> waves = new Dictionary<int, Wave>();

        public WaveController(GameObject gameObject, GameScene scene) : base(gameObject)
        {
            this.scene = scene;

            waves.Add(1, new Wave(new List<Vector2> { new Vector2(25,30),
                new Vector2(-20, -30), new Vector2(120, 90)},
                new List<Enemy> { new Golem(null), new Orb(null), new Slime(null) }));
        }

        public void NewWave(int waveNumber)
        {
            Random randy = new Random();
            if (waves.ContainsKey(waveNumber))
            {
                foreach (Enemy enemy in waves[waveNumber].enemies)
                {
                    scene.AddObject(ObjectBuilder.BuildEnemy(
                        waves[waveNumber].positions[randy.Next(0, waves[waveNumber].positions.Count)],
                        enemy));
                }
            }
        }
    }
}