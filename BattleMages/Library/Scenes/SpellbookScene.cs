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

        private Vector2 baseSpellPosition;
        private Vector2[] runePositions;

        private Scene oldScene;
        private bool tabPressed = true; //Assume that the TAB key is being pressed as soon as the scene is created.

        //Holds objects in a tab to delete them on tab change
        private List<GameObject> objectsInTab = new List<GameObject>();
        //Same for the rune grid
        private List<GameObject> objectsInRuneGrid = new List<GameObject>();

        //Seperate ints to keep track of each tab's current page so that they are kept when switching tabs.
        private int spellListPage = 0;
        private int runeListPage = 0;

        //NULL if no spell is being edited. Determines if runes or player spells should be shown.
        private PlayerSpell currentlyEditing;
        private SpellInfo selectedSpell;
        private RuneInfo selectedRune;

        private SpriteFont font;
        private string bottomLeftText = string.Empty;

        private Vector2 TopLeft
        {
            get
            {
                return GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight / 2);
            }
        }

        public SpellbookScene(Scene oldScene)
        {
            baseSpellPosition = scPosition + new Vector2(60, 60);
            runePositions = new Vector2[]
            {
                scPosition + new Vector2(60, 25), //Top slot
                scPosition + new Vector2(120 - 25, 60), //Right slot
                scPosition + new Vector2(60, 120 - 25), //Bottom slot
                scPosition + new Vector2(25, 60), //Left slot
            };

            var content = GameWorld.Instance.Content;

            spellCircle = content.Load<Texture2D>("Images/BMspellcircle");
            background = content.Load<Texture2D>("Backgrounds/BMspellBookbg");

            this.oldScene = oldScene;
            font = GameWorld.Instance.Content.Load<SpriteFont>("FontBM");

            //Rune grid buttons

            //Spell
            var spellSpr1 = content.Load<Texture2D>("Images/Button_Rune");
            var spellSpr2 = content.Load<Texture2D>("Images/Button_Rune_Hover");
            AddObject(new Button(
                spellSpr1,
                spellSpr2,
                baseSpellPosition - new Vector2(spellSpr1.Width / 2, spellSpr2.Height / 2),
                () =>
                {
                    if (currentlyEditing != null && selectedSpell != null)
                    {
                        currentlyEditing.SetSpell(StaticData.Spells.IndexOf(selectedSpell));
                    }
                    UpdateRuneGrid();
                }, false,
                () => { if (currentlyEditing != null) { currentlyEditing.SetSpell(-1); UpdateRuneGrid(); } }
                ));
            for (int i = 0; i < runePositions.Length; i++)
            {
                int thisPos = i;
                var runeSpr1 = content.Load<Texture2D>("Images/Button_Rune");
                var runeSpr2 = content.Load<Texture2D>("Images/Button_Rune_Hover");
                AddObject(new Button(
                    runeSpr1,
                    runeSpr2,
                    runePositions[thisPos] - new Vector2(runeSpr1.Width / 2, runeSpr1.Height / 2),
                    () =>
                    {
                        if (currentlyEditing != null && selectedRune != null)
                        {
                            currentlyEditing.SetRune(thisPos, StaticData.Runes.IndexOf(selectedRune));
                        }
                        UpdateRuneGrid();
                    }, false,
                    () => { if (currentlyEditing != null) { currentlyEditing.SetRune(thisPos, -1); UpdateRuneGrid(); } }
                    ));
            }

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
                //Draw sprite for the base spell of the spell
                SpellInfo spellInfo = spell.GetSpell();
                if (spellInfo != null)
                    AddTabObject(RuneIcon(TopLeft + new Vector2(18 + 8 + 4, 32 + 8 + nextSpellYPos), spell.GetSpell().TextureName));
                for (int i = 0; i < spell.RuneCount; i++)
                {
                    RuneInfo rune = spell.GetRune(i);
                    if (rune != null)
                        AddTabObject(RuneIcon(TopLeft + new Vector2(18 + 8 + 4 + 16 + 16 * i, 32 + 8 + nextSpellYPos), rune.TextureName));
                }
                nextSpellYPos += 16;
            }
            UpdateRuneGrid();
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
                        selectedRune = null;
                        selectedSpell = thisSpell;
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
                        selectedRune = thisRune;
                        selectedSpell = null;
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
            if (currentlyEditing == null) return;

            SpellInfo spell = currentlyEditing.GetSpell();
            if (spell != null)
            {
                var go = RuneIcon(baseSpellPosition, spell.TextureName);
                AddRuneGridObject(go);
            }

            for (int i = 0; i < currentlyEditing.RuneCount; i++)
            {
                RuneInfo runeInfo = currentlyEditing.GetRune(i);
                if (runeInfo != null)
                {
                    var go = RuneIcon(runePositions[i], runeInfo.TextureName);
                    AddRuneGridObject(go);
                }
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