using System.Collections.Generic;
using Data.ViewEntities;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public interface ICampaignDataService
    {
        DL_Campaign Get(long id, bool recursive = true);

        List<DL_Campaign> Get();

        void InsertOrReplace(DL_Campaign item, bool recursive);

        void Delete(DL_Campaign item);

        List<DL_Item> GetUnlockableItems(int campaignId);

        List<DL_Scenario> GetUnlockedScenarios(int campaignId);

        List<DL_Campaign> GetCampaignsFlat();

        List<DL_CampaignUnlockedScenario> GetUnlockedCampaignScenarios(int campaignId);

        List<DL_VIEW_CampaignParties> GetParties(int campaignId);

        List<DL_Character> GetRetiredCharacters(int campaignId);

        List<DL_AchievementType> GetAchievementTypesFlat();

        List<DL_VIEW_Campaign> GetCampaigns();

        void SaveUnlockedScenarioData(DL_CampaignUnlockedScenario unlockedScenarioData);
    }
}