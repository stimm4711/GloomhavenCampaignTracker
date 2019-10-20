using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using GloomhavenCampaignTracker.Droid;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Business
{
    /// <summary>
    /// Holds all data of a campaign
    /// </summary>
    public class Campaign
    {
        readonly ICampaignDataService _dataService;
        private List<string> m_loadingMessages;
        
        #region "Init"

        /// <summary>
        /// creates a new instance with default data
        /// </summary>
        /// <param name="campaignname"></param>
        /// <returns></returns>
        public static Campaign NewInstance(string campaignname)
        {
            var camp = new DL_Campaign
            {
                Name = campaignname,
                CityProsperity = 1,
                DonatedGold = 0,
                Parties = new List<DL_CampaignParty>(),
                GlobalAchievements = new List<DL_CampaignGlobalAchievement>(),
                UnlockedScenarios = new List<DL_CampaignUnlockedScenario>(),
                CampaignUnlocks = new DL_CampaignUnlocks(),
                CityEventDeckString = "",
                RoadEventDeckString = "",
                RiftEventDeckString = "",
                UnlockedItems = new List<DL_Item>()
            };

            DataServiceCollection.CampaignDataService.InsertOrReplace(camp, false);

            Campaign campaign = null;
            campaign = new Campaign(camp);           
            return campaign;
        }

        internal CampaignUnlockedScenario GetUnlockedScenario(int id)
        {
            var scen = CampaignData.UnlockedScenarios.FirstOrDefault(x => x.ID_Scenario == id);
            return new CampaignUnlockedScenario(scen);
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="campaign"></param>
        public Campaign(DL_Campaign campaign)
        {
            _dataService = DataServiceCollection.CampaignDataService;
            CampaignData = campaign;
           
            if (CampaignData != null) LoadCampaign();          

        }

        /// <summary>
        /// loads data and sets properties
        /// </summary>
        private void LoadCampaign()
        {
            m_loadingMessages = new List<string>();
            InitRoadEvents();
            InitCityEvents();
            InitRiftEvents();
            InitGlobalAchievements();
            InitUnlockedClasses();
        }

        private void InitUnlockedClasses()
        {
            try
            {
                SetUnlockedClasses();
            }
            catch
            {
                ResetUnlockedClasses();
            }
        }

        private void ResetUnlockedClasses()
        {
            m_loadingMessages.Add("Unlocked Classes loading error. Unlocked Classes set to default.");
            CampaignData.CampaignUnlocks.UnlockedClassesIds = "1,2,3,4,5,6";
        }

        private void SetUnlockedClasses()
        {
            // init unlocked classes       
            if(CampaignData.CampaignUnlocks == null)
            {
                var unlocksold = CampaignUnlocksRepository.Get().FirstOrDefault(x => x.ID_Campaign == CampaignData.Id);
                               
                if(unlocksold != null)
                {
                    try
                    {
                        // Fixed in Update 1.3.9:
                        // In Update 1.3.5 I made the Save method async and that lead to some problems.
                        // One of those was, that the unlocks won't load even if the dataset looks good.
                        // Delete old and create new helped
                        CampaignUnlocksRepository.Delete(unlocksold);
                        unlocksold.Id = 0;
                        CampaignUnlocksRepository.InsertOrReplace(unlocksold);                        
                    } 
                    finally
                    {
                        CampaignData.CampaignUnlocks = unlocksold;
                    }
                }
                else
                {
                    CampaignData.CampaignUnlocks = new DL_CampaignUnlocks();
                }
            }
            UnlockedClassesIds = Helper.StringToIntList(CampaignData.CampaignUnlocks.UnlockedClassesIds);
        }

        private void InitGlobalAchievements()
        {
            // init global achievements
            foreach (var ga in CampaignData?.GlobalAchievements)
            {
                try
                {
                    GlobalAchievements.Add(new CampaignGlobalAchievement(ga));
                }
                catch
                {
                    m_loadingMessages.Add($"Can't load Global Achievement {ga.Achievement.Name}.");
                    CampaignData.GlobalAchievements.Remove(ga);
                }
            }
        }

        private void InitCityEvents()
        {
            try
            {
                // init city event deck
                if (string.IsNullOrEmpty(CampaignData.CityEventDeckString))
                {
                    CityEventDeck.InitializeDeck();
                    CampaignData.CityEventDeckString = CityEventDeck.ToString();
                }
                else
                {
                    CityEventDeck.FromString(CampaignData.CityEventDeckString);
                }
            }
            catch
            {
                m_loadingMessages.Add("Can't load City Event Deck. Initialized Event Deck.");
                CityEventDeck.InitializeDeck();
                CampaignData.CityEventDeckString = CityEventDeck.ToString();
            }
        }

        private void InitRoadEvents()
        {
            try
            {
                // init roadevent deck
                if (string.IsNullOrEmpty(CampaignData.RoadEventDeckString))
                {
                    RoadEventDeck.InitializeDeck();
                    CampaignData.RoadEventDeckString = RoadEventDeck.ToString();
                }
                else
                {
                    RoadEventDeck.FromString(CampaignData.RoadEventDeckString);
                }
            }
            catch
            {
                m_loadingMessages.Add("Can't load Road Event Deck. Initialized Event Deck.");
                RoadEventDeck.InitializeDeck();
                CampaignData.RoadEventDeckString = RoadEventDeck.ToString();
            }
        }

        private void InitRiftEvents()
        {
            try
            {
                // init rift deck
                if (string.IsNullOrEmpty(CampaignData.RiftEventDeckString))
                {
                    RiftEventDeck.ClearDeck();
                    CampaignData.RiftEventDeckString = RiftEventDeck.ToString();
                }
                else
                {
                    RiftEventDeck.FromString(CampaignData.RiftEventDeckString);
                }
            }
            catch
            {
                m_loadingMessages.Add("Can't load Rift Event Deck. Initialized Event Deck.");
                RiftEventDeck.ClearDeck();
                CampaignData.RiftEventDeckString = RiftEventDeck.ToString();
            }
        }

        internal List<string> GetLoadingMessages()
        {
            return m_loadingMessages;
        }

        #endregion

        #region "Properties"

        /// <summary>
        /// Data object
        /// </summary>
        public DL_Campaign CampaignData { get; set; }

        /// <summary>
        /// Unlocked Class Ids
        /// </summary>
        public List<int> UnlockedClassesIds { get; set; } = new List<int>();

        /// <summary>
        /// Road Event Deck
        /// </summary>
        public EventDeck RoadEventDeck { get; } = new EventDeck();

        public EventDeck RiftEventDeck { get; } = new EventDeck();

        /// <summary>
        /// City Event Deck
        /// </summary>
        public EventDeck CityEventDeck { get; } = new EventDeck();

        /// <summary>
        /// Global Achievements
        /// </summary>
        public List<CampaignGlobalAchievement> GlobalAchievements { get; } = new List<CampaignGlobalAchievement>();       

        /// <summary>
        /// City Propsperity Level
        /// </summary>
        public int CityProsperity
        {
            get => CampaignData.CityProsperity;
            set => CampaignData.CityProsperity = value;
        }                        

        public DL_CampaignParty CurrentParty { get; set; }

        public IEnumerable<DL_Character> SelectedCharacters { get; set; }
        
        public int ScenarioLevelModifier { get; set; }

        #endregion

        #region "Data operations"

        /// <summary>
        /// Save Campaign data
        /// </summary>
        public void Save(bool recursive = true)
        {
            try
            {
                _dataService.InsertOrReplace(CampaignData, recursive);
                System.Diagnostics.Debug.WriteLine("Saved Campaign");
            }
            catch
            { }
        }

        /// <summary>
        /// Delete Campaign from db
        /// </summary>
        public void Delete()
        {
            _dataService.Delete(CampaignData);
        }

        #endregion

        #region "EventDecks"

        /// <summary>
        /// Sets the event deck as string
        /// </summary>
        /// <param name="eventType"></param>
        public void SetEventDeckString(EventTypes eventType)
        {
            if (eventType == EventTypes.RoadEvent)
                // road event deck
                CampaignData.RoadEventDeckString = RoadEventDeck.ToString();
            else if (eventType == EventTypes.CityEvent)
                // city event deck
                CampaignData.CityEventDeckString = CityEventDeck.ToString();
            else if (eventType == EventTypes.RiftEvent)
                // rift event deck
                CampaignData.RiftEventDeckString = RiftEventDeck.ToString();

            Save(recursive: false);
        }

        public void AddEventToDeck(Context context, int eventnumber, EventTypes eventtype)
        {
            if (eventtype == EventTypes.RoadEvent)
            {
                RoadEventDeck.AddCard(eventnumber);
            }
            else if (eventtype == EventTypes.CityEvent)
            {
                CityEventDeck.AddCard(eventnumber);
            }
            else if (eventtype == EventTypes.RiftEvent)
            {
                RiftEventDeck.AddCard(eventnumber);
            }

            EventhistoryHelper.AddEventHistory(context, eventnumber, (int)eventtype);
           
            SetEventDeckString(eventtype);
        }     

        public void InitializeEventDeck(EventTypes eventType)
        {
            if (eventType == EventTypes.RoadEvent)
            {
                CampaignEventHistoryLogItemRepository.DisableOldEvents(CampaignData.Id, eventType: 2);
                SetEventDeckString(EventTypes.RoadEvent);
            }
            else if (eventType == EventTypes.CityEvent)
            {
                CampaignEventHistoryLogItemRepository.DisableOldEvents(CampaignData.Id, eventType: 1);
                SetEventDeckString(EventTypes.CityEvent);
            }
            else if (eventType == EventTypes.RiftEvent)
            {
                CampaignEventHistoryLogItemRepository.DisableOldEvents(CampaignData.Id, eventType: 3);
                SetEventDeckString(EventTypes.RiftEvent);
            }
        }

        #endregion

        #region "Global Achievements"

        /// <summary>
        /// Returns if the campaign has a gained global achievement
        /// </summary>
        /// <param name="internalNumber"></param>
        /// <returns></returns>
        public bool HasGlobalAchievement(int internalNumber)
        {
            return GlobalAchievements.Any(x => x.AchievementEnumNumber == internalNumber);
        }

        /// <summary>
        /// Returns if the campaign has a gained global achievement
        /// </summary>
        /// <param name="gaNumber"></param>
        /// <returns></returns>
        public bool HasGlobalAchievement(GlobalAchievementsInternalNumbers gaNumber)
        {
            return GlobalAchievements.Any(x => x.AchievementEnumNumber == (int)gaNumber);
        }        

        /// <summary>
        /// Adds a global achievement
        /// </summary>
        /// <param name="achType"></param>
        /// <param name="ach"></param>
        public CampaignGlobalAchievement AddGlobalAchievement(DL_AchievementType achType, DL_Achievement ach = null)
        {
            // create data object of link table
            var ga = new DL_CampaignGlobalAchievement
            {
                AchievementType = achType,
                Campaign = CampaignData,
                ID_AchievementType = achType.Id,
                ID_Campaign = CampaignData.Id,
                Step = 1
            };

            // set the sub achievement if applicadable
            if (achType.Achievements != null && achType.Achievements.Count > 0)
            {
                ga.Achievement = ach ?? achType.Achievements.First();
            }

            // add the data object to the campaign  data
            CampaignData.GlobalAchievements.Add(ga);

            // create and add the business object to the campaign
            CampaignGlobalAchievement campaignGlobalAchievement = new CampaignGlobalAchievement(ga);
            GlobalAchievements.Add(campaignGlobalAchievement);

            return campaignGlobalAchievement;
        }

        internal void ClearLoadingMessages()
        {
            m_loadingMessages.Clear();
        }

        /// <summary>
        /// Removes a global achievement
        /// </summary>
        /// <param name="campAchievement"></param>
        public void RemoveGlobalAchievement(CampaignGlobalAchievement campAchievement)
        {
            // remove global achievement from campaign
            GlobalAchievements.Remove(campAchievement);

            // remove global achievement from campaign data
            CampaignData.GlobalAchievements.Remove(campAchievement.GlobalAchievement);
        }

        /// <summary>
        /// Removes a global achievement by internal number
        /// </summary>
        /// <param name="gaNumber"></param>
        public void RemoveGlobalAchievement(GlobalAchievementsInternalNumbers gaNumber)
        {
            var ga = GlobalAchievements.FirstOrDefault(x => x.AchievementEnumNumber == (int)gaNumber);

            if(ga != null)
            {
                RemoveGlobalAchievement(ga);
            }          
        }

        #endregion

        #region "Unlocked Scenarios"

        /// <summary>
        /// Adds an unlocked scenario
        /// </summary>
        /// <param name="scenario"></param>
        /// <returns></returns>
        public CampaignUnlockedScenario AddUnlockedScenario(int scenario_id)
        {
            var scenario = DataServiceCollection.ScenarioDataService.Get(scenario_id);

            if (scenario == null) return null;

            // Create Data Object of link table
            var unlockedScenarioData = new DL_CampaignUnlockedScenario
            {
                Scenario = scenario,
                Campaign = CampaignData,
                ID_Scenario = scenario.Id,
                ID_Campaign = CampaignData.Id,
                Completed = false,
                ScenarioTreasures = new List<DL_Treasure>(),
                CampaignScenarioTreasures = new List<DL_CampaignScenarioTreasure>()
            };

            foreach(var t in scenario.Treasures)
            {
                unlockedScenarioData.CampaignScenarioTreasures.Add(new DL_CampaignScenarioTreasure()
                {
                    ScenarioTreasure = t,
                    ScenarioTreasure_ID = t.Id,
                    UnlockedScenario = unlockedScenarioData
                });
            }

            // add data object to campaign data
            CampaignData.UnlockedScenarios.Add(unlockedScenarioData);

            // Create a new business object for the data object
            CampaignUnlockedScenario campaignUnlockedScenario = new CampaignUnlockedScenario(unlockedScenarioData);

            campaignUnlockedScenario.Save();

            return campaignUnlockedScenario;
        }

        internal void RemoveUnlockedScenario(CampaignUnlockedScenario campScenario)
        {
            if (campScenario != null)
            {
                RemoveScenario(campScenario.UnlockedScenarioData);
            }
        }

        /// <summary>
        /// Remove an unlocked scenario
        /// </summary>
        /// <param name="campScenario"></param>
        public void RemoveScenario(DL_CampaignUnlockedScenario campScenario)
        {
            if (campScenario != null)
            {
                // remove from campaign data
                CampaignData.UnlockedScenarios.Remove(campScenario);
                CampaignUnlockedScenarioRepository.Delete(campScenario);
            }
        }


        #endregion

        #region "Campaign Unlocks"

        /// <summary>
        /// Add an unlocked class
        /// </summary>
        /// <param name="classid"></param>
        public void AddUnlockedClass(int classid)
        {
            // add it not already assigned
            if (!UnlockedClassesIds.Contains(classid))
            {
                UnlockedClassesIds.Add(classid);
            }

            // make unlocked classes to string
            CampaignData.CampaignUnlocks.UnlockedClassesIds = Helper.IntListToString(UnlockedClassesIds);
        }
       
        /// <summary>
        /// Removes an unlocked ItemDesign
        /// </summary>
        /// <param name="classid"></param>
        public void RemoveUnlockedClass(int classid)
        {
            // remove from list
            UnlockedClassesIds.Remove(classid);

            // make unlocked classes to string
            CampaignData.CampaignUnlocks.UnlockedClassesIds = Helper.IntListToString(UnlockedClassesIds);
        }


        #endregion

        #region "City"

        public void RemoveDonationToTheSanctuary()
        {
            CampaignData.DonatedGold -= 10;
        }

        public void AddDonationToTheSanctuary()
        {
            CampaignData.DonatedGold += 10;         
        }

        public void IncreaseCityProsperity()
        {
            CityProsperity = System.Math.Min(65, CityProsperity + 1);
        }

        public void DecreaseCityProsperity()
        {
            CityProsperity = System.Math.Max(1, CityProsperity - 1);
        }

        public int GetProsperityLevel()
        {
            CityProsperity = System.Math.Max(1, CityProsperity);
            var prosperityLevel = Helper.GetProsperityLevel(CityProsperity);
            return prosperityLevel;
        }

        public double GetProsperityStepValue(int prosperityLevel)
        {
            double prosperityMeterValue;
            var stepsToNextLevel = Helper.GetStepsToNextLevel(prosperityLevel);

            if (stepsToNextLevel == 0)
            {
                prosperityMeterValue = 1;
            }
            else
            {
                var currentLevelSteps = 1;
                if (prosperityLevel > 1)
                {
                    currentLevelSteps = Helper.GetStepsToNextLevel(prosperityLevel - 1);
                }

                var range = stepsToNextLevel - currentLevelSteps;
                var currentSteps = CityProsperity - currentLevelSteps;

                prosperityMeterValue = (double)currentSteps / range;
            }

            return prosperityMeterValue;
        }

        #endregion

        #region "Party"

        /// <summary>
        /// Sets the current active party
        /// </summary>
        /// <param name="currentPartyId"></param>
        public void SetCurrentParty(int currentPartyId)
        {
            if (CampaignData.Parties.Any())
            {
                if (CampaignData.Parties.Any(x => x.Id == currentPartyId))
                {
                    CurrentParty = CampaignData.Parties.FirstOrDefault(x => x.Id == currentPartyId);
                }
                else
                {
                    var partyid = CampaignData.Parties.FirstOrDefault()?.Id;
                    if (partyid.HasValue)
                    {
                        CurrentParty = CampaignData.Parties.FirstOrDefault(x => x.Id == partyid.Value);
                    }
                }
            }
            else
            {
                try
                {
                    CurrentParty = DataServiceCollection.PartyDataService.Get(currentPartyId);
                }
                catch
                {
                    
                }
            }
        }

        /// <summary>
        /// Adds a Party to the campaign
        /// </summary>
        /// <param name="partyname"></param>
        public void AddParty(string partyname)
        {
            // Create party data
            var party = new DL_CampaignParty()
            {
                Campaign = CampaignData,
                ID_Campaign = CampaignData.Id,
                Name = partyname,
                PartyAchievements = new List<DL_CampaignPartyAchievement>(),
                Reputation = 0,
                Notes = "",
                CurrentLocationNumber = 0
            };

            DataServiceCollection.PartyDataService.InsertOrReplace(party);

            CurrentParty = party;

            if (CampaignData.Parties == null)
            {
                CampaignData.Parties = new List<DL_CampaignParty>();
            }

            // add party to campaign data
            CampaignData.Parties.Add(party);

            // set the new party as current party
            CampaignData.CurrentParty_ID = party.Id;        
        }

        #endregion

        /// <summary>
        /// Add a gained achievement to the party
        /// </summary>
        /// <param name="pa"></param>
        public void AddPartyAchievement(DL_PartyAchievement pa)
        {
            // create partyachievement data object
            var campPartyAchievement = new DL_CampaignPartyAchievement()
            {
                Party = CurrentParty,
                ID_PartyAchievement = pa.Id,
                ID_Party = CurrentParty.Id,
                PartyAchievement = pa
            };

            // add achievement data object to party data  
            CurrentParty.PartyAchievements.Add(campPartyAchievement);
            DataServiceCollection.PartyDataService.InsertOrReplace(CurrentParty);
        }

        /// <summary>
        /// returns if party has ganied achievement
        /// </summary>
        /// <param name="internalNumber"></param>
        /// <returns></returns>
        public bool HasPartyAchievement(int internalNumber)
        {
            return CurrentParty.PartyAchievements != null && CurrentParty.PartyAchievements.Any(x => x.PartyAchievement != null && x.PartyAchievement.InternalNumber == internalNumber);
        }

        /// <summary>
        /// Removes a party achievement from party
        /// </summary>
        /// <param name="campAchievement"></param>
        public void RemoveAchievement(DL_CampaignPartyAchievement campAchievement)
        {
            // remove from party achievements
            CurrentParty.PartyAchievements.Remove(campAchievement);
            DataServiceCollection.PartyDataService.InsertOrReplace(CurrentParty);
        }

        public bool HasTheDrakesPartyAchievements()
        {
            var ach1 = HasPartyAchievement((int)PartyAchievementsInternalNumbers.TheDrakesTreasure);
            var ach2 = HasPartyAchievement((int)PartyAchievementsInternalNumbers.TheDrakesCommand);
            return  ach1 && ach2;
        }
    }
}