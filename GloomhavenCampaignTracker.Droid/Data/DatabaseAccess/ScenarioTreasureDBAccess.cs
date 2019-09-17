using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class ScenarioTreasureDBAccess : IDBAccess<DL_ScenarioTreasure>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public ScenarioTreasureDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_ScenarioTreasure> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_ScenarioTreasure>(recursive: recursive);
            }
        }

        public DL_ScenarioTreasure Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_ScenarioTreasure>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_ScenarioTreasure item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_ScenarioTreasure> items)
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

        internal void Insert(IEnumerable<DL_ScenarioTreasure> items)
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

        public void Delete(DL_ScenarioTreasure item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}