using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.Business;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class SzenarioRewardsFragment : RewardFragmentFragment
    {
        private CampaignUnlockedScenario _campaignScenario;
        private ScenarioRewardsCharacterViewPagerAdapter _adapter;
        private ListView _lstviewScenariosUnlocked;
        private ScenarioListviewtAdapter _listAdapter;
        private List<int> _unlockedScenarioNumbers;       

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

            var btnRoadEvent = _view.FindViewById<Button>(Resource.Id.btnRoadE);
            var btnCityEvent = _view.FindViewById<Button>(Resource.Id.btnCityE);
            var btnRiftEvent = _view.FindViewById<Button>(Resource.Id.btnRiftE);

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

            btnRoadEvent.Click += RoadEventButton_Click;
            btnCityEvent.Click += CityEventButton_Click;
            btnRiftEvent.Click += RiftEventButton_Click;

            if (IsCasualMode())
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
                SetUnlockedScenarios();                
            }

            InitTextViews();

            return _view;
        }

        private void SetUnlockedScenarios()
        {
            var lstUnlockedScenarios = new List<CampaignUnlockedScenario>();
            var currentCampaign = GCTContext.CampaignCollection.CurrentCampaign;
            _unlockedScenarioNumbers = _campaignScenario.GetUnlockedScenarios().Where(x => !currentCampaign.IsScenarioUnlocked(x)).ToList();
            var lstScenariosWithSection = new List<int>()
                {
                    98,100,99
                };

            if (lstScenariosWithSection.Contains(_campaignScenario.Scenario.ScenarioNumber))
            {
                // Choose Section for scenario unlock
                var view = LayoutInflater.Inflate(Resource.Layout.alertdialog_listview, null);
                var listview = view.FindViewById<ListView>(Resource.Id.listView);
                listview.ItemsCanFocus = true;
                listview.ChoiceMode = ChoiceMode.Single;
                var selectableSections = ScenarioHelper.GetSelectableSection(_campaignScenario.Scenario.ScenarioNumber);

                var adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleListItemSingleChoice, selectableSections.Select(x => $"Section {x}").ToArray());
                listview.Adapter = adapter;

                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                    .SetCustomView(view)
                    .SetTitle(Context.Resources.GetString(Resource.String.SelectSection))
                    .SetMessage(Context.Resources.GetString(Resource.String.SelectSectionNumber))
                    .SetPositiveButton(Context.Resources.GetString(Resource.String.Select), (senderAlert, args) =>
                    {
                        if (listview.CheckedItemPosition == -1) return;

                        var selectedSection = selectableSections.ElementAt(listview.CheckedItemPosition);

                        _unlockedScenarioNumbers = ScenarioHelper.GetUnlockedScenarioNumbersOfSection(selectedSection);
                        
                        _listAdapter = new ScenarioListviewtAdapter(Context, _unlockedScenarioNumbers.Select(x => DataServiceCollection.ScenarioDataService.GetScenarioByScenarioNumber(x)).ToList());
                        _lstviewScenariosUnlocked.Adapter = _listAdapter;
                    })
                    .Show();
            }
            else if (_campaignScenario.Scenario.ScenarioNumber == 13)
            {
                // Choose the Scenario to unlock

                // Show dialog with selectable scenarios and radio buttons
                var view = LayoutInflater.Inflate(Resource.Layout.alertdialog_listview, null);
                var listview = view.FindViewById<ListView>(Resource.Id.listView);
                listview.ItemsCanFocus = true;
                listview.ChoiceMode = ChoiceMode.Single;

                IEnumerable<Scenario> selectableScenarios = GCTContext.ScenarioCollection.Items.Where(x => _unlockedScenarioNumbers.Contains(x.ScenarioNumber));

                var adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleListItemSingleChoice, selectableScenarios.Select(x => $"# {x.ScenarioNumber}   {x.ScenarioName}").ToArray());
                listview.Adapter = adapter;

                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                    .SetCustomView(view)
                    .SetTitle(Context.Resources.GetString(Resource.String.SelectUnlockedScenario))
                    .SetMessage(Context.Resources.GetString(Resource.String.ChooseScenarioToUnlock))
                    .SetPositiveButton(Context.Resources.GetString(Resource.String.UnlockScenario), (senderAlert, args) =>
                    {
                        if (listview.CheckedItemPosition == -1) return;

                        var scenario = selectableScenarios.ElementAt(listview.CheckedItemPosition);

                        if (scenario == null) return;

                        _unlockedScenarioNumbers.Clear();
                        _unlockedScenarioNumbers.Add(scenario.ScenarioNumber);

                        _listAdapter = new ScenarioListviewtAdapter(Context, _unlockedScenarioNumbers.Select(x => DataServiceCollection.ScenarioDataService.GetScenarioByScenarioNumber(x)).ToList());
                        _lstviewScenariosUnlocked.Adapter = _listAdapter;
                    })
                    .Show();
            }
            else
            {
                _listAdapter = new ScenarioListviewtAdapter(Context, _unlockedScenarioNumbers.Select(x => DataServiceCollection.ScenarioDataService.GetScenarioByScenarioNumber(x)).ToList());
                _lstviewScenariosUnlocked.Adapter = _listAdapter;
            }
        }

        protected override void Save()
        {
            if(!_saved)
            {
                _saved = true;
                
                if (!IsCasualMode())
                {
                    if (int.TryParse(_prospLevelText.Text, out int prospChange)) GCTContext.CurrentCampaign.CityProsperity += prospChange;
                    if (int.TryParse(_reputationTextView.Text, out int repChange)) GCTContext.CurrentCampaign.CurrentParty.Reputation += repChange;

                    _campaignScenario.Completed = true;
                    foreach (var scenarioNumber in _unlockedScenarioNumbers)
                    {
                        GCTContext.CurrentCampaign.AddUnlockedScenario(scenarioNumber);
                    }

                    _campaignScenario.Save();
                }

                _adapter.SaveCharacterRewards();
               
                GCTContext.CurrentCampaign.Save();                

                Activity.Finish();
            }
            else
            {
                Toast.MakeText(Context, "Rewards were already saved!", ToastLength.Long).Show();
            }
        }

        private void FinishSaving()
        {

        }
    }
}