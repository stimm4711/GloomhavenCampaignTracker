using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class DataServiceCollection
    {
        public static ItemDataService ItemDataService { get; set; } = new ItemDataService();
        public static CharacterDataService CharacterDataService { get; set; } = new CharacterDataService();
        public static ICampaignDataService CampaignDataService { get; set; } = new CampaignDataService();
        public static ScenarioDataService ScenarioDataService { get; set; } = new ScenarioDataService();
        public static PartyDataService PartyDataService { get; set; } = new PartyDataService();
        public static ClassAbilityDataService ClassAbilityDataService { get; set; } = new ClassAbilityDataService();

        public static void Clear()
        {
            CampaignDataService = new CampaignDataService();
            CharacterDataService = new CharacterDataService();
            ItemDataService = new ItemDataService();
            ScenarioDataService = new ScenarioDataService();
            PartyDataService = new PartyDataService();
            ClassAbilityDataService = new ClassAbilityDataService();
        }
    }
}