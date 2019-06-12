using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class CampaignPartyRepository
    {
        private readonly CampaignPartyDBAccess _db;
        protected static CampaignPartyRepository Me;

        static CampaignPartyRepository()
        {
            Me = new CampaignPartyRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new CampaignPartyRepository();
        }

        protected CampaignPartyRepository()
        {
            _db = new CampaignPartyDBAccess();
        }

        public static DL_CampaignParty Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_CampaignParty> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_CampaignParty> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_CampaignParty item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_CampaignParty> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_CampaignParty item)
        {
            Me._db.Delete(item);
        }
    }
}