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
        private  static IceShard iceShard;
        private static Fireball fireball;
        private static Lightning lightning;
        private static EarthSpikes earthspikes;

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

            #region Damage runes
                            new RuneInfo("Lesser Damage up rune",
                            "+damage (0.30%)",
                            "rune1",
                            LesserDamageUpRune),

                             new RuneInfo("Lesser improved Damage up rune",
                            "+damage (0.50%)",
                            "rune2",
                            LesserDamageUpRune2),

                              new RuneInfo("Minor Damage up rune",
                            "+damage (0.70%)",
                            "rune3",
                            MinorDamageUpRune),

                               new RuneInfo("Minor improved Damage up rune",
                            "+damage (100%)",
                            "rune4",
                            MinorDamageUpRune2),

                                new RuneInfo("Greater Damage up rune",
                            "+damage (125%)",
                            "rune5",
                            GreaterDamageUpRune),

                                 new RuneInfo("Greater improved Damage up rune",
                            "+damage (150%)",
                            "rune6",
                            GreaterDamageUpRune2),

                                  new RuneInfo("Major Damage up rune",
                            "+damage (200%)",
                            "rune7",
                            MajorDamageUpRune),

                                   new RuneInfo("Major improved Damage up rune",
                            "+damage (300%)",
                            "rune8",
                            MajorDamageUpRune2),
                            #endregion

            #region Reduce cooldown runes
                new RuneInfo("Lesser Reduce Cooldown",
                "- Cooldown (0.20%)",
                "rune9",
                LesserReduceCooldown),

                  new RuneInfo("Lesser Improved Reduce Cooldown",
                "- Cooldown (0.25%)",
                "rune10",
               LesserReduceCooldown2),

                   new RuneInfo("Minor Reduce Cooldown",
                "- Cooldown (0.30%)",
                "rune11",
              MinorReduceCooldown),

                    new RuneInfo("Minor improved Reduce Cooldown",
                "- Cooldown (0.35%)",
                "rune12",
              MinorReduceCooldown2),

                     new RuneInfo("Greater Reduce Cooldown",
                "- Cooldown (0.40%)",
                "rune13",
               GreaterReduceCooldown),

                      new RuneInfo("Greater Improved Reduce Cooldown",
                "- Cooldown (0.45%)",
                "rune14",
              GreaterReduceCooldown2),

                       new RuneInfo("Major Reduce Cooldown",
                "- Cooldown (0.50%)",
                "rune15",
               MajorReduceCooldown),
#endregion

              new RuneInfo("Decrease Mana Cost",
                "- Cooldown (0.15%)",
                "rune16",
                DecreaseManaCostRune),

              new RuneInfo("Increase Projectile Speed",
              "doubles projectile speed\n of the spells",
              "rune17",
              DoubleProjectileSpeed),
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
        #region Damage runes method region
        /// <summary>
        /// Damage rune methods with different strengths
        /// </summary>
        /// <param name="spell"></param>
        private static void LesserDamageUpRune(Spell spell)
        {
            spell.Damage = (int)(spell.Damage * 0.3f);
        }

        private static void LesserDamageUpRune2(Spell spell)
        {
            spell.Damage = (int)(spell.Damage * 0.5f);
        }

        private static void MinorDamageUpRune(Spell spell)
        {
            spell.Damage = (int)(spell.Damage * 0.7f);
        }

        private static void MinorDamageUpRune2(Spell spell)
        {
            spell.Damage = (int)(spell.Damage * 1f);
        }

        private static void GreaterDamageUpRune(Spell spell)
        {
            spell.Damage = (int)(spell.Damage * 1.25f);
        }

        private static void GreaterDamageUpRune2(Spell spell)
        {
            spell.Damage = (int)(spell.Damage * 1.5f);
        }

        private static void MajorDamageUpRune(Spell spell)
        {
            spell.Damage = (int)(spell.Damage * 2f);
        }

        private static void MajorDamageUpRune2(Spell spell)
        {
            spell.Damage = (int)(spell.Damage * 3f);
        }
        #endregion

        #region Cooldown reducing runes
        private static void LesserReduceCooldown (Spell spell)
        {
            spell.CooldownTime = (int)spell.CooldownTime * 0.2f;
        }

        private static void LesserReduceCooldown2(Spell spell)
        {
            spell.CooldownTime = (int)spell.CooldownTime * 0.25f;
        }

        private static void MinorReduceCooldown(Spell spell)
        {
            spell.CooldownTime = (int)spell.CooldownTime * 0.3f;
        }

        private static void MinorReduceCooldown2(Spell spell)
        {
            spell.CooldownTime = (int)spell.CooldownTime * 0.35f;
        }

        private static void GreaterReduceCooldown(Spell spell)
        {
            spell.CooldownTime = (int)spell.CooldownTime * 0.4f;
        }

        private static void GreaterReduceCooldown2(Spell spell)
        {
            spell.CooldownTime = (int)spell.CooldownTime * 0.45f;
        }

        private static void MajorReduceCooldown(Spell spell)
        {
            spell.CooldownTime = (int)spell.CooldownTime * 0.5f;
        }
        #endregion

        private static void DecreaseManaCostRune(Spell spell)
        {
            spell.ManaCost = (int)(spell.ManaCost * 0.15f);
        }

        private static void DoubleProjectileSpeed(Spell spell)
        {
            fireball.Velocity = fireball.Diff * 240f;
            iceShard.Velocity = iceShard.Diff * 200f;
            lightning.WaitTimer = 0.15f;
            earthspikes.Timer = 2;            
        }
    }
}