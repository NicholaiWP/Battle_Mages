using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Battle_Mages
{
    public class SpellInfo
    {
        public delegate Spell SpellSpawnDelegate(GameObject go, Vector2 targetPos, RuneInfo[] runes);

        public string Name { get; }
        public string Description { get; }
        private SpellSpawnDelegate spawnFunc;

        public SpellInfo(string name, string description, SpellSpawnDelegate spawnFunc)
        {
            Name = name;
            Description = description;
            this.spawnFunc = spawnFunc;
        }

        public Spell CreateSpell(GameObject go, Vector2 targetPos, RuneInfo[] runes)
        {
            return spawnFunc(go, targetPos, runes);
        }
    }
}