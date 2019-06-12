using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class ClassDBAccess : IDBAccess<DL_Class>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public ClassDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_Class> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_Class>(recursive: recursive);
            }
        }

        public DL_Class Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_Class>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_Class item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_Class> items)
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

        internal void Insert(IEnumerable<DL_Class> items)
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

        public void Delete(DL_Class item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}