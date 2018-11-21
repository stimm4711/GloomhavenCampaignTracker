using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Linq;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class GloomhavenSettingsDBAccess : IDBAccess<DL_GlommhavenSettings>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public GloomhavenSettingsDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_GlommhavenSettings> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_GlommhavenSettings>(recursive: recursive);
            }
        }

        public DL_GlommhavenSettings Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_GlommhavenSettings>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_GlommhavenSettings item)
        {
            lock (locker)
            {
                Connection.InsertOrReplace(item);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_GlommhavenSettings> items)
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

        internal void Insert(IEnumerable<DL_GlommhavenSettings> items)
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

        public void Delete(DL_GlommhavenSettings item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}