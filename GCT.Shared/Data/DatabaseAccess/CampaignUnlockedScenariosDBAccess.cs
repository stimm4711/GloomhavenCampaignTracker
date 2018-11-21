using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Linq;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class CampaignUnlockedScenariosDBAccess : IDBAccess<DL_CampaignUnlockedScenario>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public CampaignUnlockedScenariosDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_CampaignUnlockedScenario> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_CampaignUnlockedScenario>(recursive: recursive);
            }
        }

        public DL_CampaignUnlockedScenario Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_CampaignUnlockedScenario>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_CampaignUnlockedScenario item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_CampaignUnlockedScenario> items)
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

        internal void Insert(IEnumerable<DL_CampaignUnlockedScenario> items)
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

        public void Delete(DL_CampaignUnlockedScenario item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}