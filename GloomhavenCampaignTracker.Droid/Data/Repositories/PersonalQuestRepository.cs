using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class PersonalQuestRepository
    {
        private readonly PersonalQuestDBAccess _db;
        protected static PersonalQuestRepository Me;

        static PersonalQuestRepository()
        {
            Me = new PersonalQuestRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new PersonalQuestRepository();
        }

        protected PersonalQuestRepository()
        {
            _db = new PersonalQuestDBAccess();
        }

        public static DL_PersonalQuest Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_PersonalQuest> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_PersonalQuest> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        internal static List<DL_PersonalQuest> GetWithNumbers(string PQNumbers, bool reqursive)
        {
            return Me._db.GetWithNumbers(PQNumbers, reqursive);
        }

        public static void InsertOrReplace(DL_PersonalQuest item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_PersonalQuest> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_PersonalQuest item)
        {
            Me._db.Delete(item);
        }
    }
}