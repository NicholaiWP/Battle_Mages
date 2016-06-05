using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public class WaveController : Component
    {
        private int waveNumber;
        private List<Wave> waves;
        private int baseRuneToUnlock;

        private bool challengeEnded = false;
        private SpriteFont font;

        public WaveController(List<Wave> waves, int baseRuneToUnlock)
        {
            WaveNumber = 0;
            this.waves = new List<Wave>(waves);
            this.baseRuneToUnlock = baseRuneToUnlock;

            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
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

        private void Initialize(InitializeMsg msg)
        {
            font = GameWorld.Load<SpriteFont>("TitleFont");
        }

        private void Update(UpdateMsg msg)
        {
            int enemyCount = 0;

            foreach (GameObject go in GameWorld.Scene.ActiveObjects)
            {
                if (go.GetComponent<Enemy>() != null)
                {
                    enemyCount++;
                }
            }

            if (enemyCount == 0 && !challengeEnded)
            {
                NextWave();
            }
        }

        private void Draw(DrawMsg msg)
        {
            if (challengeEnded)
            {
                BaseRune rune = StaticData.BaseRunes[baseRuneToUnlock];
                string text = "U has wonned a new rune: " + rune.Name;
                Vector2 offset = -font.MeasureString(text) / 2;

                msg.Drawer[DrawLayer.UI].DrawString(font, text, GameWorld.Camera.Position + offset, Color.Purple);
            }
        }

        public void NextWave()
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
                bool hasRune = false;
                foreach (BaseRune baseRune in GameWorld.State.AvailableBaseRunes)
                {
                    if (StaticData.BaseRunes.IndexOf(baseRune) == baseRuneToUnlock) hasRune = true;
                }

                if (!hasRune)
                {
                    GameWorld.State.AvailableBaseRunes.Add(StaticData.BaseRunes[baseRuneToUnlock]);
                }

                challengeEnded = true;
                //GameWorld.State.Save();
                //GameWorld.ChangeScene(new LobbyScene());
            }
        }
    }
}