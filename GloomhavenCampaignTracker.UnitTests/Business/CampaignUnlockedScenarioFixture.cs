using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Business;
using NUnit.Framework;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.UnitTests
{
    class CampaignUnlockedScenarioFixture
    {
        private CampaignUnlockedScenario _objectTotest;

        [TearDown]
        public void Tear()
        {
            _objectTotest = null;
        }

        [Test]
        public void Constructor()
        {
            var campaignUnlockedScenario = new DL_CampaignUnlockedScenario()
            {
                Completed = false,
                Id = 1,
                ID_Campaign = 1,
                ID_Scenario = 23,
                ScenarioTreasures = new List<DL_Treasure>()
            };

            _objectTotest = new CampaignUnlockedScenario(campaignUnlockedScenario);

        }

    }
}