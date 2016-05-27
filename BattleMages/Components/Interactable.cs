﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    internal class Interactable : Component
    {
        private Collider collider;
        private Action action;

        public Interactable(Action action)
        {
            this.action = action;
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
        }

        private void Initialize(InitializeMsg msg)
        {
            collider = GameObject.GetComponent<Collider>();
        }

        private void Update(UpdateMsg msg)
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