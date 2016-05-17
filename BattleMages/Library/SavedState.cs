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
            //SQLiteConnection.CreateFile("BMdatabase.db");
            //CreateTables();
            InsertToTables();
        }

        private void CreateTables()
        {
            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS SpellBook(Id integer primary key, spellId int)",
                connection))
            {
                command.ExecuteNonQuery();
            }

            using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS Runes(RuneId integer, SBId integer, FOREIGN KEY(SBId) REFERENCES SpellBook(Id))",
                connection))
            {
                command.ExecuteNonQuery();
            }
            using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS SpellBar(SBId integer, FOREIGN KEY REFERENCES(SBId) SpellBook(Id))",
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
            int sbPrimaryId = 1;
            foreach (PlayerSpell spell in spellBook)
            {
                using (SQLiteCommand command = new SQLiteCommand(@"Insert into SpellBook Values(null, @spellId)",
                        connection))
                {
                    command.Parameters.AddWithValue("@spellId", spell.SpellId);
                    command.ExecuteNonQuery();
                }
                for (int i = 0; i < spell.RuneCount; i++)
                {
                    using (SQLiteCommand command = new SQLiteCommand(@"Insert into Runes Values(@runeId, @SBId)"))
                    {
                        command.Parameters.AddWithValue("@runeId", spell.RuneIds[i]);
                        command.Parameters.AddWithValue("@SBId", sbPrimaryId);
                        command.ExecuteNonQuery();
                    }
                }
                sbPrimaryId++;
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