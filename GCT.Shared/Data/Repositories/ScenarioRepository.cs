using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class ScenarioRepository
    {
        private readonly ScenarioDBAccess _db;
        protected static ScenarioRepository Me;

        static ScenarioRepository()
        {
            Me = new ScenarioRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new ScenarioRepository();
        }

        protected ScenarioRepository()
        {
            _db = new ScenarioDBAccess();
        }

        public static DL_Scenario Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static DL_Scenario GetScenarioByScenarioNumber(int number)
        {
            return Me._db.GetScenarioByScenarioNumber(number);
        }

        internal static void Insert(IEnumerable<DL_Scenario> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_Scenario> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_Scenario item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_Scenario> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_Scenario item)
        {
            Me._db.Delete(item);
        }

        public static void UpdateScenarioRegion(int scenarioID, int regionId)
        {
            Me._db.UpdateScenarioRegion(scenarioID, regionId);
        }

        internal static void UpdateScenarioUnlockedScenarios(int scenarioNumber, string commaSeparatedScenarioNumbers)
        {
            Me._db.UpdateScenarioUnlockedScenarios(scenarioNumber, commaSeparatedScenarioNumbers);
        }
    }
}