using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Views
{
    public class PersonalQuestCounterView : LinearLayout
    {
        private TextView _countername;
        private TextView _countervalue;
        private Button _decreaseCounter;
        private Button _raiseCounter;
        private DL_CharacterPersonalQuestCounter _counter;
        public event EventHandler<ThresholdReachedEventArgs> ThresholdReached;
        public event EventHandler<ThresholdReachedEventArgs> ThresholdNotReached;

        public PersonalQuestCounterView(Context context) : base(context)
        {
            Initialize(context);
        }

        public PersonalQuestCounterView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(context);
        }

        public PersonalQuestCounterView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize(context);
        }

        public PersonalQuestCounterView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize(context);
        }

        private void Initialize(Context context)
        {
            LayoutInflater inflater = LayoutInflater.FromContext(context);
            inflater.Inflate(Resource.Layout.view_personalquestcounter, this);

            _countername = FindViewById<TextView>(Resource.Id.countername);
            _countervalue = FindViewById<TextView>(Resource.Id.counter);
            _decreaseCounter = FindViewById<Button>(Resource.Id.decreasecounterButton);
            _raiseCounter = FindViewById<Button>(Resource.Id.raisecounterbutton);

            _decreaseCounter.Click += _decreaseCounter_Click;
            _raiseCounter.Click += _raiseCounter_Click;
        }

        public void SetCounter(DL_CharacterPersonalQuestCounter counter)
        {
            _counter = counter;
            UpdateView();

            //if (_counter.PersonalQuestCounter.PersonalQuest.QuestNumber == 512)
            //{
            //    _decreaseCounter.Enabled = false;
            //    _raiseCounter.Enabled = false;
            //}
            //else
            //{
            //    _decreaseCounter.Enabled = true;
            //    _raiseCounter.Enabled = true;
            //}
        }

        private void UpdateView()
        {
            _countername.Text = _counter.PersonalQuestCounter.CounterName;
            _countervalue.Text = $"{_counter.Value} / {_counter.PersonalQuestCounter.CounterValue}";

            if(_counter.Value == _counter.PersonalQuestCounter.CounterValue)
            {
                _countervalue.SetTextColor(Color.ForestGreen);
                _countername.SetTextColor(Color.ForestGreen);
            }
            else
            {
                _countervalue.SetTextColor(Color.DarkGray);
                _countername.SetTextColor(Color.DarkGray);
            }

          
        }

        private void _raiseCounter_Click(object sender, EventArgs e)
        {
            if(_counter.Value < _counter.PersonalQuestCounter.CounterValue)
            {
                if (_counter.PersonalQuestCounter.PersonalQuest.QuestNumber == 525)
                {
                    if (_counter.Value + 10 <= _counter.PersonalQuestCounter.CounterValue)
                        _counter.Value += 10;

                    if (_counter.Value % 10 != 0)
                        _counter.Value += (10 - _counter.Value % 10);
                }
                else
                {
                    _counter.Value += 1;                    
                }

                UpdateView();
                CharacterPersonalQuestCountersRepository.InsertOrReplace(_counter);
                if (_counter.PersonalQuestCounter.CounterScenarioUnlock > 50 && _counter.Value == _counter.PersonalQuestCounter.CounterValue)
                {
                    ThresholdReachedEventArgs args = new ThresholdReachedEventArgs();
                    args.Threshold = _counter.PersonalQuestCounter.CounterValue;
                    args.ScenarioUnlocked = _counter.PersonalQuestCounter.CounterScenarioUnlock;
                    OnThresholdReached(args);
                }
            }           
        }

        private void _decreaseCounter_Click(object sender, EventArgs e)
        {
            if(_counter.Value > 0)
            {
                if (_counter.PersonalQuestCounter.PersonalQuest.QuestNumber == 525)
                {
                    if (_counter.Value - 10 >= 0)
                        _counter.Value -= 10;

                    if (_counter.Value % 10 != 0)
                        _counter.Value += (10 - _counter.Value % 10);
                }
                else
                {
                    _counter.Value -= 1;
                }
                               
                UpdateView();
                CharacterPersonalQuestCountersRepository.InsertOrReplace(_counter);
                if (_counter.PersonalQuestCounter.CounterScenarioUnlock > 50 && _counter.Value +1 == _counter.PersonalQuestCounter.CounterValue)
                {
                    ThresholdReachedEventArgs args = new ThresholdReachedEventArgs();
                    args.Threshold = _counter.PersonalQuestCounter.CounterValue;
                    args.ScenarioUnlocked = _counter.PersonalQuestCounter.CounterScenarioUnlock;
                    OnThresholdNotReached(args);
                }
            }
        }

        protected virtual void OnThresholdReached(ThresholdReachedEventArgs e)
        {
            ThresholdReached?.Invoke(this, e);
        }

        protected virtual void OnThresholdNotReached(ThresholdReachedEventArgs e)
        {
            ThresholdNotReached?.Invoke(this, e);
        }

        public class ThresholdReachedEventArgs : EventArgs
        {
            public int Threshold { get; set; }
            public int ScenarioUnlocked { get; set; }
        }
    }
}