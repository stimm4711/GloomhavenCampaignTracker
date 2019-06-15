using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.CustomControls;
using Android.Support.Design.Widget;
using System;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.Threading.Tasks;
using System.Net.Http;
using Plugin.Connectivity;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.Percent;
using System.Collections.Generic;

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

        public override void OnStop()
        {
            GCTContext.CampaignCollection.CurrentCampaign.SetEventDeckString(_eventType);
            base.OnStop();
        }        

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _dataChanged = false;

            _eventType = GetEventType;

            if (GCTContext.CampaignCollection.CurrentCampaign != null)
            {
                if (_eventType == EventTypes.CityEvent)
                {
                    _eventDeck = GCTContext.CampaignCollection.CurrentCampaign.CityEventDeck;
                }
                else if(_eventType == EventTypes.RoadEvent)
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
            _view = inflater.Inflate(Resource.Layout.fragment_events, container, false);
            _draw = _view.FindViewById<Button>(Resource.Id.drawEventCardButton);
            _add = _view.FindViewById<Button>(Resource.Id.addEventCardButton);
            _remove = _view.FindViewById<Button>(Resource.Id.removeEventCardButton);
            _eventhistory = _view.FindViewById<ListView>(Resource.Id.eventhistoryListView);

            var fab = _view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            var initEventDeckButton = _view.FindViewById<ImageButton>(Resource.Id.initEventDeckButton);                            

            if (initEventDeckButton != null && !initEventDeckButton.HasOnClickListeners)
            {
                initEventDeckButton.Click += (sender, e) =>
                {                    
                    InitializeDeck();                       
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

            if (_eventhistory != null && !_eventhistory.HasOnClickListeners)
            {
                _eventhistory.ItemClick += _eventhistory_ItemClick;
            }

            if (fab != null && !fab.HasOnClickListeners)
            {
                fab.Click += Fab_Click;
            }

            InitHistoryAdapter();

            return _view;
        }

        private void InitializeDeck()
        {
            new CustomDialogBuilder(base.Context, Resource.Style.MyDialogTheme)
                        .SetTitle(Resources.GetString(Resource.String.InitializeEventDeck))
                        .SetPositiveButton(Resources.GetString(Resource.String.Initialize), (senderAlert, args) =>
                        {
                            if (_eventType == EventTypes.RiftEvent)
                                _eventDeck.ClearDeck();
                            else
                                _eventDeck.InitializeDeck();

                            GCTContext.CurrentCampaign.InitializeEventDeck(_eventType);
                            InitHistoryAdapter();
                        })
                        .SetNegativeButton(base.Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                        .Show();
        }

        private void InitHistoryAdapter()
        {
            if (_eventhistory != null)
            {
                _eventhistory.Adapter = new CampaignEventHistoryAdapter(Context, _eventType);
            }
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            var convertView = Activity.LayoutInflater.Inflate(Resource.Layout.alertdialog_adddrawnevent, null);
            var outcome = convertView.FindViewById<EditText>(Resource.Id.outcome);
            var eventnumbertext = convertView.FindViewById<TextView>(Resource.Id.eventnumbertext);
            var radiooptionA = convertView.FindViewById<RadioButton>(Resource.Id.optionA);
            var editText = convertView.FindViewById<EditText>(Resource.Id.edittext);

            eventnumbertext.Visibility = ViewStates.Gone;           

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetTitle(Resources.GetString(Resource.String.AddEventToHistory))
                .SetView(convertView)
                .SetPositiveButton(Resources.GetString(Resource.String.Add), (senderAlert, args) =>
                {
                    var selected = radiooptionA.Checked ? 1 : 2;
                    if (int.TryParse(editText.Text, out int cardId))
                    {
                        EventDrawn(cardId, outcome, selected);
                    }
                })
                .SetNegativeButton(Resources.GetString(Resource.String.NoCancel), (senderAlert, args) =>   {  })
            .Show();
        }

        private void _eventhistory_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var convertView = Activity.LayoutInflater.Inflate(Resource.Layout.alertdialog_eventhistoryitem, null);
                var outcome = convertView.FindViewById<EditText>(Resource.Id.outcome);
                var radiooptionA = convertView.FindViewById<RadioButton>(Resource.Id.optionA);
                var radiooptionB = convertView.FindViewById<RadioButton>(Resource.Id.optionB);
                var pqimage = convertView.FindViewById<ImageView>(Resource.Id.itemimage);
                var showEventButton = convertView.FindViewById<Button>(Resource.Id.show_eventcard);
                var eventnumbertext = convertView.FindViewById<TextView>(Resource.Id.eventnumbertext);

                var ev = (EventWrapper)_eventhistory.Adapter.GetItem(e.Position);

                if (!ev.Action.Contains(Resources.GetString(Resource.String.Drawn))) return;

                string cardIdText = ev.ReferenceNumber.ToString();
                if (ev.ReferenceNumber < 10)
                {
                    cardIdText = $"0{ev.ReferenceNumber}";
                }

                string eventtypeurl = "https://raw.githubusercontent.com/stimm4711/gloomhaven/master/images/events/base/road/re-";
                if (_eventType == EventTypes.CityEvent)
                {
                    eventtypeurl = "https://raw.githubusercontent.com/stimm4711/gloomhaven/master/images/events/base/city/ce-";
                }
                else if(_eventType == EventTypes.RiftEvent)
                {
                    eventtypeurl = "https://raw.githubusercontent.com/stimm4711/gloomhaven/master/images/events/base/rift/rf-";
                }

                showEventButton.Click += (s, args) =>
                {
                    var diag = new EventFrontImageViewDialogBuilder(Context, Resource.Style.MyTransparentDialogTheme, _eventType)
                            .SetEventNumber(cardIdText)
                            .Show();
                };

                radiooptionA.CheckedChange += (s, args) =>
                {
                   var x = GetEventBackImageBitmapFromUrlAsync(eventtypeurl + cardIdText + "-b.png", pqimage, convertView, radiooptionA);
                };

                outcome.Text = ev.Outcome;
                radiooptionA.Checked = ev.Decision == 1;
                radiooptionB.Checked = ev.Decision != 1;
                eventnumbertext.Text = $"{Resources.GetString(Resource.String.EventNumber)}: {ev.ReferenceNumber}";
                var eventback = GetEventBackImageBitmapFromUrlAsync(eventtypeurl + cardIdText + "-b.png", pqimage, convertView, radiooptionA);

                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                    .SetTitle($"{Resources.GetString(Resource.String.EditDrawnEvent)} {ev.ReferenceNumber}")
                    .SetView(convertView)
                    .SetPositiveButton(Resources.GetString(Resource.String.Save), (senderAlert, args) =>
                    {
                        var op = radiooptionA.Checked ? 1 : 2;
                        ev.Outcome = outcome.Text;
                        ev.Decision = op;
                        CampaignEventHistoryLogItemRepository.InsertOrReplace(ev.Item);
                        InitHistoryAdapter();
                    })
                    .SetNegativeButton(Resources.GetString(Resource.String.NoCancel), (senderAlert, args) =>  { })
                    .Show();
            }
            else
            {
                var convertView = Activity.LayoutInflater.Inflate(Resource.Layout.alertdialog_drawnevent, null);
                var outcome = convertView.FindViewById<EditText>(Resource.Id.outcome);
                var eventnumbertext = convertView.FindViewById<TextView>(Resource.Id.eventnumbertext);
                var radiooptionA = convertView.FindViewById<RadioButton>(Resource.Id.optionA);
                var radiooptionB = convertView.FindViewById<RadioButton>(Resource.Id.optionB);

                var ev = (EventWrapper)_eventhistory.Adapter.GetItem(e.Position);

                if (!ev.Action.Contains(Resources.GetString(Resource.String.Drawn))) return;

                eventnumbertext.Text = $"{Resources.GetString(Resource.String.EventNumber)}: {ev.ReferenceNumber}";
                outcome.Text = ev.Outcome;
                radiooptionA.Checked = ev.Decision == 1;
                radiooptionB.Checked = ev.Decision != 1;

                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                    .SetTitle($"{Resources.GetString(Resource.String.EditDrawnEvent)} {ev.ReferenceNumber}")
                    .SetView(convertView)
                    .SetPositiveButton(Resources.GetString(Resource.String.Save), (senderAlert, args) =>
                    {
                        var op = radiooptionA.Checked ? 1 : 2;
                        ev.Outcome = outcome.Text;
                        ev.Decision = op;
                        CampaignEventHistoryLogItemRepository.InsertOrReplace(ev.Item);
                        InitHistoryAdapter();
                    })
                    .SetNegativeButton(Resources.GetString(Resource.String.NoCancel), (senderAlert, args) =>
                    {
                    })
                    .Show();
            }

            
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
                            Toast.MakeText(Context,Resources.GetString(Resource.String.EventNotPresent), ToastLength.Short).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(Context, Resources.GetString(Resource.String.EnterValidNumber), ToastLength.Short).Show();
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
                            Toast.MakeText(Context, Resources.GetString(Resource.String.EventAlreadyInEventdeck), ToastLength.Short).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(Context, Resources.GetString(Resource.String.EnterValidNumber), ToastLength.Short).Show();
                    }                    
                })
                .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        private void DrawEvent()
        {
            var cardId = _eventDeck.DrawCard();

            cardId = RedrawIfRequirementNotMet(cardId);

            if (cardId == -1)
            {
                Toast.MakeText(Context, Resources.GetString(Resource.String.EventdeckEmpty), ToastLength.Short).Show();
                return;
            }  
            
            if (cardId == -2)
            { 
                Toast.MakeText(Context, "No valid card in deck.", ToastLength.Long).Show();
                return;
            }

            if (CrossConnectivity.Current.IsConnected)
            {
                // Show eventcard if there is internet connection
                var convertView = Activity.LayoutInflater.Inflate(Resource.Layout.alertdialog_drawnevent_withImage, null);
                var pqimage = convertView.FindViewById<ImageView>(Resource.Id.itemimage);
                var turnButton = convertView.FindViewById<Button>(Resource.Id.turn);
                var deciscion_layout = convertView.FindViewById<LinearLayout>(Resource.Id.deciscion_layout);
                var result_layout = convertView.FindViewById<LinearLayout>(Resource.Id.result_layout);
                var removeButton = convertView.FindViewById<Button>(Resource.Id.remove_button);
                var putUnderButton = convertView.FindViewById<Button>(Resource.Id.putunder);
                var outcome = convertView.FindViewById<EditText>(Resource.Id.outcome);
                var radiooptionA = convertView.FindViewById<RadioButton>(Resource.Id.optionA);
                var eventnumbertext = convertView.FindViewById<TextView>(Resource.Id.eventnumbertext);

                deciscion_layout.Visibility = ViewStates.Visible;
                result_layout.Visibility = ViewStates.Gone;

                string cardIdText = cardId.ToString();
                if (cardId < 10)
                {
                    cardIdText = $"0{cardId}";
                }

                string eventtypeurl = "https://raw.githubusercontent.com/stimm4711/gloomhaven/master/images/events/base/road/re-";
                if (_eventType == EventTypes.CityEvent)
                {
                    eventtypeurl = "https://raw.githubusercontent.com/stimm4711/gloomhaven/master/images/events/base/city/ce-";
                }
                else if (_eventType == EventTypes.RiftEvent)
                {
                    eventtypeurl = "https://raw.githubusercontent.com/stimm4711/gloomhaven/master/images/events/base/rift/rf-";
                }
                var eventFront = GetImageBitmapFromUrlAsync(eventtypeurl + cardIdText + "-f.png", pqimage, convertView);
                eventnumbertext.Text = $"{Resources.GetString(Resource.String.EventNumber)}: {cardId}";

                var alert = new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                 .SetView(convertView)
                 .SetNeutralButton(Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                 .Show();

                removeButton.Click += (sender, args) =>
                {
                    var op = radiooptionA.Checked ? 1 : 2;
                    EventDrawn(cardId, outcome, op);
                    RemoveCard(cardId);
                    alert.Dismiss();
                };

                putUnderButton.Click += (sender, args) =>
                {
                    var op = radiooptionA.Checked ? 1 : 2;
                    EventDrawn(cardId, outcome, op);
                    PutBack(cardId);
                    alert.Dismiss();
                };

                turnButton.Click += (sender, args) =>
                {
                    if (deciscion_layout.Visibility == ViewStates.Visible)
                    {
                        pqimage.SetImageDrawable(null);
                        convertView.FindViewById<ProgressBar>(Resource.Id.loadingPanel).Visibility = ViewStates.Visible;

                        var eventback = GetEventBackImageBitmapFromUrlAsync(eventtypeurl + cardIdText + "-b.png", pqimage, convertView, radiooptionA);

                        deciscion_layout.Visibility = ViewStates.Gone;
                        result_layout.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        pqimage.SetImageDrawable(null);
                        convertView.FindViewById<ProgressBar>(Resource.Id.loadingPanel).Visibility = ViewStates.Visible;

                        var eventimage = GetImageBitmapFromUrlAsync(eventtypeurl + cardIdText + "-f.png", pqimage, convertView);

                        deciscion_layout.Visibility = ViewStates.Visible;
                        result_layout.Visibility = ViewStates.Gone;
                    }
                };
            }
            else
            {
                // Show eventnumber if there is no internet connection
                var convertView = Activity.LayoutInflater.Inflate(Resource.Layout.alertdialog_drawnevent, null);
                var eventnumbertext = convertView.FindViewById<TextView>(Resource.Id.eventnumbertext);
                eventnumbertext.Text = $"{Resources.GetString(Resource.String.EventNumber)}: {cardId}";
                var radiooptionA = convertView.FindViewById<RadioButton>(Resource.Id.optionA);
                var outcome = convertView.FindViewById<EditText>(Resource.Id.outcome);

                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                   .SetTitle(string.Format(Resources.GetString(Resource.String.DrawnEventNumber), cardId))
                   .SetView(convertView)
                   .SetPositiveButton(Resources.GetString(Resource.String.PutEventBack), (senderAlert, args) =>
                   {
                       var op = radiooptionA.Checked ? 1 : 2;
                       EventDrawn(cardId, outcome, op);
                       PutBack(cardId);
                   })
                   .SetNegativeButton(Resources.GetString(Resource.String.RemoveEvent), (senderAlert, args) =>
                   {
                       var op = radiooptionA.Checked ? 1 : 2;
                       EventDrawn(cardId, outcome, op);
                       RemoveCard(cardId);
                   })
                   .SetNeutralButton(Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                   .Show();
            }
        }

        private int RedrawIfRequirementNotMet(int cardId)
        {
            var needRedraw = true;
            var lstDrawnCards = new List<int>();
            lstDrawnCards.Add(cardId);
            while (needRedraw)
            {
                needRedraw = false;

                if (_eventType == EventTypes.CityEvent)
                {
                    if (cardId == 84 && !Campaign.HasPartyAchievement((int)PartyAchievementsInternalNumbers.Opportunists))
                    {
                        PutBack(cardId);
                        needRedraw = true;
                    }

                    if (cardId == 85 && !Campaign.HasPartyAchievement((int)PartyAchievementsInternalNumbers.AStrongbox))
                    {
                        PutBack(cardId);
                        needRedraw = true;
                    }

                    if (cardId == 86 && !Campaign.HasPartyAchievement((int)PartyAchievementsInternalNumbers.Custodians))
                    {
                        PutBack(cardId);
                        needRedraw = true;
                    }

                    if (cardId == 87 && !Campaign.HasPartyAchievement((int)PartyAchievementsInternalNumbers.GuardDetail))
                    {
                        PutBack(cardId);
                        needRedraw = true;
                    }
                }

                if (_eventType == EventTypes.RiftEvent)
                {
                    if (cardId == 16 && !Campaign.HasPartyAchievement((int)PartyAchievementsInternalNumbers.HuntedPrey))
                    {
                        PutBack(cardId);
                        needRedraw = true;
                    }

                    if (cardId == 17 && !Campaign.HasPartyAchievement((int)PartyAchievementsInternalNumbers.Accomplices))
                    {
                        PutBack(cardId);
                        needRedraw = true;
                    }

                    if (cardId == 18 && !Campaign.HasPartyAchievement((int)PartyAchievementsInternalNumbers.Saboteurs))
                    {
                        PutBack(cardId);
                        needRedraw = true;
                    }
                }

                if (needRedraw)
                {                   
                    Toast.MakeText(Context, "Requirement not met. Put event under deck and redrawn a new event.", ToastLength.Long).Show();
                    cardId = _eventDeck.DrawCard();
                    
                    if (lstDrawnCards.Contains(cardId))
                    {                        
                        return -2;
                    }
                }
            }

            return cardId;
        }

        private async Task<Bitmap> GetEventBackImageBitmapFromUrlAsync(string url, ImageView imagen, View view, RadioButton radiooptionA)
        {
            int decision = radiooptionA.Checked ? 0 : 1;

            Bitmap imageBitmap = null;

            using (var httpClient = new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }       

            imagen.SetImageBitmap(GetScaleBitmap(imageBitmap, decision));

            view.FindViewById<ProgressBar>(Resource.Id.loadingPanel).Visibility = ViewStates.Gone;

            return imageBitmap;
        }

        private Bitmap GetScaleBitmap(Bitmap bitmap, int decision)
        {
            Bitmap output = Bitmap.CreateBitmap(bitmap.Width, (bitmap.Height / 2), Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(output);

            Paint paint = new Paint();
            Rect rectS = new Rect(0, decision * (bitmap.Height / 2), bitmap.Width, bitmap.Height / 2 + (decision * (bitmap.Height / 2)));
            Rect rectD = new Rect(0, 0, bitmap.Width, bitmap.Height / 2);
            RectF rectF = new RectF(rectD);
            float roundPx = 0;

            paint.AntiAlias = true;
            canvas.DrawARGB(0, 0, 0, 0);
            canvas.DrawRoundRect(rectF, roundPx, roundPx, paint);
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(bitmap, rectS, rectD, paint);

            return output;
        }

        private async Task<Bitmap> GetImageBitmapFromUrlAsync(string url, ImageView imagen, View view)
        {
            Bitmap imageBitmap = null;

            using (var httpClient = new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            imagen.SetImageBitmap(imageBitmap);

            view.FindViewById<ProgressBar>(Resource.Id.loadingPanel).Visibility = ViewStates.Gone;

            return imageBitmap;
        }

        private void EventDrawn(int cardId, TextView outcome, int selectedOption)
        {
            EventhistoryHelper.DrawnEventHistory(Context, cardId, (int) _eventType, outcome.Text, selectedOption);
            InitHistoryAdapter();
        }

        private void AddCard(int cardId, bool doShuffle)
        {
            EventhistoryHelper.AddEventHistory(Context, cardId, (int)_eventType, doShuffle);

            _eventDeck.AddCard(cardId, doShuffle);
            InitHistoryAdapter();
        }

        private void RemoveCard(int cardId)
        {
            EventhistoryHelper.RemoveEventHistory(Context, cardId, (int)_eventType);

            _eventDeck.RemoveCard(cardId);
            InitHistoryAdapter();
        }      

        private void PutBack(int cardId)
        {
            _eventDeck.RemoveCard(cardId);
            _eventDeck.PutBack(cardId);
        }
    }
}