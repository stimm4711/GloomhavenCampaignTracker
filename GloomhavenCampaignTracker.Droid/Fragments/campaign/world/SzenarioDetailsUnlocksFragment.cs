using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class SzenarioDetailsUnlocksFragment : CampaignDetailsFragmentBase
    {
        private static readonly string scenarioID = "scenarioID";

        internal static SzenarioDetailsUnlocksFragment NewInstance(DL_CampaignUnlockedScenario scenario)
        {
            var frag = new SzenarioDetailsUnlocksFragment { Arguments = new Bundle() };
            frag.Arguments.PutInt(scenarioID, scenario.Id);
            return frag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_scenariodetails_unlocks, container, false);
                       
            //_view.FindViewById<ListView>(Resource.Id.lv_requirements);        
            

            return _view;
        }
       
    }
}