using System;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class EventOutcomeFragment : RewardFragmentFragment
    {
        private EvenOutcomeCharacterViewPagerAdapter _adapter;
        private Button _btnAddScenario;
        private EventTypes _eventType;
        private int _cardId;
        private int _cardOption;

        internal static EventOutcomeFragment NewInstance(int cardnumber, int selectedOption, int eventtype)
        {
            var frag = new EventOutcomeFragment { Arguments = new Bundle() };
            frag.Arguments.PutInt(DetailsActivity.EventTypeString, eventtype);
            frag.Arguments.PutInt(DetailsActivity.EventCardNumber, cardnumber);
            frag.Arguments.PutInt(DetailsActivity.EventCardOption, selectedOption);
            return frag;
        }

        public EventTypes EventType
        {
            get
            {
                if (Arguments == null) return _eventType;
                var eventTypeId = Arguments.GetInt(DetailsActivity.EventTypeString, 1);
                _eventType = (EventTypes)eventTypeId;

                return _eventType;
            }
        }

        public int CardNumber()
        {
            if (Arguments == null) return _cardId;
            _cardId = Arguments.GetInt(DetailsActivity.EventCardNumber, 1);
            return _cardId;
        }

        public int SelectedOption()
        {
            if (Arguments == null) return _cardOption;
            _cardOption = Arguments.GetInt(DetailsActivity.EventCardOption, 1);
            return _cardOption;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_event_rewards, container, false);
                     
            _decreaseprospButton = _view.FindViewById<Button>(Resource.Id.decreaseprospButton);
            _raiseprospbutton = _view.FindViewById<Button>(Resource.Id.raiseprospbutton);
            _decreaseReputationButton = _view.FindViewById<Button>(Resource.Id.decreaseReputationButton);
            _raiseReputationButton = _view.FindViewById<Button>(Resource.Id.raiseReputationButton);
            _achievementGAButton = _view.FindViewById<Button>(Resource.Id.btnGA);
            _achievementPAButton = _view.FindViewById<Button>(Resource.Id.btnPA);
            _prospLevelText = _view.FindViewById<TextView>(Resource.Id.prospLevelText);
            _reputationTextView = _view.FindViewById<TextView>(Resource.Id.reputationTextView);
            _btnAddScenario = _view.FindViewById<Button>(Resource.Id.btnAddScenario);

            var viewPager = _view.FindViewById<ViewPager>(Resource.Id.characterrewardsviewpager);
            var tabLayout = _view.FindViewById<TabLayout>(Resource.Id.rewards_characters_tabs);

            var btnRoadEvent = _view.FindViewById<Button>(Resource.Id.btnRoadE);
            var btnCityEvent = _view.FindViewById<Button>(Resource.Id.btnCityE);
            var btnRiftEvent = _view.FindViewById<Button>(Resource.Id.btnRiftE);

            var characters = CharacterRepository.GetPartymembersFlat(GCTContext.CurrentCampaign.CurrentParty.Id).Where(x => !x.Retired).ToList();
            GCTContext.CharacterCollection = characters;
            
            _adapter = new EvenOutcomeCharacterViewPagerAdapter(Context, ChildFragmentManager, characters);
            viewPager.Adapter = _adapter;
            tabLayout.SetupWithViewPager(viewPager);

            GetEventDecisionBackImageAndSetImageView(EventType, SelectedOption(), CardNumber());

            if (!_decreaseprospButton.HasOnClickListeners)
            {
                _decreaseprospButton.Click += DecreaseprospButton_Click; ;
            }

            if (!_raiseprospbutton.HasOnClickListeners)
            {
                _raiseprospbutton.Click += Raiseprospbutton_Click; ;
            }

            if (!_decreaseReputationButton.HasOnClickListeners)
            {
                _decreaseReputationButton.Click += DecreaseReputationButton_Click; ;
            }

            if (!_raiseReputationButton.HasOnClickListeners)
            {
                _raiseReputationButton.Click += RaiseReputationButton_Click; ;
            }

            if (!_achievementGAButton.HasOnClickListeners)
            {
                _achievementGAButton.Click += AchievementGAButton_Click; ;
            }

            if (!_achievementPAButton.HasOnClickListeners)
            {
                _achievementPAButton.Click += AchievementPAButton_Click; ;
            }

            if (!_btnAddScenario.HasOnClickListeners)
            {
                _btnAddScenario.Click += BtnAddScenario_Click;
            }

            btnRoadEvent.Click += RoadEventButton_Click;
            btnCityEvent.Click += CityEventButton_Click;
            btnRiftEvent.Click += RiftEventButton_Click;

            var uiLayout = _view.FindViewById<LinearLayout>(Resource.Id.uiLayout);
            if (EventType == EventTypes.RoadEvent)
            {                
                uiLayout.SetBackgroundResource(Resource.Drawable.bg_roadevent);
            }
            else if(EventType == EventTypes.CityEvent)
            {
                uiLayout.SetBackgroundResource(Resource.Drawable.bg_cityevent);
            }
            else if (EventType == EventTypes.RiftEvent)
            {
                uiLayout.SetBackgroundResource(Resource.Drawable.bg_riftevent);
            }

            InitTextViews();

            return _view;
        }

        private async void GetEventDecisionBackImageAndSetImageView(EventTypes eventType, int decision, int cardId)
        {
            var pqimage = _view.FindViewById<ImageView>(Resource.Id.itemimage);

            string cardIdText = _cardId.ToString();
            if (cardId < 10)
            {
                cardIdText = $"0{cardId}";
            }

            string eventURL = Helper.GetEventBackURL(eventType, cardIdText);
            var eventback = await Helper.GetImageBitmapFromUrlAsync(eventURL);

            pqimage.SetImageBitmap(Helper.GetEventBackScaleBitmap(eventback, decision));
        }

        private void BtnAddScenario_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(Activity, typeof(DetailsActivity));
            intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.UnlockedScenarios);
            StartActivity(intent);
        }            

        protected override void Save()
        {
            if (!_saved)
            {
                _saved = true;
                _adapter.SaveCharacterRewards();
                base.Save();
            }
            else
            {
                Toast.MakeText(Context, "Rewards were already saved!", ToastLength.Long).Show();
            }
        }
    }
}