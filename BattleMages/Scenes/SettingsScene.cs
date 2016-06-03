using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class SettingsScene : Scene
    {
        private Texture2D background;

        private DisplayMode currentResolution;
        private DisplayMode resolutionHolder;
        private List<DisplayMode> resolutions = new List<DisplayMode>();
        private string currentResolutionString;
        private List<string> resolutionStrings = new List<string>();
        private GraphicsDeviceManager graphics;
        private CursorLockToken keybindSwappingLock;
        private SpriteFont fontBM;
        private List<DisplayMode> allResolutions = new List<DisplayMode>(GraphicsAdapter.DefaultAdapter.SupportedDisplayModes);
        public int ElementAtNumber { get; set; }
        public PlayerBind ChosenKeyToRebind { get; set; }

        public SettingsScene()
        {
            graphics = GameWorld.Graphics;
            var content = GameWorld.Instance.Content;
            //Back button
            var backSpr = content.Load<Texture2D>("Textures/UI/Menu/Back");
            AddObject(ObjectBuilder.BuildButton(
                    new Vector2(GameWorld.Camera.Position.X + backSpr.Width / 2, GameWorld.Camera.Position.Y + backSpr.Height * 2f),
                    backSpr,
                    backSpr,
                    () => { GameWorld.ChangeScene(new MenuScene()); }
                ));

            //Keybind up
            /* var keyBindUpSpr = content.Load<Texture2D>("Textures/UI/Menu/Rebind");
             AddObject(ObjectBuilder.BuildButton(
                 new Vector2(GameWorld.Camera.Position.X - keyBindUpSpr.Width / 2, GameWorld.Camera.Position.Y - 78),
                 keyBindUpSpr,
                 keyBindUpSpr,
                 () =>
                 {
                     keybindSwappingLock = GameWorld.Cursor.Lock();
                     ChosenKeyToRebind = PlayerBind.Up;
                 }));
             //Res down
              var resDown = content.Load<Texture2D>("Textures/UI/Menu/ResDown");
              AddObject(ObjectBuilder.BuildButton(
                  new Vector2(GameWorld.Camera.Position.X - 64 - resDown.Width / 2, GameWorld.Camera.Position.Y - 50),
                  resDown,
                  resDown,
                  () => { ElementAtNumber--; }
                  ));
              //Res up
              var resUp = content.Load<Texture2D>("Textures/UI/Menu/ResUp");
              AddObject(ObjectBuilder.BuildButton(
                  new Vector2(GameWorld.Camera.Position.X + 64 - resUp.Width / 2, GameWorld.Camera.Position.Y - 50),
                  resUp,
                  resUp,
                  () => { ElementAtNumber++; }
                  ));*/

            fontBM = content.Load<SpriteFont>("FontBM");
            background = content.Load<Texture2D>("Textures/Backgrounds/Menu");
            DisplayMode lastResolution = null;
            int i = 0;
            foreach (DisplayMode dmode in allResolutions)
            {
                resolutionStrings.Add(dmode.Width + "x" + dmode.Height);
                if (dmode.Width == GameWorld.Instance.ResScreenWidth &&
                    dmode.Height == GameWorld.Instance.ResScreenHeight)
                {
                    currentResolution = dmode;
                    currentResolutionString = (dmode.Width + "x" + dmode.Height);
                    ElementAtNumber = allResolutions.IndexOf(dmode);
                }

                if (lastResolution != dmode)
                {
                    resolutions.Add(dmode);
                    i++;
                }
                lastResolution = dmode;
            }

            int x = 10;
            int y = 60;
            foreach (string res in resolutionStrings)
            {
                var button = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/mediumButton");
                var buttonHover = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/mediumButtonHL");
                AddObject(ObjectBuilder.BuildButton(new Vector2(x + GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                y + GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2), button, buttonHover,
                    () => ElementAtNumber = resolutionStrings.IndexOf(res)));
                y += 25;

                if (y >= 200)
                {
                    x += 60;
                    y = 60;
                }
            }

            var fullScreenButton = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/mediumButton");
            var fullScreenbuttonHover = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/mediumButtonHL");
            AddObject(ObjectBuilder.BuildButton(new Vector2(250 + GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                60 + GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2), fullScreenButton, fullScreenbuttonHover,
                () =>
                {
                    if (!graphics.IsFullScreen) graphics.IsFullScreen = true;
                    if (graphics.IsFullScreen) graphics.IsFullScreen = false;
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

        public override void Draw(Drawer drawer)
        {
            int x = 20;
            int y = 65;
            Color color;
            drawer[DrawLayer.Background].Draw(background, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
               GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2));

            for (int i = 0; i < resolutionStrings.Count; i++)
            {
                color = Color.Black;
                if (resolutionStrings[i] == currentResolutionString)
                {
                    color = Color.LightYellow;
                }
                drawer[DrawLayer.AboveUI].DrawString(fontBM, resolutionStrings[i], new Vector2(x + GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                y + GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2), color);

                y += 25;

                if (y >= 200)
                {
                    x += 60;
                    y = 65;
                }
            }
            color = Color.Black;

            if (graphics.IsFullScreen)
            {
                color = Color.LightYellow;
            }

            drawer[DrawLayer.AboveUI].DrawString(fontBM, "Full Screen", new Vector2(257 + GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                66 + GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2), color);

            drawer[DrawLayer.UI].DrawString(fontBM,
                GameWorld.PlayerControls.KeyToString(GameWorld.PlayerControls.GetBinding(PlayerBind.Up)),
                new Vector2(GameWorld.Camera.Position.X + 64, GameWorld.Camera.Position.Y - 72), Color.Black);

            base.Draw(drawer);
        }
    }
}