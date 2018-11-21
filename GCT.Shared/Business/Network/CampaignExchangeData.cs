using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace Business.Network
{
    public class CampaignExchangeData
    {
        public DL_Campaign Campaign { get; set; }

        public IEnumerable<DL_CampaignParty> CampaignParties { get; set; }

        public IEnumerable<DL_CampaignPartyAchievement> PartyAchievements { get; set; }

        public IEnumerable<DL_CampaignGlobalAchievement> CampaignGlobalAchievements { get; set; }

        public DL_CampaignUnlocks CampaignUnlocks { get; set; }

        public IEnumerable<DL_CampaignEventHistoryLogItem> CampaignEventHistoryLogItems { get; set; }

        public IEnumerable<DL_CampaignUnlockedItem> CampaignUnlockedItems { get; set; }

        public IEnumerable<DL_CampaignUnlockedScenario> CampaignUnlockedScenarios { get; set; }

        public IEnumerable<DL_Treasure> ScenarioTreasures { get; set; }

        public IEnumerable<DL_Character> Characters { get; set; }

        public IEnumerable<DL_Ability> CharacterAbilities { get; set; }

        public IEnumerable<DL_CharacterItem> CharacterItems { get; set; }

        public IEnumerable<DL_CharacterPerk> CharacterPerks { get; set; }

        public IEnumerable<DL_Perk> Perks { get; set; }

        public IEnumerable<DL_AbilityEnhancement> AbilityEnhancements { get; set; }
    }

    public class CityExchangeData
    {
        public int Prosperity { get; set; }

        public int Donations { get; set; }
    }
}
