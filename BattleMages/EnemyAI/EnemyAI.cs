using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public enum EnemyType
    {
        CloseRange,
        Ranged,
        DodgingCloseRange,
        DodgingRanged,
        Boss
    }

    public abstract class EnemyAI
    {
        protected bool implementDodge;
        protected Character character;
        protected Enemy enemy;
        protected Transform transform;
        protected float attackRange;
        protected float targetingRange;
        protected Player player;
        public bool IsDodging { get; set; }

        protected EnemyAI(Character character, Enemy enemy, Transform transform, bool implementDodge)
        {
            this.character = character;
            this.enemy = enemy;
            this.transform = transform;
            this.implementDodge = implementDodge;
        }

        public abstract void Attack();

        public virtual bool InAttackRange()
        {
            foreach (GameObject potentialTarget in GameWorld.CurrentScene.ActiveObjects)
            {
                if (potentialTarget.GetComponent<Player>() != null)
                {
                    Vector2 vecToTarget = Vector2.Subtract(transform.Position, potentialTarget.Transform.Position);
                    float lengthToTarget = vecToTarget.Length();
                    if (lengthToTarget <= attackRange)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual void Targeting()
        {
            foreach (GameObject potentialTarget in GameWorld.CurrentScene.ActiveObjects)
            {
                if (potentialTarget.GetComponent<Player>() != null)
                {
                    Vector2 vecToTarget = Vector2.Subtract(transform.Position, potentialTarget.Transform.Position);
                    float lengthToTarget = vecToTarget.Length();
                    if (lengthToTarget <= targetingRange && lengthToTarget >= attackRange)
                    {
                        Vector2 movement = Vector2.Zero;
                        if (transform.Position.Y - 50 > potentialTarget.Transform.Position.Y + 50)
                            movement.Y -= 1;
                        if (transform.Position.Y + 50 < potentialTarget.Transform.Position.Y - 50)
                            movement.Y += 1;
                        if (transform.Position.X - 50 > potentialTarget.Transform.Position.X + 50)
                            movement.X -= 1;
                        if (transform.Position.X + 50 < potentialTarget.Transform.Position.X - 50)
                            movement.X += 1;
                        character.MoveDirection = movement;
                    }
                    break;
                }
            }
        }

        public void Dodge()
        {
            foreach (Collider col in GetCollidersInScene())
            {
                if (Utils.InsideCircle(col.GameObject.Transform.Position, enemy.GameObject.Transform.Position, 100) &&
                    col != enemy.GameObject.GetComponent<Collider>() &&
                    col.GameObject.GetComponent<Projectile>() == null &&
                    col.GameObject.GetComponent<Player>() == null &&
                    col.GameObject.GetComponent<Enemy>() == null)
                {
                    break;
                }
            }
        }

        private IEnumerable<Collider> GetCollidersInScene()
        {
            return GameWorld.CurrentScene.ActiveObjects.Select(a => a.GetComponent<Collider>()).Where(a => a != null);
        }
    }
}