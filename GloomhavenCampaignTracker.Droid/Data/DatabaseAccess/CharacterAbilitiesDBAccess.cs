using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class CharacterAbilitiesDBAccess : IDBAccess<DL_CharacterAbility>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public CharacterAbilitiesDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_CharacterAbility> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_CharacterAbility>(recursive: recursive);
            }
        }

        public DL_CharacterAbility Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_CharacterAbility>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_CharacterAbility item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_CharacterAbility> items)
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

        internal void Insert(IEnumerable<DL_CharacterAbility> items)
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

        public void Delete(DL_CharacterAbility item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}