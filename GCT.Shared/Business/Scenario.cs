using System.Collections.Generic;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Shared.Business
{
    public class Scenario : ISpinneritem
    {
        public Scenario(DL_Scenario scenarioData)
        {
            ScenarioData = scenarioData;
        }

        private void SetBlockingPartyAchievements()
        {
            var blockPartyAchInternalNumbers = ScenarioData.BlockingPartyAchievements.Split(',');

            m_blockingPartyAchievements.Clear();
            foreach (var i in blockPartyAchInternalNumbers)
            {
                if (int.TryParse(i, out int id))
                {
                    m_blockingPartyAchievements.Add(id);
                }
            }
        }

        private void SetUnlockScenariosOnComletation()
        {
            if (string.IsNullOrEmpty(ScenarioData.UnlockedScenarios)) return;

            var scenarioIds = ScenarioData.UnlockedScenarios.Split(',');

            m_unlockScenariosOnCompletation.Clear();
            foreach (var i in scenarioIds)
            {
                if (int.TryParse(i, out int scenarioId))
                {
                    m_unlockScenariosOnCompletation.Add(scenarioId);
                }
            }
        }

        private void SetRequiredCompletedGlobalAchievements()
        {
            if (string.IsNullOrEmpty(ScenarioData.RequiredGlobalAchievements)) return;

            var reqGlobAchInternalNumbers = ScenarioData.RequiredGlobalAchievements.Split(',');

            m_requiredCompletedGlobalAchievements.Clear();
            foreach (var i in reqGlobAchInternalNumbers)
            {
                if (int.TryParse(i, out int id))
                {
                    m_requiredCompletedGlobalAchievements.Add(id);
                }
            }
        }

        private void SetBlockingGlobalAchievements()
        {
            if (string.IsNullOrEmpty(ScenarioData.BlockingGlobalAchievements)) return;

            var blockGlobAchInternalNumbers = ScenarioData.BlockingGlobalAchievements.Split(',');

            m_blockingGlobalAchievements.Clear();
            foreach (var i in blockGlobAchInternalNumbers)
            {
                if (int.TryParse(i, out int id))
                {
                    m_blockingGlobalAchievements.Add(id);
                }
            }
        }

        private void SetRequiredPartyAchievements()
        {
            if (string.IsNullOrEmpty(ScenarioData.RequiredPartyAchievements)) return;

            var partyAchInternalNumbers = ScenarioData.RequiredPartyAchievements.Split(',');

            m_requiredPartyAchievements.Clear();
            foreach (var i in partyAchInternalNumbers)
            {
                if (int.TryParse(i, out int id))
                {
                    m_requiredPartyAchievements.Add(id);
                }
            }
        }



        public int ScenarioNumber => ScenarioData.Scenarionumber;

        public string ScenarioName => ScenarioData.Name;

        public int ScenarioId => ScenarioData.Id;

        public DL_Scenario ScenarioData { get; }

        private List<int> m_unlockScenariosOnCompletation;
       

        private HashSet<int> m_requiredCompletedGlobalAchievements;
        public HashSet<int> RequiredCompletedGlobalAchievements
        {
            get
            {
                if (m_requiredCompletedGlobalAchievements != null) return m_requiredCompletedGlobalAchievements;

                m_requiredCompletedGlobalAchievements = new HashSet<int>();
                SetRequiredCompletedGlobalAchievements();
                return m_requiredCompletedGlobalAchievements;
            } 

        }
        
        private HashSet<int> m_blockingGlobalAchievements;
        public HashSet<int> BlockingGlobalAchievements
        {
            get
            {
                if (m_blockingGlobalAchievements != null) return m_blockingGlobalAchievements;

                m_blockingGlobalAchievements = new HashSet<int>();
                SetBlockingGlobalAchievements();
                return m_blockingGlobalAchievements;
            }

        }

        private HashSet<int> m_requiredPartyAchievements;
        public HashSet<int> RequiredPartyAchievements
        {
            get
            {
                if (m_requiredPartyAchievements != null) return m_requiredPartyAchievements;

                m_requiredPartyAchievements = new HashSet<int>();
                SetRequiredPartyAchievements();
                return m_requiredPartyAchievements;
            }
        }

        private HashSet<int> m_blockingPartyAchievements;
        public HashSet<int> BlockingPartyAchievements
        {
            get
            {
                if (m_blockingPartyAchievements != null) return m_blockingPartyAchievements;

                m_blockingPartyAchievements = new HashSet<int>();
                SetBlockingPartyAchievements();
                return m_blockingPartyAchievements;
            }
        }

        public string Spinnerdisplayvalue
        {
            get
            {
                if(GCTContext.ShowScenarioNames)
                {
                    return $"# {ScenarioNumber}   {ScenarioName}";
                }
                else
                {
                    return $"# {ScenarioNumber}";
                }                
            }
        }
    }
}