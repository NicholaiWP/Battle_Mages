using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class EnemyCloseRange : EnemyAI
    {
        public EnemyCloseRange(Character character, Enemy enemy, Transform transform) :
            base(character, enemy, transform)
        {
        }

        public override void Attack()
        {
            throw new NotImplementedException();
        }

        public override void Targeting()
        {
            throw new NotImplementedException();
        }
    }
}