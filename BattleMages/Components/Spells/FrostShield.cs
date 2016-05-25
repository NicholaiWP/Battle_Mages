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
        private float period;

        public FrostShield(GameObject go, SpellCreationParams p, bool spawnSubshards) : base(go, p)
        {
            period = 2;
            radius = 50;
            Damage = 15;
            CooldownTime = 2;
            ManaCost = 40;
            ApplyAttributeRunes();

            /*if (spawnSubshards)
            {
                for (int i = 0; i <= 2; i++)
                {
                    Vector2 pos = Vector2.Zero;

                    foreach (GameObject player in GameWorld.CurrentScene.ActiveObjects)
                    {
                        if (player.GetComponent<Player>() != null)
                        {
                            pos = player.Transform.Position;
                        }
                    }

                    GameObject newShardGameObject = new GameObject(pos);

                    newShardGameObject.AddComponent(new FrostShield(newShardGameObject,
                        new SpellCreationParams(p.AttributeRunes, GameObject.Transform.Position,
                        p.VelocityOffset), false));

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
        #region collision detect

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

        #endregion collision detect

        foreach (GameObject go in GameWorld.CurrentScene.ActiveObjects)
        {
            if (go.GetComponent<Player>() != null)
            {
                Vector2 center = go.Transform.Position;

                angle += period * GameWorld.DeltaTime;
                float x = (float)Math.Cos(angle) * radius + center.X;
                float y = (float)Math.Sin(angle) * radius + center.Y;
                GameObject.Transform.Position = new Vector2(x, y);
            }
        }
    }

    private void Draw(DrawMsg msg)
    {
        msg.Drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
    }
}
}