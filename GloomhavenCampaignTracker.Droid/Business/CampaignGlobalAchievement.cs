using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Business
{
    public class CampaignGlobalAchievement
    {
        public CampaignGlobalAchievement(DL_CampaignGlobalAchievement campaignGlobalAchievementTypeData)
        {
            GlobalAchievement = campaignGlobalAchievementTypeData;
        }

        public int AchievementEnumNumber
        {
            get
            {
                var achNumber = AchievementType.InternalNumber;
                if (AchievementType.Achievements != null && AchievementType.Achievements.Count > 0)
                {
                    achNumber += Achievement.InternalNumber;
                }
                else if (AchievementType.Steps > 1)
                {
                    achNumber += GlobalAchievement.Step;
                }

                return  achNumber;
            }
        }

        public DL_CampaignGlobalAchievement GlobalAchievement { get; }

        public DL_AchievementType AchievementType => GlobalAchievement.AchievementType;

        public DL_Achievement Achievement
        {
            get { return GlobalAchievement.Achievement; }
            set { GlobalAchievement.Achievement = value; }
        }

        public DL_Campaign  Campaign => GlobalAchievement.Campaign;

        public int Step
        {
            get { return GlobalAchievement.Step; }
            set { GlobalAchievement.Step = value; }
        }

        public int ID_Achievement
        {
            get { return GlobalAchievement.ID_Achievement; }
            set { GlobalAchievement.ID_Achievement = value; }
        }

        public void Save()
        {
            CampaignGlobalAchievementRepository.InsertOrReplace(GlobalAchievement);
        }

        public void DecreaseAchievementSteps()
        {
            if (Step <= 1) return;
            Step--;
            Save();
        }

        public void IncreaseAchievementSteps()
        {
            if (Step >= AchievementType.Steps) return;
            Step++;
            Save();            
        }

        public void SetAchievement(DL_Achievement ach)
        {
            ID_Achievement = ach.Id;
            Achievement = ach;
            Save();
        }
    }

   
}