﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Lightning : Spell

    {
        private Texture2D sprite;
        private Collider collider;
        private float waitTimer;
        private float existenceTimer;
        private bool hadACollider;

        public float WaitTimer { get { return waitTimer; } set {waitTimer = value; }
        }

        public Lightning(GameObject go, SpellCreationParams p) : base(go, p)
        {
            GameObject.Transform.Position = p.AimTarget;
            Damage = 40;
            CooldownTime = 2f;
            ManaCost = 50;
            ApplyRunes();

            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/Lightning_bigger");
            WaitTimer = 0.3f;
            existenceTimer = 0.05f;
            hadACollider = false;
            GameWorld.SoundManager.PlaySound("lightningStrike");
            GameWorld.SoundManager.SoundVolume = 0.7f;
            Listen<UpdateMsg>(Update);
            Listen <DrawMsg>(Draw);
        }

        private void Draw(DrawMsg msg)
        {
            if (WaitTimer <= 0)
            {
                msg.Drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position - new Vector2(0, sprite.Height), Color.White);
            }
        }

        private void Update(UpdateMsg msg)
        {
            if (WaitTimer <= 0 && !hadACollider)
            {
                collider = new Collider(GameObject, new Vector2(10, 10));
                hadACollider = true;

                foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
                {
                    var enemy = other.GameObject.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(Damage);
                        GameWorld.CurrentScene.AddObject(ObjectBuilder.BuildFlyingLabelText(GameObject.Transform.Position,
                            Damage.ToString()));
                    }
                }
            }
            else
            {
                WaitTimer -= GameWorld.DeltaTime;
            }

            if (collider != null)
            {
                existenceTimer -= GameWorld.DeltaTime;
            }

            if (existenceTimer <= 0)
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
            }

            if (!Utils.InsideCircle(GameObject.Transform.Position, Vector2.Zero, 320))
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
            }
        }
    }
}