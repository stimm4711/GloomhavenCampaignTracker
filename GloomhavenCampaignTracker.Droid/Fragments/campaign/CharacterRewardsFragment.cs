using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Fragments.character;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public abstract class CharacterRewardsFragment : CharacterDetailFragmentBase
    {
        protected EditText _xpEditText;
        protected EditText _goldEditText;
        protected CheckBox _check1;
        protected CheckBox _check2;
        protected TextView _xpTotalText;
        protected TextView _goldTotalText;
        protected TextView _checkTotalText;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        internal void Save()
        {
            Character.Checkmarks = NewTotalCheckmarks();
            Character.Gold = NewTotalGold();
            Character.Experience = NewTotalXP();

            CharacterRepository.InsertOrReplace(Character);
        }

        /*
         * XP
         */

        protected void XpEditText_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus) return;
            if (string.IsNullOrEmpty(_xpEditText.Text)) return;
            SetTotalXP();
        }

        protected void SetTotalXP()
        {
            _xpTotalText.Text = NewTotalXP().ToString();
        }

        protected int NewTotalXP()
        {
            if (!int.TryParse(_xpEditText.Text, out int newXp)) return Character.Experience;
            return Math.Min(500, Math.Max(0, Character.Experience + newXp));
        }

        /*
         * Gold
         */

        protected void GoldEditText_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus) return;
            if (string.IsNullOrEmpty(_goldEditText.Text)) return;
            SetTotalGold();
        }

        protected void SetTotalGold()
        {
            _goldTotalText.Text = NewTotalGold().ToString();
        }

        protected virtual int NewTotalGold()
        {
            if (!int.TryParse(_goldEditText.Text, out int newGold)) return Character.Gold;
            return Math.Max(0, Character.Gold + newGold);
        }

        /*
         * Checkmarks
         */

        protected void Check_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            SetTotalCheckmarks();
        }

        protected void SetTotalCheckmarks()
        {
            _checkTotalText.Text = NewTotalCheckmarks().ToString();
        }

        protected virtual int NewTotalCheckmarks()
        {
            return Math.Min(18, Math.Max(0,Character.Checkmarks + CalcNewCHeckmarks()));
        }

        protected abstract int CalcNewCHeckmarks();
    }
}