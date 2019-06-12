using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class PersonalQuestCountersRepository
    {
        private readonly PersonalQuestCounterDBAccess _db;
        protected static PersonalQuestCountersRepository Me;

        static PersonalQuestCountersRepository()
        {
            Me = new PersonalQuestCountersRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new PersonalQuestCountersRepository();
        }

        protected PersonalQuestCountersRepository()
        {
            _db = new PersonalQuestCounterDBAccess();
        }

        public static DL_PersonalQuestCounter Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_PersonalQuestCounter> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_PersonalQuestCounter> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_PersonalQuestCounter item, bool recursive = true)
        {
            Me._db.InsertOrReplace(item, recursive);
        }

        internal static void InsertOrReplace(IEnumerable<DL_PersonalQuestCounter> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_PersonalQuestCounter item)
        {
            Me._db.Delete(item);
        }
    }
}