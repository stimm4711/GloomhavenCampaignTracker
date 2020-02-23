using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign.world
{
    public class CampaignWorldFragment : CampaignFragmentBase
    {      
        private Button _showGlobalAchievementsButton;
        private Button _showUnlockedScenariosButton;
        private Button _showRoadEventsButton;
        private Button _rifteventsButton;
        private TextView _scenarioleveltext;
        private TextView _scenariolevelmodtext;
        private TextView _goldtext;
        private TextView _trapdamage;
        private TextView _xptext;
        private TextView _monsterleveltext;
        private CheckBox _solocheckbox;

        private int _currentScenarioLevel = 0;
        private int _scenariolevelMod = 0;
        private bool _isSolo = false;

        public CampaignWorldFragment() : base() {}

        public CampaignWorldFragment( FragmentManager fm) : base(fm)   {    }               

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            _view = inflater.Inflate(Resource.Layout.fragment_campaign_world, container, false);

            var detailsFrame = _view.FindViewById<View>(Resource.Id.frame_details_world);
            _isDualPane = detailsFrame != null && detailsFrame.Visibility == ViewStates.Visible;

            _showGlobalAchievementsButton = _view.FindViewById<Button>(Resource.Id.globalachievementsButton);
            _showUnlockedScenariosButton = _view.FindViewById<Button>(Resource.Id.unlockedScenariosButton);
            _showRoadEventsButton = _view.FindViewById<Button>(Resource.Id.roadeventsButton);
            _rifteventsButton = _view.FindViewById<Button>(Resource.Id.rifteventsButton);
            _scenarioleveltext = _view.FindViewById<TextView>(Resource.Id.scenarioleveltext);
            _scenariolevelmodtext = _view.FindViewById<TextView>(Resource.Id.scenariolevelmodtext);
            _goldtext = _view.FindViewById<TextView>(Resource.Id.goldtext);
            _trapdamage = _view.FindViewById<TextView>(Resource.Id.trapdamage);
            _xptext = _view.FindViewById<TextView>(Resource.Id.xptext);
            _monsterleveltext = _view.FindViewById<TextView>(Resource.Id.monsterleveltext);
            _solocheckbox = _view.FindViewById<CheckBox>(Resource.Id.solocheckbox);

            var calcscenariolevelbutton = _view.FindViewById<Button>(Resource.Id.calcscenariolevelbutton);
            var subscenariolevelButton = _view.FindViewById<Button>(Resource.Id.subscenariolevelButton);
            var addscenariolevelButton = _view.FindViewById<Button>(Resource.Id.addscenariolevelButton);

            if (_rifteventsButton != null)
            {
                if (GCTContext.Settings.IsFCActivated && GCTContext.CurrentCampaign.HasGlobalAchievement(GlobalAchievementsInternalNumbers.EndOfGloom))
                {
                    _rifteventsButton.Visibility = ViewStates.Visible;
                }
                else
                {
                    _rifteventsButton.Visibility = ViewStates.Gone;
                }
            }

            if (_isDualPane)
            {
                _dualtdetailLayout = _view.FindViewById<LinearLayout>(Resource.Id.dualdetaillayout);
                _dualtdetailLayout.Visibility = ViewStates.Invisible;
            }

            if (!_showGlobalAchievementsButton.HasOnClickListeners)
            {
                _showGlobalAchievementsButton.Click += (sender, args) =>
                {
                    ShowDetail(DetailFragmentTypes.GlobalAchievements);                   
                };
            }

            if (!_showUnlockedScenariosButton.HasOnClickListeners)
            {
                _showUnlockedScenariosButton.Click += (sender, args) =>
                {
                    ShowDetail(DetailFragmentTypes.UnlockedScenarios);
                };
            }

            if (!_showRoadEventsButton.HasOnClickListeners)
            {
                _showRoadEventsButton.Click += (sender, args) =>
                {
                    ShowDetail(DetailFragmentTypes.Roadevents);
                };
            }

            if (!_rifteventsButton.HasOnClickListeners)
            {
                _rifteventsButton.Click += (sender, args) =>
                {
                    ShowDetail(DetailFragmentTypes.Riftevents);
                };
            }

            if (!calcscenariolevelbutton.HasOnClickListeners)
            {
                calcscenariolevelbutton.Click += Calcscenariolevelbutton_Click;
            }

            if(!subscenariolevelButton.HasOnClickListeners)
            {
                subscenariolevelButton.Click += SubscenariolevelButton_Click;
            }

            if(!addscenariolevelButton.HasOnClickListeners)
            {
                addscenariolevelButton.Click += AddscenariolevelButton_Click;
            }

            if(!_solocheckbox.HasOnClickListeners)
            {
                _solocheckbox.CheckedChange += _solocheckbox_CheckedChange;
            }                       

            if (CurrentCampaign.SelectedCharacters != null && CurrentCampaign.SelectedCharacters.Any())
            {
                CalculateScenarioLevel();
            }

            if(CurrentCampaign.ScenarioLevelModifier + _currentScenarioLevel <= 7 && CurrentCampaign.ScenarioLevelModifier + _currentScenarioLevel >= 0)
            {
                _scenariolevelMod = CurrentCampaign.ScenarioLevelModifier;
                _scenariolevelmodtext.Text = $"{_scenariolevelMod}";
                SetScenarioLevel();
            }

            return _view;
        }

        private void _solocheckbox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == _isSolo) return;
            _isSolo = e.IsChecked;

            if(_isSolo)
            {
                SetSoloScenarioLevel();
            }
            else
            {
                SetScenarioLevel();
            }            
        }

        private void AddscenariolevelButton_Click(object sender, System.EventArgs e)
        {
            if (_scenariolevelMod + _currentScenarioLevel == 7) return;

            _scenariolevelMod += 1;
            _scenariolevelmodtext.Text = $"{_scenariolevelMod}";
            CurrentCampaign.ScenarioLevelModifier = _scenariolevelMod;
            SetScenarioLevel();
        }

        private void SubscenariolevelButton_Click(object sender, System.EventArgs e)
        {
            if (_scenariolevelMod + _currentScenarioLevel == 0) return;

            _scenariolevelMod -= 1;
            _scenariolevelmodtext.Text = $"{_scenariolevelMod}";
            CurrentCampaign.ScenarioLevelModifier = _scenariolevelMod;
            SetScenarioLevel();
        }

        private void SetScenarioLevel()
        {
            if (_isSolo)
            {
                SetSoloScenarioLevel();
            }
            else
            {
                var sl = _currentScenarioLevel + _scenariolevelMod;
                var goldconversion = Helper.GetGoldConverionForScenarioLevel(sl);
                var trapdamage = Helper.GetTrapdamageForScenarioLevel(sl);
                var bonusxp = Helper.GetBonusXPForScenarioLevel(sl);
                var monsterlevel = sl;

                _xptext.Text = $"{bonusxp}";
                _trapdamage.Text = $"{trapdamage}";
                _goldtext.Text = $"{goldconversion}";
                _monsterleveltext.Text = $"{monsterlevel}";
            }            
        }

        private void SetSoloScenarioLevel()
        {
            var sl = Math.Min(_currentScenarioLevel + _scenariolevelMod, 6);
            var goldconversion = Helper.GetGoldConverionForScenarioLevel(sl);
            var trapdamage = Helper.GetTrapdamageForScenarioLevel(sl) +1;
            var bonusxp = Helper.GetBonusXPForScenarioLevel(sl);
            var monsterlevel = sl + 1;

            _xptext.Text = $"{bonusxp}";
            _trapdamage.Text = $"{trapdamage}";
            _goldtext.Text = $"{goldconversion}";
            _monsterleveltext.Text = $"{monsterlevel}";
        }

        private void Calcscenariolevelbutton_Click(object sender, EventArgs e)
        {          
            var view = _inflater.Inflate(Resource.Layout.alertdialog_listview, null);
            var listview = view.FindViewById<ListView>(Resource.Id.listView);
            listview.ItemsCanFocus = true;
            listview.ChoiceMode = ChoiceMode.Multiple;

            List<DL_Character> partymembers = new List<DL_Character>();
            partymembers = DataServiceCollection.CharacterDataService.GetPartymembersUnretiredFlat(CurrentCampaign.CurrentParty.Id);

            var itemadapter = new SelectableCharacterAdapter(Context, partymembers);
            listview.Adapter = itemadapter;

            new CustomDialogBuilder(base.Context, Resource.Style.MyDialogTheme)
                .SetCustomView(view)
                .SetTitle("Select Characters")
                .SetNeutralButton(base.Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .SetNegativeButton("OK", (senderAlert, args) =>
                {                   
                    var selectedItems = itemadapter.GetSelected();

                    if (!selectedItems.Any()) return;

                    CurrentCampaign.SelectedCharacters = selectedItems.ToList();
                    CalculateScenarioLevel();
                })    
                .Show();
        }

        private void CalculateScenarioLevel( )
        {
            float sl = 0;
            foreach (var si in CurrentCampaign.SelectedCharacters)
            {
                sl += si.Level;
            }

            float avr = sl / CurrentCampaign.SelectedCharacters.Count();
            float halfavr = avr / 2;
            double senariolevel = Math.Ceiling(halfavr);
            _currentScenarioLevel = (int)senariolevel;

            _scenarioleveltext.Text = $"{_currentScenarioLevel}";

            SetScenarioLevel();
        }
    }
}