using System;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class SzenarioDetailsFragment : CampaignDetailsFragmentBase
    {
        private CampaignUnlockedScenario _campaignScenario;
        private TextView _scenarioname;
        private TextView _scenarionumber;
        private CheckBox _scenariostatus;
        private GridView _grid;

        internal static SzenarioDetailsFragment NewInstance(int scenarioId)
        {
            var frag = new SzenarioDetailsFragment { Arguments = new Bundle() };
            frag.Arguments.PutInt(DetailsActivity.SelectedScenarioId, scenarioId);
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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_scenariodetails_details, container, false);
            _scenarioname = _view.FindViewById<TextView>(Resource.Id.scenarionametextview);
            _scenarionumber = _view.FindViewById<TextView>(Resource.Id.scenarionumbertextview);
            _scenariostatus = _view.FindViewById<CheckBox>(Resource.Id.scenariostatuscheckbox);
            _grid = _view.FindViewById<GridView>(Resource.Id.imagesGridView);

            _campaignScenario = GetUnlockedScenario();

            var txt_treasures = _view.FindViewById<TextView>(Resource.Id.txt_treasures);
            var txt_region = _view.FindViewById<TextView>(Resource.Id.txt_region);

            if (_campaignScenario != null)
            {
                if (_campaignScenario.Scenario != null)
                {
                    _scenarioname.Text = _campaignScenario.ScenarioName;
                    _scenarionumber.Text = $"# {_campaignScenario.Scenarionumber}";
                    _scenariostatus.Checked = _campaignScenario.Completed;

                    if (_campaignScenario.Scenario.ScenarioData.Treasures.Any())
                    {
                        txt_treasures.Visibility = ViewStates.Visible;
                        txt_region.Text = Helper.GetRegionName(_campaignScenario.UnlockedScenarioData.Scenario.Region_ID);
                    }
                }
            }

            UpdateRequirementsListView();

            SetBackgroundOfTopLayout();

            _scenariostatus.CheckedChange += (sender, e) =>
            {
                if (_campaignScenario.Completed == _scenariostatus.Checked) return;

                SetScenarioCompletedStatus(_campaignScenario, _scenariostatus.Checked, _scenariostatus);
            };

            _grid.Adapter = new CampaignScenarioTreasureAdapter(Context, _campaignScenario.UnlockedScenarioData);

            return _view;
        }

        private void SetScenarioCompletedStatus(CampaignUnlockedScenario campScenario, bool status, CheckBox checkb)
        {
            if (status)
            {
                if (campScenario.Completed) return;

                List<CampaignUnlockedScenario> unlockedScenarios = new List<CampaignUnlockedScenario>();
                campScenario.Completed = true;

                var unlockedScenarioNumbers = campScenario.GetUnlockedScenarios();
                var currentCampaign = GCTContext.CampaignCollection.CurrentCampaign;

                var lstScenariosWithSection = new List<int>()
                {
                    98,100,99
                };

                if (lstScenariosWithSection.Contains(campScenario.Scenario.ScenarioNumber))
                {
                    // Choose Section for scenario unlock
                    var view = LayoutInflater.Inflate(Resource.Layout.alertdialog_listview, null);
                    var listview = view.FindViewById<ListView>(Resource.Id.listView);
                    listview.ItemsCanFocus = true;
                    listview.ChoiceMode = ChoiceMode.Single;
                    var selectableSections = GetSelectableSection(campScenario.Scenario.ScenarioNumber);

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

                           GetUnlockedScenarioBySection(selectedSection);
                       })
                       .Show();
                }
                else if (campScenario.Scenario.ScenarioNumber == 13)
                {
                    // Choose the Scenario to unlock

                    // Show dialog with selectable scenarios and radio buttons
                    var view = LayoutInflater.Inflate(Resource.Layout.alertdialog_listview, null);
                    var listview = view.FindViewById<ListView>(Resource.Id.listView);
                    listview.ItemsCanFocus = true;
                    listview.ChoiceMode = ChoiceMode.Single;

                    IEnumerable<Scenario> selectableScenarios = GCTContext.ScenarioCollection.Items.Where(x => unlockedScenarioNumbers.Contains(x.ScenarioNumber));

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

                            currentCampaign.AddUnlockedScenario(scenario.ScenarioNumber);
                        })
                        .Show();
                }
                else
                {
                    foreach (var scenarioNumber in unlockedScenarioNumbers)
                    {
                        _ = currentCampaign.AddUnlockedScenario(scenarioNumber);
                    }
                }

                campScenario.Save();

                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                       .SetTitle("Scenarios unlocked")
                       .SetMessage($"You've unlocked scenario(s) # {string.Join( ", # ", unlockedScenarioNumbers.ToArray())}")
                       .Show();
            }
            else
            {
                var removecampScenarios = SetIncomplete(campScenario.UnlockedScenarioData);

                var alertView = LayoutInflater.Inflate(Resource.Layout.alertdialog_listview, null);
                var lv = alertView.FindViewById<ListView>(Resource.Id.listView);
                lv.Adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleListItem1, removecampScenarios.Select(x => $"# {x.Scenario.Scenarionumber}   {x.Scenario.Name}").ToArray());

                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                    .SetCustomView(alertView)
                    .SetTitle(String.Format(Context.Resources.GetString(Resource.String.SetScenarioIncomplete), campScenario.ScenarioName))
                    .SetMessage(Context.Resources.GetString(Resource.String.ScenariosWillBeRemoved))
                    .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) =>
                    {
                        checkb.Checked = true;
                    })
                    .SetPositiveButton(Context.Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                    {
                        foreach (var cus in removecampScenarios)
                        {
                            GCTContext.CurrentCampaign.CampaignData.UnlockedScenarios.Remove(cus);
                        }

                        campScenario.Completed = false;
                        campScenario.Save();
                    })
                    .Show();
            }            
        }

        internal HashSet<DL_CampaignUnlockedScenario> SetIncomplete(DL_CampaignUnlockedScenario cs, HashSet<DL_CampaignUnlockedScenario> removededScenarios = null)
        {
            if (removededScenarios == null) removededScenarios = new HashSet<DL_CampaignUnlockedScenario>();

            if (!cs.Completed) return removededScenarios;

            var unlockedScenarioNumbers = GetUnlockedScenarios(cs);
            var currentCampaign = GCTContext.CampaignCollection.CurrentCampaign;

            var followingScenarios = new List<DL_CampaignUnlockedScenario>();
            foreach (var scenarioNumber in unlockedScenarioNumbers)
            {
                // Check if the scenario was unlocked by any other completed scenario
                if (GCTContext.CurrentCampaign.CampaignData.UnlockedScenarios.Any(x => x.Completed && x.Scenario.Scenarionumber != cs.Scenario.Scenarionumber &&
                                                        !removededScenarios.Contains(x) &&
                                                        GetUnlockedScenarios(x).Contains(scenarioNumber))) continue;

                var campScenario = GCTContext.CurrentCampaign.CampaignData.UnlockedScenarios.FirstOrDefault(x => x.Scenario.Scenarionumber == scenarioNumber);
                if (campScenario != null)
                {
                    removededScenarios.Add(campScenario);

                    foreach (var cus in SetIncomplete(campScenario, removededScenarios))
                    {
                        removededScenarios.Add(cus);
                    }                    
                }
            }

            return removededScenarios;
        }

        private List<int> GetUnlockedScenarios(DL_CampaignUnlockedScenario cs)
        {
            var unlockedScenarios = new List<int>();
            var us = cs.Scenario.UnlockedScenarios.Split(',');
            foreach (var s in us)
            {
                if (!int.TryParse(s, out int scenarioNumber)) continue;
                unlockedScenarios.Add(scenarioNumber);
            }

            return unlockedScenarios;
        }


        //internal HashSet<CampaignUnlockedScenario> SetIncomplete(CampaignUnlockedScenario cs, HashSet<CampaignUnlockedScenario> removededScenarios = null)
        //{
        //    if (removededScenarios == null) removededScenarios = new HashSet<CampaignUnlockedScenario>();

        //    if (!cs.Completed) return removededScenarios;

        //    var unlockedScenarioNumbers = cs.GetUnlockedScenarios();
        //    var currentCampaign = GCTContext.CampaignCollection.CurrentCampaign;

        //    var followingScenarios = new List<CampaignUnlockedScenario>();
        //    foreach (var scenarioNumber in unlockedScenarioNumbers)
        //    {
        //        // Check if the scenario was unlocked by any other completed scenario
        //        if (_listDataChild[_completed].Any(x => x.Scenarionumber != cs.Scenarionumber &&
        //                                                !removededScenarios.Contains(x) &&
        //                                                x.GetUnlockedScenarios().Contains(scenarioNumber))) continue;

        //        var campScenarioList = _listDataChild.Values.FirstOrDefault(x => x.Any(y => y.Scenarionumber == scenarioNumber));
        //        if (campScenarioList != null)
        //        {
        //            var campScenario = campScenarioList.FirstOrDefault(y => y.Scenarionumber == scenarioNumber);

        //            if (campScenario != null)
        //            {
        //                removededScenarios.Add(campScenario);

        //                foreach (var cus in SetIncomplete(campScenario, removededScenarios))
        //                {
        //                    removededScenarios.Add(cus);
        //                }
        //            }
        //        }
        //    }

        //    return removededScenarios;
        //}


        private void GetUnlockedScenarioBySection(int selectedSection)
        {
            switch (selectedSection)
            {
                case 59:
                    _ = GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(102);
                    break;
                case 82:
                    _ = GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(102);
                    _ = GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(103);
                    break;
                case 55:
                    _ = GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(103);
                    break;
                case 36:
                    _ = GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(103);
                    break;
                case 80:
                    _ = GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(106);
                    _ = GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(107);
                    break;
                case 16:
                    _ = GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(104);
                    _ = GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(105);
                    break;
                case 25:
                    _ = GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(104);
                    break;
            }
        }

        private static List<int> GetSelectableSection(int scenarionumber)
        {
            switch (scenarionumber)
            {
                case 98:
                    return new List<int> { 59, 82, 55 };
                case 100:
                    return new List<int> { 36, 80 };
                case 99:
                    return new List<int> { 16, 25 };               
                default:
                    return new List<int>();
            }
        }

        private void UpdateRequirementsListView()
        {
            var lv_requirements = _view.FindViewById<ListView>(Resource.Id.lv_requirements);

            var adapter = new ArrayAdapter<string>(Context, Resource.Layout.listviewitem_singleitem, _campaignScenario.GetAllRequirements());

            lv_requirements.Adapter = adapter;
        }

        private void SetBackgroundOfTopLayout()
        {
            var enableCheckbox = true;
            var color = ContextCompat.GetColor(Context, Resource.Color.gloom_primaryLighter);
            if (_campaignScenario.IsBlocked())
            {
                enableCheckbox = false;
                color = ContextCompat.GetColor(Context, Resource.Color.gloom_scenarioBlockedItemBackground);
            }
            else if (!_campaignScenario.Completed && !_campaignScenario.IsAvailable())
            {
                enableCheckbox = false;
                color = ContextCompat.GetColor(Context, Resource.Color.gloom_scenarioUnavailableItemBackground);
            }
            else if (_campaignScenario.Completed)
            {
                color = ContextCompat.GetColor(Context, Resource.Color.gloom_scenarioCompletedItemBackground);
            }

            var layoutTop = _view.FindViewById<RelativeLayout>(Resource.Id.layout_top);
            layoutTop.SetBackgroundColor(new Color(color));

            var chk = _view.FindViewById<CheckBox>(Resource.Id.scenariostatuscheckbox);
            chk.Enabled = enableCheckbox;
        }
    }
}