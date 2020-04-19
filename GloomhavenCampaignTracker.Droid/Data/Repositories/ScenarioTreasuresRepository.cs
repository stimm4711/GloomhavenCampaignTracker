using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class ScenarioTreasuresRepository
    {
        private readonly ScenarioTreasureDBAccess _db;
        protected static ScenarioTreasuresRepository Me;

        static ScenarioTreasuresRepository()
        {
            Me = new ScenarioTreasuresRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new ScenarioTreasuresRepository();
        }

        protected ScenarioTreasuresRepository()
        {
            _db = new ScenarioTreasureDBAccess();
        }

        public static DL_ScenarioTreasure Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_ScenarioTreasure> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_ScenarioTreasure> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_ScenarioTreasure item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_ScenarioTreasure> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_ScenarioTreasure item)
        {
            Me._db.Delete(item);
        }
    }
}