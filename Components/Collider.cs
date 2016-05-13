using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Collider : Component, ICanBeLoaded
    {
        //Static list of all colliders created
        private Transform transform;

        public Vector2 Size { get; }

        public Collider(GameObject gameObject, Vector2 size) : base(gameObject)
        {
            Size = size;
        }

        public Rectangle CalcColliderRect()
        {
            return new Rectangle((int)(transform.Position.X - Size.X / 2), (int)(transform.Position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y);
        }

        public bool CheckCollisionAtPosition(Vector2 position)
        {
            IEnumerable<Collider> collidersInScene = GameWorld.CurrentScene.ActiveObjects.Select(a => a.GetComponent<Collider>()).Where(a => a != null);
            var rect = new Rectangle((int)(position.X - Size.X / 2), (int)(position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y);
            foreach (var coll in collidersInScene)
            {
                if (coll != this)
                {
                    if (coll.CalcColliderRect().Intersects(rect)) return true;
                }
            }
            return false;
        }

        public bool CheckCollision(Collider other)
        {
            //Rectangle-rectangle collision
            return CalcColliderRect().Intersects(other.CalcColliderRect());
        }

        public void LoadContent(ContentManager content)
        {
            transform = GameObject.Transform;
        }
    }
}