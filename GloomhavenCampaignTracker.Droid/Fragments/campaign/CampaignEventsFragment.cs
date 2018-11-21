using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Shared;
using GloomhavenCampaignTracker.Shared.Business;
using GloomhavenCampaignTracker.Droid.CustomControls;
using System.Linq;
using Android.Support.Design.Widget;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public partial class CampaignEventsFragment : CampaignDetailsFragmentBase
    {
        private EventTypes _eventType;
        private Button _draw;
        private Button _add;
        private Button _remove;
        private EventDeck _eventDeck;
        private ListView _eventhistory;

        private const string EventTypeString = "current_eventtype";       

        public static CampaignEventsFragment NewInstance(int campId, EventTypes eventType)
        {
            var frag = new CampaignEventsFragment { Arguments = new Bundle() };
            frag.Arguments.PutInt(EventTypeString, (int)eventType);
            return frag;
        }

        public override void OnStop()
        {
            if(_dataChanged) GCTContext.CampaignCollection.CurrentCampaign.SetEventDeckString(_eventType);
            base.OnStop();
        }

        public CampaignEventsFragment()
        {
        }

        public CampaignEventsFragment(EventTypes eventType )
        {
            _eventType = eventType;            
        }

        public EventTypes GetEventType
        {
            get
            {
                if (Arguments == null) return _eventType;
                var eventTypeId = Arguments.GetInt(EventTypeString, 1);
                _eventType = (EventTypes)eventTypeId;

                return _eventType;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _eventType = GetEventType;

            if(GCTContext.CampaignCollection.CurrentCampaign != null)
            {
                _eventDeck = _eventType == EventTypes.RoadEvent ? GCTContext.CampaignCollection.CurrentCampaign.RoadEventDeck :
                                                                GCTContext.CampaignCollection.CurrentCampaign.CityEventDeck;
            }
           
            _view = inflater.Inflate(Resource.Layout.fragment_events, container, false);           
            _draw = _view.FindViewById<Button>(Resource.Id.drawEventCardButton);
            _add = _view.FindViewById<Button>(Resource.Id.addEventCardButton);
            _remove = _view.FindViewById<Button>(Resource.Id.removeEventCardButton);
            _eventhistory = _view.FindViewById<ListView>(Resource.Id.eventhistoryListView);
            if(_eventhistory != null) _eventhistory.Adapter = new CampaignEventHistoryAdapter(Context, _eventType);

            var fab = _view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            var initEventDeckButton = _view.FindViewById<ImageButton>(Resource.Id.initEventDeckButton);

            _dataChanged = false;

            if (initEventDeckButton != null && !initEventDeckButton.HasOnClickListeners)
            {
                initEventDeckButton.Click += (sender, e) =>
                {
                    new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                        .SetTitle("Initialize eventdeck for a new deck?")
                        .SetPositiveButton("Initialize", (senderAlert, args) =>
                        {
                            _dataChanged = true;
                            _eventDeck.InitializeDeck();
                            GCTContext.CurrentCampaign.InitializeEventDeck(_eventType);
                            _eventhistory.Adapter = new CampaignEventHistoryAdapter(Context, _eventType);
                        })
                        .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                        .Show();                   
                };
            }

            if (_draw != null && !_draw.HasOnClickListeners)
            {
                _draw.Click += (sender, e) =>
                {
                    DrawEvent();
                };
            }

            if (_add != null && !_add.HasOnClickListeners)
            {
                _add.Click += (sender, e) =>
                {
                    AddEvent();                   
                };                
            }

            if (_remove != null && !_remove.HasOnClickListeners)
            {
                _remove.Click += (sender, e) =>
                {
                    RemoveEvent();
                };
            }

            if(_eventhistory != null && !_eventhistory.HasOnClickListeners)
            {
                _eventhistory.ItemClick += _eventhistory_ItemClick;
            }

            if(fab != null && !fab.HasOnClickListeners)
            {
                fab.Click += Fab_Click;
            }

            return _view;
        }

        private void Fab_Click(object sender, System.EventArgs e)
        {
            var convertView = Activity.LayoutInflater.Inflate(Resource.Layout.alertdialog_adddrawnevent, null);
            var outcome = convertView.FindViewById<EditText>(Resource.Id.outcome);
            var eventnumbertext = convertView.FindViewById<TextView>(Resource.Id.eventnumbertext);
            var radiooptionA = convertView.FindViewById<RadioButton>(Resource.Id.optionA);
            var editText = convertView.FindViewById<EditText>(Resource.Id.edittext);

            eventnumbertext.Visibility = ViewStates.Gone;           

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetTitle("Add drawn event to history")
                .SetView(convertView)
                .SetPositiveButton("Save changes", (senderAlert, args) =>
                {
                    var op = radiooptionA.Checked ? 1 : 2;
                    if (int.TryParse(editText.Text, out int cardId))
                    {
                        EventDrawn(cardId, outcome, op);
                    }
                })
                .SetNegativeButton(Resources.GetString(Resource.String.NoCancel), (senderAlert, args) =>   {  })
            .Show();
        }

        private void _eventhistory_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var convertView = Activity.LayoutInflater.Inflate(Resource.Layout.alertdialog_drawnevent, null);
            var outcome = convertView.FindViewById<EditText>(Resource.Id.outcome);
            var eventnumbertext = convertView.FindViewById<TextView>(Resource.Id.eventnumbertext);
            var radiooptionA = convertView.FindViewById<RadioButton>(Resource.Id.optionA);
            var radiooptionB = convertView.FindViewById<RadioButton>(Resource.Id.optionB);

            var ev = (EventWrapper)_eventhistory.Adapter.GetItem(e.Position); 

            if (!ev.Action.Contains("Drawn")) return;

            eventnumbertext.Text = $"Event number: {ev.ReferenceNumber}";
            outcome.Text = ev.Outcome;
            radiooptionA.Checked = ev.Decision == 1;
            radiooptionB.Checked = ev.Decision != 1;
                
            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetTitle($"Edit drawn event {ev.ReferenceNumber}")
                .SetView(convertView)
                .SetPositiveButton("Save changes", (senderAlert, args) =>
                {
                    _dataChanged = true;
                    var op = radiooptionA.Checked ? 1 : 2;
                    ev.Outcome = outcome.Text;
                    ev.Decision = op;
                    _eventhistory.Adapter = new CampaignEventHistoryAdapter(Context, _eventType);
                })                       
                .SetNegativeButton(Resources.GetString(Resource.String.NoCancel), (senderAlert, args) =>
                {
                })
                .Show();
        }

        private void RemoveEvent()
        {
            var convertView = Activity.LayoutInflater.Inflate(Resource.Layout.alertdialog_eventreferencenumber, null);
            var editText = convertView.FindViewById<EditText>(Resource.Id.edittext);

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle(Resources.GetString(Resource.String.RemoveAddEventTitle))                
                .SetPositiveButton(Resources.GetString(Resource.String.RemoveEvent), (senderAlert, args) =>
                {
                    if (int.TryParse(editText.Text, out int cardId))
                    {
                        if (_eventDeck.GetItems().Contains(cardId))
                        {
                            RemoveCard(cardId);
                        }
                        else
                        {
                            Toast.
                                MakeText(Context, "Event is not present in eventdeck.", ToastLength.Short).
                                Show();
                        }
                    }
                    else
                    {
                        Toast.
                            MakeText(Context, "Enter a valid number please.", ToastLength.Short).
                            Show();
                    }
                })
                .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        private void AddEvent()
        {
            var convertView = Activity.LayoutInflater.Inflate(Resource.Layout.alertdialog_addeventreferencenumber, null);
            var editText = convertView.FindViewById<EditText>(Resource.Id.edittext);
            var check = convertView.FindViewById<CheckBox>(Resource.Id.shufflecheck);

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle(Resources.GetString(Resource.String.RemoveAddEventTitle))  
                .SetPositiveButton(Resources.GetString(Resource.String.AddEvent), (senderAlert, args) =>
                {
                    if (int.TryParse(editText.Text, out int cardId))
                    {
                        if (!_eventDeck.GetItems().Contains(cardId))
                        {
                            AddCard(int.Parse(editText.Text), check.Checked);
                        }
                        else
                        {
                            Toast.
                                MakeText(Context, "Event already is in eventdeck.", ToastLength.Short).
                                Show();
                        }
                    }
                    else
                    {
                        Toast.
                            MakeText(Context, "Enter a valid number please.", ToastLength.Short).
                            Show();
                    }                    
                })
                .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        private void DrawEvent()
        {
            var cardId = _eventDeck.DrawCard();

            if (cardId == -1)
            {
                Toast.
                    MakeText(Context, "Eventdeck is empty", ToastLength.Short).
                    Show();
                return;
            }

            var convertView = Activity.LayoutInflater.Inflate(Resource.Layout.alertdialog_drawnevent, null);
            var outcome = convertView.FindViewById<EditText>(Resource.Id.outcome);
            var eventnumbertext = convertView.FindViewById<TextView>(Resource.Id.eventnumbertext);
            var radiooptionA = convertView.FindViewById<RadioButton>(Resource.Id.optionA);

            eventnumbertext.Text = $"Event number: {cardId}";

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetTitle(string.Format(Resources.GetString(Resource.String.DrawnEventNumber), cardId))
                .SetView(convertView)
                .SetPositiveButton(Resources.GetString(Resource.String.PutEventBack), (senderAlert, args) =>
                {
                    _dataChanged = true;
                    var op = radiooptionA.Checked ? 1 : 2;

                    EventDrawn(cardId, outcome, op);

                    PutBack(cardId);                    
                })
                .SetNegativeButton(Resources.GetString(Resource.String.RemoveEvent), (senderAlert, args) =>
                {
                    _dataChanged = true;
                    var op = radiooptionA.Checked ? 1 : 2;
                    EventDrawn(cardId, outcome, op);

                    RemoveCard(cardId);
                })
                .Show();           
        }

        private void EventDrawn(int cardId, TextView outcome, int selectedOption)
        {
            _dataChanged = true;
            GCTContext.CurrentCampaign.DrawnEventHistory(cardId, (int) _eventType, outcome.Text, selectedOption);
            _eventhistory.Adapter = new CampaignEventHistoryAdapter(Context, _eventType);
        }

        private void AddCard(int cardId, bool doShuffle)
        {
            _dataChanged = true;
            _eventDeck.AddCard(cardId, doShuffle);

            GCTContext.CurrentCampaign.AddEventHistory(cardId, (int)_eventType, doShuffle);  
            _eventhistory.Adapter = new CampaignEventHistoryAdapter(Context, _eventType);
        }

        private void RemoveCard(int cardId)
        {
            _dataChanged = true;
            _eventDeck.RemoveCard(cardId);

            GCTContext.CurrentCampaign.RemoveEventHistory(cardId, (int)_eventType);            
            _eventhistory.Adapter = new CampaignEventHistoryAdapter(Context, _eventType);
        }

        private void PutBack(int cardId)
        {
            _dataChanged = true;
            _eventDeck.RemoveCard(cardId);
            _eventDeck.PutBack(cardId);           
        }
    }
}