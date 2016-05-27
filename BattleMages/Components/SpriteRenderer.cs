using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class SpriteRenderer : Component
    {
        //Fields
        private string spriteName;

        private Rectangle rectangle;
        private Vector2 offset;
        private Animator animator;
        private Texture2D sprite;

        //Properties
        public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }

        public Texture2D Sprite { get { return sprite; } }

        public Vector2 Offset { get { return offset; } set { offset = value; } }

        public float Opacity { get; set; } = 1;

        /// <summary>
        /// A constructor for the sprite renderer
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="spriteName"></param>
        public SpriteRenderer(string spriteName)
        {
            this.spriteName = spriteName;

            Listen<InitializeMsg>(Initialize);
            Listen<DrawMsg>(Draw);
        }

        private void Initialize(InitializeMsg msg)
        {
            animator = GameObject.GetComponent<Animator>();
            sprite = GameWorld.Load<Texture2D>(spriteName);
            if (spriteName != "Player Images/playerSpriteSheet")
                rectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
            else
                rectangle = new Rectangle(0, 0, 32, 32);
        }

        private void Draw(DrawMsg msg)
        {
            Color color = new Color(Opacity, Opacity, Opacity, Opacity);
            if (spriteName != "Player Images/playerSpriteSheet")
            {
                msg.Drawer[DrawLayer.Gameplay].Draw(sprite,
                    position: GameObject.Transform.Position - new Vector2(sprite.Width / 2, sprite.Height / 2) + offset,
                    sourceRectangle: rectangle,
                    origin: Vector2.Zero,
                    rotation: 0f,
                    color: color,
                    effects: SpriteEffects.None);
            }
            else
            {
                msg.Drawer[DrawLayer.Gameplay].Draw(sprite,
                   position: GameObject.Transform.Position - new Vector2(32 / 2, 32 / 2) + offset,
                   sourceRectangle: rectangle,
                   origin: Vector2.Zero,
                   rotation: 0f,
                   color: color,
                   effects: SpriteEffects.None);
            }
        }
    }
}