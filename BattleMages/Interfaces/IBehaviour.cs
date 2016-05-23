﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public interface IBehaviour
    {
        void ExecuteBehaviour(float targetingRange, float attackRange);
    }
}