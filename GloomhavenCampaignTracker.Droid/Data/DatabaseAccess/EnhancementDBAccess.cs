using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class EnhancementDBAccess : IDBAccess<DL_Enhancement>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public EnhancementDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }
        
        public List<DL_Enhancement> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_Enhancement>(recursive: recursive);
            }
        }

        public DL_Enhancement Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_Enhancement>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_Enhancement item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_Enhancement> items)
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

        internal void Insert(IEnumerable<DL_Enhancement> items)
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

        public void Delete(DL_Enhancement item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}