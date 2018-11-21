using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class CampaignEventHistoryLogItemDBAccess : IDBAccess<DL_CampaignEventHistoryLogItem>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public CampaignEventHistoryLogItemDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_CampaignEventHistoryLogItem> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_CampaignEventHistoryLogItem>(recursive: recursive);
            }
        }

        public DL_CampaignEventHistoryLogItem Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_CampaignEventHistoryLogItem>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_CampaignEventHistoryLogItem item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_CampaignEventHistoryLogItem> items)
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

        internal void Insert(IEnumerable<DL_CampaignEventHistoryLogItem> items)
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

        public void Delete(DL_CampaignEventHistoryLogItem item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }
    }
}