using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class EnhancementRepository
    {
        private readonly EnhancementDBAccess _db;
        protected static EnhancementRepository Me;

        static EnhancementRepository()
        {
            Me = new EnhancementRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new EnhancementRepository();
        }

        protected EnhancementRepository()
        {
            _db = new EnhancementDBAccess();
        }

        public static DL_Enhancement Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_Enhancement> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_Enhancement> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_Enhancement item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_Enhancement> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_Enhancement item)
        {
            Me._db.Delete(item);
        }
    }
}