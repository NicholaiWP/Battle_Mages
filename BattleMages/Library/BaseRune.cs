using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public AttributeRune[] AttributeRunes { get; }

        /// <summary>
        /// The target in world coordinates the spell should aim for.
        /// </summary>
        public Vector2 AimTarget { get; }

        /// <summary>
        /// Offset used by moving spells. Usually set to the speed of the player.
        /// </summary>
        public Vector2 VelocityOffset { get; }

        public SpellCreationParams(AttributeRune[] attributeRunes, Vector2 aimTarget, Vector2 velocityOffset)
        {
            AttributeRunes = attributeRunes;
            AimTarget = aimTarget;
            VelocityOffset = velocityOffset;
        }
    }

    /// <summary>
    /// A class that stores info for a specific base rune. Can be used to instantiate a spell.
    /// </summary>
    public class BaseRune
    {
        public delegate Spell SpellSpawnDelegate(SpellCreationParams creationParams);

        public string Name { get; }
        public string Description { get; }
        public string TextureName { get; }
        private SpellSpawnDelegate spawnFunc;

        /// <summary>
        /// Creates a new base rune with a name, a description, and a delegate method to call for creating a spell from it.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="spawnFunc"></param>
        public BaseRune(string name, string description, string textureName, SpellSpawnDelegate spawnFunc)
        {
            Name = name;
            Description = description;
            TextureName = textureName;
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
    }
}