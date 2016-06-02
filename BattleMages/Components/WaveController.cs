using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class WaveController : Component
    {
        private int waveNumber;
        private List<Wave> waves;

        public WaveController(List<Wave> waves)
        {
            waveNumber = 0;
            this.waves = new List<Wave>(waves);
        }

        public void UpdateWave()
        {
            if (waves.Count > waveNumber)
            {
                for (int i = 0; i < waves[waveNumber].Enemies.Count; i++)
                {
                    GameWorld.Scene.AddObject(ObjectBuilder.BuildEnemy(waves[waveNumber].positions[i],
                        waves[waveNumber].Enemies[i]));
                }
                waveNumber++;
            }
            else
            {
                GameWorld.State.Save();
                GameWorld.ChangeScene(new LobbyScene());
            }
        }
    }
}