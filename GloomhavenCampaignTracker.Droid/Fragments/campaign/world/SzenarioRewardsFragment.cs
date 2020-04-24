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
    public class SzenarioRewardsFragment : RewardFragmentFragment
    {
        private CampaignUnlockedScenario _campaignScenario;
        private ScenarioRewardsCharacterViewPagerAdapter _adapter;
        private ListView _lstviewScenariosUnlocked;
        private ScenarioListviewtAdapter _listAdapter;

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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_scenario_rewards, container, false);
            _decreaseprospButton = _view.FindViewById<Button>(Resource.Id.decreaseprospButton);
            _raiseprospbutton = _view.FindViewById<Button>(Resource.Id.raiseprospbutton);
            _decreaseReputationButton = _view.FindViewById<Button>(Resource.Id.decreaseReputationButton);
            _raiseReputationButton = _view.FindViewById<Button>(Resource.Id.raiseReputationButton);
            _achievementGAButton = _view.FindViewById<Button>(Resource.Id.btnGA);
            _achievementPAButton = _view.FindViewById<Button>(Resource.Id.btnPA);
            _prospLevelText = _view.FindViewById<TextView>(Resource.Id.prospLevelText);
            _reputationTextView = _view.FindViewById<TextView>(Resource.Id.reputationTextView);
            _lstviewScenariosUnlocked = _view.FindViewById<ListView>(Resource.Id.lstviewScenariosUnlocked);

            var viewPager = _view.FindViewById<ViewPager>(Resource.Id.characterrewardsviewpager);
            var tabLayout = _view.FindViewById<TabLayout>(Resource.Id.rewards_characters_tabs);

            var characters = CharacterRepository.GetPartymembersFlat(GCTContext.CurrentCampaign.CurrentParty.Id).Where(x => !x.Retired).ToList();
            GCTContext.CharacterCollection = characters;
            _adapter = new ScenarioRewardsCharacterViewPagerAdapter(Context, ChildFragmentManager, characters);
            viewPager.Adapter = _adapter;
            tabLayout.SetupWithViewPager(viewPager);

            _campaignScenario = GetUnlockedScenario();

            if (!_decreaseprospButton.HasOnClickListeners)
            {
                _decreaseprospButton.Click += DecreaseprospButton_Click; ;
            }

            if (!_raiseprospbutton.HasOnClickListeners)
            {
                _raiseprospbutton.Click += Raiseprospbutton_Click; ;
            }

            if (!_decreaseReputationButton.HasOnClickListeners)
            {
                _decreaseReputationButton.Click += DecreaseReputationButton_Click; ;
            }

            if (!_raiseReputationButton.HasOnClickListeners)
            {
                _raiseReputationButton.Click += RaiseReputationButton_Click; ;
            }

            if (!_achievementGAButton.HasOnClickListeners)
            {
                _achievementGAButton.Click += AchievementGAButton_Click; ;
            }

            if (!_achievementPAButton.HasOnClickListeners)
            {
                _achievementPAButton.Click += AchievementPAButton_Click; ;
            }

            if(IsCasualMode())
            {
                var prospRepLayout = _view.FindViewById<LinearLayout>(Resource.Id.layout_ProspRep);
                prospRepLayout.Visibility = ViewStates.Gone;

                var achievementLayout = _view.FindViewById<LinearLayout>(Resource.Id.layout_achievements);
                achievementLayout.Visibility = ViewStates.Gone;

                var scenarioLayout = _view.FindViewById<LinearLayout>(Resource.Id.layout_scenario);
                scenarioLayout.Visibility = ViewStates.Gone;

                var eventlayout = _view.FindViewById<LinearLayout>(Resource.Id.layout_events);
                eventlayout.Visibility = ViewStates.Gone;
            }
            else
            {
                var unlockedScenarioNumbers = _campaignScenario.GetUnlockedScenarios().Where(x => !GCTContext.CurrentCampaign.IsScenarioUnlocked(x));

                _listAdapter = new ScenarioListviewtAdapter(Context, unlockedScenarioNumbers.Select(x => DataServiceCollection.ScenarioDataService.Get(x)).ToList());
                _lstviewScenariosUnlocked.Adapter = _listAdapter;
            }

            InitTextViews();

            return _view;
        }
        
        protected override void Save()
        {
            _adapter.SaveCharacterRewards();

            if (!IsCasualMode())
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