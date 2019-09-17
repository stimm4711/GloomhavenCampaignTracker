using System.Linq;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.Collections.Generic;
using System;

namespace GloomhavenCampaignTracker.Business
{
    /// <summary>
    /// Unlocked Scenarios of a campaign
    /// </summary>
    public class CampaignUnlockedScenario : Java.Lang.Object
    {
        private ICampaignDataService _dataService;
        private List<int> _unlockedScenarios;
        private Lazy<Scenario> _lazyScenario;

        /// <summary>
        /// Needs a db object 
        /// </summary>
        /// <param name="unlockedScenariosData"></param>
        public CampaignUnlockedScenario(DL_CampaignUnlockedScenario unlockedScenariosData)
        {
            _dataService = DataServiceCollection.CampaignDataService;

            UnlockedScenarioData = unlockedScenariosData;

            _lazyScenario = new Lazy<Scenario>(() => 
            {
                return new Scenario(UnlockedScenarioData.Scenario);
            });
        }

        /// <summary>
        /// returns the db object
        /// </summary>
        public DL_CampaignUnlockedScenario UnlockedScenarioData { get; }

        /// <summary>
        /// return the scenario name
        /// </summary>
        public string ScenarioName => UnlockedScenarioData.Scenario.Name;

        /// <summary>
        /// returns the scenario number
        /// </summary>
        public int Scenarionumber => UnlockedScenarioData.Scenario.Scenarionumber;

        public Scenario Scenario => _lazyScenario.Value;

        /// <summary>
        /// returns the scenario Id
        /// </summary>
        public int ScenarioId => UnlockedScenarioData.Scenario.Id;

        /// <summary>
        /// returns or sets the completed state
        /// </summary>
        public bool Completed
        {
            get { return UnlockedScenarioData.Completed; }
            set { UnlockedScenarioData.Completed = value; }
        }

        public List<DL_Treasure> Treasures
        {
            get { return UnlockedScenarioData?.ScenarioTreasures; }
        }

        public void AddTreasure(int treasureNumber, string content, bool isLooted = false)
        {
            var treasure = new DL_Treasure()
            {
                CampaignScenario_ID = UnlockedScenarioData.Id,
                Looted = isLooted,
                Number = treasureNumber,
                UnlockedScenario = UnlockedScenarioData,
                Content = content               
            };

            UnlockedScenarioData.ScenarioTreasures.Add(treasure);            
        } 
        
        public string GetTreasureText()
        {
            if(Treasures != null)
            {
                return $"{Treasures.Count(x => x.Looted)}/{Treasures.Count}";
            }   
            else
            {
                return "0/0";
            }
        }

        /// <summary>
        /// saves the scenario data
        /// </summary>
        public void Save()
        {          
            // save with dataservice
            _dataService.SaveUnlockedScenarioData(UnlockedScenarioData);
        }       

        /// <summary>
        /// Get all global achievements achieved that block this scenario
        /// </summary>
        /// <returns>Achievementnames + (global)</returns>
        public List<string> GetBlockingGlobalAchievements()
        {
            List<string> blockingAchievementNames = new List<string>();

            foreach (var blockGlobAch in Scenario.BlockingGlobalAchievements)
            {
                if (GCTContext.CurrentCampaign.HasGlobalAchievement(blockGlobAch))
                    blockingAchievementNames.Add(GCTContext.AchievementCollectiom.GlobalAchievementInternalNumberToName(blockGlobAch)+" (global)");
            }

            return blockingAchievementNames;
        }

        /// <summary>
        /// Get all global achievements needed and not achieved
        /// </summary>
        /// <returns>Achievementnames + (global)</returns>
        public List<string> GetNeededGlobalAchievements()
        {
            List<string> neededAchievementNames = new List<string>();

            foreach (var neededGlobAch in Scenario.RequiredCompletedGlobalAchievements)
            {
                if (!GCTContext.CurrentCampaign.HasGlobalAchievement(neededGlobAch))
                    neededAchievementNames.Add(GCTContext.AchievementCollectiom.GlobalAchievementInternalNumberToName(neededGlobAch) + " (global)");
            }

            return neededAchievementNames;
        }

