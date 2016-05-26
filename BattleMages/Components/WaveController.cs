using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class WaveController : Component
    {
        private Dictionary<int, Wave> waves = new Dictionary<int, Wave>();

        public WaveController(GameObject gameObject) : base(gameObject)
        {
            waves.Add(1, new Wave(new List<Vector2> { new Vector2(25,30),
                new Vector2(-20, -30), new Vector2(120, 90)},
                new List<Enemy> { new Golem(null), new Orb(null), new Slime(null) }));

            waves.Add(2, new Wave(new List<Vector2> { new Vector2(300, 0), new Vector2(0, 300), new Vector2(-300, 0), new Vector2(0, -300) },
                new List<Enemy> { new Golem(null),new Golem(null),new Golem(null),new Golem(null),}));

            waves.Add(3, new Wave(new List<Vector2> { new Vector2(200,0), new Vector2(200, 10), new Vector2(200, -10), new Vector2(200, 20),
            new Vector2(200,-20), new Vector2(210,0), new Vector2(190,0), new Vector2(220,0), new Vector2(180,0)},
            new List<Enemy> { new Orb(null), new Orb(null), new Orb(null), new Orb(null), new Orb(null),
            new Orb(null),new Orb(null),new Orb(null),new Orb(null) }));
        }


        public void NewWave(int waveNumber)
        {
            if (waves.ContainsKey(waveNumber))
            {
                for (int i = 0; i < waves[waveNumber].enemies.Count; i++)
                {
                    GameWorld.Scene.AddObject(ObjectBuilder.BuildEnemy(waves[waveNumber].positions[i],
                        waves[waveNumber].enemies[i]));
                }
            }
            else
            {
                GameWorld.ChangeScene(new LobbyScene());
            }
        }
    }
}