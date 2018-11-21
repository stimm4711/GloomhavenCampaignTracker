using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Linq;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class CampaignUnlockedItemsDBAccess : IDBAccess<DL_CampaignUnlockedItem>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public CampaignUnlockedItemsDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_CampaignUnlockedItem> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_CampaignUnlockedItem>(recursive: recursive);
            }
        }

        public DL_CampaignUnlockedItem Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_CampaignUnlockedItem>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_CampaignUnlockedItem item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_CampaignUnlockedItem> items)
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

        internal void Insert(IEnumerable<DL_CampaignUnlockedItem> items)
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

        public void Delete(DL_CampaignUnlockedItem item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}