using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Projectile : Component, ICanBeDrawn, ICanUpdate
    {
        private int damage;
        private Vector2 velocity;
        private Texture2D sprite;
        private Collider collider;

        public Projectile(GameObject go, Enemy enemy, Vector2 targetPos) : base(go)
        {
            damage = enemy.Damage;
            var diff = targetPos - GameObject.Transform.Position;
            diff.Normalize();
            velocity = diff * 120f;
            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/fireball");
            //GameWorld.SoundManager.PlaySound("Lightning");

            collider = new Collider(GameObject, new Vector2(8, 8));
            GameObject.AddComponent(collider);
        }

        public void Update()
        {
            GameObject.Transform.Position += velocity * GameWorld.DeltaTime;
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var player = other.GameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.DealDamage(damage);
                    GameWorld.CurrentScene.RemoveObject(GameObject);
                }
            }
            if (!Utils.InsideCircle(GameObject.Transform.Position, Vector2.Zero, 320))
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
            }
        }

        public void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
        }
    }
}