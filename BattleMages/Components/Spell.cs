using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public abstract class Spell : Component
    {
        private SpellStats stats;

        protected SpellStats Stats { get { return stats; } }

        public Spell(SpellCreationParams p)
        {
            stats = p.SpellInfo.CalcStats();
        }
    }
}