using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
    public class SpellbookScene : Scene
    {
        private Texture2D background;
        private Vector2 bgPos = GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight / 2);
        private Vector2 spellCirclePos = GameWorld.Camera.Position - new Vector2(-GameWorld.GameWidth / 18, (GameWorld.GameHeight / 2) - 5);

        //Positions on rune grid
        private Vector2 baseRunePosition;
        private Vector2[] attrRunePositions;

        private Scene oldScene;
        private bool tabPressed = true; //Assume that the TAB key is being pressed as soon as the scene is created.

        //Holds objects in a tab to delete them on tab change
        private UITab leftTab;
        private UITab rightTab;

        //NULL if no spell is being edited.
        private SpellInfo currentlyEditing;

        //Selected runes/spells (These are used when runes/spells are being dragged around)
        private BaseRune selectedBaseRune;
        private AttributeRune selectedAttrRune;
        private SpellInfo selectedPlayerSpell;

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
            this.oldScene = oldScene;

            baseRunePosition = spellCirclePos + new Vector2(60, 60);
            attrRunePositions = new Vector2[]
            {
                spellCirclePos + new Vector2(60, 25), //Top slot
                spellCirclePos + new Vector2(120 - 25, 60), //Right slot
                spellCirclePos + new Vector2(60, 120 - 25), //Bottom slot
                spellCirclePos + new Vector2(25, 60), //Left slot
            };

            background = GameWorld.Load<Texture2D>("Textures/Backgrounds/Spellbook");
            font = GameWorld.Instance.Content.Load<SpriteFont>("FontBM");

            leftTab = new UITab(this, GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight / 2), new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight));
            rightTab = new UITab(this, GameWorld.Camera.Position - new Vector2(0, GameWorld.GameHeight / 2), new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight));
            OpenSpellsTab();
            OpenRunesTab();
        }

        #region General purpose methods

        private GameObject RuneIcon(Vector2 position, string textureName)
        {
            GameObject go = new GameObject(position);
            go.AddComponent(new SpriteRenderer(StaticData.RuneImagePath + textureName));
            return go;
        }

        #endregion General purpose methods

        private void OpenSpellsTab()
        {
            UITab t = rightTab;
            t.Clear();

            bottomRightText = "Your active spells:";
            //New spell button
            var newSpellSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/NewSpell");
            var newSpellSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/NewSpell_Hover");
            t.AddObject(ObjectBuilder.BuildButton(
                t.TopLeft + new Vector2(16, 16),
                newSpellSpr1,
                newSpellSpr2,
                () =>
                {
                    SpellInfo newSpell = new SpellInfo();
                    GameWorld.State.SpellBook.Add(newSpell);
                    currentlyEditing = newSpell;
                    OpenRuneGrid();
                }
                ));

            //Player spell list
            var btnSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton");
            var btnSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton_Hover");
            var btnSpr3 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton_Highlighted");
            int nextSpellYPos = 0;

            foreach (SpellInfo spell in GameWorld.State.SpellBook)
            {
                SpellInfo thisSpell = spell;

                //Draggable thing
                GameObject spellObj = new GameObject(t.TopLeft + new Vector2(30, 40 + nextSpellYPos));
                spellObj.AddComponent(new DragDropItem("playerspell", btnSpr1, btnSpr2, () => { selectedPlayerSpell = thisSpell; }));
                spellObj.AddComponent(new SpellInfoRenderer(thisSpell));
                t.AddObject(spellObj);

                //Edit spell button
                GameObject editObj = new GameObject(t.TopLeft + new Vector2(64, 40 + nextSpellYPos));
                editObj.AddComponent(new SpriteRenderer("Textures/UI/Spellbook/EditSpell"));
                editObj.AddComponent(new Button(btnSpr1, btnSpr2, () =>
                {
                    currentlyEditing = thisSpell;
                    OpenRuneGrid();
                }, centered: true
                ));
                t.AddObject(editObj);

                //Delete spell button
                GameObject deleteObj = new GameObject(t.TopLeft + new Vector2(64 + 16, 40 + nextSpellYPos));
                deleteObj.AddComponent(new SpriteRenderer("Textures/UI/Spellbook/DeleteSpell"));
                deleteObj.AddComponent(new Button(btnSpr1, btnSpr2, () =>
                {
                    for (int i = 0; i < GameWorld.State.SpellBar.Count; i++)
                    {
                        if (GameWorld.State.SpellBar[i] == GameWorld.State.SpellBook.IndexOf(thisSpell))
                        {
                            GameWorld.State.SpellBar[i] = -1;
                        }
                    }
                    GameWorld.State.SpellBook.Remove(thisSpell);
                    OpenSpellsTab();
                }, centered: true
                ));
                t.AddObject(deleteObj);

                nextSpellYPos += 16;
            }

            //Action bar slots
            for (int i = 0; i < GameWorld.State.SpellBar.Count; i++)
            {
                int thisIndex = i;
                int spellId = GameWorld.State.SpellBar[thisIndex];

                SpellInfo spell = null;
                if (spellId < GameWorld.State.SpellBook.Count && spellId >= 0) spell = GameWorld.State.SpellBook[spellId];

                GameObject slotObj = new GameObject(t.TopLeft + new Vector2(GameWorld.GameWidth / 4 - 24 * 2 + thisIndex * 24, GameWorld.GameHeight - 32));
                slotObj.AddComponent(new SpellInfoRenderer(spell));
                slotObj.AddComponent(new DragDropPoint("playerspell", btnSpr1, btnSpr3, btnSpr2, () =>
                {
                    if (selectedPlayerSpell != null)
                    {
                        for (int j = 0; j < GameWorld.State.SpellBar.Count; j++)
                        {
                            if (GameWorld.State.SpellBar[j] == GameWorld.State.SpellBook.IndexOf(selectedPlayerSpell)) GameWorld.State.SpellBar[j] = -1;
                        }
                        GameWorld.State.SpellBar[thisIndex] = GameWorld.State.SpellBook.IndexOf(selectedPlayerSpell);
                        OpenSpellsTab();
                    }
                }
                ));
                t.AddObject(slotObj);
            }
        }

        private void OpenRunesTab()
        {
            UITab t = leftTab;
            t.Clear();

            bottomLeftText = "Hover over any rune to view its description.";

            var runeBtnSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton");
            var runeBtnSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton_Hover");
            var runeBtnSpr3 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton_Highlighted");
            int nextRuneX = 0;
            int nextRuneY = 0;
            int nextRunePos = 0;

            Vector2 runeStartPos = TopLeft + new Vector2(28, 28);

            foreach (var baseRune in StaticData.BaseRunes)
            {
                BaseRune thisBaseRune = baseRune;
                Vector2 pos = runeStartPos + new Vector2(nextRuneX, nextRuneY);

                GameObject runeObj = new GameObject(pos);
                runeObj.AddComponent(new DragDropItem("baserune", runeBtnSpr1, runeBtnSpr2,
                    () => { selectedAttrRune = null; selectedBaseRune = thisBaseRune; },
                    () => { bottomLeftText = thisBaseRune.Name + " (Base rune)" + Environment.NewLine + thisBaseRune.Description; }));
                runeObj.AddComponent(new SpriteRenderer(StaticData.RuneImagePath + thisBaseRune.TextureName));
                t.AddObject(runeObj);

                nextRunePos++;
                nextRuneX += 16;
                if (nextRunePos % 8 == 0)
                {
                    nextRuneY += 16;
                    nextRuneX = 0;
                }
            }

            nextRuneY += 24;
            nextRuneX = 0;

            foreach (var rune in GameWorld.State.AvailableRunes)
            {
                AttributeRune thisAttrRune = rune;
                Vector2 pos = runeStartPos + new Vector2(nextRuneX, nextRuneY);

                GameObject runeObj = new GameObject(pos);
                runeObj.AddComponent(new DragDropItem("attrrune", runeBtnSpr1, runeBtnSpr2,
                    () => { selectedAttrRune = thisAttrRune; selectedBaseRune = null; },
                    () => { bottomLeftText = thisAttrRune.Name + " (Attribute rune)" + Environment.NewLine + thisAttrRune.Description; }));
                runeObj.AddComponent(new SpriteRenderer(StaticData.RuneImagePath + thisAttrRune.TextureName));
                t.AddObject(runeObj);

                nextRunePos++;
                nextRuneX += 16;
                if (nextRunePos % 8 == 0)
                {
                    nextRuneY += 16;
                    nextRuneX = 0;
                }
            }
        }

        private void OpenRuneGrid()
        {
            if (currentlyEditing == null) return;
            UITab t = rightTab;
            t.Clear();

            bottomRightText = "Drag a rune to an appropriate slot to place it there.";

            //Spell circle
            GameObject spellCircleObj = new GameObject(spellCirclePos + new Vector2(60, 60));
            spellCircleObj.AddComponent(new SpriteRenderer("Textures/UI/Spellbook/RuneGrid"));
            t.AddObject(spellCircleObj);

            //Done button
            var doneSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/Done");
            var doneSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/Done_Hover");
            t.AddObject(ObjectBuilder.BuildButton(
            new Vector2(GameWorld.Camera.Position.X + 16, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 16),
            doneSpr1,
            doneSpr2,
            () =>
            {
                currentlyEditing = null;
                OpenSpellsTab();
            }
            ));
            //Rune grid buttons

            //Base runes
            var runeSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton");
            var runeSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton_Hover");
            var runeSpr3 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton_Highlighted");

            GameObject baseRuneObj = new GameObject(baseRunePosition);
            baseRuneObj.AddComponent(new DragDropPoint("baserune", runeSpr1, runeSpr3, runeSpr2, () =>
            {
                if (currentlyEditing != null && selectedBaseRune != null)
                {
                    currentlyEditing.SetBaseRune(StaticData.BaseRunes.IndexOf(selectedBaseRune));
                }
                OpenRuneGrid();
            }));
            t.AddObject(baseRuneObj);

            /*t.AddObject(ObjectBuilder.BuildButton(
                baseRunePosition - new Vector2(runeSpr1.Width / 2, runeSpr2.Height / 2),
                runeSpr1,
                runeSpr2,
                () =>
                {
                },
                () => { if (currentlyEditing != null) { currentlyEditing.SetBaseRune(-1); OpenRuneGrid(); } }
                ));*/
            //Attribute runes
            for (int i = 0; i < attrRunePositions.Length; i++)
            {
                int thisPos = i;

                GameObject attrRuneObj = new GameObject(attrRunePositions[i]);
                attrRuneObj.AddComponent(new DragDropPoint("attrrune", runeSpr1, runeSpr3, runeSpr2, () =>
                    {
                        if (currentlyEditing != null && selectedAttrRune != null)
                        {
                            currentlyEditing.SetAttributeRune(thisPos, StaticData.AttributeRunes.IndexOf(selectedAttrRune));
                        }
                        OpenRuneGrid();
                    }));
                t.AddObject(attrRuneObj);

                /*t.AddObject(ObjectBuilder.BuildButton(
                    attrRunePositions[thisPos] - new Vector2(runeSpr1.Width / 2, runeSpr1.Height / 2),
                    runeSpr1,
                    runeSpr2,
                    () =>
                    {
                    },
                    () => { if (currentlyEditing != null) { currentlyEditing.SetAttributeRune(thisPos, -1); OpenRuneGrid(); } }
                    ));*/
            }

            //Images on top of buttons
            BaseRune spell = currentlyEditing.GetBaseRune();
            if (spell != null)
            {
                var go = RuneIcon(baseRunePosition, spell.TextureName);
                t.AddObject(go);
            }

            for (int i = 0; i < SpellInfo.AttributeRuneSlotCount; i++)
            {
                AttributeRune runeInfo = currentlyEditing.GetAttributeRune(i);
                if (runeInfo != null)
                {
                    var go = RuneIcon(attrRunePositions[i], runeInfo.TextureName);
                    t.AddObject(go);
                }
            }
        }

        public override void Update()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Tab))
            {
                if (!tabPressed)
                {
                    GameWorld.State.Save();
                    GameWorld.ChangeScene(oldScene);
                }
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
            drawer[DrawLayer.UI].DrawString(font, Utils.WarpText(bottomLeftText, 120, font), leftTab.TopLeft + new Vector2(24, GameWorld.GameHeight - 58), textColor);
            drawer[DrawLayer.UI].DrawString(font, Utils.WarpText(bottomRightText, 120, font), rightTab.TopLeft + new Vector2(20, GameWorld.GameHeight - 58), textColor);

            drawer[DrawLayer.Background].Draw(background, bgPos, Color.White);

            base.Draw(drawer);
        }
    }
}