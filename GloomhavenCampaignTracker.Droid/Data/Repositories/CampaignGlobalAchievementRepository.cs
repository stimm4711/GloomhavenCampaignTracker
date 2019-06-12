using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class CampaignGlobalAchievementRepository
    {
        private readonly CampaignGlobalAchievementDBAccess _db;
        protected static CampaignGlobalAchievementRepository Me;

        static CampaignGlobalAchievementRepository()
        {
            Me = new CampaignGlobalAchievementRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new CampaignGlobalAchievementRepository();
        }

        protected CampaignGlobalAchievementRepository()
        {
            _db = new CampaignGlobalAchievementDBAccess();
        }

        public static DL_CampaignGlobalAchievement Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_CampaignGlobalAchievement> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_CampaignGlobalAchievement> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_CampaignGlobalAchievement item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_CampaignGlobalAchievement> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_CampaignGlobalAchievement item)
        {
            Me._db.Delete(item);
        }
    }
}