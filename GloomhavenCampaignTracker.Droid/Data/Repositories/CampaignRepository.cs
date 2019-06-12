using Data.ViewEntities;
using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class CampaignRepository
    {
        private readonly CampaignDBAccess _db;
        protected static CampaignRepository Me;

        static CampaignRepository()
        {
            Me = new CampaignRepository();
        }
        public static bool IsInitilized()
        {
            return Me == null;
        }
        public static void Initialize()
        {
            Me = new CampaignRepository();
        }
        protected CampaignRepository()
        {
            _db = new CampaignDBAccess();
        }
       
        public static DL_Campaign Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static void Insert(IEnumerable<DL_Campaign> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_Campaign> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_Campaign item, bool recursive = true)
        {
            Me._db.InsertOrReplace(item, recursive);
        }

        internal static List<DL_Campaign> GetCampaignsFlat()
        {
            return Me._db.GetCampaignsFlat();
        }

        internal static void InsertOrReplace(IEnumerable<DL_Campaign> items)
        {
            Me._db.InsertOrReplace(items);
        }

        internal static List<DL_VIEW_CampaignParties> GetParties(int campaignId)
        {
            return Me._db.GetParties(campaignId);
        }

        internal static List<DL_VIEW_Campaign> GetCampaigns()
        {
            return Me._db.GetCampaigns();
        }

        internal static List<DL_Character> GetRetiredCharacters(int campaignId)
        {
            return Me._db.GetRetiredCharacters(campaignId);
        }

        public static void Delete(DL_Campaign item)
        {
            Me._db.Delete(item);
        }

        internal static List<DL_Item> GetUnlockableItems(int campaignId)
        {
            return Me._db.GetUnlockableItems(campaignId);
        }

        internal static List<DL_Scenario> GetUnlockedScenarios(int campaignId)
        {
            return Me._db.GetUnlockedScenarios(campaignId);
        }

        internal static List<DL_CampaignUnlockedScenario> GetUnlockedCampaignScenarios(int campaignId)
        {
            return Me._db.GetUnlockedCampaignScenarios(campaignId);
        }

        public static List<DL_AchievementType> GetAchievementTypes()
        {
            return Me._db.GetAchievementTypes();            
        }
    }
}