using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class CampaignUnlocksRepository
    {
        private readonly CampaignUnlocksDBAccess _db;
        protected static CampaignUnlocksRepository Me;

        static CampaignUnlocksRepository()
        {
            Me = new CampaignUnlocksRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new CampaignUnlocksRepository();
        }

        protected CampaignUnlocksRepository()
        {
            _db = new CampaignUnlocksDBAccess();
        }

        public static DL_CampaignUnlocks Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_CampaignUnlocks> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_CampaignUnlocks> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_CampaignUnlocks item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_CampaignUnlocks> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_CampaignUnlocks item)
        {
            Me._db.Delete(item);
        }
    }
}