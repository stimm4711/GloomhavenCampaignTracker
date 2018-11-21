using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class AchievementRepository
    {
        private readonly AchievementDBAccess _db;
        protected static AchievementRepository Me;

        static AchievementRepository()
        {
            Me = new AchievementRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new AchievementRepository();
        }

        protected AchievementRepository()
        {
            _db = new AchievementDBAccess();
        }

        public static DL_Achievement Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_Achievement> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_Achievement> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_Achievement item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_Achievement> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_Achievement item)
        {
            Me._db.Delete(item);
        }
    }
}