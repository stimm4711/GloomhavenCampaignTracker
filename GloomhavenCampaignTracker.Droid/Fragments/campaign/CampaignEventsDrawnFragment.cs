using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using Android.Content;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public partial class CampaignEventsDrawnFragment : CampaignDetailsFragmentBase
    {
        private EventTypes _eventType;
        private int _cardId;
        private EventDeck _eventDeck;

        public static CampaignEventsDrawnFragment NewInstance(int CardNumber, int eventType)
        {
            var frag = new CampaignEventsDrawnFragment { Arguments = new Bundle() };
            frag.Arguments.PutInt(DetailsActivity.EventTypeString, (int)eventType);
            frag.Arguments.PutInt(DetailsActivity.EventCardNumber, (int)CardNumber);
            return frag;
        }

        public EventTypes GetEventType
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

        public override void OnStop()
        {
            //GCTContext.CampaignCollection.CurrentCampaign.SetEventDeckString(_eventType);
            base.OnStop();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _eventType = GetEventType;
            _cardId = CardNumber();

            if (GCTContext.CampaignCollection.CurrentCampaign != null)
            {
                if (_eventType == EventTypes.CityEvent)
                {
                    _eventDeck = GCTContext.CampaignCollection.CurrentCampaign.CityEventDeck;
                }
                else if (_eventType == EventTypes.RoadEvent)
                {
                    _eventDeck = GCTContext.CampaignCollection.CurrentCampaign.RoadEventDeck;
                }
                else if (_eventType == EventTypes.RiftEvent)
                {
                    _eventDeck = GCTContext.CampaignCollection.CurrentCampaign.RiftEventDeck;
                }
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Show eventcard if there is internet connection
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_eventdrawn, container, false);
            var pqimage = _view.FindViewById<ImageView>(Resource.Id.itemimage);
            var turnButton = _view.FindViewById<Button>(Resource.Id.turn);
            var deciscion_layout = _view.FindViewById<LinearLayout>(Resource.Id.deciscion_layout);
            var result_layout = _view.FindViewById<LinearLayout>(Resource.Id.result_layout);
            var removeButton = _view.FindViewById<Button>(Resource.Id.remove_button);
            var putUnderButton = _view.FindViewById<Button>(Resource.Id.putunder);
            var radiooptionA = _view.FindViewById<RadioButton>(Resource.Id.optionA);
            var enterOutcomeButton = _view.FindViewById<Button>(Resource.Id.enterOutcome);

            deciscion_layout.Visibility = ViewStates.Visible;
            result_layout.Visibility = ViewStates.Gone;

            string cardIdText = _cardId.ToString();
            if (_cardId < 10)
            {
                cardIdText = $"0{_cardId}";
            }
           
            GetEventFrontAndSetImage(pqimage, cardIdText);

            removeButton.Click += (sender, args) =>
            {
                EventDrawn(_cardId, GetSelectedOptiuon(radiooptionA));
                RemoveCard(_cardId);
                Activity.Finish();
            };

            putUnderButton.Click += (sender, args) =>
            {
                EventDrawn(_cardId, GetSelectedOptiuon(radiooptionA));
                PutBack(_cardId);
                Activity.Finish();
            };

            enterOutcomeButton.Click += (sender, args) =>
            {
                var intent = new Intent();
                intent.SetClass(Activity, typeof(DetailsActivity));
                intent.PutExtra(DetailsActivity.EventCardNumber, _cardId);
                intent.PutExtra(DetailsActivity.EventTypeString, (int)_eventType);
                intent.PutExtra(DetailsActivity.EventCardOption, radiooptionA.Checked ? 0 : 1);
                intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.EventOutcome);
                StartActivity(intent);
            };

            turnButton.Click += (sender, args) =>
            {
                pqimage.SetImageDrawable(null);
                _view.FindViewById<ProgressBar>(Resource.Id.loadingPanel).Visibility = ViewStates.Visible;

                if (deciscion_layout.Visibility == ViewStates.Visible)
                {                   
                    GetEventDecisionBackImageAndSetImageView(pqimage, radiooptionA, cardIdText);

                    deciscion_layout.Visibility = ViewStates.Gone;
                    result_layout.Visibility = ViewStates.Visible;
                }
                else
                {
                    GetEventFrontAndSetImage(pqimage, cardIdText);

                    deciscion_layout.Visibility = ViewStates.Visible;
                    result_layout.Visibility = ViewStates.Gone;
                }
            };

            return _view;
        }

        private async void GetEventDecisionBackImageAndSetImageView(ImageView pqimage, RadioButton radiooptionA, string cardIdText)
        {
            int decision = radiooptionA.Checked ? 0 : 1;

            string eventtypeurl = Helper.GetEventFrontURL(_eventType);

            var eventback = await Helper.GetImageBitmapFromUrlAsync(eventtypeurl + cardIdText + "-b.png");

            pqimage.SetImageBitmap(Helper.GetEventBackScaleBitmap(eventback, decision));

            _view.FindViewById<ProgressBar>(Resource.Id.loadingPanel).Visibility = ViewStates.Gone;
        }

        private async void GetEventFrontAndSetImage(ImageView pqimage, string cardIdText)
        {
            string eventtypeurl = Helper.GetEventFrontURL(_eventType);
            string eventFrontURI = eventtypeurl + cardIdText + "-f.png";
            var eventFront = await Helper.GetImageBitmapFromUrlAsync(eventFrontURI);
            pqimage.SetImageBitmap(eventFront);
            _view.FindViewById<ProgressBar>(Resource.Id.loadingPanel).Visibility = ViewStates.Gone;
        }

        private static int GetSelectedOptiuon(RadioButton radiooptionA)
        {
            return radiooptionA.Checked ? 1 : 2;
        }     

        private void EventDrawn(int cardId, int selectedOption)
        {
            EventhistoryHelper.DrawnEventHistory(Context, cardId, (int) _eventType, selectedOption);
        }       

        private void RemoveCard(int cardId)
        {
            EventhistoryHelper.RemoveEventHistory(Context, cardId, (int)_eventType);
            _eventDeck.RemoveCard(cardId);
        }      

        private void PutBack(int cardId)
        {
            _eventDeck.RemoveCard(cardId);
            _eventDeck.PutBack(cardId);
        }
    }
}