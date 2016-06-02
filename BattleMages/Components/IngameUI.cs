using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public class IngameUI : Component
    {
        private Texture2D healthBar;
        private Texture2D manaBar;
        private Texture2D coinsSprite;
        private Texture2D aboveUI;
        private Texture2D behindUI;

        private float healthbarSize = 1f;
        private float manabarSize = 1f;

        private Player player;

        private SpriteFont haxFont;
        private Vector2[] spellBarPositions;
        private GameObject[] spellInfoRenderers;

        public IngameUI() : base()
        {
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Initialize(InitializeMsg msg)
        {
            haxFont = GameWorld.Load<SpriteFont>("FontBM");
            behindUI = GameWorld.Load<Texture2D>("Textures/UI/Ingame/UIBackground");
            aboveUI = GameWorld.Load<Texture2D>("Textures/UI/Ingame/UIForeground");
            healthBar = GameWorld.Load<Texture2D>("Textures/UI/Ingame/HealthBar");
            manaBar = GameWorld.Load<Texture2D>("Textures/UI/Ingame/ManaBar");
            coinsSprite = GameWorld.Load<Texture2D>("Textures/UI/Ingame/Coin");

            //Spell bar icons

            //Calc positions
            spellBarPositions = new Vector2[GameWorld.State.SpellBar.Count];
            spellInfoRenderers = new GameObject[GameWorld.State.SpellBar.Count];

            //Some values to use for calculations
            int texSize = 19;
            int space = texSize + 8; //Width of spell texture + space between them
            int num = GameWorld.State.SpellBar.Count;
            Vector2 startPos = new Vector2((-space * num) / 2f, GameWorld.GameHeight / 2 - texSize / 2 - 8);

            for (int i = 0; i < num; i++)
            {
                spellBarPositions[i] = new Vector2(startPos.X + space * i, startPos.Y);

                //Create gameobject for this spell
                GameObject spellObj = new GameObject(spellBarPositions[i]);
                SpellInfo spell = null; //Default to null if this slot has an invalid spell ID
                if (GameWorld.State.SpellBar[i] < GameWorld.State.SpellBook.Count) spell = GameWorld.State.SpellBook[GameWorld.State.SpellBar[i]];
                spellObj.AddComponent(new SpellInfoRenderer(spell));

                GameWorld.Scene.AddObject(spellObj);
                spellInfoRenderers[i] = spellObj;
            }
        }

        private void Update(UpdateMsg msg)
        {
            player = GameWorld.Scene.ActiveObjects.Select(a => a.GetComponent<Player>()).Where(a => a != null).FirstOrDefault();
            if (player != null)
            {
                healthbarSize = Math.Max(0, MathHelper.Lerp(healthbarSize, player.CurrentHealth / (float)Player.MaxHealth, GameWorld.DeltaTime * 10f));
                manabarSize = Math.Max(0, MathHelper.Lerp(manabarSize, player.CurrentMana / Player.MaxMana, GameWorld.DeltaTime * 10f));
            }

            for (int i = 0; i < spellInfoRenderers.Length; i++)
            {
                spellInfoRenderers[i].Transform.Position = GameWorld.Camera.Position + spellBarPositions[i];
            }
        }

        private void Draw(DrawMsg msg)
        {
            int offset = 6;
            int halfOffset = offset / 2;
            Vector2 topLeft = GameWorld.Camera.Position - new Vector2((GameWorld.GameWidth / 2), (GameWorld.GameHeight / 2));
            Vector2 topRight = GameWorld.Camera.Position - new Vector2((-GameWorld.GameWidth / 2), (GameWorld.GameHeight / 2));

            Vector2 healthBarPos = new Vector2(topLeft.X + 13, topLeft.Y + 2);
            Vector2 manaBarPos = new Vector2(topLeft.X + 2, topLeft.Y + 15);

            msg.Drawer[DrawLayer.Gameplay].Draw(behindUI, position: topLeft);

            if (player != null)
            {
                msg.Drawer[DrawLayer.UI].Draw(healthBar, position: healthBarPos, scale: new Vector2(healthbarSize, 1));
                msg.Drawer[DrawLayer.UI].Draw(manaBar, position: manaBarPos, scale: new Vector2(manabarSize, 1));
                //Draws currency with spritefont

                msg.Drawer[DrawLayer.AboveUI].DrawString(haxFont, player.Currency.ToString(), new Vector2(topRight.X - player.Currency.ToString().Length * 7 -(coinsSprite.Width + offset), topRight.Y + 3.5f), Color.LightYellow);
            }

            msg.Drawer[DrawLayer.AboveUI].Draw(aboveUI, position: topLeft);

            msg.Drawer[DrawLayer.UI].Draw(coinsSprite, position: new Vector2(topRight.X - (coinsSprite.Width + offset), topRight.Y));
        }
    }
}