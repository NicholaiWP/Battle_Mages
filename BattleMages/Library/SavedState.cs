using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    /// <summary>
    /// Contains a bunch of stuff that is both saved between scenes and between plays.
    /// Use for anything that will be stored in the savegame database.
    /// </summary>
    public class SavedState
    {
        private List<PlayerSpell> spellBook = new List<PlayerSpell>();
        private List<PlayerSpell> spellBar = new List<PlayerSpell>();

        public List<PlayerSpell> SpellBook { get { return spellBook; } }
        public List<PlayerSpell> SpellBar { get { return spellBar; } }

        public void Save()
        {
        }
    }

    public class PlayerSpell
    {
        private int spellId;
        private int[] runeIds;

        public int RuneCount { get { return runeIds.Length; } }

        public PlayerSpell(int spellId, int[] runeIds)
        {
            this.spellId = spellId;
            this.runeIds = runeIds.ToArray();
        }

        public SpellInfo GetSpell()
        {
            return StaticData.Spells[spellId];
        }

        public RuneInfo GetRune(int pos)
        {
            return StaticData.Runes[runeIds[pos]];
        }
    }
}