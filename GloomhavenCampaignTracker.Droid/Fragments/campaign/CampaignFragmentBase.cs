using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.party;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.world;
using GloomhavenCampaignTracker.Droid.Fragments.character;
using GloomhavenCampaignTracker.Shared;
using GloomhavenCampaignTracker.Shared.Business;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public partial class CampaignFragmentBase : Fragment
    {
        protected View _view;
        protected LayoutInflater _inflater;
        protected bool _isDualPane;
        protected FragmentManager _fragmentManager;
        protected bool _dataChanged = false;
        protected View _dualtdetailLayout;

        public CampaignFragmentBase() {}

        public CampaignFragmentBase( FragmentManager fm)
        {
            _fragmentManager = fm;
        }

        protected Campaign CurrentCampaign => GCTContext.CampaignCollection.CurrentCampaign;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {          
            _inflater = inflater;          

            return _view;
        }

        public override void OnStop()
        {
            if (_dataChanged)
            {
                if (GCTContext.CurrentCampaign != null) GCTContext.CurrentCampaign.Save();
            }
            _dataChanged = false;
            base.OnStop();
        }

        protected Fragment ShowDetail(DetailFragmentTypes fragType, int characterId = -1)
        {
            if (_isDualPane)
            {
                FragmentTransaction fragTrans = null;
                Fragment detailsFrag = null ;
                var title = _view.FindViewById<TextView>(Resource.Id.dualdetailtitle);
                switch (fragType)
                {
                    case DetailFragmentTypes.GlobalAchievements:
                        detailsFrag = new CampaignWorldGlobalAchievementsFragment();
                        title.Text = "Global Achievements";
                        _dualtdetailLayout.Visibility = ViewStates.Visible;
                        fragTrans = _fragmentManager.BeginTransaction().Replace(Resource.Id.frame_details_world, detailsFrag);
                        fragTrans.Commit();
                        break;
                    case DetailFragmentTypes.UnlockedScenarios:
                        detailsFrag = new CampaignWorldUnlockedScenariosFragmentExpandListView();
                        title.Text = "Scenarios";
                        _dualtdetailLayout.Visibility = ViewStates.Visible;
                        fragTrans = _fragmentManager.BeginTransaction().Replace(Resource.Id.frame_details_world, detailsFrag);
                        fragTrans.Commit();
                        break;
                    case DetailFragmentTypes.Itemstore:
                        detailsFrag = new ItemstoreViewPagerTabs();
                        title.Text = "Item Store";
                        _dualtdetailLayout.Visibility = ViewStates.Visible;
                        fragTrans = _fragmentManager.BeginTransaction().Replace(Resource.Id.frame_details_city, detailsFrag);
                        fragTrans.Commit();
                        break;
                    case DetailFragmentTypes.Cityevents:
                        detailsFrag = new CampaignEventsFragment(EventTypes.CityEvent);
                        title.Text = "City Events";
                        _dualtdetailLayout.Visibility = ViewStates.Visible;
                        fragTrans = _fragmentManager.BeginTransaction().Replace(Resource.Id.frame_details_city, detailsFrag);
                        fragTrans.Commit();
                        break;
                    case DetailFragmentTypes.Roadevents:
                        detailsFrag = new CampaignEventsFragment(EventTypes.RoadEvent);
                        title.Text = "Road Events";
                        _dualtdetailLayout.Visibility = ViewStates.Visible;
                        fragTrans = _fragmentManager.BeginTransaction().Replace(Resource.Id.frame_details_world, detailsFrag);
                        fragTrans.Commit();
                        break;
                    case DetailFragmentTypes.PartyAchievements:
                        detailsFrag = new CampaignPartyAchievementsFragment();
                        title.Text = "Party Achievements";
                        _dualtdetailLayout.Visibility = ViewStates.Visible;
                        fragTrans = _fragmentManager.BeginTransaction().Replace(Resource.Id.frame_details_party, detailsFrag);
                        fragTrans.Commit();
                        break;
                    case DetailFragmentTypes.PartyMember:
                        //detailsFrag = CharacterExpandableListFragment.NewInstance(true);
                        detailsFrag = CharacterListFragment.NewInstance(true);
                        title.Text = "Party Members";
                        _dualtdetailLayout.Visibility = ViewStates.Visible;
                        fragTrans = _fragmentManager.BeginTransaction().Replace(Resource.Id.frame_details_party, detailsFrag);
                        fragTrans.Commit();
                        break;
                    case DetailFragmentTypes.CharacterDetail:
                        detailsFrag = CharacterDetailFragmentViewPager.NewInstance(characterId, true);
                        title.Text = "Character Sheet";
                        _dualtdetailLayout.Visibility = ViewStates.Visible;
                        fragTrans = _fragmentManager.BeginTransaction().Replace(Resource.Id.frame_details_party, detailsFrag);
                        fragTrans.Commit();
                        break;
                }

                return detailsFrag;
            }

            var intent = new Intent();
            intent.SetClass(Activity, typeof(DetailsActivity));
            intent.PutExtra(DetailsActivity.SelectedFragId, (int) fragType);
            StartActivity(intent);
            return null;
        }
    }
}