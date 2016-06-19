using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

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
        private SQLiteConnection connection = new SQLiteConnection("Data Source = SavedGame.db; Version = 3; foreign keys = true;");
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
                using (SQLiteCommand command = new SQLiteCommand("create table SpellBook(ID string primary key, BaseRuneID int references AvailableBaseRunes(BaseRuneID))",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("create table AttributeRunes(ID integer primary key, RuneID integer references AvailableRunes(RuneID), SpellBookID string REFERENCES SpellBook(ID) ON DELETE CASCADE ON UPDATE CASCADE)",
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

                using (SQLiteCommand command = new SQLiteCommand("create table SpellBar(ID integer primary key, SpellBookID string REFERENCES SpellBook(ID)  ON DELETE CASCADE ON UPDATE CASCADE)",
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

            List<BaseRune> abr = new List<BaseRune>(availableBaseRunes);
            List<AttributeRune> aar = new List<AttributeRune>(availableRunes);
            Dictionary<Guid, SpellInfo> sb = new Dictionary<Guid, SpellInfo>(spellBook);
            List<Guid?> sbar = new List<Guid?>(spellBar);

            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand(@"Update PlayerGold set Gold = @gold",
                connection))
            {
                command.Parameters.AddWithValue("@gold", PlayerGold);
                command.ExecuteNonQuery();
            }

            foreach (BaseRune bR in abr)
            {
                using (SQLiteCommand command = new SQLiteCommand(@"Select BaseRuneID from AvailableBaseRunes where BaseRuneID = @ID",
                    connection))
                {
                    command.Parameters.AddWithValue("@ID", StaticData.BaseRunes.IndexOf(bR));
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Insert into AvailableBaseRunes Values(@ID)",
                                connection))
                            {
                                cmd.Parameters.AddWithValue("@ID", StaticData.BaseRunes.IndexOf(bR));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            foreach (AttributeRune aR in aar)
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
                        }
                    }
                }
            }

            foreach (var pair in sb)
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
                                    if (pair.Value.AttrRuneIDs[t] != -1)
                                    {
                                        cmd.Parameters.AddWithValue("@runeID", pair.Value.AttrRuneIDs[t]);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@runeID", null);
                                    }
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
                            if (reader.IsDBNull(0) || pair.Value.AttrRuneIDs[runePos] != reader.GetInt32(0))
                            {
                                using (SQLiteCommand cmd = new SQLiteCommand(@"Update AttributeRunes Set RuneID = @runeID where ID like @ID",
                                    connection))
                                {
                                    cmd.Parameters.AddWithValue("@ID", attrRuneID);
                                    if (pair.Value.AttrRuneIDs[runePos] == -1)
                                    {
                                        cmd.Parameters.AddWithValue("@runeID", null);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@runeID", pair.Value.AttrRuneIDs[runePos]);
                                    }
                                    cmd.ExecuteReader();
                                }
                            }
                            attrRuneID++;
                            runePos++;
                        }
                    }
                }
            }
            int i = 0;
            foreach (var guid in sbar)
            {
                i++;
                using (SQLiteCommand command = new SQLiteCommand("Select Count (*) from SpellBar",
                    connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (i > reader.GetInt32(0))
                            {
                                using (SQLiteCommand cmd = new SQLiteCommand(@"Insert into SpellBar Values(@ID, @SBID)",
                                    connection))
                                {
                                    cmd.Parameters.AddWithValue("@ID", sbar.IndexOf(guid));
                                    cmd.Parameters.AddWithValue("@SBID", null);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                using (SQLiteCommand command = new SQLiteCommand(@"Update SpellBar set SpellBookID = @SBID where ID = @ID",
                    connection))
                {
                    command.Parameters.AddWithValue("@ID", sbar.IndexOf(guid));
                    if (guid == null)
                    {
                        command.Parameters.AddWithValue("@SBID", null);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@SBID", guid.ToString());
                    }
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

                using (SQLiteCommand command = new SQLiteCommand("Select SpellBookID, RuneID from AttributeRunes Inner join SpellBook on SpellBookID = SpellBook.ID",
                    connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        SpellInfo si = new SpellInfo();
                        while (reader.Read())
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Select BaseRuneID from SpellBook where ID = @ID",
                                connection))
                            {
                                cmd.Parameters.AddWithValue("@ID", reader.GetString(0));
                                using (SQLiteDataReader newReader = cmd.ExecuteReader())
                                {
                                    if (newReader.Read())
                                    {
                                        if (!SpellBook.ContainsKey(Guid.Parse(reader.GetString(0))))
                                        {
                                            si.SetBaseRune(newReader.GetInt32(0));
                                            SpellBook.Add(Guid.Parse(reader.GetString(0)), si);
                                        }
                                    }
                                }
                            }
                            if (reader.IsDBNull(1))
                            {
                                si.SetAttributeRune(runePos, -1);
                            }
                            else
                            {
                                si.SetAttributeRune(runePos, reader.GetInt32(1));
                            }
                            runePos++;
                            if (runePos >= 4)
                            {
                                runePos = 0;
                                si = new SpellInfo();
                            }
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
                            if (reader.IsDBNull(0))
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