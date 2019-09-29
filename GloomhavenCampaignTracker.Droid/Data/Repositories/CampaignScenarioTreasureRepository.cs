using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class CampaignScenarioTreasureRepository
    {
        private readonly CampaignScenarioTreasureDBAccess _db;
        protected static CampaignScenarioTreasureRepository Me;

        static CampaignScenarioTreasureRepository()
        {
            Me = new CampaignScenarioTreasureRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new CampaignScenarioTreasureRepository();
        }

        protected CampaignScenarioTreasureRepository()
        {
            _db = new CampaignScenarioTreasureDBAccess();
        }

        public static DL_CampaignScenarioTreasure Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_CampaignScenarioTreasure> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_CampaignScenarioTreasure> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_CampaignScenarioTreasure item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_CampaignScenarioTreasure> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_CampaignScenarioTreasure item)
        {
            Me._db.Delete(item);
        }
    }
}