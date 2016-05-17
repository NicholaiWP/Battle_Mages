﻿using Microsoft.Xna.Framework;
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
        private DisplayMode currentResolution;
        private DisplayMode resolutionHolder;
        private List<DisplayMode> resolutions = new List<DisplayMode>();
        private string currentResolutionString;
        private GraphicsDeviceManager graphics;

        public int ElementAtNumber { get; set; }
        public bool SwappingKeyBind { get; set; } = false;
        public PlayerBind ChosenKeyToRebind { get; set; }

        public SettingsScene()
        {
            graphics = GameWorld.Graphics;
            var content = GameWorld.Instance.Content;
            //Back button
            var backSpr = content.Load<Texture2D>("Images/Back");
            AddObject(new Button(
                    backSpr,
                    backSpr,
                    new Vector2(-backSpr.Width / 2, backSpr.Height * 2f),
                    () => { GameWorld.ChangeScene(new MenuScene()); }
                ));
            /* settingsButtons.Add(new Button(MenuButtons.KeyBindDown));
             settingsButtons.Add(new Button(MenuButtons.KeyBindLeft));
             settingsButtons.Add(new Button(MenuButtons.KeyBindRight));*/
            //Keybind up
            var keyBindUpSpr = content.Load<Texture2D>("Images/Rebind");
            AddObject(new Button(
                keyBindUpSpr,
                keyBindUpSpr,
                new Vector2(-keyBindUpSpr.Width / 2, -78),
                () =>
                {
                    SwappingKeyBind = true;
                    ChosenKeyToRebind = PlayerBind.Up;
                    ChosenKeyToRebind = PlayerBind.Down;
                    ChosenKeyToRebind = PlayerBind.Left;
                    ChosenKeyToRebind = PlayerBind.Right;
                    ChosenKeyToRebind = PlayerBind.Spell1;
                    ChosenKeyToRebind = PlayerBind.Spell2;
                    ChosenKeyToRebind = PlayerBind.Spell3;
                    ChosenKeyToRebind = PlayerBind.Spell4;
                }));
            //Res down
            var resDown = content.Load<Texture2D>("Images/ResDown");
            AddObject(new Button(
                resDown,
                resDown,
                new Vector2(-64 - resDown.Width / 2, -50),
                () => { ElementAtNumber--; }
                ));
            //Res up
            var resUp = content.Load<Texture2D>("Images/ResUp");
            AddObject(new Button(
                resUp,
                resUp,
                new Vector2(64 - resUp.Width / 2, -50),
                () => { ElementAtNumber++; }
                ));

            fontBM = content.Load<SpriteFont>("FontBM");
            background = content.Load<Texture2D>("Images/BMmenu");
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
            if (SwappingKeyBind)
            {
                GameWorld.Cursor.CanClick = false;
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

            foreach (GameObject button in ActiveObjects)
            {
                button.Update();
            }
        }

        private void ReadNewKey()
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] keyPressed = kbState.GetPressedKeys();
            if (keyPressed.Length == 1)
            {
                GameWorld.PlayerControls.ChangeBinding(ChosenKeyToRebind, keyPressed[0]);
                SwappingKeyBind = false;
                GameWorld.Cursor.CanClick = true;
            }
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Background].Draw(background, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
               GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2));

            foreach (GameObject go in ActiveObjects)
            {
                go.Draw(drawer);
            }

            drawer[DrawLayer.UI].DrawString(fontBM, currentResolutionString, Vector2.Zero,
                Color.Black);
            drawer[DrawLayer.UI].DrawString(fontBM,
                GameWorld.PlayerControls.KeyToString(GameWorld.PlayerControls.GetBinding(PlayerBind.Up)),
                new Vector2(64, -72), Color.Black);
        }
    }
}