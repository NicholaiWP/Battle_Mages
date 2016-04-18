﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class SpriteRenderer : Component, ICanBeLoaded, ICanBeDrawn
    {
        //Fields
        private string spriteName;

        private Color color = Color.White;
        private float drawDepth;
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
        /// <param name="drawDepth"></param>
        public SpriteRenderer(GameObject gameObject, string spriteName, float drawDepth) : base(gameObject)
        {
            if (spriteName == "CollisionTexture")
            {
                color = Color.Transparent;
            }
            this.spriteName = spriteName;
            this.drawDepth = drawDepth;
        }

        /// <summary>
        /// Method for loading the starting content of the sprite renderer this is only called once
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GameObject.GetComponent("Animator");
            sprite = content.Load<Texture2D>(spriteName);
            rectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
        }

        /// <summary>
        /// The method for drawing
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Gameplay].Draw(sprite, position: GameObject.Transform.Position + offset,
                sourceRectangle: rectangle,
                origin: Vector2.Zero,
                rotation: 0f,
                color: color,
                effects: SpriteEffects.None);
        }
    }
}