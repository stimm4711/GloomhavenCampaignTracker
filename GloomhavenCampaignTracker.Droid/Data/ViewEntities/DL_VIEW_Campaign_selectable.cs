namespace Data.ViewEntities
{
    public class DL_VIEW_Campaign_selectable : Java.Lang.Object
    {
        private DL_VIEW_Campaign _campaignView;

        public DL_VIEW_Campaign_selectable(DL_VIEW_Campaign campaignView)
        {
            _campaignView = campaignView;
        }

        public int CampaignId
        {
            get { return _campaignView.CampaignId; }
        }

        public string Campaignname 
        {
            get { return _campaignView.Campaignname; }
        }

        public int ScenarioCount
        {
            get { return _campaignView.ScenarioCount; }
        }

        public int GlobalAchievementCount
        {
            get { return _campaignView.GlobalAchievementCount; }
        }

        public bool IsSelected { get; set; }
    }
}
