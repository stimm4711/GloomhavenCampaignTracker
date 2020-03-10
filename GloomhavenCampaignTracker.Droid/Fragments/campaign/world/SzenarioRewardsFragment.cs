using System;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.Business;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class SzenarioRewardsFragment : CampaignDetailsFragmentBase
    {
        private CampaignUnlockedScenario _campaignScenario;

        private View _view;
        private TabLayout _tabLayout;
        private ViewPager _viewPager;
        private ScenarioRewardsCharacterViewPagerAdapter _adapter;
        private Button _save;
        private Button _cancel;
        private Button _decreaseprospButton;
        private Button _raiseprospbutton;
        private Button _decreaseReputationButton;
        private Button _raiseReputationButton;
        private Button _achievementGAButton;
        private Button _achievementPAButton;
        private TextView _prospLevelText;
        private TextView _reputationTextView;
        private ListView _lstviewScenariosUnlocked;
        private ArrayAdapter<string> _listAdapter;

        private int _reputaionModifier = 0;
        private int _prospertityModifier = 0;

        internal static SzenarioRewardsFragment NewInstance(int scenarioId, bool casual)
        {
            var frag = new SzenarioRewardsFragment { Arguments = new Bundle() };
            frag.Arguments.PutInt(DetailsActivity.SelectedScenarioId, scenarioId);
            frag.Arguments.PutBoolean(DetailsActivity.CasualMode, casual);
            return frag;
        }

        private CampaignUnlockedScenario GetUnlockedScenario()
        {
            if (Arguments == null) return null;

            var id = Arguments.GetInt(DetailsActivity.SelectedScenarioId, 0);
            if (id <= 0) return null;

            var campScenario = GCTContext.CurrentCampaign.GetUnlockedScenario(id);

            return campScenario;
        }

        private bool IsCasualMode()
        {
            if (Arguments == null) return false;

            var casual = Arguments.GetBoolean(DetailsActivity.CasualMode, false);
            return casual;
        }

        public override void OnResume()
        {
            base.OnResume();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_scenario_rewards, container, false);
            _viewPager = _view.FindViewById<ViewPager>(Resource.Id.characterrewardsviewpager);
            _tabLayout = _view.FindViewById<TabLayout>(Resource.Id.rewards_characters_tabs);
            _save = _view.FindViewById<Button>(Resource.Id.btnSave);
            _cancel = _view.FindViewById<Button>(Resource.Id.btnCancel);
            _decreaseprospButton = _view.FindViewById<Button>(Resource.Id.decreaseprospButton);
            _raiseprospbutton = _view.FindViewById<Button>(Resource.Id.raiseprospbutton);
            _decreaseReputationButton = _view.FindViewById<Button>(Resource.Id.decreaseReputationButton);
            _raiseReputationButton = _view.FindViewById<Button>(Resource.Id.raiseReputationButton);
            _achievementGAButton = _view.FindViewById<Button>(Resource.Id.btnGA);
            _achievementPAButton = _view.FindViewById<Button>(Resource.Id.btnPA);
            _prospLevelText = _view.FindViewById<TextView>(Resource.Id.prospLevelText);
            _reputationTextView = _view.FindViewById<TextView>(Resource.Id.reputationTextView);
            _lstviewScenariosUnlocked = _view.FindViewById<ListView>(Resource.Id.lstviewScenariosUnlocked);

            var characters = CharacterRepository.GetPartymembersFlat(GCTContext.CurrentCampaign.CurrentParty.Id).Where(x => !x.Retired).ToList();
            GCTContext.CharacterCollection = characters;
            _adapter = new ScenarioRewardsCharacterViewPagerAdapter(Context, ChildFragmentManager, characters);
            _viewPager.Adapter = _adapter;
            _tabLayout.SetupWithViewPager(_viewPager);

            _campaignScenario = GetUnlockedScenario();

            if(!_save.HasOnClickListeners)
            {
                _save.Click += _save_Click;
            }

            if (!_cancel.HasOnClickListeners)
            {
                _cancel.Click += _cancel_Click; ;
            }

            if (!_decreaseprospButton.HasOnClickListeners)
            {
                _decreaseprospButton.Click += _decreaseprospButton_Click; ;
            }

            if (!_raiseprospbutton.HasOnClickListeners)
            {
                _raiseprospbutton.Click += _raiseprospbutton_Click; ;
            }

            if (!_decreaseReputationButton.HasOnClickListeners)
            {
                _decreaseReputationButton.Click += _decreaseReputationButton_Click; ;
            }

            if (!_raiseReputationButton.HasOnClickListeners)
            {
                _raiseReputationButton.Click += _raiseReputationButton_Click; ;
            }

            if (!_achievementGAButton.HasOnClickListeners)
            {
                _achievementGAButton.Click += _achievementGAButton_Click; ;
            }

            if (!_achievementPAButton.HasOnClickListeners)
            {
                _achievementPAButton.Click += _achievementPAButton_Click; ;
            }

            if(IsCasualMode())
            {
                var prospRepLayout = _view.FindViewById<LinearLayout>(Resource.Id.layout_ProspRep);
                prospRepLayout.Visibility = ViewStates.Gone;

                var achievementLayout = _view.FindViewById<LinearLayout>(Resource.Id.layout_achievements);
                achievementLayout.Visibility = ViewStates.Gone;

                var scenarioLayout = _view.FindViewById<LinearLayout>(Resource.Id.layout_scenario);
                scenarioLayout.Visibility = ViewStates.Gone;
            }
            else
            {
                var unlockedScenarioNumbers = _campaignScenario.GetUnlockedScenarios().Where(x => !GCTContext.CurrentCampaign.IsScenarioUnlocked(x));

                _listAdapter = new ArrayAdapter<string>(Context, Resource.Layout.listviewitem_singleitem, unlockedScenarioNumbers.Select(x => DataServiceCollection.ScenarioDataService.Get(x).Name).ToList());
                _lstviewScenariosUnlocked.Adapter = _listAdapter;
            }         

            return _view;
        }

        private void _achievementPAButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(Activity, typeof(DetailsActivity));
            intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.PartyAchievements);
            StartActivity(intent);
        }

        private void _achievementGAButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(Activity, typeof(DetailsActivity));
            intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.GlobalAchievements);
            StartActivity(intent);
        }

        private void _raiseReputationButton_Click(object sender, EventArgs e)
        {
            _reputationTextView.Text = (_reputaionModifier += 1).ToString();
        }

        private void _decreaseReputationButton_Click(object sender, EventArgs e)
        {
            _reputationTextView.Text = (_reputaionModifier -= 1).ToString();
        }

        private void _raiseprospbutton_Click(object sender, EventArgs e)
        {
            _prospLevelText.Text = (_prospertityModifier += 1).ToString();
        }

        private void _decreaseprospButton_Click(object sender, EventArgs e)
        {
            _prospLevelText.Text = (_prospertityModifier -= 1).ToString();
        }

        private void _cancel_Click(object sender, EventArgs e)
        {
            
        }

        private void _save_Click(object sender, EventArgs e)
        {
            _adapter.SaveCharacterRewards();

            if(!IsCasualMode())
            {
                if (int.TryParse(_prospLevelText.Text, out int prospChange)) GCTContext.CurrentCampaign.CityProsperity += prospChange;
                if (int.TryParse(_reputationTextView.Text, out int repChange)) GCTContext.CurrentCampaign.CurrentParty.Reputation += repChange;

                ScenarioHelper.SetScenarioCompleted(Context, LayoutInflater, _campaignScenario);
            }          

            GCTContext.CurrentCampaign.Save();            

            Activity.Finish();
        }
    }
}