using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
    internal class SpellbookScene : Scene
    {
        private Texture2D background;
        private Texture2D spellCircle;
        private Vector2 bgPosition = GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight / 2);
        private Vector2 scPosition = GameWorld.Camera.Position - new Vector2(- GameWorld.GameWidth / 18, (GameWorld.GameHeight / 2) - 5);
        private Vector2 mcPosition = GameWorld.Camera.Position - new Vector2(- GameWorld.GameWidth / 16, (GameWorld.GameHeight / 2) - 120);

        private Scene oldScene;
        private bool tabPressed = true; //Assume that the TAB key is being pressed as soon as the scene is created.

        //Seperate ints to keep track of each tab's current page so that they are kept when switching tabs.
        private int spellListPage = 0;
        private int runeListPage = 0;

        //NULL if no spell is being edited. Determines if runes or player spells should be shown.
        private PlayerSpell currentlyEditing;

        private SpriteFont font;

        public SpellbookScene(Scene oldScene)
        {
            var content = GameWorld.Instance.Content;

            spellCircle = content.Load<Texture2D>("Images/BMspellcircle");
            background = content.Load<Texture2D>("Backgrounds/BMspellBookbg");

            this.oldScene = oldScene;
            font = GameWorld.Instance.Content.Load<SpriteFont>("FontBM");
        }

        public override void Update()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Tab))
            {
                if (!tabPressed)
                    GameWorld.ChangeScene(oldScene);
            }
            else
            {
                if (tabPressed)
                    tabPressed = false;
            }

            foreach (GameObject go in ActiveObjects)
            {
                go.Update();
            }
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.UI].DrawString(font, "ManaCost:", mcPosition, Color.White);

            drawer[DrawLayer.Gameplay].Draw(spellCircle, scPosition, Color.White);
            drawer[DrawLayer.Background].Draw(background, bgPosition, Color.White);
            foreach (GameObject go in ActiveObjects)
            {
                go.Draw(drawer);
            }
        }
    }
}