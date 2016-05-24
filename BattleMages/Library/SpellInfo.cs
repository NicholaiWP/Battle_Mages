using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class SpellInfo
    {
        /// <summary>
        /// Defines the number of attribute rune slots a spell has.
        /// </summary>
        public const int AttributeRuneSlotCount = 4;

        private int spellId;
        private int[] runeIds = new int[AttributeRuneSlotCount];

        public SpellInfo()
        {
            spellId = -1;
            for (int i = 0; i < runeIds.Length; i++)
                runeIds[i] = -1;
        }

        public BaseRune GetBaseRune()
        {
            if (spellId < 0 || spellId >= StaticData.BaseRunes.Count) return null;
            return StaticData.BaseRunes[spellId];
        }

        public AttributeRune GetAttributeRune(int pos)
        {
            int id = runeIds[pos];
            if (id < 0 || id >= StaticData.AttributeRunes.Count) return null;
            return StaticData.AttributeRunes[id];
        }

        public void SetBaseRune(int spellId)
        {
            this.spellId = spellId;
        }

        public void SetAttributeRune(int pos, int runeId)
        {
            runeIds[pos] = runeId;
        }
    }
}