using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
                "reduces Cooldown on spells",
                "rune3",
                DescreaseCooldown),

                //new AttributeRune("Projectile block",
                //"your spells can block enemy projectiles",
                //"rune4",
                //CollideAbilityRune),
            };

            //Add new base runes to this array
            baseRunes = new BaseRune[]
            {
                new BaseRune("Fireball",
                "A ball of fire",
                "fireballRune",
                (p) => { return new Fireball( p); }),

                new BaseRune("Icicle",
                "Sharp chunks of ice",
                "iceshardsRune",
                ( p) => { return new IceShard( p, true); }),

                new BaseRune("Lightning",
                "A fierce lightning which strikes\n from the sky",
                "lightningRune",
                ( p) => {return new Lightning( p); }),

                new BaseRune("EarthSpikes",
                "Hard spikes will rise from the\n ground",
                "earthspikesRune",
               ( p) => {return new EarthSpikes( p); }),

                new BaseRune("FrostShield",
                "Make 4 orbs of ice that rotate around you",
                "frostShieldRune",
                ( p) => {return new FrostShield( p, true, 0); })
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
            spell.CooldownTime -= spell.CooldownTime * 0.30f;
        }

        private static void CollideAbilityRune(Spell spell)
        {
            //work in progress
        }
    }
}