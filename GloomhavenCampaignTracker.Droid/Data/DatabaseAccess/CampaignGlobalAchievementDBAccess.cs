using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class CampaignGlobalAchievementDBAccess : IDBAccess<DL_CampaignGlobalAchievement>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public CampaignGlobalAchievementDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_CampaignGlobalAchievement> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_CampaignGlobalAchievement>(recursive: recursive);
            }
        }

        public DL_CampaignGlobalAchievement Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_CampaignGlobalAchievement>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_CampaignGlobalAchievement item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_CampaignGlobalAchievement> items)
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

        internal void Insert(IEnumerable<DL_CampaignGlobalAchievement> items)
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

        public void Delete(DL_CampaignGlobalAchievement item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }
    }
}