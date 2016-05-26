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
        private float frameTime;

        public FrostShield(GameObject go, SpellCreationParams p, bool spawnSubshards, float angleDegrees) : base(go, p)
        {
            frameTime = GameWorld.DeltaTime;
            speed = 2;
            radius = 50;
            Damage = 8;
            CooldownTime = 2;
            ManaCost = 40;
            ApplyAttributeRunes();
            angle = MathHelper.ToRadians(angleDegrees);

            if (spawnSubshards)
            {
                for (int i = 0; i <= 2; i++)
                {
                    GameObject newShardGameObject = new GameObject(Vector2.Zero);
                    newShardGameObject.AddComponent(new FrostShield(newShardGameObject,
                           new SpellCreationParams(p.AttributeRunes, GameObject.Transform.Position, p.VelocityOffset),
                           false, 90 * (i + 1)));
                    GameWorld.CurrentScene.AddObject(newShardGameObject);
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

            #region collision detect

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
                    GameWorld.SoundManager.StopSound("FrostShield");
                }
                else if (other.GameObject.GetComponent<Projectile>() != null)
                {
                    GameWorld.CurrentScene.RemoveObject(other.GameObject);
                    GameWorld.CurrentScene.RemoveObject(GameObject);
                    GameWorld.SoundManager.StopSound("FrostShield");
                }
            }

            #endregion collision detect

            foreach (GameObject go in GameWorld.CurrentScene.ActiveObjects)
            {
                if (go.GetComponent<Player>() != null)
                {
                    Vector2 center = go.Transform.Position;

                    angle += speed * GameWorld.DeltaTime;
                    float x = (float)Math.Cos(angle) * radius + center.X;
                    float y = (float)Math.Sin(angle) * radius + center.Y;
                    GameObject.Transform.Position = new Vector2(x, y);
                }
            }
        }

        private void Draw(DrawMsg msg)
        {
            if (frameTime > 0)
            {
                frameTime -= GameWorld.DeltaTime;
            }
            else
            {
                msg.Drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
            }
        }
    }
}