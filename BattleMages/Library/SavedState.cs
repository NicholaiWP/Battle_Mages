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
        private List<SpellInfo> spellBook = new List<SpellInfo>();
        private List<int> spellBar = new List<int>();
        private SQLiteConnection connection = new SQLiteConnection("Data Source = BMdatabase.db; Version = 3;");

        public List<SpellInfo> SpellBook { get { return spellBook; } }
        public List<int> SpellBar { get { return spellBar; } }

        public int PlayerGold { get; set; } = 1000;

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

            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO SpellBook VALUES(null, @spellId, @rune1Id, @rune2Id, @rune3Id, @rune4Id)",
                connection))
            {
            }
        }
    }
}