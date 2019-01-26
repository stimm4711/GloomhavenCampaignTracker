using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Resources;

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

        internal List<DL_CampaignEventHistoryLogItem> GetEvents(int id, int eventtype)
        {
            lock (locker)
            {
                var query = "Select * " +
                    "From DL_CampaignEventHistoryLogItem " +
                    "where ID_Campaign = ? " +
                    "and EventType = ? " +
                    "order by Position desc";

                return Connection.Query<DL_CampaignEventHistoryLogItem>(query, id, eventtype);
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
        
        internal void DisableOldEvents(int campaignId, int eventtypeId)
        {
            lock (locker)
            {
                var query = "Update DL_CampaignEventHistoryLogItem " +
                    "Set ID_Campaign = 0-ID_Campaign " +
                    "where ID_Campaign = ? " +
                    "and EventType = ?";
                   
                Connection.Query<DL_Character>(query, campaignId, eventtypeId);
            }
        }
    }
}