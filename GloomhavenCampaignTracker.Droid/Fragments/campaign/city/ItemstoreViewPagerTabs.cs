using System.Threading.Tasks;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using GloomhavenCampaignTracker.Business.Network;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.Fragments.campaign;

namespace GloomhavenCampaignTracker.Droid.Fragments.character
{
    public class ItemstoreViewPagerTabs : CampaignDetailsFragmentBase
    {
        private TabLayout _tabLayout;
        private ViewPager _viewPager;
        private CampaignItemsViewPagerAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_itemstore_tabbed, container, false);
            _viewPager = _view.FindViewById<ViewPager>(Resource.Id.itemstoresviewpager);
            _tabLayout = _view.FindViewById<TabLayout>(Resource.Id.itemstore_tabs);

            UpdateData();           

            return _view;
        }

        private void UpdateData()
        {
            if (GloomhavenClient.IsClientRunning())
            {
                Task.Run(async () =>
                {
                    await GloomhavenClient.Instance.UpdateCampaignItemStore();

                    Activity.RunOnUiThread(() =>
                    {
                        SetData();
                    });
                });
            }
            else
            {
                SetData();
            }
        }

        private void SetData()
        {
            if (Campaign != null)
            {
                _adapter = new CampaignItemsViewPagerAdapter(Context, ChildFragmentManager, Campaign);
            }

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
            _dataChanged = false;
            base.OnStop();
        }
    }
}