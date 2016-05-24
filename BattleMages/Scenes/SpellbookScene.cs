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

        private Vector2 baseRunePosition;
        private Vector2[] attrRunePositions;

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
        private SpellInfo currentlyEditing;
        private BaseRune selectedBaseRune;
        private AttributeRune selectedAttrRune;
        private int selectedSpellBarSlot = -1;

        private SpriteFont font;
        private string bottomLeftText = string.Empty;
        private string bottomRightText = string.Empty;

        private Vector2 TopLeft
        {
            get
            {
                return GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight / 2);
            }
        }

        public SpellbookScene(Scene oldScene)
        {
            baseRunePosition = scPosition + new Vector2(60, 60);
            attrRunePositions = new Vector2[]
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
            AddObject(ObjectBuilder.BuildButton(
                baseRunePosition - new Vector2(spellSpr1.Width / 2, spellSpr2.Height / 2),
                spellSpr1,
                spellSpr2,
                () =>
                {
                    if (currentlyEditing != null && selectedBaseRune != null)
                    {
                        currentlyEditing.SetBaseRune(StaticData.BaseRunes.IndexOf(selectedBaseRune));
                    }
                    UpdateRuneGrid();
                },
                () => { if (currentlyEditing != null) { currentlyEditing.SetBaseRune(-1); UpdateRuneGrid(); } }
                ));
            for (int i = 0; i < attrRunePositions.Length; i++)
            {
                int thisPos = i;
                var runeSpr1 = content.Load<Texture2D>("Images/Button_Rune");
                var runeSpr2 = content.Load<Texture2D>("Images/Button_Rune_Hover");
                AddObject(ObjectBuilder.BuildButton(
                    attrRunePositions[thisPos] - new Vector2(runeSpr1.Width / 2, runeSpr1.Height / 2),
                    runeSpr1,
                    runeSpr2,
                    () =>
                    {
                        if (currentlyEditing != null && selectedAttrRune != null)
                        {
                            currentlyEditing.SetAttributeRune(thisPos, StaticData.AttributeRunes.IndexOf(selectedAttrRune));
                        }
                        UpdateRuneGrid();
                    },
                    () => { if (currentlyEditing != null) { currentlyEditing.SetAttributeRune(thisPos, -1); UpdateRuneGrid(); } }
                    ));
            }

            OpenSpellsTab();
        }

        #region General purpose methods

        private GameObject RuneIcon(Vector2 position, string textureName)
        {
            GameObject go = new GameObject(position);
            go.AddComponent(new SpriteRenderer(go, "Rune Images/" + textureName));
            return go;
        }

        #endregion

        #region Tabs (Left pane)

        private void AddTabObject(GameObject go)
        {
            objectsInTab.Add(go);
            AddObject(go);
        }

        private void ClearTab()
        {
            foreach (GameObject go in objectsInTab)
            {
                RemoveObject(go);
            }
            objectsInTab.Clear();
        }

        private void OpenSpellsTab()
        {
            bottomRightText = "Select a spell to start\nediting it. Select a spell bar\nslot then select a spell to\nassign the spell to the slot.";
            bottomLeftText = "Action bar";
            ClearTab();
            var content = GameWorld.Instance.Content;
            //New spell button
            var newSpellSpr1 = content.Load<Texture2D>("Images/Button_NewSpell");
            var newSpellSpr2 = content.Load<Texture2D>("Images/Button_NewSpell_Hover");
            AddTabObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 16, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 16),
                newSpellSpr1,
                newSpellSpr2,
                () =>
                {
                    SpellInfo newSpell = new SpellInfo();
                    GameWorld.State.SpellBook.Add(newSpell);
                    OpenRunesTab(newSpell);
                }
                ));
            var playerSpellSpr1 = content.Load<Texture2D>("Images/Button_PlayerSpell");
            var playerSpellSpr2 = content.Load<Texture2D>("Images/Button_PlayerSpell_Hover");
            int nextSpellYPos = 0;
            foreach (SpellInfo spell in GameWorld.State.SpellBook)
            {
                SpellInfo thisSpell = spell;
                AddTabObject(ObjectBuilder.BuildButton(
                    new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 18, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 32 + nextSpellYPos),
                    playerSpellSpr1,
                    playerSpellSpr2,
                    () =>
                    {
                        if (selectedSpellBarSlot >= 0)
                        {
                            GameWorld.State.SpellBar[selectedSpellBarSlot] = GameWorld.State.SpellBook.IndexOf(thisSpell);
                            selectedSpellBarSlot = -1;
                            OpenSpellsTab();
                        }
                        else
                        {
                            OpenRunesTab(thisSpell);
                        }
                    }
                    ));
                //Draw sprite for the base spell of the spell
                BaseRune spellInfo = spell.GetBaseRune();
                if (spellInfo != null)
                    AddTabObject(RuneIcon(TopLeft + new Vector2(18 + 8 + 4, 32 + 8 + nextSpellYPos), spell.GetBaseRune().TextureName));
                for (int i = 0; i < SpellInfo.AttributeRuneSlotCount; i++)
                {
                    AttributeRune rune = spell.GetAttributeRune(i);
                    if (rune != null)
                        AddTabObject(RuneIcon(TopLeft + new Vector2(18 + 8 + 4 + 16 + 16 * i, 32 + 8 + nextSpellYPos), rune.TextureName));
                }
                nextSpellYPos += 16;
            }
            //Action bar slots
            for (int i = 0; i < GameWorld.State.SpellBar.Count; i++)
            {
                int thisIndex = i;
                int spellId = GameWorld.State.SpellBar[thisIndex];

                var btnSpr1 = content.Load<Texture2D>("Images/Button_Rune");
                var btnSpr2 = content.Load<Texture2D>("Images/Button_Rune_Hover");
                AddTabObject(ObjectBuilder.BuildButton(
                        TopLeft + new Vector2(20 + thisIndex * 24, GameWorld.GameHeight - 40),
                        btnSpr1,
                        btnSpr2,
                        () => { selectedSpellBarSlot = thisIndex; }
                    ));
                AddTabObject(RuneIcon(TopLeft + new Vector2(20 + thisIndex * 24 + 8, GameWorld.GameHeight - 40 + 8), GameWorld.State.SpellBook[spellId].GetBaseRune().TextureName));
            }
            UpdateRuneGrid();
        }

        private void OpenRunesTab(SpellInfo spellToEdit)
        {
            bottomRightText = "Left click a rune to select\nthen left click a slot\nto add the selected rune.\nRight click to clear slots.";
            currentlyEditing = spellToEdit;
            ClearTab();
            var content = GameWorld.Instance.Content;
            //Done button
            var doneSpr1 = content.Load<Texture2D>("Images/Button_Done");
            var doneSpr2 = content.Load<Texture2D>("Images/Button_Done_Hover");
            AddTabObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 16, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 16),
                doneSpr1,
                doneSpr2,
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

            foreach (var spell in StaticData.BaseRunes)
            {
                BaseRune thisSpell = spell;
                AddTabObject(ObjectBuilder.BuildButton(
                    new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 18 + nextRuneX, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 32 + nextRuneY),
                    content.Load<Texture2D>("Rune Images/" + thisSpell.TextureName),
                    runeSpr2,
                    () =>
                    {
                        bottomLeftText = thisSpell.Name + Environment.NewLine + thisSpell.Description;
                        selectedAttrRune = null;
                        selectedBaseRune = thisSpell;
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

            foreach (var rune in StaticData.AttributeRunes)
            {
                AttributeRune thisRune = rune;
                AddTabObject(ObjectBuilder.BuildButton(
                    new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 18 + nextRuneX, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 32 + nextRuneY),
                    content.Load<Texture2D>("Rune Images/" + thisRune.TextureName),
                    runeSpr2,
                    () =>
                    {
                        bottomLeftText = thisRune.Name + Environment.NewLine + thisRune.Description;
                        selectedAttrRune = thisRune;
                        selectedBaseRune = null;
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

        #endregion

        #region Rune grid (Right pane)

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

            BaseRune spell = currentlyEditing.GetBaseRune();
            if (spell != null)
            {
                var go = RuneIcon(baseRunePosition, spell.TextureName);
                AddRuneGridObject(go);
            }

            for (int i = 0; i < SpellInfo.AttributeRuneSlotCount; i++)
            {
                AttributeRune runeInfo = currentlyEditing.GetAttributeRune(i);
                if (runeInfo != null)
                {
                    var go = RuneIcon(attrRunePositions[i], runeInfo.TextureName);
                    AddRuneGridObject(go);
                }
            }
        }

        #endregion

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

            base.Update();
        }

        public override void Draw(Drawer drawer)
        {
            Color textColor = new Color(120, 100, 80);
            drawer[DrawLayer.UI].DrawString(font, bottomRightText, mcPosition, textColor);
            drawer[DrawLayer.UI].DrawString(font, bottomLeftText, GameWorld.Camera.Position + new Vector2(-GameWorld.GameWidth / 2 + 20, GameWorld.GameHeight / 2 - 60), textColor);

            drawer[DrawLayer.Gameplay].Draw(spellCircle, scPosition, Color.White);
            drawer[DrawLayer.Background].Draw(background, bgPosition, Color.White);

            base.Draw(drawer);
        }
    }
}