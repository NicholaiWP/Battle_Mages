using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battle_Mages
{
    public class Cursor
    {
        //Fields
        private Texture2D[] sprite = new Texture2D[2];

        private Vector2 position;

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
                    GameWorld.Camera.WorldMatrix);
                if (position.X > 674 + GameWorld.Camera.Position.X)
                {
                    position.X = 674 + GameWorld.Camera.Position.X;
                }
                if (position.Y > 375 + GameWorld.Camera.Position.Y)
                {
                    position.Y = 375 + GameWorld.Camera.Position.Y;
                }
                return position;
            }
        }

        public int CursorPictureNumber { get; set; } = 0;

        /// <summary>
        /// Method for loading the content of the cursor, it is the pictures that are placed in an array
        /// so we can change the pictures of the mouse when certain things happen
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            sprite[0] = content.Load<Texture2D>("Images/BMcursor2");
            sprite[1] = content.Load<Texture2D>("Images/battle-mages spell-cursor");
        }

        /// <summary>
        /// Method for drawing the cursor
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite[CursorPictureNumber],
                position: Position,
                origin: Vector2.Zero,
                color: Color.White,
                effects: SpriteEffects.None);
        }
    }
}
