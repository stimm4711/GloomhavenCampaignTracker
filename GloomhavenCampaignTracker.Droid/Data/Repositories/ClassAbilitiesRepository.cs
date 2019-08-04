using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign;
using System;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class ClassAbilitiesRepository
    {
        private readonly ClassAbilitiesDBAccess _db;
        protected static ClassAbilitiesRepository Me;

        static ClassAbilitiesRepository()
        {
            Me = new ClassAbilitiesRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new ClassAbilitiesRepository();
        }

        protected ClassAbilitiesRepository()
        {
            _db = new ClassAbilitiesDBAccess();
        }

        public static DL_ClassAbility Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static List<DL_ClassAbility> GetSelectableAbilities(int iD_class, int characterlevel)
        {
            return Me._db.GetSelectable(iD_class, characterlevel);
        }

        internal static void Insert(IEnumerable<DL_ClassAbility> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_ClassAbility> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_ClassAbility item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_ClassAbility> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_ClassAbility item)
        {
            Me._db.Delete(item);
        }
    }
}