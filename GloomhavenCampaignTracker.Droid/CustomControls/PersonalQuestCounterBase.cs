using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Droid.Views
{
    public abstract class PersonalQuestCounterBase : LinearLayout
    {
        protected TextView _countername;
        protected DL_CharacterPersonalQuestCounter _counter;

        public PersonalQuestCounterBase(Context context) : base(context)
        {
            Initialize(context);
        }

        public PersonalQuestCounterBase(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(context);
        }

        public PersonalQuestCounterBase(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize(context);
        }

        public PersonalQuestCounterBase(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize(context);
        }

        protected abstract void Initialize(Context context);

        public void SetCounter(DL_CharacterPersonalQuestCounter counter)
        {
            _counter = counter;
            UpdateView();
        }

        protected virtual void UpdateView()
        {
            _countername.Text = _counter.PersonalQuestCounter.CounterName;

            if(_counter.Value == _counter.PersonalQuestCounter.CounterValue)
            {
                _countername.SetTextColor(Color.ForestGreen);
            }
            else
            {
                _countername.SetTextColor(Color.DarkGray);
            }
        }       
    }
}