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
        public Vector2 size;
        public Vector2 position;
        private bool isDown;
        public bool isClicked;

        //white RGB color
        private Color color = new Color(255, 255, 255, 255);

        /// <summary>
        /// Button class' constructor
        /// </summary>
        /// <param name="newTexture"></param>
        /// <param name="graphics"></param>
        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            //changes the size of the buttons
            size = new Vector2(graphics.Viewport.Width / 5, graphics.Viewport.Height / 20);
        }

        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            //sets a mouseRectangle
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);
            //If the mouse's rectangle intersects with a rectangle check if the button is clicked
            //-by using the bool "isDown".
            if (mouseRectangle.Intersects(rectangle))
            {
                if (color.A == 255)
                {
                    isDown = false;
                }
                else if (color.A == 0)
                {
                    isDown = true;
                }
                else if (isDown)
                {
                    color.A += 3;
                }
                else
                {
                    color.A -= 3;
                }

                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;
            }
            else if (color.A < 255)
            {
                color.A += 3;
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
            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}