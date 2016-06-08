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

        //Holds objects in a tab to delete them on tab change
        private UITab leftTab;
        private UITab rightTab;

        //NULL if no spell is being edited.
        private SpellInfo currentlyEditing;

        //Selected runes/spells (These are used when runes/spells are being dragged around)
        private BaseRune selectedBaseRune;
        private AttributeRune selectedAttrRune;
        private Guid? selectedPlayerSpell;

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
                    if (GameWorld.State.SpellBook.Count < 8)
                    {
                        SpellInfo newSpell = new SpellInfo();
                        GameWorld.State.SpellBook.Add(Guid.NewGuid(), newSpell);
                        currentlyEditing = newSpell;
                        OpenRuneGrid();
                    }
                }
                ));

            //Player spell list
            var btnSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/smallButton");
            var btnSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/smallButtonHL");
            var btnSpr3 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/smallButtonHL");

            var shit = GameWorld.Load<Texture2D>("Textures/UI/Ingame/SpellbarSpellOutline");
            int nextSpellXPos = 0;
            int nextSpellYPos = 0;
            int nextSpellIndex = 0;

            foreach (KeyValuePair<Guid, SpellInfo> pair in GameWorld.State.SpellBook)
            {
                SpellInfo thisSpell = pair.Value;

                //Draggable thing
                GameObject spellObj = new GameObject(t.TopLeft + new Vector2(30 + nextSpellXPos, 42 + nextSpellYPos));
                spellObj.AddComponent(new DragDropItem("playerspell", btnSpr1, shit, () => { selectedPlayerSpell = pair.Key; }));
                spellObj.AddComponent(new SpellInfoRenderer(thisSpell));
                t.AddObject(spellObj);

                //Edit spell button
                GameObject editObj = new GameObject(t.TopLeft + new Vector2(55 + nextSpellXPos, 42 + nextSpellYPos));
                editObj.AddComponent(new Button(btnSpr1, btnSpr2, () =>
                {
                    currentlyEditing = thisSpell;
                    OpenRuneGrid();
                }, centered: true
                ));
                editObj.AddComponent(new SpriteRenderer("Textures/UI/Spellbook/edit", layerToUse: DrawLayer.UI));
                t.AddObject(editObj);

                //Delete spell button
                GameObject deleteObj = new GameObject(t.TopLeft + new Vector2(55 + 16 + nextSpellXPos, 42 + nextSpellYPos));
                deleteObj.AddComponent(new Button(btnSpr1, btnSpr2, () =>
                {
                    if (GameWorld.State.SpellBook.Count > 1)
                    {
                        for (int i = 0; i < GameWorld.State.SpellBar.Count; i++)
                        {
                            if (GameWorld.State.SpellBar[i] == pair.Key)
                            {
                                GameWorld.State.SpellBar[i] = null;
                            }
                        }
                        GameWorld.State.SpellBook.Remove(pair.Key);
                        OpenSpellsTab();
                    }
                }, centered: true
                ));
                deleteObj.AddComponent(new SpriteRenderer("Textures/UI/Spellbook/remove", layerToUse: DrawLayer.UI));
                t.AddObject(deleteObj);

                nextSpellIndex++;
                nextSpellYPos += 22;
                if (nextSpellIndex % 4 == 0)
                {
                    nextSpellXPos += 64;
                    nextSpellYPos = 0;
                }
            }

            //Action bar slots
            for (int i = 0; i < GameWorld.State.SpellBar.Count; i++)
            {
                int thisIndex = i;

                SpellInfo spell = GameWorld.State.GetSpellbarSpell(thisIndex);

                GameObject slotObj = new GameObject(t.TopLeft + new Vector2(GameWorld.GameWidth / 4 - 24 * 2 + thisIndex * 30, GameWorld.GameHeight - 32));
                slotObj.AddComponent(new DragDropPoint("playerspell", btnSpr1, shit, btnSpr2, () =>
                {
                    if (selectedPlayerSpell != null)
                    {
                        for (int j = 0; j < GameWorld.State.SpellBar.Count; j++)
                        {
                            if (GameWorld.State.SpellBar[j] == selectedPlayerSpell)
                                GameWorld.State.SpellBar[j] = null;
                        }
                        GameWorld.State.SpellBar[thisIndex] = selectedPlayerSpell;
                        OpenSpellsTab();
                    }
                }
                ));
                slotObj.AddComponent(new SpellInfoRenderer(spell));
                t.AddObject(slotObj);
            }
        }

        private void OpenRunesTab()
        {
            UITab t = leftTab;
            t.Clear();

            bottomLeftText = "Hover over any rune to view its description.";

            var runeBtnSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/smallButton");
            var runeBtnSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/smallButtonHL");
            var runeBtnSpr3 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/smallButtonHL");
            int nextRuneX = 0;
            int nextRuneY = 0;
            int nextRunePos = 0;

            Vector2 runeStartPos = TopLeft + new Vector2(28, 40);

            foreach (var baseRune in GameWorld.State.AvailableBaseRunes)
            {
                BaseRune thisBaseRune = baseRune;
                Vector2 pos = runeStartPos + new Vector2(nextRuneX, nextRuneY);

                GameObject runeObj = new GameObject(pos);
                runeObj.AddComponent(new DragDropItem("baserune", runeBtnSpr1, runeBtnSpr2,
                    () => { selectedAttrRune = null; selectedBaseRune = thisBaseRune; },
                    () => { bottomLeftText = thisBaseRune.Name + " (Base rune)" + Environment.NewLine + thisBaseRune.Description; }));
                runeObj.AddComponent(new SpriteRenderer(StaticData.RuneImagePath + thisBaseRune.TextureName, layerToUse: DrawLayer.UI));
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
                runeObj.AddComponent(new SpriteRenderer(StaticData.RuneImagePath + thisAttrRune.TextureName, layerToUse: DrawLayer.UI));
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

            bottomRightText = "Drag a base rune to the center slot or attribute runes to the outer slots to use them.";

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
                if (currentlyEditing.GetBaseRune() != null)
                {
                    currentlyEditing = null;
                    OpenSpellsTab();
                }
                else
                {
                    bottomRightText = "You cannot save a spell with no base rune. Drag a base rune to the center slot.";
                }
            }
            ));
            //Rune grid buttons

            //Base runes
            var runeSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/smallButton");
            var runeSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/smallButtonHL");
            var runeSpr3 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/smallButtonHL");

            GameObject baseRuneObj = new GameObject(baseRunePosition);
            baseRuneObj.AddComponent(new DragDropPoint("baserune", runeSpr1, runeSpr3, runeSpr2, () =>
            {
                if (currentlyEditing != null && selectedBaseRune != null)
                {
                    currentlyEditing.SetBaseRune(StaticData.BaseRunes.IndexOf(selectedBaseRune));
                }
                OpenRuneGrid();
            }));
            BaseRune baseRune = currentlyEditing.GetBaseRune();
            if (baseRune != null)
                baseRuneObj.AddComponent(new SpriteRenderer(StaticData.RuneImagePath + baseRune.TextureName, layerToUse: DrawLayer.UI));
            t.AddObject(baseRuneObj);

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
                AttributeRune rune = currentlyEditing.GetAttributeRune(i);
                if (rune != null)
                    attrRuneObj.AddComponent(new SpriteRenderer(StaticData.RuneImagePath + rune.TextureName, layerToUse: DrawLayer.UI));
                t.AddObject(attrRuneObj);
            }
        }

        public override void Update()
        {
            if (GameWorld.KeyPressed(Keys.Tab))
            {
                GameWorld.State.Save();
                GameWorld.ChangeScene(oldScene);
            }

            base.Update();
        }

        public override void Draw(Drawer drawer)
        {
            Color textColor = new Color(120, 100, 80);
            drawer[DrawLayer.UI].DrawString(font, Utils.WarpText(bottomLeftText, 120, font), leftTab.TopLeft + new Vector2(24, GameWorld.GameHeight - 58), textColor);
            drawer[DrawLayer.UI].DrawString(font, Utils.WarpText(bottomRightText, 120, font), rightTab.TopLeft + new Vector2(20, GameWorld.GameHeight - 58), textColor);

            //Rune tab title
            drawer[DrawLayer.UI].DrawString(font, "Your runes", TopLeft + new Vector2(24, 20), textColor);

            drawer[DrawLayer.Background].Draw(background, bgPos, Color.White);

            base.Draw(drawer);
        }
    }
}