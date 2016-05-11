using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class EnemyRanged : EnemyAI
    {
        public EnemyRanged(Character character, Enemy enemy, Transform transform) :
            base(character, enemy, transform)
        {
            attackRange = 100 * enemy.Level;
            targetingRange = 500 * enemy.Level;
        }

        public override void Attack()
        {
        }

        public override void Targeting()
        {
            base.Targeting();
        }
    }
}