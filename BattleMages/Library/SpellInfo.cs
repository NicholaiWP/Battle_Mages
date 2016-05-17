using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleMages
{
    /// <summary>
    /// Parameters used by created spells to alter behavour.
    /// </summary>
    public class SpellCreationParams
    {
        /// <summary>
        /// A list of runes to be applied to the spell on creation.
        /// </summary>
        public RuneInfo[] Runes { get; }
        /// <summary>
        /// The target in world coordinates the spell should aim for.
        /// </summary>
        public Vector2 AimTarget { get; }
        /// <summary>
        /// Offset used by moving spells. Usually set to the speed of the player.
        /// </summary>
        public Vector2 VelocityOffset { get; }

        public SpellCreationParams(RuneInfo[] runes, Vector2 aimTarget, Vector2 velocityOffset)
        {
            Runes = runes;
            AimTarget = aimTarget;
            VelocityOffset = velocityOffset;
        }
    }

    /// <summary>
    /// A class that stores info for a specific base spell. Can be used to instantiate a component for the spell.
    /// </summary>
    public class SpellInfo
    {
        public delegate Spell SpellSpawnDelegate(GameObject go, SpellCreationParams creationParams);

        public string Name { get; }
        public string Description { get; }
        private SpellSpawnDelegate spawnFunc;

        /// <summary>
        /// Creates a new instance of SpellInfo with a name, a description, and a delegate method to call creating the spell component.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="spawnFunc"></param>
        public SpellInfo(string name, string description, SpellSpawnDelegate spawnFunc)
        {
            Name = name;
            Description = description;
            this.spawnFunc = spawnFunc;
        }

        /// <summary>
        /// Instantiates a component for this spell, using the delegate method supplied on construction.
        /// </summary>
        /// <param name="go">Game object to own the created component. The component should be added to this object using AddComponent().</param>
        /// <param name="creationParams">Arguments used by the created spell to alter behaviour.</param>
        /// <returns></returns>
        public Spell CreateSpell(GameObject go, SpellCreationParams creationParams)
        {
            return spawnFunc(go, creationParams);
        }
    }
}