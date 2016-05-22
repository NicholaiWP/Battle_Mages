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
    public class Button : Component, ICanUpdate, ICanBeDrawn
    {
        private int hoverNumber = 0;
        private Texture2D[] sprites = new Texture2D[2];
        private Rectangle rectangle;

        private ClickDelegate onClick;
        private ClickDelegate onRightClick;
        private Vector2 startPos;
        private float offset;
        private bool wiggle;

        public delegate void ClickDelegate();

        public Button(GameObject gameObject, Texture2D normalTex, Texture2D hoverTex, ClickDelegate onClick, ClickDelegate onRightClick = null, bool wiggle = false) : base(gameObject)
        {
            sprites[0] = normalTex;
            sprites[1] = hoverTex;
            startPos = GameObject.Transform.Position;
            offset = GameObject.Transform.Position.Y * 0.02f;
            this.onClick = onClick;
            this.wiggle = wiggle;
            this.onRightClick = onRightClick;
        }

        public void UpdatePosition(Vector2 newPos)
        {
            GameObject.Transform.Position = startPos + newPos;
        }

        public void Update()
        {
            if (wiggle)
            {
                offset += 0.02f;
                GameObject.Transform.Position = startPos + new Vector2((float)Math.Sin(offset) * 8, 0);
            }

            rectangle = new Rectangle((int)GameObject.Transform.Position.X, (int)GameObject.Transform.Position.Y,
                (sprites[hoverNumber].Width),
                (sprites[hoverNumber].Height));

            if (rectangle.Contains(GameWorld.Cursor.Position))
            {
                if (hoverNumber == 0)
                {
                    hoverNumber = 1;
                }

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
                hoverNumber = 0;
            }
        }

        public void Draw(Drawer drawer)
        {
            SpriteBatch spriteBatch = drawer[DrawLayer.UI];
            spriteBatch.Draw(sprites[hoverNumber],
                destinationRectangle: rectangle,
                origin: Vector2.Zero,
                effects: SpriteEffects.None,
                color: Color.White);
        }
    }
}