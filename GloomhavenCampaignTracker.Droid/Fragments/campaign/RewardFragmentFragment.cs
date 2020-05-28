using System;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class RewardFragmentFragment : CampaignDetailsFragmentBase
    {
        protected Button _decreaseprospButton;
        protected Button _raiseprospbutton;
        protected Button _decreaseReputationButton;
        protected Button _raiseReputationButton;
        protected Button _achievementGAButton;
        protected Button _achievementPAButton;
        protected TextView _prospLevelText;
        protected TextView _reputationTextView;

        private int _reputaionModifier = 0;
        private int _prospertityModifier = 0;

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.saveMenu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            try
            {
                if (item.ItemId == Resource.Id.men_save)
                {
                    Save();
                }
            }
            catch
            {
                return base.OnOptionsItemSelected(item);
            }

            return base.OnOptionsItemSelected(item);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            HasOptionsMenu = true;

            return view;
        }

        protected void InitTextViews()
        {
            if (GCTContext.CurrentCampaign.CurrentParty.Reputation == 20)
            {
                _reputationTextView.Text = "Max";
            }

            if (GCTContext.CurrentCampaign.CurrentParty.Reputation == -20)
            {
                _reputationTextView.Text = "Min";
            }

            if (GCTContext.CurrentCampaign.CityProsperity == 65)
            {
                _prospLevelText.Text = "Max";
            }

            if (GCTContext.CurrentCampaign.CityProsperity == 1)
            {
                _prospLevelText.Text = "Min";
            }
        }

        protected void AchievementPAButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(Activity, typeof(DetailsActivity));
            intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.PartyAchievements);
            StartActivity(intent);
        }

        protected void AchievementGAButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(Activity, typeof(DetailsActivity));
            intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.GlobalAchievements);
            StartActivity(intent);
        }

        protected void RoadEventButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(Activity, typeof(DetailsActivity));
            intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.Roadevents);
            StartActivity(intent);
        }

        protected void CityEventButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(Activity, typeof(DetailsActivity));
            intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.Cityevents);
            StartActivity(intent);
        }

        protected void RiftEventButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(Activity, typeof(DetailsActivity));
            intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.Riftevents);
            StartActivity(intent);
        }

        protected void RaiseReputationButton_Click(object sender, EventArgs e)
        {
            if (GCTContext.CurrentCampaign.CurrentParty.Reputation + _reputaionModifier >= 20)
            {
                _reputationTextView.Text = "Max";
                return;
            }
            
            _reputationTextView.Text = (_reputaionModifier += 1).ToString();
        }

        protected void DecreaseReputationButton_Click(object sender, EventArgs e)
        {
            if (GCTContext.CurrentCampaign.CurrentParty.Reputation + _reputaionModifier <= -20)
            {
                _reputationTextView.Text = "Min";
                return;
            }
            _reputationTextView.Text = (_reputaionModifier -= 1).ToString();
        }

        protected void Raiseprospbutton_Click(object sender, EventArgs e)
        {
            if (GCTContext.CurrentCampaign.CityProsperity + _prospertityModifier >= 65)
            {
                _prospLevelText.Text = "Max";
                return;
            }
            _prospLevelText.Text = (_prospertityModifier += 1).ToString();
        }

        protected void DecreaseprospButton_Click(object sender, EventArgs e)
        {
            if (GCTContext.CurrentCampaign.CityProsperity + _prospertityModifier <= 1)
            {
                _prospLevelText.Text = "Min";
                return;
            }
            _prospLevelText.Text = (_prospertityModifier -= 1).ToString();
        }

        protected virtual void Save()
        {
            if (int.TryParse(_prospLevelText.Text, out int prospChange)) GCTContext.CurrentCampaign.CityProsperity += prospChange;
            if (int.TryParse(_reputationTextView.Text, out int repChange)) GCTContext.CurrentCampaign.CurrentParty.Reputation += repChange;

            GCTContext.CurrentCampaign.Save();

            Activity.Finish();
        }
    }
}