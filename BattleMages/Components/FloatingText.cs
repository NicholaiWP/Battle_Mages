using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    internal class FloatingText : Component, ICanBeDrawn, ICanUpdate
    {
        private SpriteFont font;
        private string text;

        private Vector2 velocity;

        private const float xSpeedMax = 100;
        private const float ySpeed = -100f;

        private float alpha = 1;

        public FloatingText(GameObject gameObject, string text) : base(gameObject)
        {
            Random r = new Random();
            font = GameWorld.Instance.Content.Load<SpriteFont>("FontBM");
            this.text = text;
            velocity = new Vector2(((float)r.NextDouble() - 0.5f) * xSpeedMax, ySpeed);
        }

        public void Update()
        {
            velocity += new Vector2(0, 200) * GameWorld.DeltaTime;

            GameObject.Transform.Position += velocity * GameWorld.DeltaTime;

            alpha -= GameWorld.DeltaTime;
            if (alpha <= 0f)
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
            }
        }

        public void Draw(Drawer drawer)
        {
            Color c = Color.Purple;
            c.A = (byte)(alpha * 255);
            drawer[DrawLayer.Foreground].DrawString(font, text, GameObject.Transform.Position, c);
        }
    }
}