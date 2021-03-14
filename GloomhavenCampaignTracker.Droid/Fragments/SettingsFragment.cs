﻿using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

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
            var btnCheckItems = view.FindViewById<Button>(Resource.Id.checkitemsButton);
            var checkBSv2 = view.FindViewById<CheckBox>(Resource.Id.cb_bs_v2Cards);
            var checkDRv2 = view.FindViewById<CheckBox>(Resource.Id.cb_dr_v2Cards);

            if (GCTContext.CurrentCampaign != null && GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.HiddenClassUnlocked)
                checkBSv2.Visibility = ViewStates.Visible;

            checkItems.Checked = GCTContext.Settings.IsShowItems;
            checkpq.Checked = GCTContext.Settings.IsShowPq;
            checkscenarios.Checked = GCTContext.Settings.IsShowScenarios;
            showTreasuresCheck.Checked = GCTContext.Settings.IsShowTreasure;

            checkItems.CheckedChange += CheckItems_CheckedChange;
            checkpq.CheckedChange += Checkpq_CheckedChange;
            checkscenarios.CheckedChange += Checkscenarios_CheckedChange;
            showTreasuresCheck.CheckedChange += ShowTreasuresCheck_CheckedChange;
            checkBSv2.CheckedChange += CheckBSv2_CheckedChange;
            checkDRv2.CheckedChange += CheckDRv2_CheckedChange;

            if (!btnCheckItems.HasOnClickListeners)
                btnCheckItems.Click += BtnCheckItems_Click;

            return view;
        }

        private void CheckDRv2_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.Settings.IsUseDRv2) return;
            GCTContext.Settings.IsUseDRv2 = e.IsChecked;
        }

        private void CheckBSv2_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == GCTContext.Settings.IsUseBSv2) return;
            GCTContext.Settings.IsUseBSv2 = e.IsChecked;
        }

        private void BtnCheckItems_Click(object sender, System.EventArgs e)
        {
            var items = ItemRepository.Get();

            if (items.Count < 164)
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