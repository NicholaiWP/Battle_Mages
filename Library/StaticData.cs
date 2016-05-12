using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Battle_Mages.Library
{
    public static class StaticData
    {
        private static RuneInfo[] runes;
        private static SpellInfo[] spells;

        public static ReadOnlyCollection<SpellInfo> Spells
        {
            get
            {
                return new ReadOnlyCollection<SpellInfo>(spells);
            }
        }

        public static ReadOnlyCollection<RuneInfo> Runes
        {
            get
            {
                return new ReadOnlyCollection<RuneInfo>(runes);
            }
        }

        static StaticData()
        {
            runes = new RuneInfo[]
            {
                new RuneInfo("Damage up rune",
                "+damage",
                DamageUpRune),
            };

            spells = new SpellInfo[]
            {
                new SpellInfo("Fireball",
                "A ball of fire",
                (go, runes) => { return new Fireball(go, runes); }),
            };
        }

        private static void DamageUpRune(Spell spell)
        {
            spell.Damage += 10;
        }
    }
}