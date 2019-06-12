using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Linq;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class PartyAchievementDBAccess : IDBAccess<DL_PartyAchievement>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public PartyAchievementDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_PartyAchievement> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_PartyAchievement>(recursive: recursive);
            }
        }

        public DL_PartyAchievement Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_PartyAchievement>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_PartyAchievement item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_PartyAchievement> items)
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

        internal void Insert(IEnumerable<DL_PartyAchievement> items)
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

        public void Delete(DL_PartyAchievement item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}