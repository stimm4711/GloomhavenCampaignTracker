using System;
using System.Collections.Generic;
using System.Linq;
using Data.ViewEntities;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class CampaignDataService : ICampaignDataService
    {
        public void Delete(DL_Campaign item)
        {
            CampaignRepository.Delete(item);
        }

        public DL_Campaign Get(long id, bool recursive = true)
        {
            return CampaignRepository.Get(id, recursive);
        }

        public List<DL_Campaign> Get()
        {
            return CampaignRepository.Get();
        }

        public void InsertOrReplace(DL_Campaign item, bool recursive)
        {
            CampaignRepository.InsertOrReplace(item, recursive);
        }

        public List<DL_Item> GetUnlockableItems(int campaignId)
        {
            return CampaignRepository.GetUnlockableItems(campaignId);
        }

        public List<DL_Scenario> GetUnlockedScenarios(int campaignId)
        {
            return CampaignRepository.GetUnlockedScenarios(campaignId);
        }

        public List<DL_Campaign> GetCampaignsFlat()
        {
            return CampaignRepository.GetCampaignsFlat();
        }

        public List<DL_CampaignUnlockedScenario> GetUnlockedCampaignScenarios(int campaignId)
        {
            return CampaignRepository.GetUnlockedCampaignScenarios(campaignId);
        }

        public List<DL_VIEW_CampaignParties> GetParties(int campaignId)
        {
            return CampaignRepository.GetParties(campaignId);
        }

        public List<DL_VIEW_Campaign> GetCampaigns()
        {
            return CampaignRepository.GetCampaigns();
        }

        public List<DL_Character> GetRetiredCharacters(int campaignId)
        {
            return CampaignRepository.GetRetiredCharacters(campaignId);
        }

        public List<DL_AchievementType> GetAchievementTypesFlat()
        {
            return CampaignRepository.GetAchievementTypes();
        }

        public void SaveUnlockedScenarioData(DL_CampaignUnlockedScenario unlockedScenarioData)
        {
            CampaignUnlockedScenarioRepository.InsertOrReplace(unlockedScenarioData);
        }
    }
}