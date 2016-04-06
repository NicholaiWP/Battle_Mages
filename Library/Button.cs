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
        //fields

        private int hoverNumber;
        public Vector2 vector;
        public Texture2D[] sprite = new Texture2D[2];
        public Rectangle rectangle;
        public Vector2 position;
        public bool isClicked = false;

        /// <summary>
        /// Button class' constructor
        /// </summary>
        /// <param name="newSprite1"></param>
        /// <param name="graphics"></param>
        public Button(Texture2D newSprite1, Texture2D newSprite2)
        {
            //changes the size of the buttons
            sprite[0] = newSprite1;
            sprite[1] = newSprite2;
            hoverNumber = 0;
        }

        public void Update()
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y,
                (int)(sprite[hoverNumber].Width * MenuScreenManager.GetInstance.scale),
                (int)(sprite[hoverNumber].Height * MenuScreenManager.GetInstance.scale));
            //sets a mouseRectangle
            //If the mouse's rectangle intersects with a rectangle check if the button is clicked
            //-by using the bool "isClicked".
            if (Cursor.GetInstance.GetRectangle.Intersects(rectangle))
            {
                if (hoverNumber == 0)
                {
                    hoverNumber = 1;
                    isClicked = false;
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                    MenuScreenManager.GetInstance.mouseCanClickButton)
                {
                    isClicked = true;
                    MenuScreenManager.GetInstance.mouseCanClickButton = false;
                }
            }
            else
            {
                hoverNumber = 0;
                isClicked = false;
            }
        }

        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Load(Texture2D newTexture, Vector2 newPosition)
        {
            // sprite = newTexture;
            //position = newPosition;
        }

        /// <summary>
        /// Draws the rectangle using a texture, rectangle and color
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            /* spriteBatch.Draw(sprite[hoverNumber], rectangle, null, Color.White,
             0f, Vector2.Zero, SpriteEffects.None, 0.1f);*/

            spriteBatch.Draw(sprite[hoverNumber], position, null, Color.White, 0f, Vector2.Zero,
                MenuScreenManager.GetInstance.scale, SpriteEffects.None, 0.1f);
        }
    }
}