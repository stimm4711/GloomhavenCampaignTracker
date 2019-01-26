namespace GloomhavenCampaignTracker.Business
{
    public class CampaignScenarioSearch
    {
        public bool FindAvailable(CampaignUnlockedScenario cus)
        {
            return cus.IsAvailable();
        }

        public bool FindBlocked(CampaignUnlockedScenario cus)
        {
            return cus.IsBlocked() && !cus.Completed;
        }

        public bool FindUnAvailable(CampaignUnlockedScenario cus)
        {
            return !cus.IsAvailable() && !cus.IsBlocked() && !cus.Completed;
        }
        public bool FindCompleted(CampaignUnlockedScenario cus)
        {
            return cus.Completed;
        }


    }
}