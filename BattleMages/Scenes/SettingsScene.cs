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

        private DisplayMode currentResolution;
        private DisplayMode resolutionHolder;
        private List<DisplayMode> resolutions = new List<DisplayMode>();
        private string currentResolutionString;
        private GraphicsDeviceManager graphics;
        private CursorLockToken keybindSwappingLock;
        private SpriteFont fontBM;

        public int ElementAtNumber { get; set; }
        public PlayerBind ChosenKeyToRebind { get; set; }

        public SettingsScene()
        {
            graphics = GameWorld.Graphics;
            var content = GameWorld.Instance.Content;
            //Back button
            var backSpr = content.Load<Texture2D>("Textures/UI/Menu/Back");
            AddObject(ObjectBuilder.BuildButton(
                    new Vector2(GameWorld.Camera.Position.X - backSpr.Width / 2, GameWorld.Camera.Position.Y + backSpr.Height * 2f),
                    backSpr,
                    backSpr,
                    () => { GameWorld.ChangeScene(new MenuScene()); }
                ));
            /* settingsButtons.Add(ObjectBuilder.BuildButton(MenuButtons.KeyBindDown));
             settingsButtons.Add(ObjectBuilder.BuildButton(MenuButtons.KeyBindLeft));
             settingsButtons.Add(ObjectBuilder.BuildButton(MenuButtons.KeyBindRight));*/
            //Keybind up
            var keyBindUpSpr = content.Load<Texture2D>("Textures/UI/Menu/Rebind");
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
                ));

            fontBM = content.Load<SpriteFont>("FontBM");
            background = content.Load<Texture2D>("Textures/Backgrounds/Menu");
            DisplayMode lastResolution = null;
            int i = 0;
            foreach (DisplayMode dmode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (dmode.Height == GameWorld.Instance.HalfViewPortHeight * 2 &&
                    dmode.Width == GameWorld.Instance.HalfViewPortWidth * 2)
                {
                    currentResolution = dmode;
                    currentResolutionString = (dmode.Width + "x" + dmode.Height);
                }

                if (lastResolution != dmode)
                {
                    resolutions.Add(dmode);
                    i++;
                }
                lastResolution = dmode;
            }
            ElementAtNumber = i - 1;
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
            drawer[DrawLayer.Background].Draw(background, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
               GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2));

            drawer[DrawLayer.UI].DrawString(fontBM, currentResolutionString, new Vector2(GameWorld.Camera.Position.X,
                GameWorld.Camera.Position.Y), Color.Black);
            drawer[DrawLayer.UI].DrawString(fontBM,
                GameWorld.PlayerControls.KeyToString(GameWorld.PlayerControls.GetBinding(PlayerBind.Up)),
                new Vector2(GameWorld.Camera.Position.X + 64, GameWorld.Camera.Position.Y - 72), Color.Black);

            base.Draw(drawer);
        }
    }
}