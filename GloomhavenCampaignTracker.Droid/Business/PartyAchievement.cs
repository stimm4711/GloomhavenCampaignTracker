using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Business
{ 
    public class PartyAchievement : ISpinneritem
    {
        private DL_PartyAchievement _partyachievementData;

        public PartyAchievement(DL_PartyAchievement partyachievementData)
        {
            _partyachievementData = partyachievementData;
        }

        public DL_PartyAchievement PartyAchievementData => _partyachievementData;

        public string Spinnerdisplayvalue => _partyachievementData.Name;
    }
}