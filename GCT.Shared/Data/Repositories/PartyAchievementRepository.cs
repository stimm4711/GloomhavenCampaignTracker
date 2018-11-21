using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class PartyAchievementRepository
    {
        private readonly PartyAchievementDBAccess _db;
        protected static PartyAchievementRepository Me;

        static PartyAchievementRepository()
        {
            Me = new PartyAchievementRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new PartyAchievementRepository();
        }

        protected PartyAchievementRepository()
        {
            _db = new PartyAchievementDBAccess();
        }

        public static DL_PartyAchievement Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_PartyAchievement> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_PartyAchievement> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_PartyAchievement item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_PartyAchievement> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_PartyAchievement item)
        {
            Me._db.Delete(item);
        }
    }
}