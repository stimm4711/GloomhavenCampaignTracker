using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Linq;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class CampaignUnlocksDBAccess : IDBAccess<DL_CampaignUnlocks>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public CampaignUnlocksDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_CampaignUnlocks> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_CampaignUnlocks>(recursive: recursive);
            }
        }

        public DL_CampaignUnlocks Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_CampaignUnlocks>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_CampaignUnlocks item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_CampaignUnlocks> items)
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

        internal DL_CampaignUnlocks GetCampaignUnlocks(int id)
        {
            lock (locker)
            {
                var query = "Select * from DL_CampaignUnlocks where ID_Campaign = ?";
                return Connection.ExecuteScalar<DL_CampaignUnlocks>(query, id);
            }
        }

        internal void Insert(IEnumerable<DL_CampaignUnlocks> items)
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

        public void Delete(DL_CampaignUnlocks item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}