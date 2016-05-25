using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class IceShard : Spell
    {
        private Texture2D sprite;
        private Vector2 velocity;
        private Vector2 diff;
        private Collider collider;

        public IceShard(GameObject go, SpellCreationParams p, bool spawnSubshards) : base(go, p)
        {
            Damage = 5;
            CooldownTime = 0.6f;
            ManaCost = 20;
            ApplyAttributeRunes();

            diff = p.AimTarget - GameObject.Transform.Position;
            diff.Normalize();
            velocity = diff * 100f;

            if (spawnSubshards)
            {
                for (int i = 0; i <= 1; i++)
                {
                    GameObject newShardGameObject = new GameObject(GameObject.Transform.Position);

                    //Set aim target to be rotated based on which shard this is
                    Vector2 target = Utils.RotateVector(diff, i == 0 ? 20 : -20);

                    newShardGameObject.AddComponent(new IceShard(newShardGameObject, new SpellCreationParams(p.AttributeRunes, target + GameObject.Transform.Position, p.VelocityOffset), false));
                    GameWorld.CurrentScene.AddObject(newShardGameObject);
                }
            }

            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/ice");
            collider = new Collider(GameObject, new Vector2(8, 8));
            GameObject.AddComponent(collider);


            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
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
                    GameWorld.SoundManager.PlaySound("iceshardsbreaking");
                    GameWorld.SoundManager.SoundVolume = 0.9f;
 
                    GameWorld.CurrentScene.AddObject(ObjectBuilder.BuildFlyingLabelText(GameObject.Transform.Position, Damage.ToString()));
                    GameWorld.CurrentScene.RemoveObject(GameObject);
                }
            }
            if (!Utils.InsideCircle(GameObject.Transform.Position, Vector2.Zero, 320))
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
            }
        }
    }
}