using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Collider : Component, ICanBeLoaded
    {
        //Static list of all colliders created
        private Transform transform;

        public Vector2 Size { get; }
        public bool Solid { get; }

        public Collider(GameObject gameObject, Vector2 size, bool solid = false) : base(gameObject)
        {
            Size = size;
            Solid = solid;
        }

        public Rectangle CalcColliderRect()
        {
            return new Rectangle((int)(transform.Position.X - Size.X / 2), (int)(transform.Position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y);
        }

        public bool CheckCollisionAtPosition(Vector2 position, bool solidOnly = false)
        {
            IEnumerable<Collider> collidersInScene = GetCollidersInScene();
            var rect = new Rectangle((int)(position.X - Size.X / 2), (int)(position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y);
            foreach (var coll in collidersInScene)
            {
                if (coll != this && (!solidOnly || coll.Solid))
                {
                    if (coll.CalcColliderRect().Intersects(rect)) return true;
                }
            }
            return false;
        }

        public List<Collider> GetCollisionsAtPosition(Vector2 position, bool solidOnly = false)
        {
            List<Collider> result = new List<Collider>();

            IEnumerable<Collider> collidersInScene = GetCollidersInScene();
            var rect = new Rectangle((int)(position.X - Size.X / 2), (int)(position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y);
            foreach (var coll in collidersInScene)
            {
                if (coll != this && (!solidOnly || coll.Solid) && coll.CalcColliderRect().Intersects(rect))
                {
                    result.Add(coll);
                }
            }
            return result;
        }

        private IEnumerable<Collider> GetCollidersInScene()
        {
            return GameWorld.CurrentScene.ActiveObjects.Select(a => a.GetComponent<Collider>()).Where(a => a != null);
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