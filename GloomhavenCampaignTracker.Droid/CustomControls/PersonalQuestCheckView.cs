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
    public class PersonalQuestCheckView : LinearLayout
    {
        private TextView _countername;
        private CheckBox _countervalue;       
        private DL_CharacterPersonalQuestCounter _counter;

        public PersonalQuestCheckView(Context context) : base(context)
        {
            Initialize(context);
        }

        public PersonalQuestCheckView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(context);
        }

        public PersonalQuestCheckView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize(context);
        }

        public PersonalQuestCheckView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize(context);
        }

        private void Initialize(Context context)
        {
            LayoutInflater inflater = LayoutInflater.FromContext(context);
            inflater.Inflate(Resource.Layout.view_personalquestcheck, this);

            _countername = FindViewById<TextView>(Resource.Id.countername);
            _countervalue = FindViewById<CheckBox>(Resource.Id.pqcheck);

            _countervalue.CheckedChange += _countervalue_CheckedChange;
        }

        private void _countervalue_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if(_counter.Value == 1 != _countervalue.Checked)
            {
                if (_countervalue.Checked)
                {
                    _counter.Value = 1;
                }
                else
                {
                    _counter.Value = 0;
                }
            }
            
            UpdateView();
            CharacterPersonalQuestCountersRepository.InsertOrReplace(_counter);
        }

        public void SetCounter(DL_CharacterPersonalQuestCounter counter)
        {
            _counter = counter;
            UpdateView();
        }

        private void UpdateView()
        {
            _countername.Text = _counter.PersonalQuestCounter.CounterName;
            _countervalue.Checked = _counter.Value > 0;

            if (_counter.Value == _counter.PersonalQuestCounter.CounterValue)
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