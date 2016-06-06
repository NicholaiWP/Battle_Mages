using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
    public class SettingsScene : Scene
    {
        private Texture2D background;
        private const int shownButtons = 3;
        private UITab resTab;
        private DisplayMode currentResolution;
        private DisplayMode resolutionHolder;
        private List<DisplayMode> resolutions = new List<DisplayMode>();
        private string currentResolutionString;
        private List<string> resolutionStrings = new List<string>();
        private GraphicsDeviceManager graphics;
        private CursorLockToken keybindSwappingLock;
        private int minIndex;
        private SpriteFont fontBM;
        private List<DisplayMode> allResolutions = new List<DisplayMode>(GraphicsAdapter.DefaultAdapter.SupportedDisplayModes);
        public int ElementAtNumber { get; set; }
        public PlayerBind ChosenKeyToRebind { get; set; }

        public SettingsScene()
        {
            resTab = new UITab(this, Vector2.Zero, Vector2.Zero);
            graphics = GameWorld.Graphics;
            var content = GameWorld.Instance.Content;
            //Back button
            var backBtn = content.Load<Texture2D>("Textures/UI/Spellbook/mediumButton");
            var backBtnHL = content.Load<Texture2D>("Textures/UI/Spellbook/mediumButtonHL");
            AddObject(ObjectBuilder.BuildButton(
                    new Vector2(GameWorld.Camera.Position.X - backBtn.Width / 2,
                    GameWorld.Camera.Position.Y + 50),
                    backBtn,
                    backBtnHL,
                    () => { GameWorld.ChangeScene(new MenuScene()); }
                ));

            fontBM = content.Load<SpriteFont>("FontBM");
            background = content.Load<Texture2D>("Textures/Backgrounds/Menu");
            string lastResolution = null;
            foreach (DisplayMode dmode in allResolutions)
            {
                if (dmode.Width == GameWorld.Instance.ResScreenWidth &&
                    dmode.Height == GameWorld.Instance.ResScreenHeight)
                {
                    currentResolution = dmode;
                    currentResolutionString = (dmode.Width + "x" + dmode.Height);
                    ElementAtNumber = allResolutions.IndexOf(dmode);
                }

                if (lastResolution != dmode.Width + "x" + dmode.Height)
                {
                    resolutionStrings.Add(dmode.Width + "x" + dmode.Height);
                    resolutions.Add(dmode);
                }
                lastResolution = dmode.Width + "x" + dmode.Height;
            }
            ElementAtNumber = resolutionStrings.Count - 1;
            minIndex = resolutionStrings.Count - shownButtons;
            InsertButtons();

            var btnResUpSpr1 = GameWorld.Load<Texture2D>("Textures/Misc/ArrowLeft");
            var btnResUpSpr2 = GameWorld.Load<Texture2D>("Textures/Misc/ArrowLeft");
            AddObject(ObjectBuilder.BuildButton(new Vector2(50 + GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                    62 + GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2), btnResUpSpr1, btnResUpSpr2,
                    () =>
                    {
                        if (minIndex >= 0) minIndex -= shownButtons;
                        if (minIndex <= 0)
                        {
                            minIndex = 0;
                        }
                        InsertButtons();
                    }));

            var btnResDownSpr1 = GameWorld.Load<Texture2D>("Textures/Misc/ArrowRight");
            var btnResDownSpr2 = GameWorld.Load<Texture2D>("Textures/Misc/ArrowRight");
            AddObject(ObjectBuilder.BuildButton(new Vector2(GameWorld.Camera.Position.X + GameWorld.GameWidth / 2 - 65,
                    62 + GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2), btnResDownSpr1, btnResDownSpr2,
                    () =>
                    {
                        if (minIndex <= resolutionStrings.Count) minIndex += shownButtons;
                        if (minIndex >= resolutionStrings.Count - shownButtons)
                        {
                            minIndex = resolutionStrings.Count - shownButtons;
                        }
                        InsertButtons();
                    }));
            var fullScreenButton = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/mediumButton");
            var fullScreenbuttonHover = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/mediumButtonHL");
            AddObject(ObjectBuilder.BuildButton(new Vector2(-fullScreenButton.Width / 2 + GameWorld.Camera.Position.X,
                100 + GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2), fullScreenButton, fullScreenbuttonHover,
                () =>
                {
                    graphics.IsFullScreen = !graphics.IsFullScreen;
                    graphics.ApplyChanges();
                }));

            if (currentResolution != null)
                GameWorld.Instance.ScalingVector = new Vector2(Utils.CalculateWidthScale(currentResolution.Width),
                    Utils.CalculateHeightScale(currentResolution.Height));
        }

        public override void Update()
        {
            if (keybindSwappingLock != null)
            {
                ReadNewKey();
            }
            if (ElementAtNumber >= resolutions.Count) ElementAtNumber = 0;
            if (ElementAtNumber < 0) ElementAtNumber = resolutions.Count - 1;

            currentResolution = resolutions[ElementAtNumber];

            if (currentResolution != resolutionHolder)
            {
                graphics.PreferredBackBufferWidth = currentResolution.Width;
                graphics.PreferredBackBufferHeight = currentResolution.Height;
                graphics.ApplyChanges();
                GameWorld.Instance.ResScreenWidth = currentResolution.Width;
                GameWorld.Instance.ResScreenHeight = currentResolution.Height;
                GameWorld.Instance.ScalingVector = new Vector2(Utils.CalculateWidthScale(currentResolution.Width),
                    Utils.CalculateHeightScale(currentResolution.Height));
            }
            resolutionHolder = currentResolution;

            for (int i = 0; i < resolutions.Count; i++)
            {
                if (currentResolution == resolutions.ElementAt(i))
                {
                    ElementAtNumber = i;
                    currentResolutionString = (currentResolution.Width + "x" + currentResolution.Height);
                }
            }

            base.Update();
        }

        private void ReadNewKey()
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] keyPressed = kbState.GetPressedKeys();
            if (keyPressed.Length == 1)
            {
                GameWorld.PlayerControls.ChangeBinding(ChosenKeyToRebind, keyPressed[0]);
                keybindSwappingLock.Unlock();
                keybindSwappingLock = null;
            }
        }

        private void InsertButtons()
        {
            resTab.Clear();
            int x = 70;
            int y = 60;
            foreach (string res in resolutionStrings)
            {
                if (resolutionStrings.IndexOf(res) >= minIndex &&
                resolutionStrings.IndexOf(res) < minIndex + shownButtons)
                {
                    var button = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/mediumButton");
                    var buttonHover = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/mediumButtonHL");
                    resTab.AddObject(ObjectBuilder.BuildButton(new Vector2(x + GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                    y + GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2), button, buttonHover,
                        () => ElementAtNumber = resolutionStrings.IndexOf(res)));
                    x += 60;
                }
            }
        }

        public override void Draw(Drawer drawer)
        {
            Color normalCol = Color.White;
            Color selectedCol = Color.Violet;

            Color color;
            int x = 80;
            int y = 65;
            drawer[DrawLayer.Background].Draw(background, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
               GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2));

            foreach (var res in resolutionStrings)
            {
                if (resolutionStrings.IndexOf(res) >= minIndex && resolutionStrings.IndexOf(res) < minIndex + shownButtons)
                {
                    color = normalCol;
                    if (res == currentResolutionString)
                    {
                        color = selectedCol;
                    }
                    drawer[DrawLayer.AboveUI].DrawString(fontBM, res, new Vector2(x + GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                    y + GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2), color);

                    x += 60;
                }
            }
            color = normalCol;

            drawer[DrawLayer.AboveUI].DrawString(fontBM, "Resolution", new Vector2(GameWorld.Camera.Position.X - 22,
                            GameWorld.Camera.Position.Y - 48), color);

            drawer[DrawLayer.UI].DrawString(fontBM, "Back", new Vector2(GameWorld.Camera.Position.X - 11,
                            145 + GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2), color);

            if (graphics.IsFullScreen)
            {
                color = selectedCol;
            }

            drawer[DrawLayer.AboveUI].DrawString(fontBM, "Full Screen", new Vector2(GameWorld.Camera.Position.X - 25,
                105 + GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2), color);

            base.Draw(drawer);
        }
    }
}