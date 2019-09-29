using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class ScenarioDetailsViewPagerTabs : Android.Support.V4.App.Fragment
    {
        private View _view;
        private TabLayout _tabLayout;
        private ViewPager _viewPager;
        private ScenarioDetailsViewPagerAdapter _adapter;

        public CampaignUnlockedScenario Scenario;

        internal static ScenarioDetailsViewPagerTabs NewInstance(int CampaigncenarioId)
        {
            var frag = new ScenarioDetailsViewPagerTabs { Arguments = new Bundle() };
            frag.Arguments.PutInt(DetailsActivity.SelectedScenarioId, CampaigncenarioId);
            return frag;
        }

        private CampaignUnlockedScenario GetUnlockedScenario()
        {
            if (Arguments == null) return null;

            var id = Arguments.GetInt(DetailsActivity.SelectedScenarioId, 0);
            if (id <= 0) return null;

            var campScenario = CampaignUnlockedScenarioRepository.Get(id, recursive: true);

            return new CampaignUnlockedScenario(campScenario);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_scenariodetails, container, false);
            _viewPager = _view.FindViewById<ViewPager>(Resource.Id.scenariodetailsviewpager);
            _tabLayout = _view.FindViewById<TabLayout>(Resource.Id.scenariodetails_tabs);

            Scenario = GetUnlockedScenario();

            SetTabLayout();

            return _view;
        }



        private void SetTabLayout()
        {
            _adapter = new ScenarioDetailsViewPagerAdapter(Context, ChildFragmentManager, Scenario.UnlockedScenarioData);
            _viewPager.Adapter = _adapter;
            _tabLayout.SetupWithViewPager(_viewPager);
            // Iterate over all tabs and set the custom view
            for (var i = 0; i < _tabLayout.TabCount; i++)
            {
                var tab = _tabLayout.GetTabAt(i);
                tab.SetCustomView(_adapter.GetTabView(i));
            }
        }
       
        public override void OnStop()
        {
            if (Scenario != null)
            {
                
            }

            base.OnStop();
        }      

    }
}