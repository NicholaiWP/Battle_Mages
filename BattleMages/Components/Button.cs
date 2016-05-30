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
    public class Button : Component
    {
        private bool hovering;
        private Texture2D normalTex;
        private Texture2D hoverTex;
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

        public Button(Texture2D normalTex, Texture2D hoverTex, ClickDelegate onClick, ClickDelegate onRightClick = null, bool wiggle = false)
        {
            this.normalTex = normalTex;
            this.hoverTex = hoverTex;
            this.onClick = onClick;
            this.onRightClick = onRightClick;
            this.wiggle = wiggle;

            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Initialize(InitializeMsg message)
        {
            startPos = GameObject.Transform.Position;
            offset = GameObject.Transform.Position.Y * 0.02f;
        }

        private void Update(UpdateMsg msg)
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

                if (GameWorld.Cursor.LeftButtonPressed)
                {
                    onClick?.Invoke();
                }
                else if (GameWorld.Cursor.RightButtonPressed)
                {
                    onRightClick?.Invoke();
                }
            }
            else
            {
                hovering = false;
            }
        }

        private void Draw(DrawMsg msg)
        {
            SpriteBatch spriteBatch = msg.Drawer[DrawLayer.UI];
            spriteBatch.Draw(ActiveTex,
                destinationRectangle: rectangle,
                origin: Vector2.Zero,
                effects: SpriteEffects.None,
                color: Color.White);
        }
    }
}