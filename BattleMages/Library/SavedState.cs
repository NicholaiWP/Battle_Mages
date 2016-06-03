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
        private Dictionary<Guid, SpellInfo> spellBook = new Dictionary<Guid, SpellInfo>();
        private List<Guid?> spellBar = new List<Guid?>();
        private SQLiteConnection connection = new SQLiteConnection("Data Source = BMdatabase.db; Version = 3;");
        private string databaseFileName = "BMdatabase.db";
        private Texture2D savingSprite;
        private List<AttributeRune> availableRunes = new List<AttributeRune>();
        public List<AttributeRune> AvailableRunes { get { return availableRunes; } }
        public int PlayerGold { get; set; }
        public Dictionary<Guid, SpellInfo> SpellBook { get { return spellBook; } }
        public List<Guid?> SpellBar { get { return spellBar; } }
        public bool Saving { get; private set; } = false;

        public SavedState()
        {
            savingSprite = GameWorld.Instance.Content.Load<Texture2D>("Textures/Misc/basket");
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
            //Making the starting spells for the player
            for (int i = 0; i < 5; i++)
            {
                if (i == 3) continue;
                SpellInfo ps = new SpellInfo();
                ps.SetBaseRune(i);
                //for (int j = 0; j < i; j++)
                //{
                //    ps.SetAttributeRune(j, 0);
                //}
                Guid guid = Guid.NewGuid();
                spellBook.Add(guid, ps);
                spellBar.Add(guid);
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

                using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS SpellBook(ID string primary key, BaseRuneID int)",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS AttributeRunes(ID integer primary key, RuneID integer, SpellBookID string REFERENCES SpellBook(ID))",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("create table IF NOT EXISTS SpellBar(ID integer primary key, SpellBookID string REFERENCES SpellBook(ID))",
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
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Select SpellBookID from SpellBar where SpellBookID = @SBID",
                                connection))
                            {
                                cmd.Parameters.AddWithValue("@SBID", potentialKey);
                                using (SQLiteDataReader read = cmd.ExecuteReader())
                                {
                                    if (read.Read())
                                    {
                                        using (SQLiteCommand cm = new SQLiteCommand(@"Delete from SpellBar where SpellBookID = @SBID",
                                            connection))
                                        {
                                            cm.Parameters.AddWithValue("@SBID", potentialKey);
                                            cm.ExecuteNonQuery();
                                        }
                                    }
                                }
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
                    command.Parameters.AddWithValue("@ID", pair.Key);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (pair.Value.BaseRuneID != reader.GetInt32(0))
                            {
                                using (SQLiteCommand cmd = new SQLiteCommand(@"Update SpellBook Set BaseRuneID = @BaseRuneID where ID = @ID ",
                                    connection))
                                {
                                    cmd.Parameters.AddWithValue("@ID", pair.Key);
                                    cmd.Parameters.AddWithValue("@BaseRuneID", pair.Value.BaseRuneID);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                using (SQLiteCommand command = new SQLiteCommand(@"Select RuneID from AttributeRunes where SpellBookID = @SBID",
                    connection))
                {
                    command.Parameters.AddWithValue("@SBID", pair.Key);
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

                using (SQLiteCommand command = new SQLiteCommand("Select Count(*) from SpellBook",
                    connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (spellBook.Count > reader.GetInt32(0))
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
                                        cmd.Parameters.AddWithValue("@SBID", pair.Key);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < spellBar.Count; i++)
            {
                using (SQLiteCommand command = new SQLiteCommand("Select Count(*) from SpellBar",
                    connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (i + 1 > reader.GetInt32(0))
                            {
                                using (SQLiteCommand cmd = new SQLiteCommand(@"Insert into SpellBar Values(null, @SBID)",
                                connection))
                                {
                                    cmd.Parameters.AddWithValue("@SBID", spellBar[i].ToString());
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                using (SQLiteCommand command = new SQLiteCommand(@"Update SpellBar set SpellBookID = @SBID where ID = @ID",
                    connection))
                {
                    command.Parameters.AddWithValue("@ID", i + 1);
                    command.Parameters.AddWithValue("@SBID", spellBar[i].ToString());
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
                            using (SQLiteCommand cmd = new SQLiteCommand(@"Select RuneID from AttributeRunes Where SpellBookID like @SBID",
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
                            spellBar.Add(Guid.Parse(reader.GetString(0)));
                        }
                    }
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
            if (Saving)
                drawer[DrawLayer.AboveUI].Draw(savingSprite, new Vector2(GameWorld.Camera.Position.X + GameWorld.GameWidth / 2 - savingSprite.Width,
                    GameWorld.Camera.Position.Y + GameWorld.GameHeight / 2 - savingSprite.Height));
        }
    }
}