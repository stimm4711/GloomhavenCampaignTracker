using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Linq;
using System;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class CharacterDBAccess : IDBAccess<DL_Character>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public CharacterDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_Character> Get(bool recursive = true)
        {
            lock (locker)
            {
               return Connection.GetAllWithChildren<DL_Character>(recursive: recursive);
            }
        }

        public DL_Character Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_Character>(id, recursive: recursive);
            }
        }

        public void InsertOrReplace(DL_Character item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal List<DL_Item> GetItemsOfCharacter(int id)
        {
            lock (locker)
            {
                var query = "Select items.* " +
                            "from DL_CharacterItem ci " +
                            "Inner join DL_Item items on ci.ID_Item = items.Id "+
                            "where Id_character = ?";
                var items = Connection.Query<DL_Item>(query, id);

                return items;
            }
        }

        internal DL_CampaignParty GetPartyOfCharacter(int partyid)
        {
            lock (locker)
            {
                var query = "Select * " +
                            "from DL_CampaignParty " +
                            "where Id = ?";
                var party = Connection.Query<DL_CampaignParty>(query, partyid);

                return party.FirstOrDefault();
            }
        }

        internal List<DL_PersonalQuest> GetPersonalQuestsFlat()
        {
            lock (locker)
            {
                var query = "Select Id, QuestNumber, QuestName " +
                            "from DL_PersonalQuest ";
                var quests = Connection.Query<DL_PersonalQuest>(query);

                return quests;
            }
        }

        internal List<DL_Character> GetPartymembers(int id)
        {
            lock (locker)
            {
                var query = "Select * from DL_Character where ID_Party = ?"; 
                var characters = Connection.Query<DL_Character>(query, id);

                for (int i = 0; i < characters.Count; i++)
                {
                    characters[i] = Connection.GetWithChildren<DL_Character>(characters[i].Id, recursive: true);
                }

                return characters;
            }
        }

        internal List<DL_Character> GetPartymembersFlat(int id)
        {
            lock (locker)
            {
                var query = "Select * from DL_Character where ID_Party = ?";
                var characters = Connection.Query<DL_Character>(query, id);

                return characters;
            }
        }

        internal List<DL_Character> GetPartymembersUnretiredFlat(int id)
        {
            lock (locker)
            {
                var query = "Select * from DL_Character where Retired = 0 and ID_Party = ?";
                var characters = Connection.Query<DL_Character>(query, id);

                return characters;
            }
        }

        internal List<DL_Character> GetCharactersUnretiredFlat()
        {
            lock (locker)
            {
                var query = "Select * from DL_Character where Retired = 0";
                var characters = Connection.Query<DL_Character>(query);
                return characters;
            }
        }

        internal List<DL_ClassPerk> GetClassPerks(int classid, int characterid)
        {
            lock (locker)
            {
                var query = "Select * from DL_ClassPerk where classId = ? " +
                            "Except " +
                            "Select clp.* from DL_ClassPerk clp " +
                            "Inner Join DL_CharacterPerk chp on clp.Id = chp.ID_ClassPerk " +
                            "where ID_Character = ?";

                var perks = Connection.Query<DL_ClassPerk>(query, classid, characterid);
                return perks;
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_Character> items)
        {
            lock (locker)
            {
                Connection.BeginTransaction();
                try
                {
                    Connection.InsertOrReplaceAllWithChildren(items);
                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        internal void Insert(IEnumerable<DL_Character> items)
        {
            lock (locker)
            {
                Connection.BeginTransaction();
                try
                {
                    Connection.InsertAllWithChildren(items);
                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        public void Delete(DL_Character item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }
    }
}