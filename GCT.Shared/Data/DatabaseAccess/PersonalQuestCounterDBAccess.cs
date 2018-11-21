using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class PersonalQuestCounterDBAccess : IDBAccess<DL_PersonalQuestCounter>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public PersonalQuestCounterDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_PersonalQuestCounter> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_PersonalQuestCounter>(recursive: recursive);
            }
        }

        public DL_PersonalQuestCounter Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_PersonalQuestCounter>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_PersonalQuestCounter item, bool recursive)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: recursive);
            }
        }

        public void InsertOrReplace(DL_PersonalQuestCounter item)
        {
            InsertOrReplace(item, true);
        }

        internal void InsertOrReplace(IEnumerable<DL_PersonalQuestCounter> items)
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

        internal void Insert(IEnumerable<DL_PersonalQuestCounter> items)
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

        public void Delete(DL_PersonalQuestCounter item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }       
    }
}