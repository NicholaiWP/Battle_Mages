using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class PlayerSpell
    {
        public const int PlayerRuneCount = 4;

        private int spellId;
        private int[] runeIds = new int[PlayerRuneCount];

        public int RuneCount { get { return runeIds.Length; } }

        public PlayerSpell()
        {
            spellId = -1;
            for (int i = 0; i < runeIds.Length; i++)
                runeIds[i] = -1;
        }

        public SpellInfo GetSpell()
        {
            if (spellId < 0 || spellId >= StaticData.Spells.Count) return null;
            return StaticData.Spells[spellId];
        }

        public RuneInfo GetRune(int pos)
        {
            int id = runeIds[pos];
            if (id < 0 || id >= StaticData.Runes.Count) return null;
            return StaticData.Runes[id];
        }

        public void SetRune(int pos, int runeId)
        {
            runeIds[pos] = runeId;
        }

        public void SetSpell(int spellId)
        {
            this.spellId = spellId;
        }
    }
}