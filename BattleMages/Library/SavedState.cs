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
        private List<PlayerSpell> spellBook = new List<PlayerSpell>();
        private List<PlayerSpell> spellBar = new List<PlayerSpell>();
        private SQLiteConnection connection = new SQLiteConnection("Data Source = BMdatabase.db; Version = 3;");
        private string databaseFileName = "BMdatabase.db";
        private List<int> loadSpellIds = new List<int>();
        private List<int> loadRuneIds = new List<int>();
        public List<PlayerSpell> SpellBook { get { return spellBook; } }
        public List<PlayerSpell> SpellBar { get { return spellBar; } }

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

                using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS Runes(Id integer primary key,RuneId integer, SBId integer REFERENCES SpellBook(Id))",
                    connection))
                {
                    command.ExecuteNonQuery();
                }
                using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS SpellBar(Id integer primary key, SBId integer REFERENCES SpellBook(Id))",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void Save()
        {
            connection.Open();

            foreach (PlayerSpell spell in spellBook)
            {
                using (SQLiteCommand command = new SQLiteCommand(@"Insert into SpellBook Values(null, @spellId)",
                    connection))
                {
                    command.Parameters.AddWithValue("@SpellId", spell.SpellId);
                    command.ExecuteNonQuery();
                }
                using (SQLiteCommand command = new SQLiteCommand(@"Select Id from SpellBook where spellId like @SpellId",
                    connection))
                {
                    command.Parameters.AddWithValue("@SpellId", spell.SpellId);
                    SQLiteDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        int SBId = reader.GetInt32(reader.GetOrdinal("Id"));
                    }
                }
            }
        }

        private void InsertAndUpdate()
        {
            connection.Open();
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
                    using (SQLiteCommand command = new SQLiteCommand(@"Insert into Runes Values(@runeId, @SBId)",
                        connection))
                    {
                        command.Parameters.AddWithValue("@runeId", spell.RuneIds[i]);
                        command.Parameters.AddWithValue("@SBId", sbPrimaryId);
                        command.ExecuteNonQuery();
                    }
                }
                foreach (PlayerSpell spellInSpellbar in spellBar)
                {
                    if (spellInSpellbar == spell)
                    {
                        using (SQLiteCommand command = new SQLiteCommand(@"Insert into SpellBar Values(null, @SBId)",
                            connection))
                        {
                            command.Parameters.AddWithValue("@SBId", sbPrimaryId);
                            command.ExecuteNonQuery();
                        }
                        break;
                    }
                }
                sbPrimaryId++;
            }
        }

        public void Load()
        {
            if (File.Exists(databaseFileName))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("Select SpellId from SpellBook",
                    connection))
                {
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        loadSpellIds.Add(reader.GetInt32(reader.GetOrdinal("")));
                    }
                    reader.Close();
                }
                using (SQLiteCommand command = new SQLiteCommand("Select RuneId from Runes Where SBId like @Id",
                    connection))
                {
                    for (int i = 0; i < loadSpellIds.Count; i++)
                    {
                        command.Parameters.AddWithValue("@Id", i + 1);
                        SQLiteDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            loadRuneIds.Add(reader.GetInt32(0));
                        }

                        #region Check RuneIdCount

                        if (loadRuneIds.Count == 1)
                        {
                            spellBook.Add(new PlayerSpell(loadSpellIds[i], new[] { loadRuneIds[0] }));
                        }
                        else if (loadRuneIds.Count == 2)
                        {
                            spellBook.Add(new PlayerSpell(loadSpellIds[i], new[] { loadRuneIds[0], loadRuneIds[1] }));
                        }
                        else if (loadRuneIds.Count == 3)
                        {
                            spellBook.Add(new PlayerSpell(loadSpellIds[i], new[] { loadRuneIds[0], loadRuneIds[1],
                            loadRuneIds[2]}));
                        }
                        else if (loadRuneIds.Count == 4)
                        {
                            spellBook.Add(new PlayerSpell(loadSpellIds[i], new[] { loadRuneIds[0], loadRuneIds[1],
                            loadRuneIds[2], loadRuneIds[3]}));
                        }

                        #endregion Check RuneIdCount

                        loadRuneIds.Clear();
                        reader.Close();
                    }
                }
                spellBar.AddRange(spellBook);
                connection.Close();
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