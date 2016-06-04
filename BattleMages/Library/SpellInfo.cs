using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    /// <summary>
    /// Defines a player-made spell, a combination of a base rune and several attribute runes
    /// </summary>
    public class SpellInfo
    {
        /// <summary>
        /// Defines the number of attribute rune slots a spell has.
        /// </summary>
        public const int AttributeRuneSlotCount = 4;

        private int baseRuneID;
        private int[] attrRuneIDs = new int[AttributeRuneSlotCount];

        public int BaseRuneID { get { return baseRuneID; } }
        public int[] AttrRuneIDs { get { return attrRuneIDs; } }

        public SpellInfo()
        {
            baseRuneID = -1;
            for (int i = 0; i < attrRuneIDs.Length; i++)
                attrRuneIDs[i] = -1;
        }

        public BaseRune GetBaseRune()
        {
            if (baseRuneID < 0 || baseRuneID >= StaticData.BaseRunes.Count) return null;
            return StaticData.BaseRunes[baseRuneID];
        }

        public AttributeRune GetAttributeRune(int pos)
        {
            int id = attrRuneIDs[pos];
            if (id < 0 || id >= StaticData.AttributeRunes.Count) return null;
            return StaticData.AttributeRunes[id];
        }

        public void SetBaseRune(int baseRuneID)
        {
            this.baseRuneID = baseRuneID;
        }

        public void SetAttributeRune(int pos, int attrRuneID)
        {
            attrRuneIDs[pos] = attrRuneID;
        }

        /// <summary>
        /// Calculates the stats to use for this spell by applying all attribute runes to the base rune stats.
        /// </summary>
        /// <returns></returns>
        public SpellStats CalcStats()
        {
            SpellStats stats = GetBaseRune().BaseStats;
            for (int i = 0; i < AttributeRuneSlotCount; i++)
            {
                AttributeRune attrRune = GetAttributeRune(i);
                if (attrRune != null)
                    stats = attrRune.ApplyChanges(stats);
            }
            return stats;
        }
    }
}