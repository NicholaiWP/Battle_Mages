using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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
        private string databaseFileName = "BMdatabase.db";
        public List<SpellInfo> SpellBook { get { return spellBook; } }
        public List<int> SpellBar { get { return spellBar; } }

        public void CreateDatabaseFile()
        {
            if (!File.Exists(databaseFileName))
            {
                SQLiteConnection.CreateFile(databaseFileName);
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS SpellBook(Id integer primary key, spellId int)",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS Runes(Id integer primary key, RuneId integer, SpellBookId integer REFERENCES SpellBook(Id))",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS SpellBar(Id integer primary key, SpellBookId integer REFERENCES SpellBook(Id))",
                    connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void Save()
        {
        }
    }
}