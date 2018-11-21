using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Linq;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class PersonalQuestDBAccess : IDBAccess<DL_PersonalQuest>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public PersonalQuestDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_PersonalQuest> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_PersonalQuest>(recursive: recursive);
            }
        }

        public DL_PersonalQuest Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_PersonalQuest>(id, recursive: recursive);
            }
        }

        internal List<DL_PersonalQuest> GetWithNumbers(string pQNumbers, bool reqursive)
        {
            lock (locker)
            {
                var query = "Select * " +
                           "from DL_PersonalQuest " +
                           $"Where QuestNumber in ({pQNumbers})";
                return Connection.Query<DL_PersonalQuest>(query);
            }
        }

        public void InsertOrReplace(DL_PersonalQuest item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_PersonalQuest> items)
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

        internal void Insert(IEnumerable<DL_PersonalQuest> items)
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

        public void Delete(DL_PersonalQuest item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}