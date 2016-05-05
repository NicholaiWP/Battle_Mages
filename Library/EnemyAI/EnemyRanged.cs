using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class EnemyRanged : EnemyAI
    {
        public EnemyRanged(Animator animator, Enemy enemy, Transform transform) : base(animator, enemy, transform)
        {
            attackRange = 200 * enemy.Level;
            targetingRange = 500 * enemy.Level;
        }

        public override void Attack()
        {
        }
    }
}