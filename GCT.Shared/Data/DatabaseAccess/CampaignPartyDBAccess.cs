using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class CampaignPartyDBAccess : IDBAccess<DL_CampaignParty>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public CampaignPartyDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_CampaignParty> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_CampaignParty>(recursive: recursive);
            }
        }

        public DL_CampaignParty Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_CampaignParty>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_CampaignParty item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_CampaignParty> items)
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

        internal void Insert(IEnumerable<DL_CampaignParty> items)
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

        public void Delete(DL_CampaignParty item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }
    }
}