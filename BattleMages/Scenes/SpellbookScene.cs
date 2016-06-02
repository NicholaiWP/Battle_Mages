using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
    public class SpellbookTab
    {
        private Scene scene;
        private List<GameObject> objects = new List<GameObject>();
        private Vector2 topLeft;

        public Vector2 TopLeft { get; }

        public SpellbookTab(Scene scene, Vector2 topLeft)
        {
            this.scene = scene;
            TopLeft = topLeft;
        }

        public void AddObject(GameObject obj)
        {
            objects.Add(obj);
            scene.AddObject(obj);
        }

        public void Clear()
        {
            foreach (GameObject obj in objects)
            {
                scene.RemoveObject(obj);
            }
            objects.Clear();
        }
    }

    public class SpellbookScene : Scene
    {
        private Texture2D background;
        private Vector2 bgPosition = GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight / 2);
        private Vector2 scPosition = GameWorld.Camera.Position - new Vector2(-GameWorld.GameWidth / 18, (GameWorld.GameHeight / 2) - 5);
        private Vector2 mcPosition = GameWorld.Camera.Position - new Vector2(-GameWorld.GameWidth / 16, (GameWorld.GameHeight / 2) - 120);

        private Vector2 baseRunePosition;
        private Vector2[] attrRunePositions;

        private Scene oldScene;
        private bool tabPressed = true; //Assume that the TAB key is being pressed as soon as the scene is created.

        //Holds objects in a tab to delete them on tab change
        private SpellbookTab leftTab;
        private SpellbookTab rightTab;

        //Seperate ints to keep track of each tab's current page so that they are kept when switching tabs.
        private int spellListPage = 0;
        private int runeListPage = 0;

        //NULL if no spell is being edited. Determines if runes or player spells should be shown.
        private SpellInfo currentlyEditing;

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

            baseRunePosition = scPosition + new Vector2(60, 60);
            attrRunePositions = new Vector2[]
            {
                scPosition + new Vector2(60, 25), //Top slot
                scPosition + new Vector2(120 - 25, 60), //Right slot
                scPosition + new Vector2(60, 120 - 25), //Bottom slot
                scPosition + new Vector2(25, 60), //Left slot
            };

            background = GameWorld.Load<Texture2D>("Textures/Backgrounds/Spellbook");
            font = GameWorld.Instance.Content.Load<SpriteFont>("FontBM");

            leftTab = new SpellbookTab(this, GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight / 2));
            rightTab = new SpellbookTab(this, GameWorld.Camera.Position - new Vector2(0, GameWorld.GameHeight / 2));
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
            SpellbookTab t = rightTab;
            t.Clear();

            bottomRightText = "Select a spell to start\nediting it. Select a spell bar\nslot then select a spell to\nassign the spell to the slot.";
            bottomLeftText = "Action bar";
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
            var playerSpellSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/LongButton");
            var playerSpellSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/LongButton_Hover");
            int nextSpellYPos = 0;
            foreach (SpellInfo spell in GameWorld.State.SpellBook)
            {
                SpellInfo thisSpell = spell;

                GameObject spellObj = new GameObject(t.TopLeft + new Vector2(GameWorld.GameWidth / 4 - 2, 40 + nextSpellYPos));
                spellObj.AddComponent(new Draggable("playerspell", playerSpellSpr1, playerSpellSpr2, () => { selectedPlayerSpell = thisSpell; }));
                spellObj.AddComponent(new SpellInfoRenderer(thisSpell));
                t.AddObject(spellObj);
                /*t.AddObject(ObjectBuilder.BuildDraggable(
                    new Vector2(),
                    playerSpellSpr1,
                    playerSpellSpr2,
                    () =>
                    {
                        if (selectedSpellBarSlot >= 0)
                        {
                            if (thisSpell.GetBaseRune() != null)
                            {
                                GameWorld.State.SpellBar[selectedSpellBarSlot] = GameWorld.State.SpellBook.IndexOf(thisSpell);
                                selectedSpellBarSlot = -1;
                                OpenSpellsTab();
                            }
                        }
                        else
                        {
                            OpenRunesTab(thisSpell);
                        }
                    }
                    ));*/
                //Draw sprite for the base spell of the spell
                /*BaseRune spellInfo = spell.GetBaseRune();
                if (spellInfo != null)
                    t.AddObject(RuneIcon(TopLeft + new Vector2(18 + 8 + 4, 32 + 8 + nextSpellYPos), spell.GetBaseRune().TextureName));
                for (int i = 0; i < SpellInfo.AttributeRuneSlotCount; i++)
                {
                    AttributeRune rune = spell.GetAttributeRune(i);
                    if (rune != null)
                        t.AddObject(RuneIcon(TopLeft + new Vector2(18 + 8 + 4 + 16 + 16 * i, 32 + 8 + nextSpellYPos), rune.TextureName));
                }*/
                nextSpellYPos += 16;
            }
            var btnSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton");
            var btnSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton_Hover");
            var btnSpr3 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton_Highlighted");

            //Action bar slots
            for (int i = 0; i < GameWorld.State.SpellBar.Count; i++)
            {
                int thisIndex = i;
                int spellId = GameWorld.State.SpellBar[thisIndex];

                SpellInfo spell = null;
                if (GameWorld.State.SpellBook.Count > spellId) spell = GameWorld.State.SpellBook[spellId];

                GameObject slotObj = new GameObject(t.TopLeft + new Vector2(GameWorld.GameWidth / 4 - 24 * 2 + thisIndex * 24, GameWorld.GameHeight - 32));
                slotObj.AddComponent(new SpellInfoRenderer(spell));
                slotObj.AddComponent(new DragDropPoint("playerspell", btnSpr1, btnSpr3, btnSpr2, () =>
                {
                    if (selectedPlayerSpell != null)
                    {
                        GameWorld.State.SpellBar[thisIndex] = GameWorld.State.SpellBook.IndexOf(selectedPlayerSpell);
                        OpenSpellsTab();
                    }
                }
                ));
                t.AddObject(slotObj);
            }

            //Delete spell button
            GameObject deleteObj = new GameObject(t.TopLeft + new Vector2(GameWorld.GameWidth / 2 - 32, GameWorld.GameHeight - 32));
            deleteObj.AddComponent(new SpriteRenderer("Textures/UI/Spellbook/DeleteSpell"));
            deleteObj.AddComponent(new DragDropPoint("playerspell", btnSpr1, btnSpr3, btnSpr2, () =>
            {
                if (selectedPlayerSpell != null)
                {
                    GameWorld.State.SpellBook.Remove(selectedPlayerSpell);
                    OpenSpellsTab();
                }
            }
            ));
            t.AddObject(deleteObj);

            //Edit spell button
        }

        private void OpenRunesTab()
        {
            SpellbookTab t = leftTab;
            t.Clear();

            bottomRightText = "Left click a rune to select\nthen left click a slot\nto add the selected rune.\nRight click to clear slots.";

            var runeBtnSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton");
            var runeBtnSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton_Hover");
            int nextRuneX = 0;
            int nextRuneY = 0;
            int nextRunePos = 0;

            foreach (var spell in StaticData.BaseRunes)
            {
                BaseRune thisSpell = spell;
                Vector2 pos = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 18 + nextRuneX, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 32 + nextRuneY);
                t.AddObject(ObjectBuilder.BuildButton(
                    pos,
                    runeBtnSpr1,
                    runeBtnSpr2,
                    () =>
                    {
                        bottomLeftText = thisSpell.Name + Environment.NewLine + thisSpell.Description;
                        selectedAttrRune = null;
                        selectedBaseRune = thisSpell;
                    }
                    ));
                GameObject runeImageObj = new GameObject(pos + Vector2.One * 8);
                runeImageObj.AddComponent(new SpriteRenderer(StaticData.RuneImagePath + thisSpell.TextureName));
                t.AddObject(runeImageObj);

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
                Vector2 pos = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 18 + nextRuneX, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 32 + nextRuneY);
                t.AddObject(ObjectBuilder.BuildButton(
                    new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 18 + nextRuneX, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 32 + nextRuneY),
                    runeBtnSpr1,
                    runeBtnSpr2,
                    () =>
                    {
                        bottomLeftText = thisRune.Name + Environment.NewLine + thisRune.Description;
                        selectedAttrRune = thisRune;
                        selectedBaseRune = null;
                    }
                    ));
                GameObject runeImageObj = new GameObject(pos + Vector2.One * 8);
                runeImageObj.AddComponent(new SpriteRenderer(StaticData.RuneImagePath + thisRune.TextureName));
                t.AddObject(runeImageObj);

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
            SpellbookTab t = rightTab;
            t.Clear();

            //Spell circle
            GameObject spellCircleObj = new GameObject(scPosition + new Vector2(60, 60));
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
            t.AddObject(ObjectBuilder.BuildButton(
                baseRunePosition - new Vector2(runeSpr1.Width / 2, runeSpr2.Height / 2),
                runeSpr1,
                runeSpr2,
                () =>
                {
                    if (currentlyEditing != null && selectedBaseRune != null)
                    {
                        currentlyEditing.SetBaseRune(StaticData.BaseRunes.IndexOf(selectedBaseRune));
                    }
                    OpenRuneGrid();
                },
                () => { if (currentlyEditing != null) { currentlyEditing.SetBaseRune(-1); OpenRuneGrid(); } }
                ));
            //Attribute runes
            for (int i = 0; i < attrRunePositions.Length; i++)
            {
                int thisPos = i;
                t.AddObject(ObjectBuilder.BuildButton(
                    attrRunePositions[thisPos] - new Vector2(runeSpr1.Width / 2, runeSpr1.Height / 2),
                    runeSpr1,
                    runeSpr2,
                    () =>
                    {
                        if (currentlyEditing != null && selectedAttrRune != null)
                        {
                            currentlyEditing.SetAttributeRune(thisPos, StaticData.AttributeRunes.IndexOf(selectedAttrRune));
                        }
                        OpenRuneGrid();
                    },
                    () => { if (currentlyEditing != null) { currentlyEditing.SetAttributeRune(thisPos, -1); OpenRuneGrid(); } }
                    ));
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
            //drawer[DrawLayer.UI].DrawString(font, bottomRightText, mcPosition, textColor);
            //drawer[DrawLayer.UI].DrawString(font, bottomLeftText, GameWorld.Camera.Position + new Vector2(-GameWorld.GameWidth / 2 + 20, GameWorld.GameHeight / 2 - 60), textColor);

            drawer[DrawLayer.Background].Draw(background, bgPosition, Color.White);

            base.Draw(drawer);
        }
    }
}