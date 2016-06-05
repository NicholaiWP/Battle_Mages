using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    /// <summary>
    /// A class that stores info for a specific base rune. Can be used to instantiate a spell.
    /// </summary>
    public class BaseRune
    {
        public delegate Spell SpellSpawnDelegate(SpellCreationParams creationParams);

        private SpellSpawnDelegate spawnFunc;

        public string Name { get; }
        public string Description { get; }
        public string TextureName { get; }
        public SpellStats BaseStats { get; }
        public bool CanUseOutsideRange { get; }
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Creates a new base rune with a name, a description, and a delegate method to call for creating a spell from it.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="spawnFunc"></param>
        public BaseRune(string name, string description, string textureName, SpellStats baseStats, bool canUseOutsideRange, SpellSpawnDelegate spawnFunc)
        {
            Name = name;
            Description = description;
            TextureName = textureName;
            BaseStats = baseStats;
            CanUseOutsideRange = canUseOutsideRange;
            this.spawnFunc = spawnFunc;
        }

        /// <summary>
        /// Instantiates a spell (component) from this base rune, using the delegate method supplied on construction.
        /// </summary>
        /// <param name="go">Game object to own the created component. The component should be added to this object using AddComponent().</param>
        /// <param name="creationParams">Arguments used by the created spell to alter behaviour.</param>
        /// <returns></returns>
        public Spell CreateSpell(SpellCreationParams creationParams)
        {
            return spawnFunc(creationParams);
        }

        public void LoadContent()
        {
            Texture = GameWorld.Load<Texture2D>(StaticData.RuneImagePath + TextureName);
        }
    }
}