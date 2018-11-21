using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Linq;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class AbilityDBAccess : IDBAccess<DL_Ability>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public AbilityDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_Ability> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_Ability>(recursive: recursive);
            }
        }

        public DL_Ability Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_Ability>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_Ability item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_Ability> items)
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

        internal void Insert(IEnumerable<DL_Ability> items)
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

        public void Delete(DL_Ability item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}