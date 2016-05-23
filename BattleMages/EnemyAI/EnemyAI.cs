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
                        character.Up = transform.Position.Y - 50 > potentialTarget.Transform.Position.Y + 50;
                        character.Down = transform.Position.Y + 50 < potentialTarget.Transform.Position.Y - 50;
                        character.Left = transform.Position.X - 50 > potentialTarget.Transform.Position.X + 50;
                        character.Right = transform.Position.X + 50 < potentialTarget.Transform.Position.X - 50;
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
                    bool comingFromAbove = enemy.GameObject.Transform.Position.Y >=
                        col.GameObject.Transform.Position.Y;
                    bool comingFromBelow = enemy.GameObject.Transform.Position.Y <=
                        col.GameObject.Transform.Position.Y;
                    bool comingFromLeftSide = enemy.GameObject.Transform.Position.X >=
                        col.GameObject.Transform.Position.X;
                    bool comingFromRightSide = enemy.GameObject.Transform.Position.X <=
                        col.GameObject.Transform.Position.X;

                    if (comingFromAbove)
                        character.Left = true;
                    if (comingFromBelow)
                        character.Right = true;
                    if (comingFromLeftSide)
                        character.Down = true;
                    if (comingFromRightSide)
                        character.Up = true;
                    IsDodging = true;
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