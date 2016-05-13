using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Button : GameObject
    {
        //fields

        private int hoverNumber = 0;
        private Texture2D[] sprites = new Texture2D[2];
        private Rectangle rectangle;
        private Vector2 position;

        private ClickDelegate onClick;
        private Vector2 startPos;
        private float offset;
        private bool wiggle;

        public delegate void ClickDelegate();

        /// <summary>
        /// Button class' constructor
        /// </summary>
        /// <param name="newSprite1"></param>
        /// <param name="graphics"></param>
        public Button(Texture2D normalTex, Texture2D hoverTex, Vector2 position, ClickDelegate onClick, bool wiggle = false) : base (position)
        {
            sprites[0] = normalTex;
            sprites[1] = hoverTex;
            this.position = position;
            startPos = position;
            offset = position.Y * 0.02f;
            this.onClick = onClick;
            this.wiggle = wiggle;
        }

        public void UpdatePosition(Vector2 newPos)
        {
           position = startPos + newPos;
        }

        public override void Update()
        {
            if (wiggle)
            {
                offset += 0.02f;
                position = startPos + new Vector2((float)Math.Sin(offset) * 8, 0);
            }

            rectangle = new Rectangle((int)position.X, (int)position.Y,
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
            }
            else
            {
                hoverNumber = 0;
            }
        }

        /// <summary>
        /// Draws the rectangle using a texture, rectangle and color
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(Drawer drawer)
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