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
        private Texture2D sprite;
        private Vector2 velocity;
        private Vector2 diff;
        private Collider collider;
        private SpriteRenderer spriteRenderer;
        private bool spawnSubshards;
        private SpellCreationParams p;

        public IceShard(SpellCreationParams p, bool spawnSubshards) : base(p)
        {
            this.spawnSubshards = spawnSubshards;
            this.p = p;
            Damage = 5;
            CooldownTime = 0.6f;
            ManaCost = 20;
            ApplyAttributeRunes();

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
            velocity = diff * 100f;
        }

        private void Update(UpdateMsg msg)
        {
            if (spawnSubshards)
            {
                spawnSubshards = false;
                for (int i = 0; i <= 1; i++)
                {
                    GameObject newShardGameObject = new GameObject(GameObject.Transform.Position);

                    //Set aim target to be rotated based on which shard this is
                    Vector2 target = Utils.RotateVector(diff, i == 0 ? 20 : -20);

                    newShardGameObject.AddComponent(new IceShard(
                        new SpellCreationParams(p.AttributeRunes, target + GameObject.Transform.Position, p.VelocityOffset),
                        false));
                    GameWorld.Scene.AddObject(newShardGameObject);
                }
            }
            GameObject.Transform.Position += velocity * GameWorld.DeltaTime;
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(Damage);
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