using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public static class RuneList
    {
        private static Rune[] runes;

        public static ReadOnlyCollection<Rune> Runes
        {
            get
            {
                return new ReadOnlyCollection<Rune>(runes);
            }
        }

        static RuneList()
        {
            runes = new[]
            {
                new Rune("Damage up rune",
                "+damage",
                DamageUpRune),
            };
        }

        private static void DamageUpRune(Spell spell)
        {
            spell.Damage += 10;
        }
    }

    public class Rune
    {
        public delegate void RuneAction(Spell targetSpell);

        private RuneAction action;

        public string Name { get; }
        public string Description { get; }

        public Rune(string name, string description, RuneAction action)
        {
            Name = name;
            Description = description;
            this.action = action;
        }

        public void ApplyChanges(Spell targetSpell)
        {
            action(targetSpell);
        }
    }
}