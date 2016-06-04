using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BattleMages
{
    /// <summary>
    /// Parameters used by created spells to alter behavour.
    /// </summary>
    public class SpellCreationParams
    {
        /// <summary>
        /// The player-made spell info this spell is generated from.
        /// </summary>
        public SpellInfo SpellInfo { get; }

        /// <summary>
        /// The target in world coordinates the spell should aim for.
        /// </summary>
        public Vector2 AimTarget { get; }

        /// <summary>
        /// Offset used by moving spells. Usually set to the speed of the player.
        /// </summary>
        public Vector2 VelocityOffset { get; }

        public SpellCreationParams(SpellInfo spellInfo, Vector2 aimTarget, Vector2 velocityOffset)
        {
            SpellInfo = spellInfo;
            AimTarget = aimTarget;
            VelocityOffset = velocityOffset;
        }
    }
}