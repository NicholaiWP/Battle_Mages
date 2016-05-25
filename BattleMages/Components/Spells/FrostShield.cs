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
        private float rangeFromPlayer;
        private Vector2 origin;

        public FrostShield(GameObject go, SpellCreationParams p, bool spawnSubshards) : base(go, p)
        {
            rangeFromPlayer = 50;
            origin = new Vector2(GameObject.Transform.Position.X - rangeFromPlayer, GameObject.Transform.Position.Y);
            GameObject.Transform.Translate(new Vector2(rangeFromPlayer, 0));
            Damage = 15;
            ManaCost = 65;
            ApplyAttributeRunes();

            /*if (spawnSubshards)
            {
                for (int i = 0; i <= 2; i++)
                {
                    GameObject newShardGameObject = null;

                    if(i == 0)
                    {
                        newShardGameObject = new GameObject(new Vector2());
                    }

                    newShardGameObject.AddComponent(new IceShard(newShardGameObject, new SpellCreationParams(p.AttributeRunes, target + GameObject.Transform.Position, p.VelocityOffset), false));
                    GameWorld.CurrentScene.AddObject(newShardGameObject);
                }
            }*/

            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/ice");
            collider = new Collider(GameObject, new Vector2(sprite.Width, sprite.Height));
            GameObject.AddComponent(collider);

            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Update(UpdateMsg message)
        {
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                var projectile = other.GameObject.GetComponent<Projectile>();

                if (enemy != null)
                {
                    enemy.TakeDamage(Damage);
                    GameWorld.CurrentScene.AddObject(ObjectBuilder.BuildFlyingLabelText(GameObject.Transform.Position, Damage.ToString()));
                    GameWorld.CurrentScene.RemoveObject(GameObject);
                }
                else if (projectile != null)
                {
                    GameWorld.CurrentScene.RemoveObject(other.GameObject);
                    GameWorld.CurrentScene.RemoveObject(GameObject);
                }
            }

            foreach (var go in GameWorld.CurrentScene.ActiveObjects)
            {
                if (go.GetComponent<Player>() != null)
                {
                    Vector2 vec = Vector2.Subtract(go.Transform.Position, GameObject.Transform.Position);
                    float lengthOfVec = vec.Length();
                    if (lengthOfVec > rangeFromPlayer)
                    {
                        vec.Normalize();
                        GameObject.Transform.Position += vec * 95 * GameWorld.DeltaTime;
                    }
                }
            }
        }

        private void Draw(DrawMsg msg)
        {
            msg.Drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
        }
    }
}