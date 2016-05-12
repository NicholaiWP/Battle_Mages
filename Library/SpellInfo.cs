using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages.Library
{
    public class SpellInfo
    {
        public delegate Spell SpellSpawnDelegate(GameObject go, RuneInfo[] runes);

        public string Name { get; }
        public string Description { get; }
        private SpellSpawnDelegate spawnFunc;

        public SpellInfo(string name, string description, SpellSpawnDelegate spawnFunc)
        {
            Name = name;
            Description = description;
            this.spawnFunc = spawnFunc;
        }

        public Spell CreateSpell(GameObject go, RuneInfo[] runes)
        {
            return spawnFunc(go, runes);
        }
    }
}