﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages.Spells
{
    class Icicle: Spell, ICanBeDrawn, ICanBeAnimated, ICanUpdate
    {
        private Texture2D sprite;
        private Vector2 velocity;
        private Vector2 diff;

        public Icicle(GameObject go, Vector2 targetPos, RuneInfo[] runes) : base(go, runes)
        {
            Damage = 16;
            CooldownTime = 0.6f;
            ApplyRunes();

            diff = targetPos - GameObject.Transform.Position;
            diff.Normalize();
            velocity = diff * 100f;
            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/ice");
        }

        public void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
        }

        public void OnAnimationDone(string animationsName)
        {
            
        }

        public void Update()
        {
            GameObject.Transform.Position += velocity * GameWorld.DeltaTime;
        }
    }
}
