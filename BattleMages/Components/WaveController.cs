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
        private SpriteFont bigFont;
        private SpriteFont smallFont;

        private bool hasRune;

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
            bigFont = GameWorld.Load<SpriteFont>("TitleFont");
            smallFont = GameWorld.Load<SpriteFont>("FontBM");
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
                string text = "Challenge completed!";
                Vector2 offset = -bigFont.MeasureString(text) / 2;

                string text2 = string.Empty;
                if (!hasRune)
                    text2 += "You have unlocked " + rune.Name + "! Open your spellbook to use this rune.";
                text2 += "\nUse the arena door when you're ready to go back.";
                Vector2 offset2 = -smallFont.MeasureString(text2) / 2;

                msg.Drawer[DrawLayer.UI].DrawString(bigFont, text, GameWorld.Camera.Position + new Vector2(0, -64) + offset, Color.Purple);
                msg.Drawer[DrawLayer.UI].DrawString(smallFont, text2, GameWorld.Camera.Position + new Vector2(0, 50) + offset2, Color.Purple);
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
                hasRune = false;
                foreach (BaseRune baseRune in GameWorld.State.AvailableBaseRunes)
                {
                    if (StaticData.BaseRunes.IndexOf(baseRune) == baseRuneToUnlock) hasRune = true;
                }

                if (!hasRune)
                {
                    GameWorld.State.AvailableBaseRunes.Add(StaticData.BaseRunes[baseRuneToUnlock]);
                }

                challengeEnded = true;

                GameObject doorObj = new GameObject(new Vector2(0, -368));
                doorObj.AddComponent(new Collider(new Vector2(50, 68)));
                doorObj.AddComponent(new Interactable(() =>
                {
                    GameWorld.State.Save();
                    GameWorld.ChangeScene(new LobbyScene());
                }));
                GameWorld.Scene.AddObject(doorObj);
            }
        }
    }
}