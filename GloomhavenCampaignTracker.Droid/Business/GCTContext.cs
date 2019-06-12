using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Business
{
    public class GCTContext
    {
        private static CampaignCollection m_campaignCollection;
        private static ScenarioCollection m_scenarioCollection;
        private static AchievementCollection m_achievementCollection;

        public static CampaignCollection CampaignCollection => m_campaignCollection ?? (m_campaignCollection = new CampaignCollection());

        public static ScenarioCollection ScenarioCollection => m_scenarioCollection ?? (m_scenarioCollection = new ScenarioCollection());

        public static AchievementCollection AchievementCollectiom => m_achievementCollection ?? (m_achievementCollection = new AchievementCollection());

        public static List<DL_Character> CharacterCollection { get; set; } = new List<DL_Character>();

        public static Campaign CurrentCampaign
        {
            get => CampaignCollection.CurrentCampaign;
        }

        public static bool ShowItemNames { get; set; } = true;

        public static bool ShowPersonalQuestDetails { get; set; } = true;

        public static bool ShowScenarioNames { get; set; } = false;

        public static int LastSelectedCampaignTab { get; set; } = -1;
        public static bool ShowOldPerkSheet { get; internal set; }

        public static bool ActivateForgottenCiclesContent { get; set; } = false;

        public static void Clear()
        {
            m_campaignCollection = null;
            m_achievementCollection = null;
            m_scenarioCollection = null;
        }       
    }
}