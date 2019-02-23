using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.Linq;
using System.Threading.Tasks;
using GloomhavenCampaignTracker.Business.Network;
using GloomhavenCampaignTracker.Droid.CustomControls;
using Android.Content;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign.unlocks
{
    public class CampaignUnlocksFragment : CampaignFragmentBase
    {
        private CheckBox _checkAncientTech;
        private CheckBox _checkdrakepartyachievementCheck;
        private CheckBox _donationsCheck;
        private CheckBox _partyRep10Check;
        private CheckBox _partyRep20Check;
        private CheckBox _partyRepMinus10Check;
        private CheckBox _partyRepMinus20Check;
        private CheckBox _retireCheck;
        private TextView _checkAncientText;
        private TextView _checkdrakepartyachievementText;
        private TextView _donationsText;
        private TextView _partyRep10Text;
        private TextView _partyRep20Text;
        private TextView _partyRepMinus10Text;
        private TextView _partyRepMinus20Text;
        private TextView _retireText;
        private GridView _grid;
        private Button _unlockEnvelopeXButton;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnStop()
        {
            if(_dataChanged)
            {
                CampaignUnlocksRepository.InsertOrReplace(GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks);
                _dataChanged = false;
            }
            base.OnStop();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_unlocks, container, false);

            base.OnCreateView(inflater, container, savedInstanceState);

            // get checkbox views
            _checkAncientTech = _view.FindViewById<CheckBox>(Resource.Id.ancienttechnologyUnlockCheck);
            _checkdrakepartyachievementCheck = _view.FindViewById<CheckBox>(Resource.Id.drakepartyachievementCheck);
            _donationsCheck = _view.FindViewById<CheckBox>(Resource.Id.donationsCheck);
            _partyRep10Check = _view.FindViewById<CheckBox>(Resource.Id.partyRep10Check);
            _partyRep20Check = _view.FindViewById<CheckBox>(Resource.Id.partyRep20Check);
            _partyRepMinus10Check = _view.FindViewById<CheckBox>(Resource.Id.partyRepMinus10Check);
            _partyRepMinus20Check = _view.FindViewById<CheckBox>(Resource.Id.partyRepMinus20Check);
            _retireCheck = _view.FindViewById<CheckBox>(Resource.Id.retireCheck);

            // get textviews
            _checkAncientText = _view.FindViewById<TextView>(Resource.Id.gainEnvAText);
            _checkdrakepartyachievementText = _view.FindViewById<TextView>(Resource.Id.gainDrakeAchievementText);
            _donationsText = _view.FindViewById<TextView>(Resource.Id.gainEnvBText);
            _partyRep10Text = _view.FindViewById<TextView>(Resource.Id.gainSunClassText);
            _partyRep20Text = _view.FindViewById<TextView>(Resource.Id.gainRep20Text);
            _partyRepMinus10Text = _view.FindViewById<TextView>(Resource.Id.gainClassMoonText);
            _partyRepMinus20Text = _view.FindViewById<TextView>(Resource.Id.gainRepMinus20Text);
            _retireText = _view.FindViewById<TextView>(Resource.Id.gainRetireText);
            _grid = _view.FindViewById<GridView>(Resource.Id.imagesGridView);
            _unlockEnvelopeXButton = _view.FindViewById<Button>(Resource.Id.openEnvelopeXButton);

            if (CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeXUnlocked)
            {
                _unlockEnvelopeXButton.Text = Resources.GetString(Resource.String.ShowEnvelopeXProgress);
            }

            if (!_checkAncientTech.HasOnClickListeners)
            {
                _checkAncientTech.Click += AncientTechCheckBoxClick;
            }

            if (!_checkdrakepartyachievementCheck.HasOnClickListeners)
            {
                _checkdrakepartyachievementCheck.Click += DrakesCheckBoxClick;
            }

            if (!_donationsCheck.HasOnClickListeners)
            {
                _donationsCheck.Click += CheckBoxClick;
            }

            if (!_partyRep10Check.HasOnClickListeners)
            {
                _partyRep10Check.Click += CheckBoxClick;
            }

            if (!_partyRep20Check.HasOnClickListeners)
            {
                _partyRep20Check.Click += CheckBoxClick;
            }

            if (!_partyRepMinus10Check.HasOnClickListeners)
            {
                _partyRepMinus10Check.Click += CheckBoxClick;
            }

            if (!_partyRepMinus20Check.HasOnClickListeners)
            {
                _partyRepMinus20Check.Click += CheckBoxClick;
            }

            if (!_retireCheck.HasOnClickListeners)
            {
                _retireCheck.Click += _retireCheck_Click; ;
            }

            if(!_unlockEnvelopeXButton.HasOnClickListeners)
            {
                _unlockEnvelopeXButton.Click += _unlockEnvelopeXButton_Click;
            }

            SwitchViewStates();

            return _view;
        }

        private void _unlockEnvelopeXButton_Click(object sender, System.EventArgs e)
        {
            if(!CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeXUnlocked)
            {
                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                  .SetTitle(Resources.GetString(Resource.String.WarningSpoilersAhead))
                  .SetMessage(Resources.GetString(Resource.String.SpoilerWarningMessage))
                  .SetPositiveButton(Resources.GetString(Resource.String.Proceed), (senderAlert, args) => { ShowEnvelopeXProgress(); })
                  .SetNegativeButton(Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                  .Show();
            }
            else
            {
                ShowEnvelopeXProgress();
            }
        }

        private void ShowEnvelopeXProgress()
        {
            CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeXUnlocked = true;
            CampaignUnlocksRepository.InsertOrReplace(CurrentCampaign.CampaignData.CampaignUnlocks);
            ShowDetail(DetailFragmentTypes.EnvelopeXUnlock);            
        }

        internal void Update()
        {
            Task.Run(async () =>
            {
                await GloomhavenClient.Instance.UpdateCampaignUnlocks();

                Activity.RunOnUiThread(() =>
                {
                    SwitchViewStates();
                });
            });
        }

        private void _retireCheck_Click(object sender, System.EventArgs e)
        {
            if (_retireCheck.Checked)
            {
                if (CurrentCampaign.CampaignData.Parties.Any(x => CharacterRepository.GetPartymembers(x.Id).Any(y => y.Retired)))
                {
                    if (CurrentCampaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked != true)
                    {
                        CurrentCampaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked = true;
                        _dataChanged = true;
                    }
                }
                else
                {
                    _retireCheck.Checked = false;
                    if (CurrentCampaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked != false)
                    {
                        CurrentCampaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked = false;
                        _dataChanged = true;
                    }
                }
            }
            else
            {
                if (!CurrentCampaign.CampaignData.Parties.Any(x => CharacterRepository.GetPartymembers(x.Id).Any(y => y.Retired)))
                {
                    if (CurrentCampaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked != false)
                    {
                        CurrentCampaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked = false;
                        _dataChanged = true;
                    }
                }
                else
                {
                    _retireCheck.Checked = true;
                    if (CurrentCampaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked != true)
                    {
                        CurrentCampaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked = true;
                        _dataChanged = true;
                    }
                }
            }
        }

        private void CheckBoxClick(object sender, System.EventArgs e)
        {
            SwitchViewStates();
        }

        private void AncientTechCheckBoxClick(object sender, System.EventArgs e)
        {
            if (CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeAUnlocked)
            {
                if (!CurrentCampaign.HasGlobalAchievement((int)GlobalAchievementsInternalNumbers.AncientTechnology_Step5))
                {
                    CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeAUnlocked = false;
                    _dataChanged = true;
                }
            }

            SwitchViewStates();
        }

        private void DrakesCheckBoxClick(object sender, System.EventArgs e)
        {
            if (CurrentCampaign.CampaignData.CampaignUnlocks.TheDrakePartyAchievementsUnlocked)
            {
                if (!CurrentCampaign.HasTheDrakesPartyAchievements())
                {
                    CurrentCampaign.CampaignData.CampaignUnlocks.TheDrakePartyAchievementsUnlocked = false;
                    _dataChanged = true;
                }
            }

            SwitchViewStates();
        }

        /// <summary>
        /// Set checkbox checked and textview visible states and set characterclass adapter for class icons
        /// </summary>
        public void SwitchViewStates()
        {
            _grid.Adapter = new CharacterClassAdapter(Context, true);

            _checkAncientTech.Checked = CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeAUnlocked;
            _checkdrakepartyachievementCheck.Checked = CurrentCampaign.CampaignData.CampaignUnlocks.TheDrakePartyAchievementsUnlocked;
            _donationsCheck.Checked = CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeBUnlocked;
            _partyRep10Check.Checked = CurrentCampaign.CampaignData.CampaignUnlocks.ReputationPlus10Unlocked;
            _partyRep20Check.Checked = CurrentCampaign.CampaignData.CampaignUnlocks.ReputationPlus20Unlocked;
            _partyRepMinus10Check.Checked = CurrentCampaign.CampaignData.CampaignUnlocks.ReputationMinus10Unlocked;
            _partyRepMinus20Check.Checked = CurrentCampaign.CampaignData.CampaignUnlocks.ReputationMinus20Unlocked;
            _retireCheck.Checked = CurrentCampaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked;

            _checkAncientText.Visibility = _checkAncientTech.Checked ? ViewStates.Visible : ViewStates.Invisible;

            _checkdrakepartyachievementText.Visibility = _checkdrakepartyachievementCheck.Checked ? ViewStates.Visible : ViewStates.Invisible;

            _donationsText.Visibility = _donationsCheck.Checked ? ViewStates.Visible : ViewStates.Invisible;

            _partyRep10Text.Visibility = _partyRep10Check.Checked ? ViewStates.Visible : ViewStates.Invisible;

            _partyRep20Text.Visibility = _partyRep20Check.Checked ? ViewStates.Visible : ViewStates.Invisible;

            _partyRepMinus10Text.Visibility = _partyRepMinus10Check.Checked ? ViewStates.Visible : ViewStates.Invisible;

            _partyRepMinus20Text.Visibility = _partyRepMinus20Check.Checked ? ViewStates.Visible : ViewStates.Invisible;

            _retireText.Visibility = _retireCheck.Checked ? ViewStates.Visible : ViewStates.Invisible;
        }
    }
}