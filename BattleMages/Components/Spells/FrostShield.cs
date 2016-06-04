using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public class FrostShield : Spell
    {
        private Collider collider;
        private SpriteRenderer spriteRenderer;
        private float radius;
        private float angle;
        private float speed;
        private bool spawnSubshards;
        private SpellCreationParams p;
        private float existenceTimer;

        public FrostShield(SpellCreationParams p, bool spawnSubshards, float angleDegrees) : base(p)
        {
            this.spawnSubshards = spawnSubshards;
            this.p = p;
            speed = 2;
            radius = 32;
            existenceTimer = 12;
            GameWorld.SoundManager.PlaySound("FrostShieldSound");
            spriteRenderer = new SpriteRenderer("Textures/Spells/IceShard");
            collider = new Collider(new Vector2(spriteRenderer.Rectangle.Width, spriteRenderer.Rectangle.Height));
            //This makes sure all FrostShields have the same starting angle so that their rotation looks amazing
            FrostShield firstOtherFrostshield = GameWorld.Scene.ActiveObjects
                .Select(a => a.GetComponent<FrostShield>())
                .Where(a => a != null && a != this)
                .FirstOrDefault();
            if (firstOtherFrostshield != null)
            {
                angle = firstOtherFrostshield.angle;
            }

            Listen<PreInitializeMsg>(PreInitialize);
            Listen<UpdateMsg>(Update);
        }

        private void PreInitialize(PreInitializeMsg msg)
        {
            GameObject.AddComponent(spriteRenderer);
            GameObject.AddComponent(collider);
        }

        private void Update(UpdateMsg msg)
        {
            //Spawn 2 other frostshields if told to
            if (spawnSubshards)
            {
                spawnSubshards = false;
                for (int i = 0; i <= 1; i++)
                {
                    GameObject newShardGameObject = new GameObject(GameObject.Transform.Position);
                    newShardGameObject.AddComponent(new FrostShield(
                           new SpellCreationParams(p.SpellInfo, GameObject.Transform.Position, p.VelocityOffset),
                           false, 90 * (i + 1)));
                    GameWorld.Scene.AddObject(newShardGameObject);
                }
            }
            GameWorld.SoundManager.PlaySound("FrostShield");
            GameWorld.SoundManager.SoundVolume = 1f;

            #region Collision detection

            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                var projectile = other.GameObject.GetComponent<Projectile>();

                if (enemy != null)
                {
                    enemy.TakeDamage(Stats.Damage);
                    GameWorld.SoundManager.PlaySound("iceshardsbreaking");
                    GameWorld.SoundManager.SoundVolume = 0.9f;

                    GameWorld.Scene.RemoveObject(GameObject);
                    GameWorld.SoundManager.StopSound("FrostShield");
                }
                else if (projectile != null)
                {
                    GameWorld.Scene.RemoveObject(other.GameObject);
                    GameWorld.Scene.RemoveObject(GameObject);
                    GameWorld.SoundManager.StopSound("FrostShield");
                }
            }

            #endregion Collision detection

            //Find out where in the circle of FrostShields this one should be
            List<FrostShield> totalFrostShields = GameWorld.Scene.ActiveObjects.Select(a => a.GetComponent<FrostShield>()).Where(a => a != null).ToList();
            int myIndex = totalFrostShields.IndexOf(this);
            int shieldCount = totalFrostShields.Count;

            float rotationOffset = myIndex / (float)shieldCount * ((float)Math.PI * 2);

            foreach (GameObject go in GameWorld.Scene.ActiveObjects)
            {
                if (go.GetComponent<Player>() != null)
                {
                    Vector2 center = go.Transform.Position;

                    angle += speed * GameWorld.DeltaTime;
                    float finalAngle = angle + rotationOffset;
                    float x = (float)Math.Cos(finalAngle) * radius + center.X;
                    float y = (float)Math.Sin(finalAngle) * radius + center.Y;
                    GameObject.Transform.Position = Vector2.Lerp(GameObject.Transform.Position, new Vector2(x, y), GameWorld.DeltaTime * 8);
                }
            }

            if (existenceTimer <= 0)
            {
                GameWorld.Scene.RemoveObject(GameObject);
            }
            else
            {
                existenceTimer -= GameWorld.DeltaTime;
            }
        }
    }
}