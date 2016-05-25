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
                "Sharp chunks of ice",
                "iceshardsRune",
                (go, p) => { return new IceShard(go, p, true); }),

                new SpellInfo("Lightning",
                "A fierce lightning which strikes\n from the sky",
                "lightningRune",
                (go, p) => {return new Lightning(go, p); }),

                new SpellInfo("EarthSpikes",
                "Hard spikes will rise from the\n ground",
                "earthspikesRune",
               (go, p) => {return new EarthSpikes(go, p); }),
            };
        }

        private static void DamageUpRune(Spell spell)
        {
            spell.Damage = (int)(spell.Damage * 1.25f);
        }

        private static void DecreaseManaCostRune(Spell spell)
        {
            spell.ManaCost -= (int)(spell.ManaCost * 0.15f);
        }
    }
}