using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Linq;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class ItemDBAccess : IDBAccess<DL_Item>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public ItemDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_Item> GetAllWithChildren(bool recursive = true)
        {
            lock (locker)
            {
               return Connection.GetAllWithChildren<DL_Item>(recursive: recursive).OrderBy(x=>x.Itemnumber).ToList();
            }
        }

        public List<DL_Item> Get(bool recursive = true)
        {
            lock (locker)
            {
                var query = $"Select * from DL_Item order by Itemnumber";
                return Connection.Query<DL_Item>(query);
            }
        }

        public DL_Item Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_Item>(id, recursive: recursive);
            }
        }

        internal List<DL_Item> GetByProsperity(int prosperity)
        {
            lock(locker)
            {
                var query = $"Select * from DL_Item where Prosperitylevel = ? order by Itemnumber";
                return Connection.Query<DL_Item>(query,prosperity);
            }
        }

        internal List<DL_Item> GetUnlockableItems()
        {
            lock (locker)
            {
                var query = $"Select * from DL_Item where Prosperitylevel > 9 order by Itemnumber";
                return Connection.Query<DL_Item>(query);
            }
        }

        internal List<DL_Item> GetItemByItemNumber(int itemnumber)
        {
            lock (locker)
            {
                var query = $"Select * from DL_Item where itemnumber = ?";
                return Connection.Query<DL_Item>(query, itemnumber);
            }
        }

        internal List<DL_Item> GetSelectableItems(int prosperity, int campaignId)
        {
            lock (locker)
            {
                var prosperityQuery = $"Select * from DL_Item where Prosperitylevel <= ? order by Itemnumber";
                var items =  Connection.Query<DL_Item>(prosperityQuery, prosperity);

                var unlockedQuery = $"Select i.* from DL_CampaignUnlockedItem ci" +
                                    " Inner join DL_Item i" +
                                    " on i.Id = ci.ID_Item" +
                                    " where ID_Campaign = ? order by Itemnumber";
                items.AddRange(Connection.Query<DL_Item>(unlockedQuery, campaignId));

                return items;
            }
        }

        public void InsertOrReplace(DL_Item item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_Item> items)
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

        internal void Insert(IEnumerable<DL_Item> items)
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

        public void Delete(DL_Item item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }
    }
}