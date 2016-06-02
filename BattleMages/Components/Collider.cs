using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public class Collider : Component
    {
        //Static list of all colliders created
        private Transform transform;

        private Texture2D debugTexture;

        public Vector2 Size { get; }
        public bool Solid { get; }
        public Vector2 Offset { get; set; }

        public Collider(Vector2 size, bool solid = false)
        {
            Size = size;
            Solid = solid;
            debugTexture = GameWorld.Instance.Content.Load<Texture2D>("Textures/Misc/CollisionTexture");

            Listen<InitializeMsg>(Initialize);
            Listen<DrawMsg>(Draw);
        }

        public Rectangle CalcColliderRect()
        {
            return new Rectangle((int)(transform.Position.X + Offset.X - Size.X / 2), (int)(transform.Position.Y + Offset.Y - Size.Y / 2), (int)Size.X, (int)Size.Y);
        }

        public bool CheckCollisionAtPosition(Vector2 position, bool solidOnly = false)
        {
            IEnumerable<Collider> collidersInScene = GetCollidersInScene();
            var rect = new Rectangle((int)(position.X + Offset.X - Size.X / 2), (int)(position.Y + Offset.Y - Size.Y / 2), (int)Size.X, (int)Size.Y);
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
            var rect = new Rectangle((int)(position.X + Offset.X - Size.X / 2), (int)(position.Y + Offset.Y - Size.Y / 2), (int)Size.X, (int)Size.Y);
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
            return GameWorld.Scene.ActiveObjects.Select(a => a.GetComponent<Collider>()).Where(a => a != null);
        }

        public bool CheckCollision(Collider other)
        {
            //Rectangle-rectangle collision
            return CalcColliderRect().Intersects(other.CalcColliderRect());
        }

        private void Initialize(InitializeMsg msg)
        {
            transform = GameObject.Transform;
        }

        private void Draw(DrawMsg msg)
        {
            //msg.Drawer[DrawLayer.Gameplay].Draw(debugTexture, CalcColliderRect(), new Color(Color.Red, 0.5f));
        }
    }
}