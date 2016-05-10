using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Battle_Mages
{
    public enum ColliderShape
    {
        Rectangle,
        Ellipse
    }

    public class Collider : Component, ICanBeLoaded
    {
        //Static list of all colliders created
        private static List<Collider> colliders = new List<Collider>();
        private Transform transform;

        public Vector2 Size { get; }

        public Collider(GameObject gameObject, Vector2 size) : base(gameObject)
        {
            colliders.Add(this);
            Size = size;
        }

        public Rectangle CalcColliderRect()
        {
            return new Rectangle((int)(transform.Position.X - Size.X / 2), (int)(transform.Position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y);
        }

        public bool CheckCollisionAtPosition(Vector2 position)
        {
            var rect = new Rectangle((int)(position.X - Size.X / 2), (int)(position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y);
            foreach (var coll in colliders)
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

        public override void OnDestroy()
        {
            colliders.Remove(this);
        }

        public void LoadContent(ContentManager content)
        {
            transform = GameObject.Transform;
        }
    }
}