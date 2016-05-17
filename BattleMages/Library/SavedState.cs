using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
        private SQLiteConnection connection = new SQLiteConnection("Data Source = BMdatabase.db; Version = 3;");

        public List<PlayerSpell> SpellBook { get { return spellBook; } }
        public List<PlayerSpell> SpellBar { get { return spellBar; } }

        public void Save()
        {
            SQLiteConnection.CreateFile("BMdatabase.db");
            CreateTables();
            InsertToTables();
        }

        private void CreateTables()
        {
            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS SpellBook(id integer primary key, spellId int, rune1Id int, rune2Id int, rune3Id int, rune4Id int)",
                connection))
            {
                command.ExecuteNonQuery();
            }

            using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS SpellBar(spellId int, rune1Id int, rune2Id int, rune3Id int, rune4Id int)",
                connection))
            {
                command.ExecuteNonQuery();
            }

            using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS ChallengesCompleted(challengeId int)",
                connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        private void InsertToTables()
        {
            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand("DELETE * from SpellBook", connection))
            {
                command.ExecuteNonQuery();
            }

            foreach (PlayerSpell spell in spellBook)
            {
                using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO SpellBook VALUES(null, @spellId, @rune1Id, @rune2Id, @rune3Id, @rune4Id)",
                connection))
                {
                    command.Parameters.AddWithValue("@spellId", spell.SpellId);
                    //command.Parameters.AddWithValue("@rune1Id", )
                }
            }
        }
    }

    public class PlayerSpell
    {
        private int spellId;
        private int[] runeIds;

        public int RuneCount { get { return runeIds.Length; } }
        public int SpellId { get { return spellId; } }
        public int[] RuneIds { get { return runeIds; } }

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