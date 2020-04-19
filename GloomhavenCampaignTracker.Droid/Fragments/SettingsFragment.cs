using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;

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
            var view =  inflater.Inflate(Resource.Layout.fragment_settings, container, false);
            var checkItems = view.FindViewById<CheckBox>(Resource.Id.chkShowItemnames);
            var checkpq = view.FindViewById<CheckBox>(Resource.Id.chkpersonalquest);
            var checkscenarios = view.FindViewById<CheckBox>(Resource.Id.chkunlockedscenarioname);
            var showTreasuresCheck = view.FindViewById<CheckBox>(Resource.Id.chkShowTreasures);
            
            checkItems.Checked = GCTContext.Settings.IsShowItems;
            checkpq.Checked = GCTContext.Settings.IsShowPq;
            checkscenarios.Checked = GCTContext.Settings.IsShowScenarios;
            showTreasuresCheck.Checked = GCTContext.Settings.IsShowTreasure;

            checkItems.CheckedChange += CheckItems_CheckedChange;
            checkpq.CheckedChange += Checkpq_CheckedChange;
            checkscenarios.CheckedChange += Checkscenarios_CheckedChange;
            showTreasuresCheck.CheckedChange += ShowTreasuresCheck_CheckedChange;

            return view;
        }

        private void ShowTreasuresCheck_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.Settings.IsShowTreasure) return;
            GCTContext.Settings.IsShowTreasure = e.IsChecked;
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