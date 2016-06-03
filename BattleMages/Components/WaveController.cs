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
            WaveNumber = 0;
            this.waves = new List<Wave>(waves);
        }

        public int WaveNumber
        {
            get
            {
                return waveNumber;
            }

            set
            {
                waveNumber = value;
            }
        }

        public void UpdateWave()
        {
            if (waves.Count > WaveNumber)
            {
                for (int i = 0; i < waves[WaveNumber].Enemies.Count; i++)
                {
                    GameWorld.Scene.AddObject(ObjectBuilder.BuildEnemy(waves[WaveNumber].positions[i],
                        waves[WaveNumber].Enemies[i]));
                }
                WaveNumber++;
            }
            else
            {
                GameWorld.State.Save();
                GameWorld.ChangeScene(new LobbyScene());
            }
        }
    }
}