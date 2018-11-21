using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class AchievementTypeRepository
    {
        private readonly AchievementTypeDBAccess _db;
        protected static AchievementTypeRepository Me;

        static AchievementTypeRepository()
        {
            Me = new AchievementTypeRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new AchievementTypeRepository();
        }

        protected AchievementTypeRepository()
        {
            _db = new AchievementTypeDBAccess();
        }

        public static DL_AchievementType Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_AchievementType> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_AchievementType> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_AchievementType item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_AchievementType> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_AchievementType item)
        {
            Me._db.Delete(item);
        }
    }
}