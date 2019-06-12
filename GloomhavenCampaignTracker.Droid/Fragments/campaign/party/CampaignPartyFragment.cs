using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.CustomControls;
using System.Linq;
using System.Collections.Generic;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using Android.Support.Design.Widget;
using System.Threading.Tasks;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign.party
{
    public class CampaignPartyFragment : CampaignFragmentBase
    {
        private Button _raiseRepButton;
        private Button _decreaseRepButton;
        private TextView _partyReputationText;
        private TextView _shopPrizeModText;
        private Button _partyAchievements;
        private TextView _partynameText;
        private Button _partyMemberButton;
        private Spinner _currentLocationSpinner;
        private readonly List<Scenario> _locations = new List<Scenario>();
        private SpinnerAdapter _adapter;
        private EditText _partynotes;

        public CampaignPartyFragment()
        { }

        public CampaignPartyFragment(FragmentManager fm) : base(fm) { }

        public override void OnStop()
        {
            if(_dataChanged)
            {
                DataServiceCollection.PartyDataService.InsertOrReplace(GCTContext.CurrentCampaign.CurrentParty);
                _dataChanged = false;
            }
            base.OnStop();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        private void InitLocations()
        {
            _locations.Add(new Scenario(new DL_Scenario() { Scenarionumber = 0, Name = "Gloomhaven" }));

            foreach (var s in DataServiceCollection.CampaignDataService.GetUnlockedScenarios(GCTContext.CurrentCampaign.CampaignData.Id))
            {
                _locations.Add(new Scenario(s));
            }            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {  
            base.OnCreateView(inflater, container, savedInstanceState);

            _view = inflater.Inflate(Resource.Layout.fragment_campaign_party, container, false);
            _inflater = inflater;

            var detailsFrame = _view.FindViewById<View>(Resource.Id.frame_details_party);
            _isDualPane = detailsFrame != null && detailsFrame.Visibility == ViewStates.Visible;

            _raiseRepButton = _view.FindViewById<Button>(Resource.Id.partyfragment_addReputationButton);
            _decreaseRepButton = _view.FindViewById<Button>(Resource.Id.partyfragment_subReputationButton);
            _partyReputationText =  _view.FindViewById<TextView>(Resource.Id.partyfragment_reputationTextView);
            _shopPrizeModText = _view.FindViewById<TextView>(Resource.Id.partyfrag_shopPriceModText);
            _partyAchievements = _view.FindViewById<Button>(Resource.Id.partyAchievementsButton);
            _partynameText = _view.FindViewById<TextView>(Resource.Id.partynameTextView);
            _partyMemberButton = _view.FindViewById<Button>(Resource.Id.partyMembersButton);
            _currentLocationSpinner = _view.FindViewById<Spinner>(Resource.Id.currentLocationSpinner);
            _partynotes = _view.FindViewById<EditText>(Resource.Id.partynotestext);
            var _partyNotesButton = _view.FindViewById<Button>(Resource.Id.partyNotesButton);

            if (_partynotes != null)
                _partynotes.AfterTextChanged += _partynotes_AfterTextChanged;

            if (_isDualPane)
            {
                _dualtdetailLayout = _view.FindViewById<LinearLayout>(Resource.Id.dualdetaillayout);
                _dualtdetailLayout.Visibility = ViewStates.Invisible;
            }

            if (!_locations.Any())
            {
                Task.Run(async () =>
                {
                    Scenario item = null;
                    await Task.Factory.StartNew(() => InitLocations());

                    item = _locations.FirstOrDefault(x => x.ScenarioData.Scenarionumber == CurrentCampaign.CurrentParty.CurrentLocationNumber);

                    _adapter = new SpinnerAdapter(Context, Resource.Layout.itemviewDark);
                    _adapter.AddItems(_locations);

                    Activity.RunOnUiThread(() =>
                    {
                        _currentLocationSpinner.Adapter = _adapter;
                        if (item != null)
                        {
                            var position = _locations.IndexOf(item);
                            _currentLocationSpinner.SetSelection(position);
                        }
                        else
                        {
                            _currentLocationSpinner.SetSelection(0);
                        }
                    });                     
                });
            }

            if (GCTContext.CurrentCampaign.CurrentParty == null)
            {
                _partynameText.Text = "# Party not loaded! #";
            }

            if (!_partyMemberButton.HasOnClickListeners)
            {
                if(GCTContext.CurrentCampaign.CurrentParty != null)
                {
                    _partyMemberButton.Click += _partyMemberButton_Click;
                }                
            }

            if (_partyNotesButton != null && !_partyNotesButton.HasOnClickListeners)
            {
                _partyNotesButton.Click += _partyNotesButton_Click;
            }

            if (!_raiseRepButton.HasOnClickListeners)
            {
                _raiseRepButton.Click += RaiseRepButton_Click;
            }

            if (!_decreaseRepButton.HasOnClickListeners)
            {
                _decreaseRepButton.Click += DecreaseRepButton_Click;
            }

            if (!_partyAchievements.HasOnClickListeners)
            {
                if (GCTContext.CurrentCampaign.CurrentParty != null)
                {
                    _partyAchievements.Click += _partyAchievements_Click;
                }
            }

            if (GCTContext.CurrentCampaign.CurrentParty != null)
            {
                _partynameText.Text = GCTContext.CampaignCollection.CurrentCampaign.CurrentParty.Name;

            }    
            
            if(!_partynameText.HasOnClickListeners)
            {
                _partynameText.Click += _partynameText_Click;
            }
                     
            UpdateView();

            _currentLocationSpinner.ItemSelected += _currentLocationSpinner_ItemSelected;

            return _view;
        }

        private void _partyNotesButton_Click(object sender, EventArgs e)
        {
            if (_isDualPane)
            {     
                var detailsFrag = CampaignPartyNotesFragment.NewInstance();
                var title = _view.FindViewById<TextView>(Resource.Id.dualdetailtitle);
                title.Text = "Party Notes";
                _dualtdetailLayout.Visibility = ViewStates.Visible;
                var fragTrans = _fragmentManager.BeginTransaction().Replace(Resource.Id.frame_details_party, detailsFrag);
                fragTrans.Commit();
            }
        }

        private void _partynotes_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            CurrentCampaign.CurrentParty.Notes = _partynotes.Text;
            DataServiceCollection.PartyDataService.InsertOrReplace(CurrentCampaign.CurrentParty);
        }

        private void _partynameText_Click(object sender, EventArgs e)
        {
            var convertView = LayoutInflater.Inflate(Resource.Layout.alertdialog_editpartyname, null);
            var partyNameEdit = convertView.FindViewById<TextInputEditText>(Resource.Id.party_name);

            partyNameEdit.Text = $"{CurrentCampaign.CurrentParty.Name}";

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle("Edit Partyname")
                .SetNegativeButton(Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .SetPositiveButton(Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                {
                    if (string.IsNullOrEmpty(partyNameEdit.Text)) return;
                    CurrentCampaign.CurrentParty.Name = partyNameEdit.Text;

                    SaveParty();
                    partyNameEdit.Text = CurrentCampaign.CurrentParty.Name;

                    ((CampaignViewPagerFragmentTabs)ParentFragment).UpdateAdapter();
                    ((CampaignViewPagerFragmentTabs)ParentFragment).SetPartyPage();
                })
                .Show();

            _dataChanged = true;
        }

        private void _currentLocationSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var currentlocation = _locations[_currentLocationSpinner.SelectedItemPosition]?.ScenarioData.Scenarionumber;
            if (!currentlocation.HasValue) return;

            CurrentCampaign.CurrentParty.CurrentLocationNumber = currentlocation.Value;
            _dataChanged = true;
        }

        private class Scenario : ISpinneritem
        {
            public Scenario(DL_Scenario scenario)
            {
                ScenarioData = scenario;
            }

            public DL_Scenario ScenarioData { get; }

            public string Spinnerdisplayvalue => $"# {ScenarioData.Scenarionumber}   {ScenarioData.Name}";
        }

        private void _partyMemberButton_Click(object sender, EventArgs e)
        {
            if (_isDualPane)
            {
                var detailsFrag = CharacterListFragment.NewInstance(true);
                detailsFrag.SetPartyFrag(this);
                var title = _view.FindViewById<TextView>(Resource.Id.dualdetailtitle);
                title.Text = "Party Members";
                _dualtdetailLayout.Visibility = ViewStates.Visible;
                var fragTrans = _fragmentManager.BeginTransaction().Replace(Resource.Id.frame_details_party, detailsFrag);
                fragTrans.Commit();
            }
            else
            {
                ShowDetail(DetailFragmentTypes.PartyMember);
            }           
        }

        private void _partyAchievements_Click(object sender, EventArgs e)
        {
            ShowDetail(DetailFragmentTypes.PartyAchievements);
        }

        public void ShowCharacterDetail(int characterId)
        {
            ShowDetail(DetailFragmentTypes.CharacterDetail, characterId);
        }

        private void DecreaseRepButton_Click(object sender, EventArgs e)
        {
            if (CurrentCampaign.CurrentParty == null) return;
            if (CurrentCampaign.CurrentParty.Reputation == -20) return;

            CurrentCampaign.CurrentParty.Reputation -= 1;
            SaveParty();
            UpdateReputation();

            if (CurrentCampaign.CurrentParty.Reputation == -10 && !GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.ReputationMinus10Unlocked)
            {
                GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.ReputationMinus10Unlocked = true;
                GCTContext.CurrentCampaign.AddUnlockedClass(10);
                CampaignUnlocksRepository.InsertOrReplace(GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks);

                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                    .SetTitle(Context.Resources.GetString(Resource.String.Congratulation))
                    .SetIcon(Resource.Drawable.ic_class10whitesmall)
                    .SetMessage("You have unlocked the Moon class")
                    .SetPositiveButton("OK", (senderAlert, args) =>  { })
                    .Show();

            }

            if (CurrentCampaign.CurrentParty.Reputation != -20 ||
                GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.ReputationMinus20Unlocked) return;
            
            GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.ReputationMinus20Unlocked = true;
            GCTContext.CurrentCampaign.AddEventToDeck(Context, 77, EventTypes.CityEvent);
            GCTContext.CurrentCampaign.AddEventToDeck(Context, 68, EventTypes.RoadEvent);
            CampaignUnlocksRepository.InsertOrReplace(GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks);

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)                
                .SetTitle(Context.Resources.GetString(Resource.String.Congratulation))
                .SetMessage("City Event 77 and Road Event 68 added")
                .SetPositiveButton("OK", (senderAlert, args) => { })
                .Show();

            _dataChanged = true;
        }

        private void RaiseRepButton_Click(object sender, EventArgs e)
        {
            if (CurrentCampaign.CurrentParty == null) return;
            if (CurrentCampaign.CurrentParty.Reputation == 20) return;

            CurrentCampaign.CurrentParty.Reputation += 1;
            SaveParty();
            UpdateReputation();

            if (CurrentCampaign.CurrentParty.Reputation == 10 && !GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.ReputationPlus10Unlocked)
            {
                GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.ReputationPlus10Unlocked = true;
                GCTContext.CurrentCampaign.AddUnlockedClass(7);
                CampaignUnlocksRepository.InsertOrReplace(GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks);

                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                    .SetTitle(Context.Resources.GetString(Resource.String.Congratulation))
                    .SetIcon(Resource.Drawable.ic_class7whitesmall)
                    .SetMessage("You have unlocked the Sun class")
                    .SetPositiveButton("OK", (senderAlert, args) => { })
                    .Show();
            }

            if (CurrentCampaign.CurrentParty.Reputation != 20 ||
                GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.ReputationPlus20Unlocked) return;

            GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.ReputationPlus20Unlocked = true;
            GCTContext.CurrentCampaign.AddEventToDeck(Context, 76, EventTypes.CityEvent);
            GCTContext.CurrentCampaign.AddEventToDeck(Context, 67, EventTypes.RoadEvent);
            CampaignUnlocksRepository.InsertOrReplace(GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks);

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetTitle(Context.Resources.GetString(Resource.String.Congratulation))
                .SetMessage("City Event 76 and Road Event 67 added")
                .SetPositiveButton("OK", (senderAlert, args) => { })
                .Show();

            _dataChanged = true;
        }

        private void SaveParty()
        {
            DataServiceCollection.PartyDataService.InsertOrReplace(CurrentCampaign.CurrentParty);
        }

        private void UpdateReputation()
        {
            if (CurrentCampaign.CurrentParty == null) return;

            _partyReputationText.Text = CurrentCampaign.CurrentParty.Reputation.ToString();
            var modifier = Helper.GetShopPriceModifier(CurrentCampaign.CurrentParty.Reputation);
            _shopPrizeModText.Text = modifier.ToString();
        }              

        internal void UpdateView()
        { 
            if (CurrentCampaign.CurrentParty == null) return;

            UpdateReputation();
            if (_partynotes != null)
                _partynotes.Text = CurrentCampaign.CurrentParty.Notes ;            
        }        
    }
}
