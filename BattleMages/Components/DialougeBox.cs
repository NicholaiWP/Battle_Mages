﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    internal class DialougeBox : Component
    {
        private Texture2D boxTexture;
        private SpriteFont textFont;
        private string text;
        private int charactersToDraw = 0;

        private float timer = 0;
        private const float charShowInterval = 0.03f;

        public DialougeBox(string text)
        {
            this.text = text;

            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Initialize(InitializeMsg msg)
        {
            boxTexture = GameWorld.Load<Texture2D>("Images/DialougeBox");
            textFont = GameWorld.Load<SpriteFont>("FontBM");
        }

        private void Update(UpdateMsg msg)
        {
            if (charactersToDraw < text.Length)
            {
                timer -= GameWorld.DeltaTime;
                while (timer <= 0 && charactersToDraw < text.Length - 1)
                {
                    charactersToDraw++;
                    timer += charShowInterval;
                }
            }
        }

        private void Draw(DrawMsg msg)
        {
            string warpedText = Utils.WarpText(text.Substring(0, charactersToDraw), boxTexture.Width - 8, textFont);
            Vector2 pos = GameWorld.Camera.Position + new Vector2(-boxTexture.Width / 2, GameWorld.GameHeight / 2 - boxTexture.Height);
            msg.Drawer[DrawLayer.UI].Draw(boxTexture, pos, Color.White);
            msg.Drawer[DrawLayer.UI].DrawString(textFont, warpedText, pos + new Vector2(4, 2), Color.White);
        }
    }
}