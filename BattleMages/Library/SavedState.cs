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

        private Dictionary<Guid, SpellInfo> spellBook = new Dictionary<Guid, SpellInfo>();
        private List<Guid?> spellBar = new List<Guid?>();
        private SQLiteConnection connection = new SQLiteConnection("Data Source = SavedGame.db; Version = 3;");
        private string databaseFileName = "SavedGame.db";
        private List<AttributeRune> availableRunes = new List<AttributeRune>();
        public List<AttributeRune> AvailableRunes { get { return availableRunes; } }
        private List<BaseRune> availableBaseRunes = new List<BaseRune>();
        public List<BaseRune> AvailableBaseRunes { get { return availableBaseRunes; } }
        public int PlayerGold { get; set; }

        public Dictionary<Guid, SpellInfo> SpellBook { get { return spellBook; } }
        public List<Guid?> SpellBar { get { return spellBar; } }
        public bool Saving { get; private set; } = false;

        public SavedState()
        {
            savingGo = new GameObject(Vector2.Zero);
            savingGo.AddComponent(new Animator());
            savingGo.AddComponent(new ShowSaving());
        }

        public SpellInfo GetSpellbookSpell(Guid guid)
        {
            SpellInfo result;
            spellBook.TryGetValue(guid, out result);
            return result;
        }

        public SpellInfo GetSpellbarSpell(int position)
        {
            if (spellBar[position] == null) return null;
            return SpellBook[(Guid)spellBar[position]];
        }

        public void NewGame()
        {
            if (File.Exists(databaseFileName))
            {
                File.Delete(databaseFileName);
            }

            availableRunes.Add(StaticData.AttributeRunes[0]);

            //Making the starting spells for the player
            for (int i = 0; i < 2; i++)
            {
                availableBaseRunes.Add(StaticData.BaseRunes[i]);
                SpellInfo ps = new SpellInfo();
                ps.SetBaseRune(i);

                for (int j = 0; j < SpellInfo.AttributeRuneSlotCount; j++)
                {
                    ps.SetAttributeRune(j, 0);
                }

                Guid guid = Guid.NewGuid();
                spellBook.Add(guid, ps);
                spellBar.Add(guid);
            }
            for (int i = 0; i < 2; i++)
            {
                SpellBar.Add(null);
            }
            Thread t = new Thread(() => CreateDatabaseFile());
            t.Start();
        }

        /// <summary>
        /// The database file is created in this method with the database´s tables
        /// </summary>
        private void CreateDatabaseFile()
        {
            Saving = true;
            if (!File.Exists(databaseFileName))
            {
                SQLiteConnection.CreateFile(databaseFileName);
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("create table SpellBook(ID string primary key, BaseRuneID int)",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("create table AttributeRunes(ID integer primary key, RuneID integer, SpellBookID string REFERENCES SpellBook(ID))",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("create table AvailableBaseRunes(BaseRuneID int primary key) ",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("create table AvailableRunes(RuneID int primary key)",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("create table PlayerGold (Gold int)",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand(@"Insert into PlayerGold Values(@gold)",
                    connection))
                {
                    command.Parameters.AddWithValue("@gold", 0);
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("create table SpellBar(ID integer primary key, SpellBookID string REFERENCES SpellBook(ID))",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                for (int i = 0; i < 4; i++)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(@"Insert into SpellBar Values(@ID, @SBID)",
                        connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", i);
                        cmd.Parameters.AddWithValue("@SBID", null);

                        cmd.ExecuteNonQuery();
                    }
                }
                connection.Close();
                DatabaseInform();
            }
        }

        /// <summary>
        /// In this method all spells, gold and challenges that have been completed are saved
        /// </summary>
        public void Save()
        {
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

            using (SQLiteCommand command = new SQLiteCommand(@"Update PlayerGold set Gold = @gold",
                connection))
            {
                command.Parameters.AddWithValue("@gold", PlayerGold);
                command.ExecuteNonQuery();
            }

            foreach (BaseRune bR in availableBaseRunes)
            {
                using (SQLiteCommand command = new SQLiteCommand(@"Select BaseRuneID from AvailableBaseRunes where BaseRuneID = @ID",
                    connection))
                {
                    command.Parameters.AddWithValue("@ID", availableBaseRunes.IndexOf(bR));
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Insert into AvailableBaseRunes Values(@ID)",
                                connection))
                            {
                                cmd.Parameters.AddWithValue("@ID", availableBaseRunes.IndexOf(bR));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            foreach (AttributeRune aR in availableRunes)
            {
                using (SQLiteCommand command = new SQLiteCommand(@"Select RuneID from AvailableRunes where RuneID = @ID",
                    connection))
                {
                    command.Parameters.AddWithValue("@ID", StaticData.AttributeRunes.IndexOf(aR));

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Insert into AvailableRunes Values(@ID)",
                                connection))
                            {
                                cmd.Parameters.AddWithValue("@ID", StaticData.AttributeRunes.IndexOf(aR));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                using (SQLiteCommand command = new SQLiteCommand(@"Update SpellBar set SpellBookID = @SBID where ID = @ID",
                    connection))
                {
                    command.Parameters.AddWithValue("@ID", i);
                    command.Parameters.AddWithValue("@SBID", spellBar[i].ToString());

                    command.ExecuteNonQuery();
                }
            }

            using (SQLiteCommand command = new SQLiteCommand(@"Select ID from SpellBook",
                connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string potentialKey = reader.GetString(0);
                        if (!spellBook.ContainsKey(Guid.Parse(potentialKey)))
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Delete from SpellBook where ID = @ID",
                                connection))
                            {
                                cmd.Parameters.AddWithValue("@ID", potentialKey);
                                cmd.ExecuteNonQuery();
                            }
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Delete from AttributeRunes where SpellBookID = @SBID",
                                connection))
                            {
                                cmd.Parameters.AddWithValue("@SBID", potentialKey);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            foreach (var pair in spellBook)
            {
                using (SQLiteCommand command = new SQLiteCommand(@"Select BaseRuneID from SpellBook where ID = @ID",
                    connection))
                {
                    command.Parameters.AddWithValue("@ID", pair.Key.ToString());

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (pair.Value.BaseRuneID != reader.GetInt32(0))
                            {
                                using (SQLiteCommand cmd = new SQLiteCommand(@"Update SpellBook Set BaseRuneID = @BaseRuneID where ID = @ID ",
                                    connection))
                                {
                                    cmd.Parameters.AddWithValue("@ID", pair.Key.ToString());
                                    cmd.Parameters.AddWithValue("@BaseRuneID", pair.Value.BaseRuneID);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Insert into SpellBook Values(@ID, @baseRuneID)",
                                connection))
                            {
                                cmd.Parameters.AddWithValue("@ID", pair.Key.ToString());
                                cmd.Parameters.AddWithValue("@baseRuneID", pair.Value.BaseRuneID);
                                cmd.ExecuteNonQuery();
                            }
                            for (int t = 0; t < pair.Value.AttrRuneIDs.Length; t++)
                            {
                                using (SQLiteCommand cmd = new SQLiteCommand(@"Insert into AttributeRunes Values(null, @runeID, @SBID)",
                                    connection))
                                {
                                    cmd.Parameters.AddWithValue("@runeID", pair.Value.AttrRuneIDs[t]);
                                    cmd.Parameters.AddWithValue("@SBID", pair.Key.ToString());
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                using (SQLiteCommand command = new SQLiteCommand(@"Select RuneID from AttributeRunes where SpellBookID = @SBID",
                    connection))
                {
                    command.Parameters.AddWithValue("@SBID", pair.Key.ToString());
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        //This int is the runePos in Attribute rune ids from the spellbook
                        int runePos = 0;

                        while (reader.Read())
                        {
                            if (pair.Value.AttrRuneIDs[runePos] != reader.GetInt32(0))
                            {
                                using (SQLiteCommand cmd = new SQLiteCommand(@"Update AttributeRunes Set RuneID = @runeID where ID like @ID",
                                    connection))
                                {
                                    cmd.Parameters.AddWithValue("@ID", attrRuneID);
                                    cmd.Parameters.AddWithValue("@runeID", pair.Value.AttrRuneIDs[runePos]);
                                    cmd.ExecuteReader();
                                }
                            }
                            attrRuneID++;
                            runePos++;
                        }
                    }
                }
            }

            foreach (var guid in spellBar)
            {
                using (SQLiteCommand command = new SQLiteCommand(@"Update SpellBar set SpellBookID = @SBID where ID = @ID",
                    connection))
                {
                    command.Parameters.AddWithValue("@ID", spellBar.IndexOf(guid));
                    command.Parameters.AddWithValue("@SBID", guid.ToString());

                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
            Saving = false;
        }

        public void Load()
        {
            //This is the position of the rune in the array
            int runePos = 0;

            if (File.Exists(databaseFileName))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("Select Gold from PlayerGold",
                    connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            PlayerGold = reader.GetInt32(0);
                        }
                    }
                }

                using (SQLiteCommand command = new SQLiteCommand("Select BaseRuneID from AvailableBaseRunes",
                    connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            availableBaseRunes.Add(StaticData.BaseRunes[reader.GetInt32(0)]);
                        }
                    }
                }

                using (SQLiteCommand command = new SQLiteCommand("Select RuneID from AvailableRunes",
                    connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            availableRunes.Add(StaticData.AttributeRunes[reader.GetInt32(0)]);
                        }
                    }
                }

                using (SQLiteCommand command = new SQLiteCommand("Select ID, BaseRuneID from SpellBook",
                    connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SpellInfo si = new SpellInfo();
                            si.SetBaseRune(reader.GetInt32(1));
                            string SBID = reader.GetString(0);
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Select RuneID from AttributeRunes Where SpellBookID = @SBID",
                                connection))
                            {
                                cmd.Parameters.AddWithValue("@SBID", SBID);
                                using (SQLiteDataReader read = cmd.ExecuteReader())
                                {
                                    while (read.Read())
                                    {
                                        si.SetAttributeRune(runePos, read.GetInt32(0));
                                        runePos++;
                                    }
                                    runePos = 0;
                                }
                            }
                            spellBook.Add(Guid.Parse(SBID), si);
                        }
                    }
                }

                using (SQLiteCommand command = new SQLiteCommand("Select SpellBookID from SpellBar",
                    connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string s = reader.GetString(0);
                            if (reader.GetString(0) == string.Empty)
                            {
                                spellBar.Add(null);
                            }
                            else
                            {
                                spellBar.Add(Guid.Parse(reader.GetString(0)));
                            }
                        }
                    }
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