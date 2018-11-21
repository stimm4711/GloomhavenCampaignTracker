using System.Collections.Generic;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Shared.Business
{
    public class ScenarioCollection
    {
        public ScenarioCollection()
        {
            foreach (DL_Scenario s in ScenarioRepository.Get())
            {
                Items.Add(new Scenario(s));
            }
        }

        public List<Scenario> Items { get; } = new List<Scenario>();


    }
}