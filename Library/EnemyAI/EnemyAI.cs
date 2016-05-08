using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
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

        public virtual void Targeting()
        {
            foreach (GameObject potentialTarget in GameWorld.Instance.ActiveObjects)
            {
                if (potentialTarget.GetComponent<Player>() != null)
                {
                    Vector2 vecToTarget = Vector2.Subtract(transform.Position, potentialTarget.Transform.Position);
                    float lengthToTarget = vecToTarget.Length();
                    if (lengthToTarget <= targetingRange && lengthToTarget >= attackRange)
                    {
                        character.Up = transform.Position.Y > potentialTarget.Transform.Position.Y;
                        character.Down = transform.Position.Y < potentialTarget.Transform.Position.Y;
                        character.Left = transform.Position.X > potentialTarget.Transform.Position.X;
                        character.Right = transform.Position.X < potentialTarget.Transform.Position.X;
                    }
                    break;
                }
            }
        }
    }
}