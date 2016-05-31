using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Slime : Enemy
    {
        public Slime()
        {
            health = 100;
            damage = 15;
            cooldownTimer = 1;
            attackSpeed = 0;
            targetingRange = 400;
            attackRange = 25;
        }

        protected override void PreInitialize(PreInitializeMsg msg)
        {
            base.PreInitialize(msg);
            GameObject.AddComponent(new SpriteRenderer("Textures/Enemies/Slime"));
        }

        protected override void Initialize(InitializeMsg msg)
        {
            base.Initialize(msg);
            behaviours.Add(new Hunt(this, attackRange, targetingRange));
            behaviours.Add(new Attack(this, attackRange, targetingRange));
        }
    }
}