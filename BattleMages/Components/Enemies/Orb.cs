﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Orb : Enemy
    {
        public Orb(GameObject gameObject) : base(gameObject)
        {
            health = 100;
            damage = 15;
            cooldownTimer = 2;
            attackSpeed = 0;
            targetingRange = 750;
            attackRange = 300;

        }

        protected override void Initialize(InitializeMsg msg)
        {
            base.Initialize(msg);
            MoveSpeed = 70;
            behaviours.Add(new Hunt(this, attackRange, targetingRange));
            behaviours.Add(new Attack(this, attackRange, targetingRange));
            behaviours.Add(new Dodging(this));
        }
    }
}