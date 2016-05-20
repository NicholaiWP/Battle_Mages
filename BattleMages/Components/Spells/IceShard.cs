using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class IceShard : Spell, ICanBeDrawn, ICanUpdate
    {
        private Texture2D sprite;
        private Vector2 velocity;
        private Vector2 diff;
        private Collider collider;

        public IceShard(GameObject go, SpellCreationParams p) : base(go, p)
        {
            Damage = 5;
            CooldownTime = 0.6f;
            ApplyRunes();

            diff = p.AimTarget - GameObject.Transform.Position;

            diff.Normalize();
            velocity = diff * 100f;

            for (int i = 0; i < 2; i++)
            {
                GameObject newShard = new GameObject(GameObject.Transform.Position);
                if (i == 0)
                {
                    newShard.AddComponent(new Shards(newShard, Utils.RotationPos(diff, 20), Damage));
                }
                else
                {
                    newShard.AddComponent(new Shards(newShard, Utils.RotationPos(diff, -20), Damage));
                }
                GameWorld.CurrentScene.AddObject(newShard);
            }

            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/ice");
            collider = new Collider(GameObject, new Vector2(8, 8));
        }

        public void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
        }

        public void Update()
        {
            GameObject.Transform.Position += velocity * GameWorld.DeltaTime;
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DealDamage(Damage);
                    GameWorld.CurrentScene.AddObject(ObjectBuilder.BuildFlyingLabelText(GameObject.Transform.Position, Damage.ToString()));
                    GameWorld.CurrentScene.RemoveObject(GameObject);
                }
            }
            if (!Utils.InsideCircle(GameObject.Transform.Position, 320))
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
            }
        }
    }

    public class Shards : Component, ICanBeDrawn, ICanUpdate
    {
        private Texture2D sprite;
        private Vector2 velocity;
        private Collider collider;
        private int dmg;

        public Shards(GameObject gameObject, Vector2 targetVec, int dmg) : base(gameObject)
        {
            this.dmg = dmg;
            velocity = targetVec * 100f;

            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/ice");
            collider = new Collider(GameObject, new Vector2(8, 8));
        }

        public void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
        }

        public void Update()
        {
            GameObject.Transform.Position += velocity * GameWorld.DeltaTime;
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DealDamage(dmg);
                    GameWorld.CurrentScene.AddObject(ObjectBuilder.BuildFlyingLabelText(GameObject.Transform.Position, dmg.ToString()));
                    GameWorld.CurrentScene.RemoveObject(GameObject);
                }
            }
            if (!Utils.InsideCircle(GameObject.Transform.Position, 320))
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
            }
        }
    }
}