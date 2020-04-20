using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using System.Threading.Tasks;
using GloomhavenCampaignTracker.Business.Network;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign.city
{
    public class CampaignCityFragment : CampaignFragmentBase
    {
        private ProsperityMeter _prosperityMeter;
        private TextView _cityProsperityTextView;
        private TextView _campaignDonatedGold;
        private TextView _prosperityLevelProgress;
        private bool _isItemStoreSelected;

        public CampaignCityFragment()
        {}

        public CampaignCityFragment( FragmentManager fm ) : base( fm) { }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);          
            _isItemStoreSelected = false;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            _view = inflater.Inflate(Resource.Layout.fragment_campaign_city, container, false);

            if (_view == null) return _view;

            var detailsFrame = _view.FindViewById<View>(Resource.Id.frame_details_city);
            _isDualPane = detailsFrame != null && detailsFrame.Visibility == ViewStates.Visible;

            _prosperityMeter = _view.FindViewById<ProsperityMeter>(Resource.Id.karmaMeter);
            _cityProsperityTextView = _view.FindViewById<TextView>(Resource.Id.campaignCity_prospLevelText);
            _prosperityLevelProgress = _view.FindViewById<TextView>(Resource.Id.progessTextView);
            _campaignDonatedGold = _view.FindViewById<TextView>(Resource.Id.donatedGoldText);

            var addDonationButton = _view.FindViewById<Button>(Resource.Id.raisedonationbutton);
            var removeDonationButton = _view.FindViewById<Button>(Resource.Id.decreasedonationbutton);
            var showItemStoreButton = _view.FindViewById<Button>(Resource.Id.itemstorebutton);
            var showCityEventsButton = _view.FindViewById<Button>(Resource.Id.cityeventsbutton);
            var decreaseCityProperity = _view.FindViewById<Button>(Resource.Id.decreaseprospButton);
            var increaseCityProsperity = _view.FindViewById<Button>(Resource.Id.raiseprospbutton);
            var donationsForProsperity = _view.FindViewById<TextView>(Resource.Id.textView2);

            if (_isDualPane)
            {
                _dualtdetailLayout = _view.FindViewById<LinearLayout>(Resource.Id.dualdetaillayout);
                if (_dualtdetailLayout != null) _dualtdetailLayout.Visibility = ViewStates.Invisible;
            }

            ShowHiddenDonationRegion();

            if (decreaseCityProperity != null && !decreaseCityProperity.HasOnClickListeners)
            {
                decreaseCityProperity.Click += (sender, e) =>
                {
                    _dataChanged = true;
                    DecreaseCityProsperity();
                };
            }

            if (increaseCityProsperity != null && !increaseCityProsperity.HasOnClickListeners)
            {
                increaseCityProsperity.Click += (sender, e) =>
                {
                    _dataChanged = true;
                    IncreaseCityProsperity();
                };
            }

            if (addDonationButton != null && !addDonationButton.HasOnClickListeners)
            {
                addDonationButton.Click += (sender, e) =>
                {
                    _dataChanged = true;
                    AddDonation();
                };
            }

            if (removeDonationButton != null && !removeDonationButton.HasOnClickListeners)
            {
                removeDonationButton.Click += (sender, e) =>
                {
                    _dataChanged = true;
                    RemoveDonation();
                };
            }

            if (showItemStoreButton != null && !showItemStoreButton.HasOnClickListeners)
            {
                showItemStoreButton.Click += (sender, args) =>
                {
                    _isItemStoreSelected = true;
                    ShowDetail(DetailFragmentTypes.Itemstore);
                };
            }

            if (showCityEventsButton != null && !showCityEventsButton.HasOnClickListeners)
            {
                showCityEventsButton.Click += (sender, args) =>
                {
                    _isItemStoreSelected = false;
                    ShowDetail(DetailFragmentTypes.Cityevents);
                };
            }

            UpdateProsperityViews();
            UpdateHiddenRegion();

            return _view;
        }

        internal void Update()
        {
            var prosperityLevel = CurrentCampaign.GetProsperityLevel();
            _cityProsperityTextView.Text = (prosperityLevel).ToString();
            UpdateHiddenRegion();
            _campaignDonatedGold.Text = CurrentCampaign.CampaignData.DonatedGold.ToString();
        }
       
        private void ShowHiddenDonationRegion()
        {
            var sanctuaryProsperityLayout = _view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);
            if (CurrentCampaign.CampaignData.DonatedGold >= 100 && CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeBUnlocked)
            {
                sanctuaryProsperityLayout.Visibility = ViewStates.Visible;
            }
            else
            {
                sanctuaryProsperityLayout.Visibility = ViewStates.Gone;
            }
        }

        private void UpdateHiddenRegion()
        {
            var donationText = _view.FindViewById<TextView>(Resource.Id.textView1);
            var nextProsperity = Helper.GetNextDonationValueForProsperity(GCTContext.CurrentCampaign.CampaignData.DonatedGold);
            donationText.Text = $"{nextProsperity}";            
        }

        private void RemoveDonation()
        {
            CurrentCampaign.RemoveDonationToTheSanctuary();
            CheckForProsperityDowngrade();
            if (CurrentCampaign.CampaignData.DonatedGold < 100 && CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeBUnlocked)
            {
                GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeBUnlocked = false;

                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                    .SetTitle("Removed unlock")
                    .SetMessage("Removed the content of Envelope B.")
                    .SetPositiveButton(Resources.GetString(Resource.String.OK), (senderAlert, args) => { })
                    .Show();
            }
            _campaignDonatedGold.Text = CurrentCampaign.CampaignData.DonatedGold.ToString();

            UpdateHiddenRegion();
            ShowHiddenDonationRegion();
        }

        private void AddDonation()
        {
            CurrentCampaign.AddDonationToTheSanctuary();
            if (CurrentCampaign.CampaignData.DonatedGold == 100 && !CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeBUnlocked)
            {               
                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)                
                    .SetTitle(Resources.GetString(Resource.String.Congratulation))
                    .SetMessage("You have unlocked Envelope B by donating a total of 100 gold. Select reveal to add its content. Cancel if you didn't unlock Envelope B.")
                    .SetPositiveButton("Reveal", (senderAlert, args) => {
                        GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeBUnlocked = true;
                        UpdateHiddenRegion();
                        ShowHiddenDonationRegion();
                        CheckForProsperityUpgrade();
                        _campaignDonatedGold.Text = CurrentCampaign.CampaignData.DonatedGold.ToString();
                    })
                    .SetNegativeButton("Cancel", (senderAlert, arg) =>
                    {
                        CurrentCampaign.RemoveDonationToTheSanctuary();
                        _campaignDonatedGold.Text = CurrentCampaign.CampaignData.DonatedGold.ToString();
                        ShowHiddenDonationRegion();
                    })
                    .Show();
            }
            else
            {
                CheckForProsperityUpgrade();
                UpdateHiddenRegion();
                _campaignDonatedGold.Text = CurrentCampaign.CampaignData.DonatedGold.ToString();
            }    
        }

        private void CheckForProsperityUpgrade()
        {
            var last = Helper.GetNextDonationValueForProsperity(GCTContext.CurrentCampaign.CampaignData.DonatedGold-10);
            if(last == GCTContext.CurrentCampaign.CampaignData.DonatedGold)
            {
                var textplusprosperity = _view.FindViewById<TextView>(Resource.Id.plusprosperity);

                textplusprosperity.Visibility = ViewStates.Visible;
                Animation animation = AnimationUtils.LoadAnimation(Context, Resource.Animation.alphaanimation);
                animation.AnimationStart += (sender, args) =>
                {
                    IncreaseCityProsperity();
                    textplusprosperity.Visibility = ViewStates.Invisible;
                };
                textplusprosperity.StartAnimation(animation);              
            }
        }


        private void CheckForProsperityDowngrade()
        {
            var last = Helper.GetNextDonationValueForProsperity(GCTContext.CurrentCampaign.CampaignData.DonatedGold);
            if (last == GCTContext.CurrentCampaign.CampaignData.DonatedGold+10)
            {
                DecreaseCityProsperity();
            }
        }

        private void IncreaseCityProsperity()
        {
            CurrentCampaign.IncreaseCityProsperity();            
            UpdateProsperityViews();
        }

        private void DecreaseCityProsperity()
        {
            CurrentCampaign.DecreaseCityProsperity();
            UpdateProsperityViews();
        }

        private void UpdateProsperityViews()
        {
            if (CurrentCampaign == null) return;

            var prosperityLevel = CurrentCampaign.GetProsperityLevel();
            _cityProsperityTextView.Text = (prosperityLevel).ToString();

            var prosperityMeterValue = CurrentCampaign.GetProsperityStepValue(prosperityLevel);
            _prosperityMeter.SetProsperityValue(prosperityMeterValue, true);

            _campaignDonatedGold.Text = $"{CurrentCampaign.CampaignData.DonatedGold}";

            if (_isDualPane && _isItemStoreSelected)
            {
                ShowDetail(DetailFragmentTypes.Itemstore);
            }

            _prosperityLevelProgress.Text = $"{CurrentCampaign.CityProsperity-1} / { Helper.GetStepsToNextLevel(prosperityLevel)-1}";
        }
    }
}