using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class AbilityRepository 
    {
        private readonly AbilityDBAccess _db;
        protected static AbilityRepository Me;

        static AbilityRepository()
        {
            Me = new AbilityRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new AbilityRepository();
        }

        protected AbilityRepository()
        {
            _db = new AbilityDBAccess();
        }

        public static DL_Ability Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_Ability> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_Ability> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_Ability item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_Ability> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_Ability item)
        {
            Me._db.Delete(item);
        }
    }
}