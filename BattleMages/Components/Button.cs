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

        private ClickDelegate onClick;
        private ClickDelegate onRightClick;
        private Vector2 startPos;
        private float offset;
        private bool wiggle;
        private bool centered;

        public delegate void ClickDelegate();

        private Texture2D ActiveTex
        {
            get
            {
                return hovering ? hoverTex : normalTex;
            }
        }

        public Button(Texture2D normalTex, Texture2D hoverTex, ClickDelegate onClick, ClickDelegate onRightClick = null, bool wiggle = false, bool centered = false)
        {
            this.normalTex = normalTex;
            this.hoverTex = hoverTex;
            this.onClick = onClick;
            this.onRightClick = onRightClick;
            this.wiggle = wiggle;
            this.centered = centered;

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

            Rectangle rectangle;
            if (centered)
            {
                rectangle = new Rectangle((int)GameObject.Transform.Position.X - normalTex.Width / 2, (int)GameObject.Transform.Position.Y - normalTex.Height / 2,
                    (normalTex.Width),
                    (normalTex.Height));
            }
            else
            {
                rectangle = new Rectangle((int)GameObject.Transform.Position.X, (int)GameObject.Transform.Position.Y,
                    (normalTex.Width),
                    (normalTex.Height));
            }

            if (rectangle.Contains(GameWorld.Cursor.Position))
            {
                hovering = true;
                GameWorld.Cursor.SetCursor(CursorStyle.Interactable);

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
            if (centered)
            {
                spriteBatch.Draw(ActiveTex,
                    position: GameObject.Transform.Position - Utils.HalfTexSize(ActiveTex),
                    origin: Vector2.Zero,
                    effects: SpriteEffects.None,
                    color: Color.White);
            }
            else
            {
                spriteBatch.Draw(ActiveTex,
                    position: GameObject.Transform.Position,
                    origin: Vector2.Zero,
                    effects: SpriteEffects.None,
                    color: Color.White);
            }
        }
    }
}