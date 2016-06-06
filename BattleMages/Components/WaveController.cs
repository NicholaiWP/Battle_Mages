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

        private float newWaveTimer = 0;
        private const float newWaveTimerTarget = 0.3f;

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
            bool waveDone = true;
            foreach (GameObject go in GameWorld.Scene.ActiveObjects)
            {
                if (go.GetComponent<Enemy>() != null || go.GetComponent<EnemySummoner>() != null)
                {
                    waveDone = false;
                    break;
                }
            }

            if (waveDone && !challengeEnded)
            {
                newWaveTimer += GameWorld.DeltaTime;
                if (newWaveTimer > newWaveTimerTarget)
                {
                    NextWave();
                    newWaveTimer = 0;
                }
            }
            else
            {
                newWaveTimer = 0;
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
                    int thisIndex = i;
                    int thisWaveNumber = WaveNumber;

                    GameObject summoner = new GameObject(waves[waveNumber].positions[thisIndex]);
                    summoner.AddComponent(new SpriteRenderer("Textures/Enemies/EnemySummonSheet", true) { Rectangle = new Rectangle(0, 0, 32, 32) });
                    Animator animator = new Animator();
                    animator.CreateAnimation("", new Animation(0, 25, 0, 0, 32, 32, 15, Vector2.Zero));
                    summoner.AddComponent(animator);
                    summoner.AddComponent(new EnemySummoner(() =>
                    {
                        GameWorld.Scene.AddObject(ObjectBuilder.BuildEnemy(waves[thisWaveNumber].positions[thisIndex],
                                                waves[thisWaveNumber].Enemies[thisIndex]));
                    }));
                    GameWorld.Scene.AddObject(summoner);
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