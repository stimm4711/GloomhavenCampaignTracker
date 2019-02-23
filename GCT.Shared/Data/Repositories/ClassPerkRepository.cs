using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class ClassPerkRepository
    {
        private readonly ClassPerkDBAccess _db;
        protected static ClassPerkRepository Me;

        static ClassPerkRepository()
        {
            Me = new ClassPerkRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new ClassPerkRepository();
        }

        protected ClassPerkRepository()
        {
            _db = new ClassPerkDBAccess();
        }

        public static DL_ClassPerk Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_ClassPerk> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_ClassPerk> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static List<DL_ClassPerk> GetClassPerks(int classid)
        {
            return Me._db.GetClassPerks(classid);
        }

        public static void InsertOrReplace(DL_ClassPerk item)
        {
            Me._db.InsertOrReplace(item);
        }

        public static void UpdatePerkText(DL_ClassPerk item)
        {
            Me._db.UpdatePerkText(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_ClassPerk> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_ClassPerk item)
        {
            Me._db.Delete(item);
        }
    }
}