﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Fragment = Android.Support.V4.App.Fragment;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.world;
using GloomhavenCampaignTracker.Droid.Fragments.campaign;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.party;
using Android.Views;
using Android.Content;
using GloomhavenCampaignTracker.Droid.Fragments;
using GloomhavenCampaignTracker.Droid.Fragments.character;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.unlocks;

namespace GloomhavenCampaignTracker.Droid
{
    [Activity(Label = "DetailsActivity")]
    public class DetailsActivity : AppCompatActivity
    {
        public static readonly string CurrentCampaignId = "current_campaign_id";
        public static readonly string SelectedFragId = "selected_fragid";
        public static readonly string SelectedCharacterId = "selected_character_id";
        public static readonly string SelectedScenarioId = "selected_scenario_id";
        public static readonly string CasualMode = "casual_mode";
        public static readonly string JustParty = "JustParty";
        public static readonly string ProsperityLevel = "ProsperityLevel";
        public static readonly string EventTypeString = "current_eventtype";
        public static readonly string EventCardNumber = "EventCardNumber";
        public static readonly string EventCardOption = "EventCardOption";

        private bool _justPartyCharacterView;

        private Fragment _detailsFrag;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.fragment_details_container);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbarDetails);

            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var campId = Intent.Extras.GetInt(CurrentCampaignId, 0);
            var fragId = Intent.Extras.GetInt(SelectedFragId, 1);
            _justPartyCharacterView = Intent.Extras.GetBoolean(JustParty, false);

            SetFragment(campId, fragId);
        }

        private void SetFragment(int campId, int fragId)
        {
            var scenarioID = 0;
            var casualMode = false;

            switch (fragId)
            {
                case (int)DetailFragmentTypes.GlobalAchievements:
                    SupportActionBar.Title = Resources.GetString(Resource.String.GlobalAchievements);
                    _detailsFrag = CampaignWorldGlobalAchievementsFragment.NewInstance(campId);
                    break;
                case (int)DetailFragmentTypes.UnlockedScenarios:
                    SupportActionBar.Title = Resources.GetString(Resource.String.UnlockedScenarios);
                    _detailsFrag = CampaignWorldUnlockedScenariosFragmentExpandListView.NewInstance(campId);
                    break;
                case (int)DetailFragmentTypes.Itemstore:
                    SupportActionBar.Title = Resources.GetString(Resource.String.itemstore);
                    _detailsFrag = new ItemstoreViewPagerTabs(); 
                    break;
                case (int)DetailFragmentTypes.Cityevents:
                    SupportActionBar.Title = Resources.GetString(Resource.String.CityEvents);
                    _detailsFrag = CampaignEventsFragment.NewInstance(campId, EventTypes.CityEvent);
                    break;
                case (int)DetailFragmentTypes.Roadevents:
                    SupportActionBar.Title = Resources.GetString(Resource.String.RoadEvents);
                    _detailsFrag = CampaignEventsFragment.NewInstance(campId, EventTypes.RoadEvent);
                    break;
                case (int)DetailFragmentTypes.Riftevents:
                    SupportActionBar.Title = Resources.GetString(Resource.String.RiftEvents);
                    _detailsFrag = CampaignEventsFragment.NewInstance(campId, EventTypes.RiftEvent);
                    break;
                case (int)DetailFragmentTypes.CampaignSelection:
                    SupportActionBar.Title = Resources.GetString(Resource.String.camaignSelection);
                    _detailsFrag = CampaignSelectionFragment.NewInstance();
                    break;
                case (int)DetailFragmentTypes.PartyAchievements:
                    SupportActionBar.Title = Resources.GetString(Resource.String.PartyAchievements);
                    _detailsFrag = CampaignPartyAchievementsFragment.NewInstance();
                    break;
                case (int)DetailFragmentTypes.PartySelection:
                    SupportActionBar.Title = Resources.GetString(Resource.String.PartySelection);
                    _detailsFrag = PartySelectionFragment.NewInstance();
                    break;
                case (int)DetailFragmentTypes.PartyMember:
                    SupportActionBar.Title = Resources.GetString(Resource.String.PartyMember);
                    _detailsFrag = CharacterListFragment.NewInstance(true);
                    break;
                case (int)DetailFragmentTypes.CharacterDetail:
                    var characterId = Intent.Extras.GetInt(SelectedCharacterId, 0);
                    SupportActionBar.Title = "Character Sheet";                   
                    _detailsFrag = CharacterDetailFragmentViewPager.NewInstance(characterId, _justPartyCharacterView);
                    break;
                case (int)DetailFragmentTypes.Characters:
                    SupportActionBar.Title = "Characters";
                    _detailsFrag = CharacterListFragment.NewInstance(false);
                    break;
                case (int)DetailFragmentTypes.Settings:
                    SupportActionBar.Title = "Settings";
                    _detailsFrag = SettingsFragment.NewInstance();
                    break;
                case (int)DetailFragmentTypes.Support:
                    SupportActionBar.Title = "Support Developer";
                    _detailsFrag = SupportDevFragment.NewInstance();
                    break;
                case (int)DetailFragmentTypes.Releasenotes:
                    SupportActionBar.Title = "Releasenotes";
                    _detailsFrag = ReleasenoteFragment.NewInstance();
                    break;
                case (int)DetailFragmentTypes.EnvelopeXUnlock:
                    SupportActionBar.Title = "Envelope X";
                    _detailsFrag = CampaignUnlocksEnvelopeXFragment.NewInstance();
                    break;
                case (int)DetailFragmentTypes.ScenarioDetails:
                    SupportActionBar.Title = "Scenario Details";
                    scenarioID = Intent.Extras.GetInt(SelectedScenarioId, 0); 
                    _detailsFrag = ScenarioDetailsFragment.NewInstance(scenarioID);
                    break;
                case (int)DetailFragmentTypes.ScenarioRewards:
                    SupportActionBar.Title = "Scenario Rewards";
                    scenarioID = Intent.Extras.GetInt(SelectedScenarioId, 0);
                    casualMode = Intent.Extras.GetBoolean(CasualMode, false);
                    _detailsFrag = SzenarioRewardsFragment.NewInstance(scenarioID, casualMode);
                    break;
                case (int)DetailFragmentTypes.EventDrawnOnline:
                    var cardnummer = Intent.Extras.GetInt(EventCardNumber, 0);
                    SupportActionBar.Title = $"Event Number: {cardnummer}";
                    _detailsFrag = CampaignEventsDrawnFragment.NewInstance(cardnummer, Intent.Extras.GetInt(EventTypeString, 0));
                    break;
                case (int)DetailFragmentTypes.EventOutcome:
                    SupportActionBar.Title = "Event Outcome";
                    _detailsFrag = EventOutcomeFragment.NewInstance(Intent.Extras.GetInt(EventCardNumber, 0), Intent.Extras.GetInt(EventCardOption, 0), Intent.Extras.GetInt(EventTypeString, 0));
                    break;
            }

            if (_detailsFrag != null)
            {
                SupportFragmentManager.
                    BeginTransaction().
                    Replace(Resource.Id.detailframe_container, _detailsFrag).
                    Commit();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var id = item.ItemId;
            if (id == Android.Resource.Id.Home)
            {
                Finish();
                return true;
            }

            if (_detailsFrag == null) return base.OnOptionsItemSelected(item);

            if (!(_detailsFrag is CharacterDetailFragmentViewPager)) return base.OnOptionsItemSelected(item);
                        
            Finish();

            return base.OnOptionsItemSelected(item);
        }
    }
}