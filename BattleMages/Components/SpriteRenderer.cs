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

        private Color color = Color.White;
        private Rectangle rectangle;
        private Vector2 offset;
        private Animator animator;
        private Texture2D sprite;

        //Properties
        public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }

        public Texture2D Sprite { get { return sprite; } }

        public Vector2 Offset { get { return offset; } set { offset = value; } }

        /// <summary>
        /// A constructor for the sprite renderer
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="spriteName"></param>
        public SpriteRenderer(string spriteName)
        {
            if (spriteName == "CollisionTexture")
            {
                color = Color.Transparent;
            }
            this.spriteName = spriteName;

            Listen<InitializeMsg>(Initialize);
            Listen<DrawMsg>(Draw);
        }

        private void Initialize(InitializeMsg msg)
        {
            animator = GameObject.GetComponent<Animator>();
            sprite = GameWorld.Load<Texture2D>(spriteName);
            rectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
        }

        private void Draw(DrawMsg msg)
        {
            msg.Drawer[DrawLayer.Gameplay].Draw(sprite, position: GameObject.Transform.Position - new Vector2(sprite.Width / 2, sprite.Height / 2) + offset,
                sourceRectangle: rectangle,
                origin: Vector2.Zero,
                rotation: 0f,
                color: color,
                effects: SpriteEffects.None);
        }
    }
}