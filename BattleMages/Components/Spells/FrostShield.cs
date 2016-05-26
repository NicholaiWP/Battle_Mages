using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class FrostShield : Spell
    {
        private Collider collider;
        private Texture2D sprite;
        private float radius;
        private float angle;
        private float speed;

        public FrostShield(GameObject go, SpellCreationParams p, bool spawnSubshards, float angleDegrees) : base(go, p)
        {
            speed = 2;
            radius = 32;
            Damage = 8;
            CooldownTime = 2;
            ManaCost = 40;
            ApplyAttributeRunes();

            //This makes sure all FrostShields have the same starting angle so that their rotation looks amazing
            FrostShield firstOtherFrostshield = GameWorld.Scene.ActiveObjects
                .Select(a => a.GetComponent<FrostShield>())
                .Where(a => a != null && a != this)
                .FirstOrDefault();
            if (firstOtherFrostshield != null)
            {
                angle = firstOtherFrostshield.angle;
            }

            //Spawn 2 other frostshields if told to
            if (spawnSubshards)
            {
                for (int i = 0; i <= 1; i++)
                {
                    GameObject newShardGameObject = new GameObject(GameObject.Transform.Position);
                    newShardGameObject.AddComponent(new FrostShield(newShardGameObject,
                           new SpellCreationParams(p.AttributeRunes, GameObject.Transform.Position, p.VelocityOffset),
                           false, 90 * (i + 1)));
                    GameWorld.Scene.AddObject(newShardGameObject);
                }
            }

            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/ice");

            collider = new Collider(GameObject, new Vector2(sprite.Width, sprite.Height));

            GameObject.AddComponent(collider);

            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Update(UpdateMsg message)
        {
            GameWorld.SoundManager.PlaySound("FrostShield");
            GameWorld.SoundManager.SoundVolume = 1f;

            #region Collision detection

            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                var projectile = other.GameObject.GetComponent<Projectile>();

                if (enemy != null)
                {
                    enemy.TakeDamage(Damage);
                    GameWorld.SoundManager.PlaySound("iceshardsbreaking");
                    GameWorld.SoundManager.SoundVolume = 0.9f;

                    GameWorld.Scene.AddObject(ObjectBuilder.BuildFlyingLabelText(GameObject.Transform.Position, Damage.ToString()));
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
        }

        private void Draw(DrawMsg msg)
        {
            msg.Drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position - new Vector2(sprite.Width,sprite.Height) / 2, Color.White);
        }
    }
}