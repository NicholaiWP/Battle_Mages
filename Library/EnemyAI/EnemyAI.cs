using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public abstract class EnemyAI
    {
        protected Animator animator;
        protected Enemy enemy;
        protected Transform transform;
        protected float attackRange;
        protected float targetingRange;

        protected EnemyAI(Animator animator, Enemy enemy, Transform transform)
        {
            this.animator = animator;
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
                    }
                    break;
                }
            }
        }
    }
}