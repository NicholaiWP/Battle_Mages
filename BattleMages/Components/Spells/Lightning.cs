using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Lightning : Spell, ICanBeDrawn, ICanUpdate

    {
        private Texture2D sprite;
        private Collider collider;
        private float waitTimer;
        private float existenceTimer;
        private bool hadACollider;

        public Lightning(GameObject go, SpellCreationParams p) : base(go, p)
        {
            GameObject.Transform.Position = p.AimTarget;
            Damage = 25;
            CooldownTime = 0.9f;
            ApplyRunes();
            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/Lightning_bigger");
            waitTimer = 0.9f;
            existenceTimer = 0.05f;
            hadACollider = false;
        }

        public void Draw(Drawer drawer)
        {
            if (waitTimer <= 0)
            {
                drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
            }
        }

        public void Update()
        {
            if (waitTimer <= 0 && !hadACollider)
            {
                collider = new Collider(GameObject, new Vector2(10, 10));
                hadACollider = true;

                foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
                {
                    var enemy = other.GameObject.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.DealDamage(Damage);
                        GameWorld.CurrentScene.AddObject(ObjectBuilder.BuildFlyingLabelText(GameObject.Transform.Position,
                            Damage.ToString()));
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
                GameWorld.CurrentScene.RemoveObject(GameObject);
            }

            if (!Utils.InsideCircle(GameObject.Transform.Position, 320))
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
            }
        }
    }
}