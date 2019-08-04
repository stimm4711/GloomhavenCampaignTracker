using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class ClassAbilitiesDBAccess : IDBAccess<DL_ClassAbility>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public ClassAbilitiesDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_ClassAbility> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_ClassAbility>(recursive: recursive);
            }
        }

        public DL_ClassAbility Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_ClassAbility>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_ClassAbility item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal List<DL_ClassAbility> GetSelectable(int iD_class, int characterlevel)
        {
            lock (locker)
            {
                var query = $"Select * from DL_ClassAbility where ID_Class = ? And Level <= ?";
                var abilities = Connection.Query<DL_ClassAbility>(query, iD_class, characterlevel);   
                return abilities;
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_ClassAbility> items)
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

        internal void Insert(IEnumerable<DL_ClassAbility> items)
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

        public void Delete(DL_ClassAbility item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}