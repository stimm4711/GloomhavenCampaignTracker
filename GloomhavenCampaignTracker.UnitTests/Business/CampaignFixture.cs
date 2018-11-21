using System.Linq;
using GloomhavenCampaignTracker.Shared.Business;
using NUnit.Framework;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.UnitTests.Business
{
    partial class CampaignFixture
    {
        Campaign _campaign;

        [SetUp]
        public void Setup()
        {
            DataServiceCollection.CampaignDataService = new DataServiceFake<DL_Campaign>();
            DataServiceCollection.ScenarioDataService = new DataServiceFake<DL_Scenario>();

            

            
        }

        

        
       
       

        
    }
}