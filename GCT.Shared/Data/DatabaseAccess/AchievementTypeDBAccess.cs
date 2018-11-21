using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Linq;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class AchievementTypeDBAccess : IDBAccess<DL_AchievementType>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public AchievementTypeDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_AchievementType> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_AchievementType>(recursive: recursive);
            }
        }

        public DL_AchievementType Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_AchievementType>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_AchievementType item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_AchievementType> items)
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

        internal void Insert(IEnumerable<DL_AchievementType> items)
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

        public void Delete(DL_AchievementType item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }
    }
}