        public List<string> GetAllRequirements()
        {
            List<string> neededAchievementNames = new List<string>();

            foreach (var neededGlobAch in Scenario.RequiredCompletedGlobalAchievements)
            {
                neededAchievementNames.Add($"Gained {GCTContext.AchievementCollectiom.GlobalAchievementInternalNumberToName(neededGlobAch)} (global)");
            }

            foreach (var neededPartyAch in Scenario.RequiredPartyAchievements)
            {
                neededAchievementNames.Add($"Gained {GCTContext.AchievementCollectiom.PartyAchievementInternalNumberToName(neededPartyAch)} (party)");
            }

            foreach (var blockGlobAch in Scenario.BlockingGlobalAchievements)
            {
                neededAchievementNames.Add($"Not gained {GCTContext.AchievementCollectiom.GlobalAchievementInternalNumberToName(blockGlobAch)} (global)");
            }

            foreach (var blockPartyAch in Scenario.BlockingPartyAchievements)
            {
                neededAchievementNames.Add($"Not gained {GCTContext.AchievementCollectiom.PartyAchievementInternalNumberToName(blockPartyAch)} (party)");
            }

           
            return neededAchievementNames;
        }

        /// <summary>
        /// Get all party achievements achieved that block this scenario
        /// </summary>
        /// <returns>Achievementnames + (party)</returns>
        public List<string> GetBlockingPartyAchievements()
        {
            List<string> blockingAchievementNames = new List<string>();

            foreach (var blockPartyAch in Scenario.BlockingPartyAchievements)
            {
                if (GCTContext.CurrentCampaign.HasPartyAchievement(blockPartyAch))
                    blockingAchievementNames.Add(GCTContext.AchievementCollectiom.PartyAchievementInternalNumberToName(blockPartyAch) + " (party)");
            }

            return blockingAchievementNames;
        }

        /// <summary>
        /// Get all party achievements needed and not achieved
        /// </summary>
        /// <returns>Achievementnames + (party)</returns>
        public List<string> GetNeededPartyAchievements()
        {
            List<string> neededAchievementNames = new List<string>();

            foreach (var neededPartyAch in Scenario.RequiredPartyAchievements)
            {
                if (!GCTContext.CurrentCampaign.HasPartyAchievement(neededPartyAch))
                    neededAchievementNames.Add(GCTContext.AchievementCollectiom.PartyAchievementInternalNumberToName(neededPartyAch) + " (party)");
            }

            return neededAchievementNames;
        }

        /// <summary>
        /// Is the scenario blocked by an achievement
        /// </summary>
        /// <returns></returns>
        public bool IsBlocked()
        {
            foreach (var blockGlobAch in Scenario.BlockingGlobalAchievements)
            {
                if (GCTContext.CurrentCampaign.HasGlobalAchievement(blockGlobAch)) return true;
            }

            foreach (var blockPartyAch in Scenario.BlockingPartyAchievements)
            {
                if (GCTContext.CurrentCampaign.HasPartyAchievement(blockPartyAch)) return true;
            }

            return false;
        }        

        public List<int> GetUnlockedScenarios()
        {
            if (_unlockedScenarios == null)
            {
                _unlockedScenarios = new List<int>();
                var us = UnlockedScenarioData.Scenario.UnlockedScenarios.Split(',');
                foreach (var s in us)
                {
                    if (!int.TryParse(s, out int scenarioNumber)) continue;
                    _unlockedScenarios.Add(scenarioNumber);
                }
            }

            return _unlockedScenarios;
        }

        /// <summary>
        /// Is the scenario available, means not blocked and required achievements are achieved with specials
        /// </summary>
        /// <returns></returns>
        public bool IsAvailable()
        {
            if (IsBlocked() || Completed) return false;

            // specials

            // OR-connected Requirements
            var scenarioNumbers = new List<int> { 22, 26, 33, 102, 103, 104, 105, 106, 107, 108, 109 };
            if (scenarioNumbers.Contains(Scenarionumber))
            {                
                return (Scenario.RequiredCompletedGlobalAchievements.Any(x => GCTContext.CurrentCampaign.HasGlobalAchievement(x)) ||
                Scenario.RequiredPartyAchievements.Any(x => GCTContext.CurrentCampaign.HasPartyAchievement(x))) && 
                !(Scenario.BlockingGlobalAchievements.Any(x => GCTContext.CurrentCampaign.HasGlobalAchievement(x)) ||
                Scenario.BlockingPartyAchievements.Any(x => GCTContext.CurrentCampaign.HasPartyAchievement(x)));
            }

            foreach (var reqGlobAch in Scenario.RequiredCompletedGlobalAchievements)
            {
                if (!GCTContext.CurrentCampaign.HasGlobalAchievement(reqGlobAch)) return false;
            }

            foreach (var reqPartyAch in Scenario.RequiredPartyAchievements)
            {
                if (!GCTContext.CurrentCampaign.HasPartyAchievement(reqPartyAch)) return false;
            }

            return true;
        }
    }
}