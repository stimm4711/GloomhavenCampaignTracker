using System;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.unlocks;
using GloomhavenCampaignTracker.Shared;
using GloomhavenCampaignTracker.Business;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class CampaignViewPagerFragmentTabs : Android.Support.V4.App.Fragment
    {
        View _campaignFragmentView;
        TabLayout _tabLayout;
        ViewPager _viewPager;
        CampaignViewPagerAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _campaignFragmentView = inflater.Inflate(Resource.Layout.fragment_campaignViewPagerTabs, container, false);
            _viewPager = _campaignFragmentView.FindViewById<ViewPager>(Resource.Id.viewpager);
            _tabLayout = _campaignFragmentView.FindViewById<TabLayout>(Resource.Id.sliding_tabs);


            ((MainActivity)Activity).SupportActionBar.Title = Resources.GetString(Resource.String.Campaign);

            if (GCTContext.CurrentCampaign != null)
            {
                ((MainActivity)Activity).SupportActionBar.Title = GCTContext.CampaignCollection.CurrentCampaign.CampaignData.Name;
            }

            SetTabLayout();

            _viewPager.PageSelected += _viewPager_PageSelected;

            if (GCTContext.LastSelectedCampaignTab > -1)
            {
                _viewPager.CurrentItem = GCTContext.LastSelectedCampaignTab;
            }

            HasOptionsMenu = true;

            return _campaignFragmentView;
        }

        private void SetTabLayout()
        {
            _adapter = new CampaignViewPagerAdapter(Context, ChildFragmentManager);
            _viewPager.Adapter = _adapter;
            _tabLayout.SetupWithViewPager(_viewPager);
            // Iterate over all tabs and set the custom view
            for (var i = 0; i < _tabLayout.TabCount; i++)
            {
                var tab = _tabLayout.GetTabAt(i);
                tab.SetCustomView(_adapter.GetTabView(i));
            }
        }

        private void _viewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            GCTContext.LastSelectedCampaignTab = e.Position;
            if (e.Position == 3)
            {
                if (_adapter.GetFragment(e.Position) == null) return;

                ((CampaignUnlocksFragment)_adapter.GetFragment(e.Position)).SwitchViewStates();
            }

            _adapter.NotifyDataSetChanged();
            UpdateTabViews();
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.campaignMenu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            try
            {
                if (item.ItemId == Resource.Id.campaignSelection)
                {
                    var intent = new Intent(Activity, typeof(DetailsActivity));
                    intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.CampaignSelection);
                    StartActivity(intent);
                }
                else if (item.ItemId == Resource.Id.partySelection)
                {
                    var intent = new Intent(Activity, typeof(DetailsActivity));
                    intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.PartySelection);
                    StartActivity(intent);
                }
                else if (item.ItemId == Resource.Id.editcampaignname)
                {
                    var convertView = LayoutInflater.Inflate(Resource.Layout.alertdialog_editcampaignname, null);
                    var nameEdit = convertView.FindViewById<TextInputEditText>(Resource.Id.camnpaignname);

                    nameEdit.Text = $"{GCTContext.CurrentCampaign.CampaignData.Name}";

                    new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                        .SetCustomView(convertView)
                        .SetTitle("Edit campaignname")
                        .SetNegativeButton(Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                        .SetPositiveButton(Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                        {
                            if (string.IsNullOrEmpty(nameEdit.Text)) return;
                            GCTContext.CurrentCampaign.CampaignData.Name = nameEdit.Text;

                            GCTContext.CurrentCampaign.Save(recursive: false);
                            ((MainActivity)Activity).SupportActionBar.Title = GCTContext.CampaignCollection.CurrentCampaign.CampaignData.Name;
                        })
                        .Show();
                }
            }
            catch
            {
                return base.OnOptionsItemSelected(item);
            }            

            return base.OnOptionsItemSelected(item);
        }

        internal void UpdateAdapter()
        {
            SetTabLayout();
        }

        internal void SetPartyPage()
        {
            _viewPager.SetCurrentItem(2, true); 
        }

        private void UpdateTabViews()
        {
            // Iterate over all tabs and set the custom view
            for (var i = 0; i < _tabLayout.TabCount; i++)
            {
                var tab = _tabLayout.GetTabAt(i);
                tab.SetCustomView(_adapter.GetTabView(i));
            }
        }
    }
}