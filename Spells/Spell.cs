using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public abstract class Spell
    {
        //BASE STATS
        //These stats are used by spells to modify behaviour in different ways.
        //Spells should also define base stats in their constructor.
        //These stats will then be modified by runes.
        public int Damage { get; set; }
        public int ManaCost { get; set; }
        public int ManaCooldown { get; set; }

        private Rune[] runes;

        public Spell(Rune[] runes)
        {
            this.runes = runes;
        }

        /// <summary>
        /// Call after setting spell stats
        /// </summary>
        protected void ApplyRunes()
        {
            foreach (var r in runes)
            {
                r.ApplyChanges(this);
            }
        }
    }
}