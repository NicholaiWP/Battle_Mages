using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public abstract class Spell : Component
    {
        private AttributeRune[] runes;

        //BASE STATS
        //These stats are used by spells to modify behaviour in different ways.
        //Spells should also define base stats in their constructor.
        //These stats will then be modified by runes.
        public int Damage { get; set; }
        public int BurnDamage { get; set; }
        public int ManaCost { get; set; }
        public float CooldownTime { get; set; }

        public Spell(SpellCreationParams p)
        {
            runes = p.AttributeRunes;
        }

        /// <summary>
        /// Call after setting spell stats
        /// </summary>
        protected void ApplyAttributeRunes()
        {
            foreach (var r in runes)
            {
                if (r != null)
                    r.ApplyChanges(this);
            }
        }
    }
}