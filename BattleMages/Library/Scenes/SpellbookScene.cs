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
        private Vector2 scPosition = GameWorld.Camera.Position - new Vector2(-GameWorld.GameWidth / 18, (GameWorld.GameHeight / 2) - 5);
        private Vector2 mcPosition = GameWorld.Camera.Position - new Vector2(-GameWorld.GameWidth / 16, (GameWorld.GameHeight / 2) - 120);

        private Vector2 centerRunePos;
        private Vector2 leftRunePos;
        private Vector2 rightRunePos;
        private Vector2 topRunePos;
        private Vector2 botRunePos;

        private Scene oldScene;
        private bool tabPressed = true; //Assume that the TAB key is being pressed as soon as the scene is created.

        //Holds objects in a tab to delete them on tab change
        private List<GameObject> objectsInTab = new List<GameObject>();
        private List<GameObject> objectsInRuneGrid = new List<GameObject>();

        //Seperate ints to keep track of each tab's current page so that they are kept when switching tabs.
        private int spellListPage = 0;
        private int runeListPage = 0;

        //NULL if no spell is being edited. Determines if runes or player spells should be shown.
        private PlayerSpell currentlyEditing;

        private SpriteFont font;
        private string bottomLeftText = string.Empty;

        public SpellbookScene(Scene oldScene)
        {
            centerRunePos = scPosition + new Vector2(60, 60);
            leftRunePos = scPosition + new Vector2(25, 60);
            rightRunePos = scPosition + new Vector2(120 - 25, 60);
            topRunePos = scPosition + new Vector2(60, 25);
            botRunePos = scPosition + new Vector2(60, 120 - 25);

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
                    PlayerSpell newSpell = new PlayerSpell();
                    GameWorld.State.SpellBook.Add(newSpell);
                    OpenRunesTab(newSpell);
                }
                ));
            var playerSpellSpr1 = content.Load<Texture2D>("Images/Button_PlayerSpell");
            var playerSpellSpr2 = content.Load<Texture2D>("Images/Button_PlayerSpell_Hover");
            int nextSpellYPos = 0;
            foreach (PlayerSpell spell in GameWorld.State.SpellBook)
            {
                PlayerSpell thisSpell = spell;
                AddTabObject(new Button(
                    playerSpellSpr1,
                    playerSpellSpr2,
                    new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 18, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 32 + nextSpellYPos),
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

            var runeSpr1 = content.Load<Texture2D>("Images/Button_Rune");
            var runeSpr2 = content.Load<Texture2D>("Images/Button_Rune_Hover");
            int nextRuneX = 0;
            int nextRuneY = 0;
            int nextRunePos = 0;

            foreach (var spell in StaticData.Spells)
            {
                SpellInfo thisSpell = spell;
                AddTabObject(new Button(
                    content.Load<Texture2D>("Rune Images/" + thisSpell.TextureName),
                    runeSpr2,
                    new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 18 + nextRuneX, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 32 + nextRuneY),
                    () =>
                    {
                        bottomLeftText = thisSpell.Name + Environment.NewLine + thisSpell.Description;
                    }
                    ));
                nextRunePos++;
                nextRuneX += 16;
                if (nextRunePos % 8 == 0)
                {
                    nextRuneY += 16;
                    nextRuneX = 0;
                }
            }

            nextRuneY += 16;
            nextRuneX = 0;

            foreach (var rune in StaticData.Runes)
            {
                RuneInfo thisRune = rune;
                AddTabObject(new Button(
                    content.Load<Texture2D>("Rune Images/" + thisRune.TextureName),
                    runeSpr2,
                    new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 18 + nextRuneX, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 32 + nextRuneY),
                    () =>
                    {
                        bottomLeftText = thisRune.Name + Environment.NewLine + thisRune.Description;
                    }
                    ));
                nextRunePos++;
                nextRuneX += 16;
                if (nextRunePos % 8 == 0)
                {
                    nextRuneY += 16;
                    nextRuneX = 0;
                }
            }

            UpdateRuneGrid();
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

        private void ClearRuneGrid()
        {
            foreach (GameObject go in objectsInRuneGrid)
            {
                RemoveObject(go);
            }
            objectsInRuneGrid.Clear();
        }

        private void AddRuneGridObject(GameObject go)
        {
            objectsInRuneGrid.Add(go);
            AddObject(go);
        }

        private void UpdateRuneGrid()
        {
            ClearRuneGrid();

            SpellInfo spell = currentlyEditing.GetSpell();
            if (spell != null)
            {
                var go = RuneIcon(centerRunePos, spell.TextureName);
                AddRuneGridObject(go);
            }

            RuneInfo rune0 = currentlyEditing.GetRune(0);
            if (rune0 != null)
            {
                var go = RuneIcon(topRunePos, rune0.TextureName);
                AddRuneGridObject(go);
            }

            RuneInfo rune1 = currentlyEditing.GetRune(1);
            if (rune1 != null)
            {
                var go = RuneIcon(rightRunePos, rune1.TextureName);
                AddRuneGridObject(go);
            }

            RuneInfo rune2 = currentlyEditing.GetRune(2);
            if (rune2 != null)
            {
                var go = RuneIcon(botRunePos, rune2.TextureName);
                AddRuneGridObject(go);
            }

            RuneInfo rune3 = currentlyEditing.GetRune(3);
            if (rune3 != null)
            {
                var go = RuneIcon(leftRunePos, rune3.TextureName);
                AddRuneGridObject(go);
            }
        }

        private GameObject RuneIcon(Vector2 position, string textureName)
        {
            GameObject go = new GameObject(position);
            go.AddComponent(new SpriteRenderer(go, "Rune Images/" + textureName));
            return go;
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
            drawer[DrawLayer.UI].DrawString(font, bottomLeftText, GameWorld.Camera.Position + new Vector2(-GameWorld.GameWidth / 2 + 20, GameWorld.GameHeight / 2 - 60), new Color(120, 100, 80));

            /*if (currentlyEditing != null)
            {
                SpellInfo spell = currentlyEditing.GetSpell();
                if (spell != null)
                    drawer[DrawLayer.UI].DrawString(font, spell.Name, centerRunePos, new Color(120, 100, 80));

                RuneInfo rune0 = currentlyEditing.GetRune(0);
                if (rune0 != null)
                    drawer[DrawLayer.UI].DrawString(font, rune0.Name, topRunePos, new Color(120, 100, 80));

                RuneInfo rune1 = currentlyEditing.GetRune(1);
                if (rune1 != null)
                    drawer[DrawLayer.UI].DrawString(font, rune1.Name, rightRunePos, new Color(120, 100, 80));

                RuneInfo rune2 = currentlyEditing.GetRune(2);
                if (rune2 != null)
                    drawer[DrawLayer.UI].DrawString(font, rune2.Name, botRunePos, new Color(120, 100, 80));

                RuneInfo rune3 = currentlyEditing.GetRune(3);
                if (rune3 != null)
                    drawer[DrawLayer.UI].DrawString(font, rune3.Name, leftRunePos, new Color(120, 100, 80));
            }*/

            drawer[DrawLayer.Gameplay].Draw(spellCircle, scPosition, Color.White);
            drawer[DrawLayer.Background].Draw(background, bgPosition, Color.White);
            foreach (GameObject go in ActiveObjects)
            {
                go.Draw(drawer);
            }
        }
    }
}