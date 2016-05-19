using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BattleMages
{
    /// <summary>
    /// Contains data that will never change during or between runs, such as arrays of all the spells and runes in the game.
    /// </summary>
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
            //Add new runes to this array
            runes = new RuneInfo[]
            {
                new RuneInfo("Damage up rune",
                "+damage",
                "rune1",
                DamageUpRune),
            };

            //Add new spells to this array
            spells = new SpellInfo[]
            {
                new SpellInfo("Fireball",
                "A ball of fire",
                "fireballRune",
                (go, p) => { return new Fireball(go, p); }),

                new SpellInfo("Icicle",
                "A sharp chunk of ice",
                "iceshardsRune",
                (go, p) => { return new Icicle(go, p); }),
            };
        }

        private static void DamageUpRune(Spell spell)
        {
            spell.Damage = (int)(spell.Damage * 1.25f);
        }
    }
}