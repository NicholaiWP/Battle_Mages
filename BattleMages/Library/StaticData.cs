using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BattleMages
{
    /// <summary>
    /// Contains data that will never change during or between runs, such as arrays of all the runes in the game.
    /// </summary>
    public static class StaticData
    {
        private static AttributeRune[] attributeRunes;
        private static BaseRune[] baseRunes;

        public static ReadOnlyCollection<BaseRune> BaseRunes
        {
            get
            {
                return new ReadOnlyCollection<BaseRune>(baseRunes);
            }
        }

        public static ReadOnlyCollection<AttributeRune> AttributeRunes
        {
            get
            {
                return new ReadOnlyCollection<AttributeRune>(attributeRunes);
            }
        }

        static StaticData()
        {
            //Add new attribute runes to this array
            attributeRunes = new AttributeRune[]
            {
                new AttributeRune("Damage up rune",
                "Increase damage on spell",
                "rune1",
                DamageUpRune),

                new AttributeRune("Decrease mana cost",
                "Lower Mana cost by half",
                "rune2",
                DecreaseManaCostRune),

                new AttributeRune("Lower spell cooldown",
                "-30% cooldown on spells",
                "rune3",
                DescreaseCooldown),
            };

            //Add new base runes to this array
            baseRunes = new BaseRune[]
            {
                new BaseRune("Fireball",
                "A ball of fire",
                "fireballRune",
                (go, p) => { return new Fireball(go, p); }),

                new BaseRune("Icicle",
                "Sharp chunks of ice",
                "iceshardsRune",
                (go, p) => { return new IceShard(go, p, true); }),

                new BaseRune("Lightning",
                "A fierce lightning which strikes\n from the sky",
                "lightningRune",
                (go, p) => {return new Lightning(go, p); }),

                new BaseRune("EarthSpikes",
                "Hard spikes will rise from the\n ground",
                "earthspikesRune",
               (go, p) => {return new EarthSpikes(go, p); }),

                new BaseRune("FrostShield",
                "Make 4 orbs of ice that rotate around you",
                "iceshardsRune",
                (go, p) => {return new FrostShield(go, p, true, 0); })
            };
        }

        private static void DamageUpRune(Spell spell)
        {
            spell.Damage = (int)(spell.Damage * 1.25f);
        }

        private static void DecreaseManaCostRune(Spell spell)
        {
            spell.ManaCost -= (int)(spell.ManaCost * 0.50f);
        }

        private static void DescreaseCooldown(Spell spell)
        {
            spell.CooldownTime -= (int)(spell.CooldownTime * 0.30f);
        }
    }
}