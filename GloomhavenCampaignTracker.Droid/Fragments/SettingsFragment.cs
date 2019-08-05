using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using Android.Preferences;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;

namespace GloomhavenCampaignTracker.Droid.Fragments
{
    public class SettingsFragment : Fragment
    {
        private const string _showItemnames = "showItemnames";
        private const string _showPQNames = "showPQDetails";
        private const string _showScenarioNames = "showScenarioNames";
        private const string _showOldAbilitySheet = "showOldAbilitySheet";
        private const string _activateFC = "activateFC";

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
            var btnCheckItems = view.FindViewById<Button>(Resource.Id.checkitemsButton);
            var activateFCCheck = view.FindViewById<CheckBox>(Resource.Id.activateFCCheck);
            var showOldAbilitySheetCheck = view.FindViewById<CheckBox>(Resource.Id.isShowOldAbilitySheet);

            var prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
            var isShowItems = prefs.GetBoolean(_showItemnames, true);
            var isShowPQ = prefs.GetBoolean(_showPQNames, true);
            var isShowScenarios = prefs.GetBoolean(_showScenarioNames, false);
            var isShowOldAbilitySheet = prefs.GetBoolean(_showOldAbilitySheet, false);
            var isFCActivated = prefs.GetBoolean(_activateFC, false);
            
            checkItems.Checked = isShowItems;
            checkpq.Checked = isShowPQ;
            checkscenarios.Checked = isShowScenarios;
            activateFCCheck.Checked = isFCActivated;
            showOldAbilitySheetCheck.Checked = isShowOldAbilitySheet;

            checkItems.CheckedChange += CheckItems_CheckedChange;
            checkpq.CheckedChange += Checkpq_CheckedChange;
            checkscenarios.CheckedChange += Checkscenarios_CheckedChange;
            activateFCCheck.CheckedChange += ActivateFCCheck_CheckedChange;
            showOldAbilitySheetCheck.CheckedChange += showOldAbilitySheetCheck_CheckedChange;

            if (!btnCheckItems.HasOnClickListeners)
                btnCheckItems.Click += BtnCheckItems_Click;
            return view;
        }

        private void ActivateFCCheck_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.ActivateForgottenCiclesContent) return;

            var prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
            var editor = prefs.Edit();
            editor.PutBoolean(_activateFC, e.IsChecked);
            editor.Apply();

            GCTContext.ActivateForgottenCiclesContent = e.IsChecked;
        }

        private void BtnCheckItems_Click(object sender, System.EventArgs e)
        {
            var items = ItemRepository.Get();

            if (items.Count < 151)
            {
                var countBefore = items.Count;
                GloomhavenDbHelper.UpdateMissingItems();

                items = ItemRepository.Get();
                var countAfter = items.Count;
                Toast.MakeText(Context, $"Updated items. Added {countAfter - countBefore} items.", ToastLength.Short).Show();
            }   
            else
            {
                Toast.MakeText(Context, "All items checked. No missing items found.", ToastLength.Short).Show();
            }
        }

        private void showOldAbilitySheetCheck_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.ShowAbilitySheet) return;

            var prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
            var editor = prefs.Edit();
            editor.PutBoolean(_showOldAbilitySheet, e.IsChecked);
            editor.Apply();

            GCTContext.ShowAbilitySheet = e.IsChecked;
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