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
    public enum CursorStyle
    {
        Interactable
    }

    public class Cursor
    {
        Texture2D defaultTex;
        Dictionary<CursorStyle, Texture2D> variations = new Dictionary<CursorStyle, Texture2D>();

        Texture2D activeTex;

        private Vector2 position;
        private bool canClick = true;

        private bool leftButtonHeld = false;

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

        public bool CanClick { get { return canClick; } set { canClick = value; } }

        public bool LeftButtonPressed { get; private set; }

        /// <summary>
        /// Method for loading the content of the cursor, it is the pictures that are placed in an array
        /// so we can change the pictures of the mouse when certain things happen
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            defaultTex = content.Load<Texture2D>("Images/Bmcursor2");
            activeTex = defaultTex;

            variations.Add(CursorStyle.Interactable, content.Load<Texture2D>("Images/Bmcursor1"));
        }

        public void Update()
        {
            MouseState state = Mouse.GetState();
            if (state.LeftButton == ButtonState.Pressed)
            {
                if (!leftButtonHeld)
                {
                    leftButtonHeld = true;
                    LeftButtonPressed = true;
                }
                else
                {
                    LeftButtonPressed = false;
                }
            }
            else
            {
                if (leftButtonHeld)
                {
                    leftButtonHeld = false;
                    LeftButtonPressed = false;
                }
            }
        }

        public void SetCursor(CursorStyle style)
        {
            activeTex = variations[style];
        }

        /// <summary>
        /// Method for drawing the cursor
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(activeTex,
                position: Position,
                origin: Vector2.Zero,
                color: Color.White,
                effects: SpriteEffects.None);
            activeTex = defaultTex;
        }
    }
}