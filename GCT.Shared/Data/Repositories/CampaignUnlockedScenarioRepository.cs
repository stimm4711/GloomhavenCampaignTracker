using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class CampaignUnlockedScenarioRepository
    {
        private readonly CampaignUnlockedScenariosDBAccess _db;
        protected static CampaignUnlockedScenarioRepository Me;

        static CampaignUnlockedScenarioRepository()
        {
            Me = new CampaignUnlockedScenarioRepository();
        }

        public static bool IsInitilized()
        {
            return Me == null;
        }

        public static void Initialize()
        {
            Me = new CampaignUnlockedScenarioRepository();
        }

        protected CampaignUnlockedScenarioRepository()
        {
            _db = new CampaignUnlockedScenariosDBAccess();
        }

        public static DL_CampaignUnlockedScenario Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_CampaignUnlockedScenario> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_CampaignUnlockedScenario> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_CampaignUnlockedScenario item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_CampaignUnlockedScenario> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_CampaignUnlockedScenario item)
        {
            Me._db.Delete(item);
        }

        internal static DL_CampaignUnlockedScenario GetUnlockedScenario(int scenarioNumber, int campaignId)
        {
           return Me._db.GetUnlockedScenario(scenarioNumber, campaignId);
        }

        internal static IEnumerable<DL_CampaignUnlockedScenario> GetUnlockedScenariosOfCampaign(int campaignId)
        {
            return Me._db.GetUnlockedScenariosOfCampaign(campaignId);
        }
    }
}