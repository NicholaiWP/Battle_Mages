using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public abstract class EnemyAI
    {
        protected EnemyAI()
        {
        }

        public abstract void Targeting();

        public abstract void Attack();
    }
}