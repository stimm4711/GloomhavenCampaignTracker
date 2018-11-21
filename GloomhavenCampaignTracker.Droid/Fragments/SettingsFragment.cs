using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Business;
using Android.Preferences;

namespace GloomhavenCampaignTracker.Droid.Fragments
{
    public class SettingsFragment : Fragment
    {
        private const string _showItemnames = "showItemnames";
        private const string _showPQNames = "showPQDetails";
        private const string _showScenarioNames = "showScenarioNames";
        private const string _showOldPerkSheet = "showOldPerkSheet";

        public static SettingsFragment NewInstance()
        {
            var frag = new SettingsFragment { Arguments = new Bundle() };
            return frag;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view =  inflater.Inflate(Resource.Layout.fragment_settings, container, false);
            var checkItems = view.FindViewById<CheckBox>(Resource.Id.chkShowItemnames);
            var checkpq = view.FindViewById<CheckBox>(Resource.Id.chkpersonalquest);
            var checkscenarios = view.FindViewById<CheckBox>(Resource.Id.chkunlockedscenarioname);
            var checkShowoldperksheet = view.FindViewById<CheckBox>(Resource.Id.showoldperksheet);

            var prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
            var isShowItems = prefs.GetBoolean(_showItemnames, true);
            var isShowPQ = prefs.GetBoolean(_showPQNames, true);
            var isShowScenarios = prefs.GetBoolean(_showScenarioNames, false);
            var isShowOldPerkSheet = prefs.GetBoolean(_showOldPerkSheet, false);

            checkItems.Checked = isShowItems;
            checkpq.Checked = isShowPQ;
            checkscenarios.Checked = isShowScenarios;
            checkShowoldperksheet.Checked = isShowOldPerkSheet;

            checkItems.CheckedChange += CheckItems_CheckedChange;
            checkpq.CheckedChange += Checkpq_CheckedChange;
            checkscenarios.CheckedChange += Checkscenarios_CheckedChange;
            checkShowoldperksheet.CheckedChange += CheckShowoldperksheet_CheckedChange;

            return view;
        }

        private void CheckShowoldperksheet_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.ShowOldPerkSheet) return;

            var prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
            var editor = prefs.Edit();
            editor.PutBoolean(_showOldPerkSheet, e.IsChecked);
            editor.Apply();

            GCTContext.ShowOldPerkSheet = e.IsChecked;
        }

        private void Checkscenarios_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.ShowScenarioNames) return;

            var prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
            var editor = prefs.Edit();
            editor.PutBoolean(_showScenarioNames, e.IsChecked);
            editor.Apply();

            GCTContext.ShowScenarioNames = e.IsChecked;
        }

        private void Checkpq_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.ShowPersonalQuestDetails) return;

            var prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
            var editor = prefs.Edit();
            editor.PutBoolean(_showPQNames, e.IsChecked);
            editor.Apply();

            GCTContext.ShowPersonalQuestDetails = e.IsChecked;
        }

        private void CheckItems_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.ShowItemNames) return;

            var prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
            var editor = prefs.Edit();
            editor.PutBoolean(_showItemnames, e.IsChecked);
            editor.Apply();

            GCTContext.ShowItemNames = e.IsChecked;
        }
    }
}