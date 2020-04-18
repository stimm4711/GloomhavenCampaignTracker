using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign.unlocks
{
    public class CampaignUnlocksEnvelopeXFragment : CampaignDetailsFragmentBase
    {
        private CheckBox _decryptedEnvXCheck;
        private EditText _theirnameEditText;
        private Button _savetheirnamelettersButton;
        private LinearLayout _spoilerlayout;

        public static CampaignUnlocksEnvelopeXFragment NewInstance()
        {
            var frag = new CampaignUnlocksEnvelopeXFragment { Arguments = new Bundle() };
            return frag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {            
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_envelopeXUnlock, container, false);
            _decryptedEnvXCheck = _view.FindViewById<CheckBox>(Resource.Id.decryptedEnvXCheck);
            _theirnameEditText = _view.FindViewById<EditText>(Resource.Id.theirnameEditText);
            _savetheirnamelettersButton = _view.FindViewById<Button>(Resource.Id.savetheirnamelettersButton);
            _spoilerlayout = _view.FindViewById<LinearLayout>(Resource.Id.spoilerlayout);

            var unlocks = Campaign.CampaignData.CampaignUnlocks;

            if(!string.IsNullOrEmpty(unlocks.EnvelopeXSolution))
            {
                _theirnameEditText.Text = unlocks.EnvelopeXSolution;
            }

            _decryptedEnvXCheck.Checked = unlocks.EnvelopeXEncrypted;

            _decryptedEnvXCheck.CheckedChange += _decryptedEnvXCheck_CheckedChange;
            _savetheirnamelettersButton.Click += _savetheirnamelettersButton_Click;

            UpdateLayout();

            return _view;
        }

        private void UpdateLayout()
        {
            if(_decryptedEnvXCheck.Checked)
            {
                _spoilerlayout.Visibility = ViewStates.Visible;
            }
            else
            {
                _spoilerlayout.Visibility = ViewStates.Gone;
            }

            if(GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.HiddenClassUnlocked)
            {
                _view.FindViewById<TextView>(Resource.Id.envelopex_congratulationsText).Visibility = ViewStates.Visible;
            }
        }

        private void _savetheirnamelettersButton_Click(object sender, System.EventArgs e)
        {
            Campaign.CampaignData.CampaignUnlocks.EnvelopeXSolution = _theirnameEditText.Text;

            if (_theirnameEditText.Text.ToLower() == Resources.GetString(Resource.String.classes_bladeswarmname).ToLower())
            {
                GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.HiddenClassUnlocked = true;
                GCTContext.CurrentCampaign.AddUnlockedClass(18);
                CampaignUnlocksRepository.InsertOrReplace(GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks);

                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                  .SetTitle(Resources.GetString(Resource.String.Congratulation))
                  .SetMessage(Resources.GetString(Resource.String.unlocks_envelopex_completeddialogmessage))
                  .SetPositiveButton(Resources.GetString(Resource.String.Reward), (senderAlert, args) => {
                      var uri = Android.Net.Uri.Parse(Resources.GetString(Resource.String.Bladeswarm_web));
                      var intent = new Intent(Intent.ActionView, uri);
                      StartActivity(intent);
                  })
                  .SetNegativeButton(Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                  .Show();

                UpdateLayout();
            }
            else
            {
                Toast.MakeText(Context, Resources.GetString(Resource.String.unlocks_envelopex_nothinghappens), ToastLength.Short).Show();
            }
        }

        private void _decryptedEnvXCheck_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Campaign.CampaignData.CampaignUnlocks.EnvelopeXEncrypted = e.IsChecked;          
            UpdateLayout();
        }
    }
}