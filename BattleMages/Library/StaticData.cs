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
                new AttributeRune("Rune of Might",
                "Increases the damage of\nthis spell.",
                "rune1",
                DamageUpRune),

                new AttributeRune("Rune of Persistence",
                "Makes this spell use\nless mana.",
                "rune2",
                DecreaseManaCostRune),

                new AttributeRune("Rune of Haste",
                "Lets you cast this spell faster.",
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
                "A ball of fire\nwith a chance of igniting\nthe enemy with fire",
                "fireballRune",
                (p) => { return new Fireball(p); }),

                new BaseRune("Icicle",
                "Three sharp chunks of ice\nwill spread outwards",
                "iceshardsRune",
                (p) => { return new IceShard(p, true); }),

                new BaseRune("Lightning",
                "Powerful arcane lightning that\nstrikes from the sky",
                "lightningRune",
                (p) => {return new Lightning(p); }),

                new BaseRune("EarthSpikes",
                "Sharp spikes will rise from the\nground and damage over time",
                "earthspikesRune",
                (p) => {return new EarthSpikes(p); }),

                new BaseRune("FrostShield",
                "Three orbs of frost that rotate\naround you and protect\nagainst projectiles",
                "frostShieldRune",
                (p) => {return new FrostShield(p, true, 0); })
            };
        }

        public static void LoadContent()
        {
            foreach(AttributeRune attrRune in attributeRunes)
            {
                attrRune.LoadContent();
            }
            foreach(BaseRune baseRune in baseRunes)
            {
                baseRune.LoadContent();
            }
        }

        private static void DamageUpRune(Spell spell)
        {
            spell.Damage = (int)(spell.Damage * 1.25f);
        }

        private static void DecreaseManaCostRune(Spell spell)
        {
            spell.ManaCost = (int)(spell.ManaCost * 0.80f);
        }

        private static void DescreaseCooldown(Spell spell)
        {
            spell.CooldownTime -= spell.CooldownTime * 0.30f;
        }

        private static void CollideAbilityRune(Spell spell)
        {
            
        }
    }
}