using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class CharacterAbilitiesRepository
    {
        private readonly CharacterAbilitiesDBAccess _db;
        protected static CharacterAbilitiesRepository Me;

        static CharacterAbilitiesRepository()
        {
            Me = new CharacterAbilitiesRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new CharacterAbilitiesRepository();
        }

        protected CharacterAbilitiesRepository()
        {
            _db = new CharacterAbilitiesDBAccess();
        }

        public static DL_CharacterAbility Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_CharacterAbility> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_CharacterAbility> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_CharacterAbility item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_CharacterAbility> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_CharacterAbility item)
        {
            Me._db.Delete(item);
        }
    }
}