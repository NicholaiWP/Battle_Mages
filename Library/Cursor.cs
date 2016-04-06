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
    public class Cursor
    {
        //Fields
        private Texture2D[] sprite = new Texture2D[2];

        private Vector2 position;
        private Rectangle rectangle;
        private int spriteNumber = 0;

        //Properties
        /// <summary>
        /// In this property the position is set equal to the transformed mouse position,
        /// which has been inverted from world space to view space.
        /// </summary>
        public Vector2 GetPosition
        {
            get
            {
                position = Vector2.Transform(Mouse.GetState().Position.ToVector2(),
                    Matrix.Invert(GameWorld.GetInstance.GetCamera.GetViewMatrix));
                return position;
            }
        }

        /// <summary>
        /// In this property the rectangle is set to be a rectangle on the position,
        /// with the current sprite midpoint being the position. (reason for multiplying with 0.5)
        /// </summary>
        public Rectangle GetRectangle
        {
            get
            {
                rectangle = new Rectangle((int)(GetPosition.X - (sprite[spriteNumber].Width * 0.5f)),
                    (int)(GetPosition.Y - (sprite[spriteNumber].Height * 0.5f)),
                    (int)(sprite[spriteNumber].Width * MenuScreenManager.GetInstance.scale),
                    (int)(sprite[spriteNumber].Height * MenuScreenManager.GetInstance.scale));
                return rectangle;
            }
        }

        //Singleton
        private static Cursor instance;

        public static Cursor GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Cursor();
                }
                return instance;
            }
        }

        /// <summary>
        /// Constructor for the cursor
        /// </summary>
        private Cursor()
        {
        }

        /// <summary>
        /// Method for loading the content of the cursor, it is the pictures that are placed in an array
        /// so we can change the pictures of the mouse when certain things happen
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            sprite[0] = content.Load<Texture2D>("Images/battle-mages cursor");
            sprite[1] = content.Load<Texture2D>("Images/battle-mages spell-cursor");
        }

        /// <summary>
        /// The drawing method for the cursor, with an int to choose what picture in the array that has
        /// to be drawn
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteNumber"></param>
        public void Draw(SpriteBatch spriteBatch, int spriteNumber)
        {
            this.spriteNumber = spriteNumber;
            spriteBatch.Draw(sprite[spriteNumber], GetPosition, null, Color.White,
            0f, Vector2.Zero, MenuScreenManager.GetInstance.scale, SpriteEffects.None, 0f);
        }
    }
}