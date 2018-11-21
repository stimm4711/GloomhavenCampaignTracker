using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Linq;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class ClassPerkDBAccess : IDBAccess<DL_ClassPerk>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public ClassPerkDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_ClassPerk> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_ClassPerk>(recursive: recursive);
            }
        }

        public DL_ClassPerk Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_ClassPerk>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_ClassPerk item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_ClassPerk> items)
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

        internal void Insert(IEnumerable<DL_ClassPerk> items)
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

        public void Delete(DL_ClassPerk item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}