using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;

namespace GloomhavenCampaignTracker.Droid.Fragments
{
    public class SettingsFragment : Fragment
    {
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
            var showTreasuresCheck = view.FindViewById<CheckBox>(Resource.Id.chkShowTreasures);

            var isShowItems = GCTContext.Settings.IsShowItems;
            var isShowPQ = GCTContext.Settings.IsShowPq;
            var isShowScenarios = GCTContext.Settings.IsShowScenarios;
            var isShowOldAbilitySheet = GCTContext.Settings.IsShowOldAbilitySheet;
            var isFCActivated = GCTContext.Settings.IsFCActivated;
            var isShowTreasure = GCTContext.Settings.IsShowTreasure;
            
            checkItems.Checked = isShowItems;
            checkpq.Checked = isShowPQ;
            checkscenarios.Checked = isShowScenarios;
            activateFCCheck.Checked = isFCActivated;
            showOldAbilitySheetCheck.Checked = isShowOldAbilitySheet;
            showTreasuresCheck.Checked = isShowTreasure;

            checkItems.CheckedChange += CheckItems_CheckedChange;
            checkpq.CheckedChange += Checkpq_CheckedChange;
            checkscenarios.CheckedChange += Checkscenarios_CheckedChange;
            activateFCCheck.CheckedChange += ActivateFCCheck_CheckedChange;
            showOldAbilitySheetCheck.CheckedChange += showOldAbilitySheetCheck_CheckedChange;
            showTreasuresCheck.CheckedChange += ShowTreasuresCheck_CheckedChange;

            if (!btnCheckItems.HasOnClickListeners)
                btnCheckItems.Click += BtnCheckItems_Click;
            return view;
        }

        private void ShowTreasuresCheck_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.Settings.IsShowTreasure) return;
            GCTContext.Settings.IsShowTreasure = e.IsChecked;
        }

        private void ActivateFCCheck_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.Settings.IsFCActivated) return;
            GCTContext.Settings.IsFCActivated = e.IsChecked;
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
            if (e.IsChecked == GCTContext.Settings.IsShowOldAbilitySheet) return;
            GCTContext.Settings.IsShowOldAbilitySheet = e.IsChecked;
        }

        private void Checkscenarios_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.Settings.IsShowScenarios) return;
            GCTContext.Settings.IsShowScenarios = e.IsChecked;
        }

        private void Checkpq_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.Settings.IsShowPq) return;
            GCTContext.Settings.IsShowPq = e.IsChecked;
        }

        private void CheckItems_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.Settings.IsShowItems) return;
            GCTContext.Settings.IsShowItems = e.IsChecked;
        }
    }
}