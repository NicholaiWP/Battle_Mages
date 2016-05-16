using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleMages
{
    public class SpellCreationParams
    {
        public RuneInfo[] Runes { get; }
        public Vector2 AimTarget { get; }
        public Vector2 VelocityOffset { get; }

        public SpellCreationParams(RuneInfo[] runes, Vector2 aimTarget, Vector2 velocityOffset)
        {
            Runes = runes;
            AimTarget = aimTarget;
            VelocityOffset = velocityOffset;
        }
    }

    public class SpellInfo
    {
        public delegate Spell SpellSpawnDelegate(GameObject go, SpellCreationParams creationParams);

        public string Name { get; }
        public string Description { get; }
        private SpellSpawnDelegate spawnFunc;

        public SpellInfo(string name, string description, SpellSpawnDelegate spawnFunc)
        {
            Name = name;
            Description = description;
            this.spawnFunc = spawnFunc;
        }

        public Spell CreateSpell(GameObject go, SpellCreationParams creationParams)
        {
            return spawnFunc(go, creationParams);
        }
    }
}