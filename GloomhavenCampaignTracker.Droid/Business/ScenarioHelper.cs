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
        internal static List<CampaignUnlockedScenario> SetScenarioCompleted(Context context, LayoutInflater inflater, CampaignUnlockedScenario campScenario)
        {
            var lstUnlockedScenarios = new List<CampaignUnlockedScenario>();
            if (campScenario.Completed) return lstUnlockedScenarios;

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

                        lstUnlockedScenarios.Add(currentCampaign.AddUnlockedScenario(scenario.ScenarioNumber));
                    })
                    .Show();
            }
            else
            {
                foreach (var scenarioNumber in unlockedScenarioNumbers)
                {
                    lstUnlockedScenarios.Add(currentCampaign.AddUnlockedScenario(scenarioNumber));
                }
            }

            campScenario.Save();

            return lstUnlockedScenarios;
        }

        public static HashSet<CampaignUnlockedScenario> GetRemovedScenarios(CampaignUnlockedScenario campScenario)
        {
            return SetIncomplete(campScenario.UnlockedScenarioData);            
        }

        public static List<int> GetSelectableSection(int scenarionumber)
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

        public static List<CampaignUnlockedScenario> GetUnlockedScenarioBySection(int selectedSection)
        {
            var lstCampaignUnlockedScenarios = new List<CampaignUnlockedScenario>();
            switch (selectedSection)
            {
                case 59:
                    lstCampaignUnlockedScenarios.Add(GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(102));
                    break;
                case 82:
                    lstCampaignUnlockedScenarios.Add(GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(102));
                    lstCampaignUnlockedScenarios.Add(GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(103));
                    break;
                case 55:
                    lstCampaignUnlockedScenarios.Add(GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(103));
                    break;
                case 36:
                    lstCampaignUnlockedScenarios.Add(GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(103));
                    break;
                case 80:
                    lstCampaignUnlockedScenarios.Add(GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(106));
                    lstCampaignUnlockedScenarios.Add(GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(107));
                    break;
                case 16:
                    lstCampaignUnlockedScenarios.Add(GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(104));
                    lstCampaignUnlockedScenarios.Add(GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(105));
                    break;
                case 25:
                    lstCampaignUnlockedScenarios.Add(GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(104));
                    break;
            }

            return lstCampaignUnlockedScenarios;
        }

        internal static HashSet<CampaignUnlockedScenario> SetIncomplete(DL_CampaignUnlockedScenario cs, HashSet<CampaignUnlockedScenario> removededScenarios = null)
        {
            if (removededScenarios == null) removededScenarios = new HashSet<CampaignUnlockedScenario>();

            if (!cs.Completed) return removededScenarios;

            var unlockedScenarioNumbers = GetUnlockedScenarios(cs);
            var currentCampaign = GCTContext.CampaignCollection.CurrentCampaign;

            var followingScenarios = new List<DL_CampaignUnlockedScenario>();
            foreach (var scenarioNumber in unlockedScenarioNumbers)
            {
                // Check if the scenario was unlocked by any other completed scenario
                if (GCTContext.CurrentCampaign.CampaignData.UnlockedScenarios.Any(x => x.Completed && x.Scenario.Scenarionumber != cs.Scenario.Scenarionumber &&
                                                        GetUnlockedScenarios(x).Contains(scenarioNumber))) continue;

                var campScenario = GCTContext.CurrentCampaign.CampaignData.UnlockedScenarios.FirstOrDefault(x => x.Scenario.Scenarionumber == scenarioNumber);
                if (campScenario != null)
                {
                    var scenario = GCTContext.CurrentCampaign.GetUnlockedScenario(campScenario.ID_Scenario);
                    if (!removededScenarios.Contains(scenario))
                    {
                        removededScenarios.Add(scenario);

                        foreach (var cus in SetIncomplete(campScenario, removededScenarios))
                        {
                            removededScenarios.Add(cus);
                        }
                    }                   
                }
            }

            return removededScenarios;
        }

        internal static List<int> GetUnlockedScenarioNumbersOfSection(int selectedSection)
        {
            var lstCampaignUnlockedScenarios = new List<int>();
            switch (selectedSection)
            {
                case 59:
                    lstCampaignUnlockedScenarios.Add(102);
                    break;
                case 82:
                    lstCampaignUnlockedScenarios.Add(102);
                    lstCampaignUnlockedScenarios.Add(103);
                    break;
                case 55:
                    lstCampaignUnlockedScenarios.Add(103);
                    break;
                case 36:
                    lstCampaignUnlockedScenarios.Add(103);
                    break;
                case 80:
                    lstCampaignUnlockedScenarios.Add(106);
                    lstCampaignUnlockedScenarios.Add(107);
                    break;
                case 16:
                    lstCampaignUnlockedScenarios.Add(104);
                    lstCampaignUnlockedScenarios.Add(105);
                    break;
                case 25:
                    lstCampaignUnlockedScenarios.Add(104);
                    break;
            }

            return lstCampaignUnlockedScenarios;
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