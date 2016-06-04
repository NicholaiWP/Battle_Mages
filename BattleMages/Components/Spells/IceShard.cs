using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public class IceShard : Spell
    {
        private const float baseSpeed = 250;
        private const float speedRange = 30;
        private Vector2 velocity;
        private Vector2 diff;
        private Collider collider;
        private SpriteRenderer spriteRenderer;
        private bool spawnSubshards;
        private SpellCreationParams p;
        private float speed;

        private float speedMult = 1;

        public IceShard(SpellCreationParams p, bool spawnSubshards, float speed = baseSpeed) : base(p)
        {
            this.spawnSubshards = spawnSubshards;
            this.speed = speed;
            this.p = p;

            speedMult = MathHelper.Lerp(0.6f, 1.0f, (float)GameWorld.Random.NextDouble());

            GameWorld.SoundManager.PlaySound("IceShardSound");
            spriteRenderer = new SpriteRenderer("Textures/Spells/IceShard");
            collider = new Collider(new Vector2(8, 8));

            Listen<InitializeMsg>(Initialize);
            Listen<PreInitializeMsg>(PreInitialize);
            Listen<UpdateMsg>(Update);
        }

        private void PreInitialize(PreInitializeMsg msg)
        {
            GameObject.AddComponent(spriteRenderer);
            GameObject.AddComponent(collider);
        }

        private void Initialize(InitializeMsg msg)
        {
            diff = p.AimTarget - GameObject.Transform.Position;
            diff.Normalize();
            velocity = diff * speed;
            if (spawnSubshards)
            {
                spawnSubshards = false;
                for (int i = 0; i <= 3; i++)
                {
                    GameObject newShardGameObject = new GameObject(GameObject.Transform.Position);

                    //Set aim target to be rotated based on which shard this is

                    Vector2 target = Utils.RotateVector(diff, ((float)GameWorld.Random.NextDouble() - 0.5f) * 60);
                    //Vector2 target = Utils.RotateVector(diff, i == 0 ? 20 : -20);

                    newShardGameObject.AddComponent(new IceShard(
                        new SpellCreationParams(p.SpellInfo, target + GameObject.Transform.Position, p.VelocityOffset),
                        false, baseSpeed + ((float)GameWorld.Random.NextDouble() - 0.5f) * speedRange));
                    GameWorld.Scene.AddObject(newShardGameObject);
                }
            }
        }

        private void Update(UpdateMsg msg)
        {
            speedMult -= GameWorld.DeltaTime / (Stats.Range);
            if (speedMult <= 0)
            {
                GameWorld.Scene.RemoveObject(GameObject);
            }

            GameObject.Transform.Position += velocity * speedMult * GameWorld.DeltaTime;
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(Stats.Damage);
                    GameWorld.SoundManager.PlaySound("iceshardsbreaking");
                    GameWorld.SoundManager.SoundVolume = 0.9f;

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