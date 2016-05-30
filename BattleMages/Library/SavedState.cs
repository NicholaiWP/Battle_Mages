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

                using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS SpellBook(ID integer primary key, BaseRuneID int)",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS AttributeRunes(ID integer primary key, RuneID integer, SpellBookID integer REFERENCES SpellBook(ID))",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS SpellBar(ID integer primary key, SpellBookID integer REFERENCES SpellBook(ID))",
                    connection))
                {
                    command.ExecuteNonQuery();
                    for (int i = 0; i < spellBar.Count; i++)
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(@"Insert into SpellBar Values(null, @SBID)",
                            connection))
                        {
                            cmd.Parameters.AddWithValue("@SBID", spellBar[i]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                connection.Close();
            }
        }

        public void Save()
        {
            int attrRuneID = 1;
            connection.Open();

            for (int i = 0; i < spellBook.Count; i++)
            {
                using (SQLiteCommand command = new SQLiteCommand(@"Select BaseRuneID from SpellBook where ID like @ID",
                    connection))
                {
                    command.Parameters.AddWithValue("@ID", i + 1);

                    SQLiteDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (spellBook[i].BaseRuneID != reader.GetInt32(0))
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Update SpellBook Set BaseRuneID = @BaseRuneID where ID like @ID ",
                                connection))
                            {
                                cmd.Parameters.AddWithValue("@ID", i + 1);
                                cmd.Parameters.AddWithValue("@BaseRuneID", spellBook[i].BaseRuneID);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                using (SQLiteCommand command = new SQLiteCommand(@"Select RuneID from AttributeRunes where SpellBookID like @SBID",
                    connection))
                {
                    command.Parameters.AddWithValue("@SBID", i + 1);
                    SQLiteDataReader reader = command.ExecuteReader();
                    int runeCount = 0;
                    while (reader.Read())
                    {
                        if (spellBook[i].AttrRuneIDs[runeCount] != reader.GetInt32(0))
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Update AttributeRunes Set RuneID = @runeID where ID like @ID",
                                connection))
                            {
                                cmd.Parameters.AddWithValue("@ID", attrRuneID);
                                cmd.Parameters.AddWithValue("@runeID", spellBook[i].AttrRuneIDs[runeCount]);
                                cmd.ExecuteReader();
                            }
                        }
                        attrRuneID++;
                        runeCount++;
                    }
                }

                using (SQLiteCommand command = new SQLiteCommand(@"Select Count(*) from SpellBook",
                    connection))
                {
                    SQLiteDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (i + 1 > reader.GetInt32(0))
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Insert into SpellBook Values(null, @baseRuneID)",
                            connection))
                            {
                                cmd.Parameters.AddWithValue("@baseRuneID", spellBook[i].BaseRuneID);
                                cmd.ExecuteNonQuery();
                            }
                            for (int t = 0; t < spellBook[i].AttrRuneIDs.Length; t++)
                            {
                                using (SQLiteCommand cmd = new SQLiteCommand(@"Insert into AttributeRunes Values(null, @runeID, @SBID)",
                                    connection))
                                {
                                    cmd.Parameters.AddWithValue("@runeID", spellBook[i].AttrRuneIDs[t]);
                                    cmd.Parameters.AddWithValue("@SBID", i + 1);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < spellBar.Count; i++)
            {
                using (SQLiteCommand command = new SQLiteCommand(@"Update SpellBar set SpellBookID = @SBID where ID like @ID",
                    connection))
                {
                    command.Parameters.AddWithValue("@ID", i + 1);
                    command.Parameters.AddWithValue("@SBID", spellBar[i]);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Load()
        {
        }
    }
}