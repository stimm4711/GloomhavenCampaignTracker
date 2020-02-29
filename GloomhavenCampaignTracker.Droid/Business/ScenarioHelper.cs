using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Droid.Business
{
    public class ScenarioHelper
    {
        internal static void SetScenarioCompleted(Context context, LayoutInflater inflater, CampaignUnlockedScenario campScenario)
        {
            if (campScenario.Completed) return;

            List<CampaignUnlockedScenario> unlockedScenarios = new List<CampaignUnlockedScenario>();
            campScenario.Completed = true;

            var currentCampaign = GCTContext.CampaignCollection.CurrentCampaign;
            var unlockedScenarioNumbers = campScenario.GetUnlockedScenarios().Where(x => !currentCampaign.IsScenarioUnlocked(x));

            var lstScenariosWithSection = new List<int>()
            {
                98,100,99
            };

            if (lstScenariosWithSection.Contains(campScenario.Scenario.ScenarioNumber))
            {
                // Choose Section for scenario unlock
                var view = inflater.Inflate(Resource.Layout.alertdialog_listview, null);
                var listview = view.FindViewById<ListView>(Resource.Id.listView);
                listview.ItemsCanFocus = true;
                listview.ChoiceMode = ChoiceMode.Single;
                var selectableSections = GetSelectableSection(campScenario.Scenario.ScenarioNumber);

                var adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleListItemSingleChoice, selectableSections.Select(x => $"Section {x}").ToArray());
                listview.Adapter = adapter;

                new CustomDialogBuilder(context, Resource.Style.MyDialogTheme)
                    .SetCustomView(view)
                    .SetTitle(context.Resources.GetString(Resource.String.SelectSection))
                    .SetMessage(context.Resources.GetString(Resource.String.SelectSectionNumber))
                    .SetPositiveButton(context.Resources.GetString(Resource.String.Select), (senderAlert, args) =>
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
                var view = inflater.Inflate(Resource.Layout.alertdialog_listview, null);
                var listview = view.FindViewById<ListView>(Resource.Id.listView);
                listview.ItemsCanFocus = true;
                listview.ChoiceMode = ChoiceMode.Single;

                IEnumerable<Scenario> selectableScenarios = GCTContext.ScenarioCollection.Items.Where(x => unlockedScenarioNumbers.Contains(x.ScenarioNumber));

                var adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleListItemSingleChoice, selectableScenarios.Select(x => $"# {x.ScenarioNumber}   {x.ScenarioName}").ToArray());
                listview.Adapter = adapter;

                new CustomDialogBuilder(context, Resource.Style.MyDialogTheme)
                    .SetCustomView(view)
                    .SetTitle(context.Resources.GetString(Resource.String.SelectUnlockedScenario))
                    .SetMessage(context.Resources.GetString(Resource.String.ChooseScenarioToUnlock))
                    .SetPositiveButton(context.Resources.GetString(Resource.String.UnlockScenario), (senderAlert, args) =>
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
        }

        public static void SetScenarioIncomplete(Context context, LayoutInflater inflater, CampaignUnlockedScenario campScenario, CheckBox checkb)
        {
            var removecampScenarios = SetIncomplete(campScenario.UnlockedScenarioData);

            var alertView = inflater.Inflate(Resource.Layout.alertdialog_listview, null);
            var lv = alertView.FindViewById<ListView>(Resource.Id.listView);
            lv.Adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleListItem1, removecampScenarios.Select(x => $"# {x.Scenario.Scenarionumber}   {x.Scenario.Name}").ToArray());

            new CustomDialogBuilder(context, Resource.Style.MyDialogTheme)
                .SetCustomView(alertView)
                .SetTitle(String.Format(context.Resources.GetString(Resource.String.SetScenarioIncomplete), campScenario.ScenarioName))
                .SetMessage(context.Resources.GetString(Resource.String.ScenariosWillBeRemoved))
                .SetNegativeButton(context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) =>
                {
                    checkb.Checked = true;
                })
                .SetPositiveButton(context.Resources.GetString(Resource.String.OK), (senderAlert, args) =>
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

        private static void GetUnlockedScenarioBySection(int selectedSection)
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

        internal static HashSet<DL_CampaignUnlockedScenario> SetIncomplete(DL_CampaignUnlockedScenario cs, HashSet<DL_CampaignUnlockedScenario> removededScenarios = null)
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

        private static List<int> GetUnlockedScenarios(DL_CampaignUnlockedScenario cs)
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
    }
}