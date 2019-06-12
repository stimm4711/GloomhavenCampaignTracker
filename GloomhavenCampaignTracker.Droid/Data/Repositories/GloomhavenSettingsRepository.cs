using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class GloomhavenSettingsRepository
    {
        private readonly GloomhavenSettingsDBAccess _db;
        protected static GloomhavenSettingsRepository Me;

        static GloomhavenSettingsRepository()
        {
            Me = new GloomhavenSettingsRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new GloomhavenSettingsRepository();
        }

        protected GloomhavenSettingsRepository()
        {
            _db = new GloomhavenSettingsDBAccess();
        }

        public static DL_GlommhavenSettings Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_GlommhavenSettings> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_GlommhavenSettings> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_GlommhavenSettings item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_GlommhavenSettings> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_GlommhavenSettings item)
        {
            Me._db.Delete(item);
        }
    }
}