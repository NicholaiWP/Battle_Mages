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
    public enum CursorStyle
    {
        Interactable,
        Dialouge,
        OutOfRange,
    }

    public class CursorLockToken
    {
        public event EventHandler OnUnlock;

        public CursorLockToken()
        {
        }

        public void Unlock()
        {
            OnUnlock?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Cursor
    {
        private Texture2D defaultTex;
        private Dictionary<CursorStyle, Texture2D> variations = new Dictionary<CursorStyle, Texture2D>();

        private Texture2D activeTex;
        private int activeTexIndex = -1;

        private Vector2 position;

        //As long as this list has entries mouse clicks will not be registered
        private List<CursorLockToken> lockTokens = new List<CursorLockToken>();

        private bool leftButtonPressed;
        private bool leftButtonHeld;
        private bool rightButtonPressed;
        private bool rightButtonHeld;

        /// <summary>
        /// In this property the position is set equal to the transformed mouse position,
        /// which has been inverted from world space to view space.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// Returns true if the cursor is currently locked.
        /// </summary>
        public bool CursorLocked { get { return lockTokens.Count > 0; } }

        /// <summary>
        /// Returns true if the left mouse button has been pressed this frame.
        /// </summary>
        public bool LeftButtonPressed { get { return leftButtonPressed && !CursorLocked; } }

        /// <summary>
        /// Returns true if the left mouse button is currently held down.
        /// </summary>
        public bool LeftButtonHeld { get { return leftButtonHeld && !CursorLocked; } }

        /// <summary>
        /// Returns true if the right mouse button has been pressed this frame.
        /// </summary>
        public bool RightButtonPressed { get { return rightButtonPressed && !CursorLocked; } }

        /// <summary>
        /// Returns true if the right mouse button is currently held down.
        /// </summary>
        public bool RightButtonHeld { get { return rightButtonHeld && !CursorLocked; } }

        /// <summary>
        /// Method for loading the content of the cursor, it is the pictures that are placed in an array
        /// so we can change the pictures of the mouse when certain things happen
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            defaultTex = content.Load<Texture2D>("Textures/Cursors/Normal");
            activeTex = defaultTex;

            variations.Add(CursorStyle.Interactable, content.Load<Texture2D>("Textures/Cursors/Interactable"));
            variations.Add(CursorStyle.Dialouge, content.Load<Texture2D>("Textures/Cursors/Dialouge"));
            variations.Add(CursorStyle.OutOfRange, content.Load<Texture2D>("Textures/Cursors/OutOfRange"));
        }

        public void Update()
        {
            MouseState state = Mouse.GetState();

            if (GameWorld.Instance.IsActive)
            {
                position = Vector2.Transform(state.Position.ToVector2(),
                                    GameWorld.Camera.WorldMatrix);
                if (position.X > 674 + GameWorld.Camera.Position.X)
                {
                    position.X = 674 + GameWorld.Camera.Position.X;
                }
                if (position.Y > 375 + GameWorld.Camera.Position.Y)
                {
                    position.Y = 375 + GameWorld.Camera.Position.Y;
                }
            }

            if (state.LeftButton == ButtonState.Pressed && GameWorld.Instance.IsActive)
            {
                if (!leftButtonHeld)
                {
                    leftButtonHeld = true;
                    leftButtonPressed = true;
                }
                else
                {
                    leftButtonPressed = false;
                }
            }
            else
            {
                if (leftButtonHeld)
                {
                    leftButtonHeld = false;
                    leftButtonPressed = false;
                }
            }

            if (state.RightButton == ButtonState.Pressed && GameWorld.Instance.IsActive)
            {
                if (!rightButtonHeld)
                {
                    rightButtonHeld = true;
                    rightButtonPressed = true;
                }
                else
                {
                    rightButtonPressed = false;
                }
            }
            else
            {
                if (rightButtonHeld)
                {
                    rightButtonHeld = false;
                    rightButtonPressed = false;
                }
            }
        }

        /// <summary>
        /// Locks the cursor (Causing its button properties to always return false) until Unlock() is called on the returned lock token.
        /// </summary>
        /// <returns></returns>
        public CursorLockToken Lock()
        {
            CursorLockToken token = new CursorLockToken();
            lockTokens.Add(token);
            token.OnUnlock += (sender, e) => { lockTokens.Remove(token); };
            return token;
        }

        public void SetCursor(CursorStyle style)
        {
            if (activeTexIndex < (int)style)
            {
                activeTex = variations[style];
                activeTexIndex = (int)style;
            }
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
            activeTexIndex = -1;
        }
    }
}