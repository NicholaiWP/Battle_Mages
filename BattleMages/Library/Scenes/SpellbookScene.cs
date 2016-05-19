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

        //Holds objects in a tab to delete them on tab change
        private List<GameObject> objectsInTab = new List<GameObject>();

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

            OpenSpellsTab();
        }

        private void OpenSpellsTab()
        {
            ClearTab();
            var content = GameWorld.Instance.Content;
            //New spell button
            var newSpellSpr1 = content.Load<Texture2D>("Images/Button_NewSpell");
            var newSpellSpr2 = content.Load<Texture2D>("Images/Button_NewSpell_Hover");
            AddTabObject(new Button(
                newSpellSpr1,
                newSpellSpr2,
                new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 16, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 16),
                () =>
                {
                    PlayerSpell newSpell = new PlayerSpell(-1, new int[0]);
                    GameWorld.State.SpellBook.Add(newSpell);
                    OpenRunesTab(newSpell);
                }
                ));
            var playerSpellSpr1 = content.Load<Texture2D>("Images/Button_PlayerSpell");
            var playerSpellSpr2 = content.Load<Texture2D>("Images/Button_PlayerSpell_Hover");
            int nextSpellYPos = 32;
            foreach (PlayerSpell spell in GameWorld.State.SpellBook)
            {
                PlayerSpell thisSpell = spell;
                AddTabObject(new Button(
                    playerSpellSpr1,
                    playerSpellSpr2,
                    new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 18, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + nextSpellYPos),
                    () =>
                    {
                        OpenRunesTab(thisSpell);
                    }
                    ));
                nextSpellYPos += 16;
            }
        }

        private void OpenRunesTab(PlayerSpell spellToEdit)
        {
            currentlyEditing = spellToEdit;
            ClearTab();
            var content = GameWorld.Instance.Content;
            //Done button
            var doneSpr1 = content.Load<Texture2D>("Images/Button_Done");
            var doneSpr2 = content.Load<Texture2D>("Images/Button_Done_Hover");
            AddTabObject(new Button(
                doneSpr1,
                doneSpr2,
                new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 16, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 16),
                () =>
                {
                    currentlyEditing = null;
                    OpenSpellsTab();
                }
                ));
        }

        private void ClearTab()
        {
            foreach (GameObject go in objectsInTab)
            {
                RemoveObject(go);
            }
            objectsInTab.Clear();
        }

        private void AddTabObject(GameObject go)
        {
            objectsInTab.Add(go);
            AddObject(go);
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