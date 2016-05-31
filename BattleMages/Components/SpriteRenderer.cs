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
        private Vector2 posRect;
        private Animator animator;
        private Texture2D sprite;
        private bool usingSpritesheet;

        //Properties
        public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }

        public Texture2D Sprite { get { return sprite; } }

        public Vector2 Offset { set { offset = value; } }

        public float Opacity { get; set; } = 1;

        public Vector2 PosRect { set { posRect = value; } }

        /// <summary>
        /// A constructor for the sprite renderer
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="spriteName"></param>
        public SpriteRenderer(string spriteName, bool usingSpritesheet = false)
        {
            this.spriteName = spriteName;
            this.usingSpritesheet = usingSpritesheet;
            Listen<InitializeMsg>(Initialize);
            Listen<DrawMsg>(Draw);
            sprite = GameWorld.Load<Texture2D>(spriteName);
            if (!usingSpritesheet)
                rectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
        }

        private void Initialize(InitializeMsg msg)
        {
            animator = GameObject.GetComponent<Animator>();
            if (posRect == Vector2.Zero)
                posRect = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
        }

        private void Draw(DrawMsg msg)
        {
            Color color = new Color(Opacity, Opacity, Opacity, Opacity);
            msg.Drawer[DrawLayer.Gameplay].Draw(sprite,
                position: GameObject.Transform.Position -
                posRect + offset,
                sourceRectangle: rectangle,
                origin: Vector2.Zero,
                rotation: 0f,
                color: color,
                effects: SpriteEffects.None);
        }
    }
}