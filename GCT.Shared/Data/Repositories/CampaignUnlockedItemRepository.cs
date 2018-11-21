using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class CampaignUnlockedItemRepository
    {
        private readonly CampaignUnlockedItemsDBAccess _db;
        protected static CampaignUnlockedItemRepository Me;

        static CampaignUnlockedItemRepository()
        {
            Me = new CampaignUnlockedItemRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new CampaignUnlockedItemRepository();
        }

        protected CampaignUnlockedItemRepository()
        {
            _db = new CampaignUnlockedItemsDBAccess();
        }

        public static DL_CampaignUnlockedItem Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_CampaignUnlockedItem> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_CampaignUnlockedItem> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_CampaignUnlockedItem item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_CampaignUnlockedItem> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_CampaignUnlockedItem item)
        {
            Me._db.Delete(item);
        }
    }
}