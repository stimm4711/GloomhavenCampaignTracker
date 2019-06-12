using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class ClassRepository
    {
        private readonly ClassDBAccess _db;
        protected static ClassRepository Me;

        static ClassRepository()
        {
            Me = new ClassRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new ClassRepository();
        }

        protected ClassRepository()
        {
            _db = new ClassDBAccess();
        }

        public static DL_Class Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_Class> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_Class> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_Class item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_Class> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_Class item)
        {
            Me._db.Delete(item);
        }
    }
}