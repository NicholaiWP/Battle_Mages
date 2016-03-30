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
        private Texture2D[] sprite = new Texture2D[2];
        private Vector2 position;
        private Rectangle rectangle;
        private int integer = 0;

        public Vector2 GetPosition
        {
            get
            {
                position = Vector2.Transform(Mouse.GetState().Position.ToVector2(),
                    Matrix.Invert(GameWorld.GetInstance.GetCamera.GetViewMatrix));
                return position;
            }
        }

        public Rectangle GetRectangle
        {
            get
            {
                rectangle = new Rectangle((int)(GetPosition.X - (sprite[integer].Width * 0.5f)),
                    (int)(GetPosition.Y - (sprite[integer].Height * 0.5f)),
                    sprite[integer].Width, sprite[integer].Height);
                return rectangle;
            }
        }

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

        private Cursor()
        {
        }

        public void LoadContent(ContentManager content)
        {
            sprite[0] = content.Load<Texture2D>("Images/battle-mages cursor");
            sprite[1] = content.Load<Texture2D>("Images/battle-mages spell-cursor");
        }

        public void Draw(SpriteBatch spriteBatch, int integer)
        {
            this.integer = integer;
            spriteBatch.Draw(sprite[integer], GetRectangle, Color.White);
        }
    }
}