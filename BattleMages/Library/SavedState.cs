using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    /// <summary>
    /// Contains a bunch of stuff that is both saved between scenes and between plays.
    /// Use for anything that will be stored in the savegame database.
    /// </summary>
    public class SavedState
    {
        private GameObject savingGo;
        private List<SpellInfo> spellBook = new List<SpellInfo>();
        private List<int> spellBar = new List<int>();
        private SQLiteConnection connection = new SQLiteConnection("Data Source = BMdatabase.db; Version = 3;");
        private string databaseFileName = "BMdatabase.db";
        private List<AttributeRune> availableRunes = new List<AttributeRune>();
        public List<AttributeRune> AvailableRunes { get { return availableRunes; } }
        private List<BaseRune> availableBaseRunes = new List<BaseRune>();
        public List<BaseRune> AvailableBaseRunes { get { return availableBaseRunes; } }
        public int PlayerGold { get; set; } = 1000;
        public List<SpellInfo> SpellBook { get { return spellBook; } }
        public List<int> SpellBar { get { return spellBar; } }
        public bool Saving { get; private set; } = false;

        public SavedState()
        {
            savingGo = new GameObject(Vector2.Zero);
            savingGo.AddComponent(new Animator());
            savingGo.AddComponent(new ShowSaving());
        }

        public void NewGame()
        {
            //Making the starting spells for the player
            for (int i = 0; i < 5; i++)
            {
                availableBaseRunes.Add(StaticData.BaseRunes[i]);
                if (i == 3) continue;
                SpellInfo ps = new SpellInfo();
                ps.SetBaseRune(i);
                //for (int j = 0; j < i; j++)
                //{
                //    ps.SetAttributeRune(j, 0);
                //}
                spellBook.Add(ps);
                spellBar.Add(spellBook.IndexOf(ps));
            }
        }

        /// <summary>
        /// The database file is created in this method with the database´s tables
        /// </summary>
        private void CreateDatabaseFile()
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
                }
                connection.Close();
            }
        }

        /// <summary>
        /// In this method all spells, gold and challenges that have been completed are saved
        /// </summary>
        public void Save()
        {
            CreateDatabaseFile();

            if (!Saving)
            {
                Thread t = new Thread(() => DatabaseInform());
                t.Start();
            }
        }

        private void DatabaseInform()
        {
            Saving = true;
            //This int is used for updating the RuneIDs in the table AttributeRunes,
            //it is the indicator of the id in the table.
            int attrRuneID = 1;

            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand(@"Delete from SpellBook where ID > @ID",
                connection))
            {
                command.Parameters.AddWithValue("@ID", spellBook.Count);
                command.ExecuteNonQuery();
            }

            using (SQLiteCommand command = new SQLiteCommand(@"Delete from AttributeRunes where SpellBookID > @SBID",
                 connection))
            {
                command.Parameters.AddWithValue("@SBID", spellBook.Count);
                command.ExecuteNonQuery();
            }

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
                    reader.Close();
                }
                using (SQLiteCommand command = new SQLiteCommand(@"Select RuneID from AttributeRunes where SpellBookID like @SBID",
                    connection))
                {
                    command.Parameters.AddWithValue("@SBID", i + 1);
                    SQLiteDataReader reader = command.ExecuteReader();

                    //This int is the runePos in Attribute rune ids from the spellbook
                    int runePos = 0;

                    while (reader.Read())
                    {
                        if (spellBook[i].AttrRuneIDs[runePos] != reader.GetInt32(0))
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Update AttributeRunes Set RuneID = @runeID where ID like @ID",
                                connection))
                            {
                                cmd.Parameters.AddWithValue("@ID", attrRuneID);
                                cmd.Parameters.AddWithValue("@runeID", spellBook[i].AttrRuneIDs[runePos]);
                                cmd.ExecuteReader();
                            }
                        }
                        attrRuneID++;
                        runePos++;
                    }
                    reader.Close();
                }

                using (SQLiteCommand command = new SQLiteCommand("Select Count(*) from SpellBook",
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
                    reader.Close();
                }
            }
            for (int i = 0; i < spellBar.Count; i++)
            {
                using (SQLiteCommand command = new SQLiteCommand("Select Count(*) from SpellBar",
                    connection))
                {
                    SQLiteDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (i + 1 > reader.GetInt32(0))
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Insert into SpellBar Values(null, @SBID)",
                            connection))
                            {
                                cmd.Parameters.AddWithValue("@SBID", spellBar[i]);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    reader.Close();
                }
                using (SQLiteCommand command = new SQLiteCommand(@"Update SpellBar set SpellBookID = @SBID where ID like @ID",
                    connection))
                {
                    command.Parameters.AddWithValue("@ID", i + 1);
                    command.Parameters.AddWithValue("@SBID", spellBar[i]);
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
            Saving = false;
        }

        public void Load()
        {
            //This int is the spellbook id for the foreignkeys
            int SBID = 1;

            //This is the position of the rune in the array
            int runePos = 0;

            if (File.Exists(databaseFileName))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("Select BaseRuneID from SpellBook",
                    connection))
                {
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        SpellInfo si = new SpellInfo();
                        si.SetBaseRune(reader.GetInt32(0));

                        using (SQLiteCommand cmd = new SQLiteCommand(@"Select RuneID from AttributeRunes Where SpellBookID like @SBID",
                            connection))
                        {
                            cmd.Parameters.AddWithValue("@SBID", SBID);
                            SQLiteDataReader read = cmd.ExecuteReader();
                            while (read.Read())
                            {
                                si.SetAttributeRune(runePos, read.GetInt32(0));
                                runePos++;
                            }
                            runePos = 0;
                            read.Close();
                        }
                        spellBook.Add(si);
                        SBID++;
                    }
                    reader.Close();
                }

                using (SQLiteCommand command = new SQLiteCommand("Select SpellBookID from SpellBar",
                    connection))
                {
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        spellBar.Add(reader.GetInt32(0));
                    }
                    reader.Close();
                }

                using (SQLiteCommand command = new SQLiteCommand(@"Delete from SpellBar where ID > @ID",
                    connection))
                {
                    command.Parameters.AddWithValue("@ID", spellBar.Count);
                    command.ExecuteNonQuery();
                }
                connection.Close();
                GameWorld.ChangeScene(new LobbyScene());
            }
        }

        public void Draw(Drawer drawer)
        {
            if (Saving && !GameWorld.Scene.ActiveObjects.Contains(savingGo))
                GameWorld.Scene.AddObject(savingGo);
        }
    }
}