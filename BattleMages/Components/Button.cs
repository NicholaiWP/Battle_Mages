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
    public class Button : Component, ICanUpdate, ICanBeDrawn
    {
        private bool hovering;
        Texture2D normalTex;
        Texture2D hoverTex;
        private Texture2D[] sprites = new Texture2D[2];
        private Rectangle rectangle;

        private ClickDelegate onClick;
        private ClickDelegate onRightClick;
        private Vector2 startPos;
        private float offset;
        private bool wiggle;

        public delegate void ClickDelegate();

        private Texture2D ActiveTex
        {
            get
            {
                return hovering ? hoverTex : normalTex;
            }
        }

        public Button(GameObject gameObject, Texture2D normalTex, Texture2D hoverTex, ClickDelegate onClick, ClickDelegate onRightClick = null, bool wiggle = false) : base(gameObject)
        {
            startPos = GameObject.Transform.Position;
            offset = GameObject.Transform.Position.Y * 0.02f;

            this.normalTex = normalTex;
            this.hoverTex = hoverTex;
            this.onClick = onClick;
            this.onRightClick = onRightClick;
            this.wiggle = wiggle;
        }

        public void Update()
        {
            if (wiggle)
            {
                offset += 0.02f;
                GameObject.Transform.Position = startPos + new Vector2((float)Math.Sin(offset) * 8, 0);
            }

            rectangle = new Rectangle((int)GameObject.Transform.Position.X, (int)GameObject.Transform.Position.Y,
                (ActiveTex.Width),
                (ActiveTex.Height));

            if (rectangle.Contains(GameWorld.Cursor.Position))
            {
                hovering = true;

                if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                    GameWorld.Cursor.CanClick)
                {
                    GameWorld.Cursor.CanClick = false;
                    //Invoke the onClick delegate when the button is clicked
                    onClick();
                }
                else if (Mouse.GetState().RightButton == ButtonState.Pressed && GameWorld.Cursor.CanClick)
                {
                    GameWorld.Cursor.CanClick = false;
                    onRightClick?.Invoke();
                }
            }
            else
            {
                hovering = false;
            }
        }

        public void Draw(Drawer drawer)
        {
            SpriteBatch spriteBatch = drawer[DrawLayer.UI];
            spriteBatch.Draw(ActiveTex,
                destinationRectangle: rectangle,
                origin: Vector2.Zero,
                effects: SpriteEffects.None,
                color: Color.White);
        }
    }
}