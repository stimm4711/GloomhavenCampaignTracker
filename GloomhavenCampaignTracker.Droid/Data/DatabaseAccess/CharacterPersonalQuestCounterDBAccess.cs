using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class CharacterPersonalQuestCounterDBAccess : IDBAccess<DL_CharacterPersonalQuestCounter>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public CharacterPersonalQuestCounterDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_CharacterPersonalQuestCounter> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_CharacterPersonalQuestCounter>(recursive: recursive);
            }
        }

        public DL_CharacterPersonalQuestCounter Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_CharacterPersonalQuestCounter>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_CharacterPersonalQuestCounter item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_CharacterPersonalQuestCounter> items)
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

        internal void Insert(IEnumerable<DL_CharacterPersonalQuestCounter> items)
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

        public void Delete(DL_CharacterPersonalQuestCounter item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}