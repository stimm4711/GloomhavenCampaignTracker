using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.Threading.Tasks;
using GloomhavenCampaignTracker.Business.Network;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign.world
{
    /// <summary>
    /// http://www.appliedcodelog.com/2016/06/expandablelistview-in-xamarin-android.html
    /// </summary>
    public class CampaignWorldUnlockedScenariosFragmentExpandListView : CampaignDetailsFragmentBase
    {
        private ExpandableScenarioListAdapter _listAdapter;
        private ExpandableListView _expListView;
        private List<string> _listDataHeader;
        private Dictionary<string, List<CampaignUnlockedScenario>> _listDataChild;

        public static CampaignWorldUnlockedScenariosFragmentExpandListView NewInstance(int campId)
        {
            var frag = new CampaignWorldUnlockedScenariosFragmentExpandListView { Arguments = new Bundle() };
            return frag;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Prepare list data
            InitListData();
            _dataChanged = false;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            // Use this to return your custom view for this Fragment
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_unlockedscenarios_expand, container, false);

            _expListView = _view.FindViewById<ExpandableListView>(Resource.Id.campaignWorld_lvUnlockedLocations);
          
            var fab = _view.FindViewById<FloatingActionButton>(Resource.Id.fab);

            if (!fab.HasOnClickListeners)
            {
                fab.Click += (sender, e) =>
                {
                    ShowAssignScenarioDialog(inflater);
                };
            }

            BindListToAdapter();

            return _view;
        }

        private void BindListToAdapter()
        {
            _listAdapter = new ExpandableScenarioListAdapter(Activity, _listDataHeader, _listDataChild);
            _expListView.SetAdapter(_listAdapter);

            _expListView.ExpandGroup(0);
            if (!_listDataChild[Resources.GetString(Resource.String.Unlocked)].Any())
            {
                _expListView.ExpandGroup(1);
            }
        }

        void InitListData()
        {
            var scenarios = GetCampaignScenarios().OrderBy(x => x.Scenarionumber).ToList();

            // if the client is connected to a server, sync the data
            if (GloomhavenClient.IsClientRunning())
            {
                Task.Run(async () =>
                {
                    await GloomhavenClient.Instance.UpdateGlobalAchievements();
                    await GloomhavenClient.Instance.UpdatePartyAchievements();
                    await GloomhavenClient.Instance.UpdateScenarios(scenarios);

                    Activity.RunOnUiThread(() =>
                    {
                        FillListHeaderAndChilds(scenarios);
                    });

                });
            }
            else
            {
                FillListHeaderAndChilds(scenarios);      
            }            
        }        

        private void FillListHeaderAndChilds(List<CampaignUnlockedScenario> scenarios)
        {
            _listDataHeader = new List<string>();
            _listDataChild = new Dictionary<string, List<CampaignUnlockedScenario>>();

            // Adding child data
            _listDataHeader.Add(Resources.GetString(Resource.String.Unlocked));
            _listDataHeader.Add(Resources.GetString(Resource.String.Unavailable));
            _listDataHeader.Add(Resources.GetString(Resource.String.Completed));
            _listDataHeader.Add(Resources.GetString(Resource.String.Blocked));

            if (scenarios.Any())
            {
                // Adding child data

                var campaignScenarioSearch = new CampaignScenarioSearch();

                var lstUnlocked = scenarios.FindAll(campaignScenarioSearch.FindAvailable);

                var lstUnavailable = scenarios.FindAll(campaignScenarioSearch.FindUnAvailable);

                var lstCompleted = scenarios.FindAll(campaignScenarioSearch.FindCompleted);

                var lstBlocked = scenarios.FindAll(campaignScenarioSearch.FindBlocked);

                // Header, Child data
                _listDataChild.Add(_listDataHeader[0], lstUnlocked);
                _listDataChild.Add(_listDataHeader[1], lstUnavailable);
                _listDataChild.Add(_listDataHeader[2], lstCompleted);
                _listDataChild.Add(_listDataHeader[3], lstBlocked);
            }
            else
            {
                _listDataChild.Add(_listDataHeader[0], new List<CampaignUnlockedScenario>());
                _listDataChild.Add(_listDataHeader[1], new List<CampaignUnlockedScenario>());
                _listDataChild.Add(_listDataHeader[2], new List<CampaignUnlockedScenario>());
                _listDataChild.Add(_listDataHeader[3], new List<CampaignUnlockedScenario>());
            }
        }

        private List<CampaignUnlockedScenario> GetCampaignScenarios()
        {
            if (GCTContext.CurrentCampaign == null) return new List<CampaignUnlockedScenario>();

            try
            {
                var scenarios = new List<CampaignUnlockedScenario>();

                if (GCTContext.CurrentCampaign != null)
                {
                    foreach (var sd in GCTContext.CurrentCampaign.CampaignData.UnlockedScenarios)
                    {
                        scenarios.Add(new CampaignUnlockedScenario(sd));
                    }
                }

                return scenarios;
            }
            catch
            {
                Toast.MakeText(Context, "Error on loading scenarios. Please try again.", ToastLength.Short).Show();
                return new List<CampaignUnlockedScenario>();
            }            
        }

        private void ShowAssignScenarioDialog(LayoutInflater inflater)
        {
            // create alert view
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_textinputlayoutwithspinner, null);
            var spinner = convertView.FindViewById<Spinner>(Resource.Id.spinner);

            var scenarios = DataServiceCollection.ScenarioDataService.Get();

            if (!Campaign.HasGlobalAchievement(GlobalAchievementsInternalNumbers.EndOfGloom) || !GCTContext.ActivateForgottenCiclesContent)
                scenarios = scenarios.Where(x => x.ContentOfPack == 1).ToList();

            //IEnumerable<Scenario> selectableScenarios = GCTContext.ScenarioCollection.Items;
            IEnumerable<int> assignedScenarioIds; 
            IEnumerable<DL_Scenario> selectableScenarios = scenarios;

            if (Campaign != null)
            {
                assignedScenarioIds = Campaign.CampaignData.UnlockedScenarios.Select(y => y.ID_Scenario);
                selectableScenarios = selectableScenarios.Where(x => !(assignedScenarioIds.Contains(x.Id)));
            }

            var adapter = new SpinnerAdapter(Context);
            adapter.AddItems(selectableScenarios);
            spinner.Adapter = adapter;

            // create alert
            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetMessage("Select a scenario")
                .SetTitle(Resources.GetString(Resource.String.AssignScenario))
                .SetPositiveButton(Resources.GetString(Resource.String.Assign), (senderAlert, args) =>
                {
                    var scenario = selectableScenarios.ElementAt(spinner.SelectedItemPosition);

                    if (scenario == null) return;

                    if (Campaign == null) return;

                    if (Campaign.CampaignData.UnlockedScenarios.Any(x => x.ID_Scenario == scenario.Id))
                    {
                        Toast.MakeText(Context, Resources.GetString(Resource.String.ScenarioAlreadyAssigned), ToastLength.Short).Show();
                    }
                    else
                    {
                        _listAdapter.Add(Campaign.AddUnlockedScenario(scenario.Id));
                        _listAdapter.NotifyDataSetChanged();
                    }
                })
                .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }
    }
}