﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class EarthSpikes : Spell
    {
        private Collider collider;
        private Texture2D sprite;
        private float timer;
        private float damageTimer;

        public EarthSpikes(GameObject go, SpellCreationParams p) : base(go, p)
        {
            GameObject.Transform.Position = p.AimTarget;

            Damage = 10;
            damageTimer = 0;
            CooldownTime = 3;
            ManaCost = 70;
            ApplyAttributeRunes();

            GameWorld.SoundManager.PlaySound("Earthspikes");
            GameWorld.SoundManager.SoundVolume = 1f;

            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/earthspikes");
            collider = new Collider(GameObject, new Vector2(sprite.Width, sprite.Height));
            GameObject.AddComponent(collider);
            timer = 4;

            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Update(UpdateMsg msg)
        {
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                if (enemy != null && damageTimer <= 0)
                {
                    damageTimer = 0.6f;
                    enemy.TakeDamage(Damage);
                }
            }
            timer -= GameWorld.DeltaTime;
            if (timer <= 0)
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
            }
            if (damageTimer > 0)
            {
                damageTimer -= GameWorld.DeltaTime;
            }
        }

        private void Draw(DrawMsg msg)
        {
            msg.Drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
        }
    }
}