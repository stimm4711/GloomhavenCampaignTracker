using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class ItemRepository
    {
        private readonly ItemDBAccess _db;
        protected static ItemRepository Me;

        static ItemRepository()
        {
            Me = new ItemRepository();
        }
        public static bool IsInitilized()
        {
            return Me == null;
        }
        public static void Initialize()
        {
            Me = new ItemRepository();
        }
        protected ItemRepository()
        {
            _db = new ItemDBAccess();
        }

        internal static List<DL_Item> GetByProsperity(int prosperity)
        {
           return Me._db.GetByProsperity(prosperity);
        }

        internal static List<DL_Item> GetUnlockableItems()
        {
            return Me._db.GetUnlockableItems();
        }

        internal static List<DL_Item> GetSelectableItems(int prosperity, int campaignId)
        {
            return Me._db.GetSelectableItems(prosperity, campaignId);
        }

        internal static List<DL_Item> GetItemByItemNumber(int itemnumber)
        {
            return Me._db.GetItemByItemNumber(itemnumber);
        }

        public static DL_Item Get(long id)
        {
            return Me._db.Get(id);
        }

        internal static void Insert(IEnumerable<DL_Item> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_Item> Get()
        {
            return Me._db.Get();
        }

        public static void InsertOrReplace(DL_Item item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_Item> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_Item item)
        {
            Me._db.Delete(item);
        }
    }
}