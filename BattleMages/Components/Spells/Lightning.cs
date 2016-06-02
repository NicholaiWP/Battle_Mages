using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
    public class Lightning : Spell
    {
        private Collider collider;
        private float waitTimer;
        private float existenceTimer;
        private bool hadACollider;
        private SpellCreationParams p;

        public Lightning(SpellCreationParams p) : base(p)
        {
            this.p = p;
            Damage = 40;
            CooldownTime = 2f;
            ManaCost = 40;
            ApplyAttributeRunes();
            GameWorld.SoundManager.PlaySound("lightningStrike");
            GameWorld.SoundManager.SoundVolume = 0.7f;
            waitTimer = 0.3f;
            existenceTimer = 0.05f;
            hadACollider = false;

            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
        }

        private void Initialize(InitializeMsg message)
        {
            GameObject.Transform.Position = p.AimTarget;
        }

        private void Update(UpdateMsg msg)
        {
            if (waitTimer <= 0 && !hadACollider)
            {
                GameObject.AddComponent(new SpriteRenderer("Textures/Spells/lightningStrike")
                { PosRect = new Vector2(0, 320) });
                collider = new Collider(new Vector2(10, 10));
                hadACollider = true;

                foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
                {
                    var enemy = other.GameObject.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(Damage);
                    }
                }
            }
            else
            {
                waitTimer -= GameWorld.DeltaTime;
            }

            if (collider != null)
            {
                existenceTimer -= GameWorld.DeltaTime;
            }

            if (existenceTimer <= 0)
            {
                GameWorld.Scene.RemoveObject(GameObject);
            }

            if (!Utils.InsideCircle(GameObject.Transform.Position, Vector2.Zero, 320))
            {
                GameWorld.Scene.RemoveObject(GameObject);
            }
        }
    }
}