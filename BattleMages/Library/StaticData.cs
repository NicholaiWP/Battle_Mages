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
        public const string RuneImagePath = "Textures/Runes/";
        private static AttributeRune[] attributeRunes;
        private static BaseRune[] baseRunes;
        public static Dictionary<string, Challenge> challenges = new Dictionary<string, Challenge>();

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
                new AttributeRune("Rune of Versatility",
                "Increases most stats a little.",
                "rune0",
                0,
                (s) => {
                    s.Damage = s.Damage * 1.05f;
                    s.ManaCost = s.ManaCost * 0.9f;
                    s.CooldownTime = s.CooldownTime * 0.96f;
                    s.Range = s.Range * 1.05f;
                    return s;
                }),

                 new AttributeRune("Rune of Efficiency",
                "Makes a spell use less mana.",
                "rune1",
                100,
                (s) => { s.ManaCost = s.ManaCost * 0.85f; return s;}),

                new AttributeRune("Rune of Might",
                "Increases the damage of a spell.",
                "rune2",
                100,
                (s) => { s.Damage = s.Damage * 1.25f; return s;}),

                new AttributeRune("Rune of Haste",
                "Lets you cast a spell faster.",
                "rune3",
                100,
                (s) => {s.CooldownTime = s.CooldownTime * 0.80f; return s; }),

                 new AttributeRune("Rune of Speed",
                "Lets you cast a spell even faster.",
                "rune4",
                150,
                (s) => {s.CooldownTime = s.CooldownTime * 0.69f; return s; }),

                  new AttributeRune("Rune of Desctruction",
                " Upgraded version of rune of might",
                "rune5",
                150,
                (s) => { s.Damage = s.Damage * 1.35f; s.CooldownTime *= 1.1f; return s;}),

                   new AttributeRune("Rune of Surplus",
                "Makes a spell use way less mana.",
                "rune6",
                150,
                (s) => { s.ManaCost = s.ManaCost * 0.60f; return s;}),

                new AttributeRune("Rune of Reach",
                "Increases the range of a spell.",
                "rune7",
                200,
                (s) => { s.Range *= 1.15f; return s; }),

                 new  AttributeRune("Rune of life",
                "Chance of regaining health on hit.",
                "rune8",
                200,
                (s) => {s.LifeSteal += 0.125f; return s;}),

                  new  AttributeRune("Rune of Queens",
                "Lower cooldown, mana cost, and damage.",
                "rune9",
                300,
                (s) => {s.Damage = s.Damage * 0.7f; s.ManaCost = s.ManaCost * 0.85f; s.CooldownTime *= 0.7f; return s; }),

                new AttributeRune("Rune of Kings",
                "Higher damage, but higher cooldown time.",
                "rune10",
                300,
                (s) => { s.Damage = s.Damage * 1.6f; s.CooldownTime *= 1.60f; return s; }),


                /*new AttributeRune("Rune of Speed",
                "Makes your spells move or act faster.",
                "rune5",
                50,
                (s) => { return s; }),*/

                //new AttributeRune("Projectile block",
                //"your spells can block enemy projectiles",
                //"rune4",
                //CollideAbilityRune),
            };

            //Add new base runes to this array
            baseRunes = new BaseRune[]
            {
                new BaseRune("Fireball",
                "A ball of fire with a chance of igniting the enemy with fire",
                "FireballRune",
                new SpellStats { Damage = 12, CooldownTime = 0.7f, ManaCost = 20, Range = 150 },
                true,
                (p) => { return new Fireball(p); }),

                new BaseRune("Ice Shards",
                "Three sharp chunks of ice will spread outwards",
                "IceShardsRune",
                new SpellStats { Damage = 5, CooldownTime = 0.9f, ManaCost = 30, Range = 65 },
                true,
                (p) => { return new IceShard(p, true); }),

                new BaseRune("Lightning",
                "Powerful arcane lightning that strikes from the sky",
                "LightningRune",
                new SpellStats { Damage = 40, CooldownTime = 2f, ManaCost = 30, Range = 100 },
                false,
                (p) => {return new Lightning(p); }),

                new BaseRune("Earth Spikes",
                "Sharp spikes will rise from the ground and damage over time",
                "EarthSpikesRune",
                new SpellStats { Damage = 10, CooldownTime = 3f, ManaCost = 70, Range = 130 },
                false,
                (p) => {return new EarthSpikes(p); }),

                new BaseRune("Frost Shield",
                "Three orbs of frost that rotate around you and protect against projectiles",
                "FrostShieldRune",
                new SpellStats { Damage = 8, CooldownTime = 2f, ManaCost = 40, Range = 28 },
                true,
                (p) => {return new FrostShield(p, true, 0); })
                    };

            challenges.Add("EarthBattle", new Challenge(3, new List<Wave> {
                //wave with 1 golem, 1 orb, 1 slime
                new Wave(new List<Vector2> { new Vector2(25,30),
                new Vector2(-20, -30), new Vector2(120, 90)},
                () =>  new List<Enemy> { new Golem(), new Orb(), new Slime() }),

                //wave with 4 golems
                new Wave(new List<Vector2> { new Vector2(300, 0), new Vector2(0, 300),
                new Vector2(-300, 0), new Vector2(0, -300) },
                () => new List<Enemy> {new Golem(), new Golem(), new Golem(), new Golem() }),

                //4 orbs, 2 golems, 3 slimes
                new Wave(new List<Vector2> { new Vector2(240,0), new Vector2(200, 10), new Vector2(200, -30),
                new Vector2(200, 0), new Vector2(200,-45), new Vector2(170,-60),
                new Vector2(190,0), new Vector2(90,0), new Vector2(180,0)},
                () => new List<Enemy> { new Orb(), new Orb(), new Orb(), new Orb(), new Golem(),
                new Slime(),new Slime(),new Slime(),new Golem()}),

                //wave with 4 orbs, 1 golem, 4 slimes
                new Wave(new List<Vector2> { new Vector2(240,0), new Vector2(200, 10), new Vector2(200, -10),
                new Vector2(200, 20), new Vector2(200,-60), new Vector2(170,0),
                new Vector2(190,0), new Vector2(130,0), new Vector2(180,0)},
                () => new List<Enemy> { new Orb(), new Orb(), new Orb(), new Slime(), new Slime(),
                new Orb(),new Slime(),new Slime(),new Golem() })
            }));

            challenges.Add("FrostBattle", new Challenge(4, new List<Wave> {
                // wave with 2 golems, 2 orbs
                new Wave(new List<Vector2> { new Vector2(47,88),
                new Vector2(-70, 60), new Vector2(100, -10), new Vector2(95,-66)},
                () => new List<Enemy> {new Golem(), new Golem(), new Orb(), new Orb()}),

                 //wave with 4 orbs, 2 golem, 3 slimes
                new Wave(new List<Vector2> { new Vector2(240,0), new Vector2(200, 10), new Vector2(200, -10),
                new Vector2(200, 20), new Vector2(200,-60), new Vector2(170,0),
                new Vector2(190,0), new Vector2(130,0), new Vector2(180,0)},
                () => new List<Enemy> { new Orb(), new Orb(), new Orb(), new Orb(), new Slime(),
                new Slime(),new Slime(),new Golem(),new Golem() }),

                 //wave with 3 orbs, 3 golem, 3 slimes
                new Wave(new List<Vector2> { new Vector2(240,0), new Vector2(200, 10), new Vector2(200, -10),
                new Vector2(200, 20), new Vector2(200,-60), new Vector2(170,0),
                new Vector2(190,0), new Vector2(130,0), new Vector2(180,0)},
                () => new List<Enemy> { new Orb(), new Orb(), new Orb(), new Slime(), new Slime(),
                new Slime(),new Golem(),new Golem(),new Golem() }),

            //wave with 4 orbs, 4 golems, 4 slimes
            new Wave(new List<Vector2> { new Vector2(-300, 0), new Vector2(300, 0), new Vector2(0, 300),
                new Vector2(0, -300), new Vector2(-250, 0), new Vector2(250, 0), new Vector2(0, 250),
                new Vector2(0, -250), new Vector2(-200, 0), new Vector2(200,0), new Vector2(0,200), new Vector2(0,-200)},
                () => new List<Enemy> { new Orb(), new Orb(), new Orb(), new Orb(), new Golem(), new Golem(),
                new Golem(), new Golem(), new Slime(), new Slime(), new Slime(), new Slime()})}));

            //Goltastic
            challenges.Add("ArcaneBattle", new Challenge(2, new List<Wave> {
                // wave with 4 golems
                new Wave(new List<Vector2> { new Vector2(47,88),
                new Vector2(-70, 60), new Vector2(100, -10), new Vector2(95,-66)},
                () => new List<Enemy> {new Golem(), new Golem(), new Golem(), new Golem()}),

                //wave with 8 slimes, 4 orbs
                 new Wave(new List<Vector2> {new Vector2(-250, 0), new Vector2(300, 0), new Vector2(0,170),
                 new Vector2(0, -300), new Vector2(-250, 0), new Vector2(250, 0), new Vector2(0, 250),
                new Vector2(0, -250), new Vector2(-200, 0), new Vector2(200,0), new Vector2(0,200), new Vector2(0,-200)},

                () => new List<Enemy> {new Slime(), new Slime(), new Slime(), new Slime(), new Slime(), new Slime(), new Slime(), new Slime(),
                new Orb(), new Orb(), new Orb(), new Orb()}),

                //wave with 5 slimes, 7 orbs
                 new Wave(new List<Vector2> {new Vector2(-250, 0), new Vector2(300, 0), new Vector2(0,170),
                 new Vector2(0, -300), new Vector2(-250, 0), new Vector2(250, 0), new Vector2(0, 250),
                new Vector2(0, -250), new Vector2(-200, 0), new Vector2(200,0), new Vector2(0,200), new Vector2(0,-200)},

                () => new List<Enemy> {new Slime(), new Slime(), new Slime(), new Slime(), new Slime(), new Orb(), new Orb(), new Orb(),
                new Orb(), new Orb(), new Orb(), new Orb()}),

                  //8 Golems, 4 orbs
                new Wave(new List<Vector2> { new Vector2(-300, 0), new Vector2(300, 0), new Vector2(0, 300),
                new Vector2(0, -300), new Vector2(-250, 0), new Vector2(250, 0), new Vector2(0, 250),
                new Vector2(0, -250), new Vector2(-120, 0), new Vector2(150,0), new Vector2(0,190), new Vector2(0,-200)},

                () => new List<Enemy> { new Golem(), new Golem(), new Golem(), new Golem(), new Golem(), new Golem(),
                new Golem(), new Golem(), new Orb(), new Orb(), new Orb(), new Orb()}),

                 //12 slime wave
                new Wave(new List<Vector2> { new Vector2(-280, 0), new Vector2(270, 0), new Vector2(0, 220),
                new Vector2(0, -300), new Vector2(-250, 0), new Vector2(230, 0), new Vector2(0, 210),
                new Vector2(0, -250), new Vector2(-200, 0), new Vector2(200,0), new Vector2(0,200), new Vector2(0,-200)},

                () => new List<Enemy> { new Slime(), new Slime(), new Slime(), new Slime(), new Slime(), new Slime(),
                new Slime(), new Slime(), new Slime(), new Slime(), new Slime(), new Slime()}),

                   //12 orb wave
                new Wave(new List<Vector2> { new Vector2(-280, 0), new Vector2(270, 0), new Vector2(0, 220),
                new Vector2(0, -300), new Vector2(-250, 0), new Vector2(230, 0), new Vector2(0, 210),
                new Vector2(0, -250), new Vector2(-200, 0), new Vector2(200,0), new Vector2(0,200), new Vector2(0,-200)},

                () => new List<Enemy> { new Orb(), new Orb(), new Orb(), new Orb(), new Orb(), new Orb(),
                new Orb(), new Orb(), new Orb(), new Orb(), new Orb(), new Orb()}),

                //12 golem wave
                new Wave(new List<Vector2> { new Vector2(-300, 0), new Vector2(300, 0), new Vector2(0, 300),
                new Vector2(0, -300), new Vector2(-250, 0), new Vector2(250, 0), new Vector2(0, 250),
                new Vector2(0, -250), new Vector2(-200, 0), new Vector2(200,0), new Vector2(0,200), new Vector2(0,-200)},

                () => new List<Enemy> { new Golem(), new Golem(), new Golem(), new Golem(), new Golem(), new Golem(),
                new Golem(), new Golem(), new Golem(), new Golem(), new Golem(), new Golem()})}));
        }

        public static void LoadContent()
        {
            foreach (AttributeRune attrRune in attributeRunes)
            {
                attrRune.LoadContent();
            }
            foreach (BaseRune baseRune in baseRunes)
            {
                baseRune.LoadContent();
            }
        }
    }
}