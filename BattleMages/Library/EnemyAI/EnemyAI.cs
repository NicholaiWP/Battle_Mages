using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public abstract class EnemyAI
    {
        protected Character character;
        protected Enemy enemy;
        protected Transform transform;
        protected float attackRange;
        protected float targetingRange;

        protected EnemyAI(Character character, Enemy enemy, Transform transform)
        {
            this.character = character;
            this.enemy = enemy;
            this.transform = transform;
        }

        public abstract void Attack();

        public bool InAttackRange()
        {
            foreach (GameObject potentialTarget in GameWorld.CurrentScene.ActiveObjects)
            {
                if (potentialTarget.GetComponent<Player>() != null)
                {
                    Vector2 vecToTarget = Vector2.Subtract(transform.Position, potentialTarget.Transform.Position);
                    float lengthToTarget = vecToTarget.Length();
                    if (lengthToTarget <= attackRange)
                        return true;
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
    }
}