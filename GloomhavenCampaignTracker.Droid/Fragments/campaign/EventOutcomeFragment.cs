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
    public class EventOutcomeFragment : CampaignDetailsFragmentBase
    {
        private TabLayout _tabLayout;
        private ViewPager _viewPager;
        private EvenOutcomeCharacterViewPagerAdapter _adapter;
        private Button _decreaseprospButton;
        private Button _raiseprospbutton;
        private Button _decreaseReputationButton;
        private Button _raiseReputationButton;
        private Button _achievementGAButton;
        private Button _achievementPAButton;
        private TextView _prospLevelText;
        private TextView _reputationTextView;
        private Button _btnAddScenario;
        private EventTypes _eventType;
        private int _cardId;
        private int _cardOption;

        private int _reputaionModifier = 0;
        private int _prospertityModifier = 0;

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

        public override void OnResume()
        {
            base.OnResume();
        }

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
            base.OnCreateView(inflater, container, savedInstanceState);
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_event_rewards, container, false);
            _viewPager = _view.FindViewById<ViewPager>(Resource.Id.characterrewardsviewpager);
            _tabLayout = _view.FindViewById<TabLayout>(Resource.Id.rewards_characters_tabs);
            _decreaseprospButton = _view.FindViewById<Button>(Resource.Id.decreaseprospButton);
            _raiseprospbutton = _view.FindViewById<Button>(Resource.Id.raiseprospbutton);
            _decreaseReputationButton = _view.FindViewById<Button>(Resource.Id.decreaseReputationButton);
            _raiseReputationButton = _view.FindViewById<Button>(Resource.Id.raiseReputationButton);
            _achievementGAButton = _view.FindViewById<Button>(Resource.Id.btnGA);
            _achievementPAButton = _view.FindViewById<Button>(Resource.Id.btnPA);
            _prospLevelText = _view.FindViewById<TextView>(Resource.Id.prospLevelText);
            _reputationTextView = _view.FindViewById<TextView>(Resource.Id.reputationTextView);
            _btnAddScenario = _view.FindViewById<Button>(Resource.Id.btnAddScenario);

            var characters = CharacterRepository.GetPartymembersFlat(GCTContext.CurrentCampaign.CurrentParty.Id).Where(x => !x.Retired).ToList();
            GCTContext.CharacterCollection = characters;
            _adapter = new EvenOutcomeCharacterViewPagerAdapter(Context, ChildFragmentManager, characters);
            _viewPager.Adapter = _adapter;
            _tabLayout.SetupWithViewPager(_viewPager);

            GetEventDecisionBackImageAndSetImageView(EventType, SelectedOption(), CardNumber());

            if (!_decreaseprospButton.HasOnClickListeners)
            {
                _decreaseprospButton.Click += _decreaseprospButton_Click; ;
            }

            if (!_raiseprospbutton.HasOnClickListeners)
            {
                _raiseprospbutton.Click += _raiseprospbutton_Click; ;
            }

            if (!_decreaseReputationButton.HasOnClickListeners)
            {
                _decreaseReputationButton.Click += _decreaseReputationButton_Click; ;
            }

            if (!_raiseReputationButton.HasOnClickListeners)
            {
                _raiseReputationButton.Click += _raiseReputationButton_Click; ;
            }

            if (!_achievementGAButton.HasOnClickListeners)
            {
                _achievementGAButton.Click += _achievementGAButton_Click; ;
            }

            if (!_achievementPAButton.HasOnClickListeners)
            {
                _achievementPAButton.Click += _achievementPAButton_Click; ;
            }

            if (!_btnAddScenario.HasOnClickListeners)
            {
                _btnAddScenario.Click += _btnAddScenario_Click;
            }

            HasOptionsMenu = true;

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

        private void _btnAddScenario_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(Activity, typeof(DetailsActivity));
            intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.UnlockedScenarios);
            StartActivity(intent);
        }

        private void _achievementPAButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(Activity, typeof(DetailsActivity));
            intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.PartyAchievements);
            StartActivity(intent);
        }

        private void _achievementGAButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(Activity, typeof(DetailsActivity));
            intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.GlobalAchievements);
            StartActivity(intent);
        }

        private void _raiseReputationButton_Click(object sender, EventArgs e)
        {
            _reputationTextView.Text = (_reputaionModifier += 1).ToString();
        }

        private void _decreaseReputationButton_Click(object sender, EventArgs e)
        {
            _reputationTextView.Text = (_reputaionModifier -= 1).ToString();
        }

        private void _raiseprospbutton_Click(object sender, EventArgs e)
        {
            _prospLevelText.Text = (_prospertityModifier += 1).ToString();
        }

        private void _decreaseprospButton_Click(object sender, EventArgs e)
        {
            _prospLevelText.Text = (_prospertityModifier -= 1).ToString();
        }

        private void Save()
        {
            _adapter.SaveCharacterRewards();

            if (int.TryParse(_prospLevelText.Text, out int prospChange)) GCTContext.CurrentCampaign.CityProsperity += prospChange;
            if (int.TryParse(_reputationTextView.Text, out int repChange)) GCTContext.CurrentCampaign.CurrentParty.Reputation += repChange;

            GCTContext.CurrentCampaign.Save();

            Activity.Finish();
        }
    }
}