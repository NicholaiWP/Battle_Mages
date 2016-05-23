using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
    internal class Interactable : Component, ICanBeLoaded, ICanUpdate
    {
        private Collider collider;
        private Action action;

        public Interactable(GameObject gameObject, Action action) : base(gameObject)
        {
            this.action = action;
        }

        public void LoadContent(ContentManager content)
        {
            collider = GameObject.GetComponent<Collider>();
        }

        public void Update()
        {
            if (collider.CalcColliderRect().Contains(GameWorld.Cursor.Position))
            {
                GameWorld.Cursor.SetCursor(CursorStyle.Interactable);
                MouseState mouseState = Mouse.GetState();
                if (GameWorld.Cursor.LeftButtonPressed)
                {
                    action();
                }
            }
        }
    }
}