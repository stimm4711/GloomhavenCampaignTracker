using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class CampaignScenarioTreasureDBAccess : IDBAccess<DL_CampaignScenarioTreasure>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public CampaignScenarioTreasureDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_CampaignScenarioTreasure> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_CampaignScenarioTreasure>(recursive: recursive);
            }
        }

        public DL_CampaignScenarioTreasure Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_CampaignScenarioTreasure>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_CampaignScenarioTreasure item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_CampaignScenarioTreasure> items)
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

        internal void Insert(IEnumerable<DL_CampaignScenarioTreasure> items)
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

        public void Delete(DL_CampaignScenarioTreasure item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }
    }
}