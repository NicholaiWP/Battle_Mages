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
        private List<int> spellBar = new List<int>();

        public List<PlayerSpell> SpellBook { get { return spellBook; } }
        public List<int> SpellBar { get { return spellBar; } }

        public void Save()
        {

        }
    }
}