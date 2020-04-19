using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Business
{
    public class GCTContext
    {
        private static CampaignCollection m_campaignCollection;
        private static ScenarioCollection m_scenarioCollection;
        private static AchievementCollection m_achievementCollection;
        private static Droid.Settings m_settings;

        public static Droid.Settings Settings => m_settings ?? (m_settings = new Droid.Settings());

        public static CampaignCollection CampaignCollection => m_campaignCollection ?? (m_campaignCollection = new CampaignCollection());

        public static ScenarioCollection ScenarioCollection => m_scenarioCollection ?? (m_scenarioCollection = new ScenarioCollection());

        public static AchievementCollection AchievementCollectiom => m_achievementCollection ?? (m_achievementCollection = new AchievementCollection());

        public static List<DL_Character> CharacterCollection { get; set; } = new List<DL_Character>();

        public static Campaign CurrentCampaign
        {
            get => CampaignCollection.CurrentCampaign;
        }        

        public static int LastSelectedCampaignTab { get; set; } = -1;      

        public static void Clear()
        {
            m_campaignCollection = null;
            m_achievementCollection = null;
            m_scenarioCollection = null;
        }       
    }
}