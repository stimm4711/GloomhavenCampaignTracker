using System;
using System.Collections.Generic;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class ScenarioDataService : IDataService<DL_Scenario>
    {
        public void Delete(DL_Scenario item)
        {
            ScenarioRepository.Delete(item);
        }

        public DL_Scenario Get(long id)
        {
            return ScenarioRepository.Get(id);
        }

        public List<DL_Scenario> Get()
        {
            return ScenarioRepository.Get();
        }

        public void InsertOrReplace(DL_Scenario item)
        {
            ScenarioRepository.InsertOrReplace(item);
        }

        public DL_Scenario GetScenarioByScenarioNumber(int number)
        {
            return ScenarioRepository.GetScenarioByScenarioNumber(number);
        }

        public void UpdateScenarioRegion(int scenarioID, int regionId)
        {
            ScenarioRepository.UpdateScenarioRegion(scenarioID, regionId);
        }

        internal void UpdateUnlockedScenarios(int scenarioNumber, string commaSeparatedScenarioNumbers)
        {
            ScenarioRepository.UpdateScenarioUnlockedScenarios(scenarioNumber, commaSeparatedScenarioNumbers);
        }
    }
}