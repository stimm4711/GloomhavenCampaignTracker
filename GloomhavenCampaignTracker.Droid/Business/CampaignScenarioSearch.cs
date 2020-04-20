namespace GloomhavenCampaignTracker.Business
{
    public class CampaignScenarioSearch
    {
        public static bool FindAvailable(CampaignUnlockedScenario cus)
        {
            return cus.IsAvailable();
        }

        public static bool FindBlocked(CampaignUnlockedScenario cus)
        {
            return cus.IsBlocked() && !cus.Completed;
        }

        public static bool FindUnAvailable(CampaignUnlockedScenario cus)
        {
            return !cus.IsAvailable() && !cus.IsBlocked() && !cus.Completed;
        }
        public static bool FindCompleted(CampaignUnlockedScenario cus)
        {
            return cus.Completed;
        }


    }
}