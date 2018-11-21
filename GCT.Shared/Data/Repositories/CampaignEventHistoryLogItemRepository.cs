using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class CampaignEventHistoryLogItemRepository
    {
        private readonly CampaignEventHistoryLogItemDBAccess _db;
        protected static CampaignEventHistoryLogItemRepository Me;

        static CampaignEventHistoryLogItemRepository()
        {
            Me = new CampaignEventHistoryLogItemRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new CampaignEventHistoryLogItemRepository();
        }

        protected CampaignEventHistoryLogItemRepository()
        {
            _db = new CampaignEventHistoryLogItemDBAccess();
        }

        public static DL_CampaignEventHistoryLogItem Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_CampaignEventHistoryLogItem> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_CampaignEventHistoryLogItem> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_CampaignEventHistoryLogItem item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_CampaignEventHistoryLogItem> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_CampaignEventHistoryLogItem item)
        {
            Me._db.Delete(item);
        }
    }
}