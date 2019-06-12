using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class CharacterPersonalQuestCountersRepository
    {
        private readonly CharacterPersonalQuestCounterDBAccess _db;
        protected static CharacterPersonalQuestCountersRepository Me;

        static CharacterPersonalQuestCountersRepository()
        {
            Me = new CharacterPersonalQuestCountersRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new CharacterPersonalQuestCountersRepository();
        }

        protected CharacterPersonalQuestCountersRepository()
        {
            _db = new CharacterPersonalQuestCounterDBAccess();
        }

        public static DL_CharacterPersonalQuestCounter Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_CharacterPersonalQuestCounter> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_CharacterPersonalQuestCounter> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_CharacterPersonalQuestCounter item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_CharacterPersonalQuestCounter> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_CharacterPersonalQuestCounter item)
        {
            Me._db.Delete(item);
        }
    }
}