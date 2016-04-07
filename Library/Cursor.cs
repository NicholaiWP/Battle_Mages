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

        //Properties
        /// <summary>
        /// In this property the position is set equal to the transformed mouse position,
        /// which has been inverted from world space to view space.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                position = Vector2.Transform(Mouse.GetState().Position.ToVector2(),
                    Matrix.Invert(GameWorld.Instance.Camera.ViewMatrix));
                return position;
            }
        }

        /// <summary>
        /// In this property the rectangle is set to be a rectangle on the position,
        /// with the current sprite midpoint being the position. (reason for multiplying with 0.5)
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                rectangle = new Rectangle((int)(Position.X -
                    (sprite[GameWorld.Instance.CursorPictureNumber].Width * 0.5f)),
                    (int)(Position.Y - (sprite[GameWorld.Instance.CursorPictureNumber].Height * 0.5f)),
                    (int)(sprite[GameWorld.Instance.CursorPictureNumber].Width *
                    MenuScreenManager.Instance.scale),
                    (int)(sprite[GameWorld.Instance.CursorPictureNumber].Height *
                    MenuScreenManager.Instance.scale));
                return rectangle;
            }
        }

        //Singleton
        private static Cursor instance;

        public static Cursor Instance
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
        /// Method for drawing the cursor
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite[GameWorld.Instance.CursorPictureNumber],
                position: Position,
                origin: Vector2.Zero,
                rotation: 0f,
                scale: new Vector2(MenuScreenManager.Instance.scale, MenuScreenManager.Instance.scale),
                color: Color.White,
                effects: SpriteEffects.None);
        }
    }
}