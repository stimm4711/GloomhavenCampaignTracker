﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.Business;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class ScenarioDetailsFragment : CampaignDetailsFragmentBase
    {
        private CampaignUnlockedScenario _campaignScenario;
        private TextView _scenarioname;
        private TextView _scenarionumber;
        private GridView _grid;
        private EditText _notes;
        private Button _completed;
        private Button _completedCasual;
        private Button _lostButton;

        internal static ScenarioDetailsFragment NewInstance(int scenarioId)
        {
            var frag = new ScenarioDetailsFragment { Arguments = new Bundle() };
            frag.Arguments.PutInt(DetailsActivity.SelectedScenarioId, scenarioId);
            return frag;
        }

        private CampaignUnlockedScenario GetUnlockedScenario()
        {
            if (Arguments == null) return null;

            var id = Arguments.GetInt(DetailsActivity.SelectedScenarioId, 0);
            if (id <= 0) return null;

            var campScenario = GCTContext.CurrentCampaign.GetUnlockedScenario(id);

            return campScenario;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_scenariodetails_details, container, false);
            _scenarioname = _view.FindViewById<TextView>(Resource.Id.scenarionametextview);
            _scenarionumber = _view.FindViewById<TextView>(Resource.Id.scenarionumbertextview);
            _completed = _view.FindViewById<Button>(Resource.Id.completed);
            _grid = _view.FindViewById<GridView>(Resource.Id.imagesGridView);
            _notes = _view.FindViewById<EditText>(Resource.Id.notestext);
            _completedCasual = _view.FindViewById<Button>(Resource.Id.completedCasual);
            _lostButton = _view.FindViewById<Button>(Resource.Id.lost);

            _campaignScenario = GetUnlockedScenario();

            var txt_treasures = _view.FindViewById<TextView>(Resource.Id.txt_treasures);
            var txt_region = _view.FindViewById<TextView>(Resource.Id.txt_region);

            if (_campaignScenario != null)
            {
                if (_campaignScenario.Scenario != null)
                {
                    _scenarioname.Text = _campaignScenario.ScenarioName;
                    _scenarionumber.Text = $"# {_campaignScenario.Scenarionumber}";
                    _notes.Text = _campaignScenario.UnlockedScenarioData.ScenarioNotes;

                    if (!_campaignScenario.Completed && (_campaignScenario.IsBlocked() || !_campaignScenario.IsAvailable()))
                    {
                        _completedCasual.Visibility = ViewStates.Visible;
                        _completed.Visibility = ViewStates.Gone;
                    }

                    if (_campaignScenario.Scenario.ScenarioData.Treasures.Any())
                    {
                        txt_treasures.Visibility = ViewStates.Visible;
                        txt_region.Text = Helper.GetRegionName(_campaignScenario.UnlockedScenarioData.Scenario.Region_ID);
                    }

                    if(_campaignScenario.Completed)
                    {
                        _completed.Text = "Incomplete";
                    }
                }
            }

            if (!GCTContext.Settings.IsShowTreasure)
            {
                _grid.Visibility = ViewStates.Gone;
                txt_treasures.Visibility = ViewStates.Gone;
            }

            UpdateRequirementsListView();

            SetBackgroundOfTopLayout();

            if(!_completed.HasOnClickListeners)
            {
                _completed.Click += _completed_Click;
            }

            if (!_completedCasual.HasOnClickListeners)
            {
                _completedCasual.Click += CasualButton_Click;
            }

            if (!_lostButton.HasOnClickListeners)
            {
                _lostButton.Click += _lostButton_Click; ;
            }

            _notes.FocusChange += _notes_FocusChange;

            _grid.Adapter = new CampaignScenarioTreasureAdapter(Context, _campaignScenario.UnlockedScenarioData);

            return _view;
        }

        private void _lostButton_Click(object sender, EventArgs e)
        {
            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                      .SetTitle("Scenario lost")
                      .SetMessage($"You've lost the scenarion. Do you want to enter rewards now?")
                      .SetPositiveButton(Context.Resources.GetString(Resource.String.Yes), (senderAlert, AssemblyLoadEventArgs) =>
                      {
                          var intent = new Intent();
                          intent.SetClass(Activity, typeof(DetailsActivity));
                          intent.PutExtra(DetailsActivity.SelectedScenarioId, _campaignScenario.ScenarioId);
                          intent.PutExtra(DetailsActivity.CasualMode, true);
                          intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.ScenarioRewards);
                          StartActivity(intent);
                      })
                      .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) =>
                      {
                          ScenarioCompletion();
                      })
                      .Show();
        }

        private void _completed_Click(object sender, EventArgs e)
        {
            if (!_campaignScenario.Completed)
            {
                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                  .SetTitle("Scenarios completed!")
                  //.SetMessage($"You've completed the scenarion and unlocked scenario(s) # {string.Join(", # ", unlockedScenarioNumbers.ToArray())}. Do you want to enter rewards now?")
                  .SetMessage($"You've completed the scenarion. Do you want to enter rewards now?")
                  .SetPositiveButton(Context.Resources.GetString(Resource.String.Yes), (senderAlert, AssemblyLoadEventArgs) =>
                  {
                      var intent = new Intent();
                      intent.SetClass(Activity, typeof(DetailsActivity));
                      intent.PutExtra(DetailsActivity.SelectedScenarioId, _campaignScenario.ScenarioId);
                      intent.PutExtra(DetailsActivity.CasualMode, false);
                      intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.ScenarioRewards);
                      StartActivity(intent);
                  })
                  .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) =>
                  {
                      ScenarioCompletion();
                  })
                  .Show();
            }
            else
            {
                ScenarioCompletion();
            }
        }

        private void _notes_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus) return;
            _campaignScenario.UnlockedScenarioData.ScenarioNotes = _notes.Text;
            _campaignScenario.Save();
        }

        private void CasualButton_Click(object sender, EventArgs e)
        {
            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                      .SetTitle("Scenarios completed in Casual Mode!")
                      .SetMessage($"You've completed the scenarion in Casual Mode. Do you want to enter rewards now?")
                      .SetPositiveButton(Context.Resources.GetString(Resource.String.Yes), (senderAlert, AssemblyLoadEventArgs) =>
                      {
                          var intent = new Intent();
                          intent.SetClass(Activity, typeof(DetailsActivity));
                          intent.PutExtra(DetailsActivity.SelectedScenarioId, _campaignScenario.ScenarioId);
                          intent.PutExtra(DetailsActivity.CasualMode, true);
                          intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.ScenarioRewards);
                          StartActivity(intent);
                      })
                      .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) =>
                      {
                          ScenarioCompletion();
                      })
                      .Show();
        }

        private void ScenarioCompletion()
        {
            if (_campaignScenario.Completed)
            {
                ScenarioHelper.SetScenarioIncomplete(Context, LayoutInflater, _campaignScenario); 
            }
            else
            {
                ScenarioHelper.SetScenarioCompleted(Context, LayoutInflater, _campaignScenario);
            }
        }

        private void UpdateRequirementsListView()
        {
            var lv_requirements = _view.FindViewById<ListView>(Resource.Id.lv_requirements);

            var adapter = new ArrayAdapter<string>(Context, Resource.Layout.listviewitem_singleitem, _campaignScenario.GetAllRequirements());

            lv_requirements.Adapter = adapter;
        }

        private void SetBackgroundOfTopLayout()
        {
            var enableCheckbox = true;
            var color = ContextCompat.GetColor(Context, Resource.Color.gloom_primaryLighter);
            if (_campaignScenario.IsBlocked())
            {
                enableCheckbox = false;
                color = ContextCompat.GetColor(Context, Resource.Color.gloom_scenarioBlockedItemBackground);
            }
            else if (!_campaignScenario.Completed && !_campaignScenario.IsAvailable())
            {
                enableCheckbox = false;
                color = ContextCompat.GetColor(Context, Resource.Color.gloom_scenarioUnavailableItemBackground);
            }
            else if (_campaignScenario.Completed)
            {
                color = ContextCompat.GetColor(Context, Resource.Color.gloom_scenarioCompletedItemBackground);
            }

            var layoutTop = _view.FindViewById<RelativeLayout>(Resource.Id.layout_top);
            layoutTop.SetBackgroundColor(new Color(color));

        }
    }
}