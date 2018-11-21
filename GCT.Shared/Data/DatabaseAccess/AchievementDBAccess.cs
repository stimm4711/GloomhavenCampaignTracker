using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class AchievementDBAccess : IDBAccess<DL_Achievement>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public AchievementDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_Achievement> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_Achievement>(recursive: recursive);
            }
        }

        public DL_Achievement Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_Achievement>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_Achievement item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_Achievement> items)
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

        internal void Insert(IEnumerable<DL_Achievement> items)
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

        public void Delete(DL_Achievement item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }
    }
}