using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Fireball : Spell
    {
        private Texture2D sprite;
        private Vector2 velocity;
        private Collider collider;
        private Vector2 diff;
        private SpellCreationParams p;

        public Fireball(SpellCreationParams p) : base(p)
        {
            this.p = p;
            Damage = 15;
            CooldownTime = 0.8f;
            ManaCost = 15;
            ApplyAttributeRunes();

            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/fireball");

            collider = new Collider(new Vector2(8, 8));
            GameWorld.SoundManager.PlaySound("fireball");
            GameWorld.SoundManager.SoundVolume = 0.9f;

            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Initialize(InitializeMsg message)
        {
            diff = p.AimTarget - GameObject.Transform.Position;
            diff.Normalize();
            velocity = diff * 150;
            GameObject.AddComponent(collider);
        }

        private void Draw(DrawMsg msg)
        {
            msg.Drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
        }

        private void Update(UpdateMsg msg)
        {
            GameObject.Transform.Position += velocity * GameWorld.DeltaTime;
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(Damage);
                    GameWorld.Scene.AddObject(ObjectBuilder.BuildFlyingLabelText(GameObject.Transform.Position, Damage.ToString()));
                    GameWorld.Scene.RemoveObject(GameObject);
                }
            }

            if (!Utils.InsideCircle(GameObject.Transform.Position, Vector2.Zero, 320))
            {
                GameWorld.Scene.RemoveObject(GameObject);
            }
        }
    }
}