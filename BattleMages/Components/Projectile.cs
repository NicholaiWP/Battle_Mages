using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Projectile : Component
    {
        private int damage;
        private Vector2 velocity;
        private Texture2D sprite;
        private Collider collider;
        private Vector2 targetPos;

        public Projectile(Enemy enemy, Vector2 targetPos)
        {
            this.targetPos = targetPos;
            damage = enemy.Damage;

            sprite = GameWorld.Instance.Content.Load<Texture2D>("Images/orbProjectile");
            //GameWorld.SoundManager.PlaySound("Lightning");

            collider = new Collider(new Vector2(8, 8));

            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Initialize(InitializeMsg message)
        {
            var diff = targetPos - GameObject.Transform.Position;
            diff.Normalize();
            velocity = diff * 120f;
            GameObject.AddComponent(collider);
        }

        private void Update(UpdateMsg msg)
        {
            GameObject.Transform.Position += velocity * GameWorld.DeltaTime;
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var player = other.GameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                    GameWorld.Scene.RemoveObject(GameObject);
                }
            }
            if (!Utils.InsideCircle(GameObject.Transform.Position, Vector2.Zero, 320))
            {
                GameWorld.Scene.RemoveObject(GameObject);
            }
        }

        private void Draw(DrawMsg msg)
        {
            msg.Drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
        }
    }
}