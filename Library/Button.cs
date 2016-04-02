using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Button
    {
        public Vector2 vector;
        public Texture2D texture;
        public Rectangle rectangle;
        public Vector2 position;
        public bool isClicked = false;

        //white RGB color
        private Color color = Color.White;

        /// <summary>
        /// Button class' constructor
        /// </summary>
        /// <param name="newTexture"></param>
        /// <param name="graphics"></param>
        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            //changes the size of the buttons
            texture = newTexture;
        }

        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            //sets a mouseRectangle
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y,1,1);
            //If the mouse's rectangle intersects with a rectangle check if the button is clicked
            //-by using the bool "isDown".
            if (mouseRectangle.Intersects(rectangle))
            {
                if (color == Color.White)
                {
                    color = Color.Black;
                    isClicked = false;
                }

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;
                }
            }
            else 
            {
                color = Color.White;
                isClicked = false;
            }
        }

        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Load(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture;
            position = newPosition;
        }

        /// <summary>
        /// Draws the rectangle using a texture, rectangle and color
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, null, color,
                        0f, Vector2.Zero, SpriteEffects.None, 0.1f);
        }
    }
}