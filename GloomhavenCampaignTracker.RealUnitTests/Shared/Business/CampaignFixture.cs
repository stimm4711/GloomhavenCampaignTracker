using System.Collections.Generic;
using System.Linq;
using GloomhavenCampaignTracker.RealUnitTests.Fakes;
using GloomhavenCampaignTracker.Shared.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GloomhavenCampaignTracker.RealUnitTests.Shared.Business
{
    [TestClass]
    public class CampaignFixture
    {
        private Campaign _campaign;
        private const string Campaignname = "testcampaign";

        [TestInitialize]
        public void SetUp()
        {
            DataServiceCollection.CampaignDataService = new CampaignDataServiceFake();
            _campaign = Campaign.NewInstance(Campaignname);
        }

        #region "Init and Constructor"

        [TestMethod]
        public void NewInstanceTest()
        {
            Assert.IsNull(_campaign.CurrentParty);
            Assert.IsNotNull(_campaign.CityEventDeck);
            Assert.IsNotNull(_campaign.GlobalAchievements);
            Assert.IsNotNull(_campaign.RoadEventDeck);
            Assert.IsNotNull(_campaign.UnlockedClassesIds);
            Assert.IsNotNull(_campaign.UnlockedScenarios);

            Assert.AreEqual(0, _campaign.GlobalAchievements.Count);

            Assert.IsNotNull(_campaign.CampaignData);
            Assert.IsNotNull(_campaign.CampaignData.Parties);
            Assert.IsNotNull(_campaign.CampaignData.EventDeckHistory);
            Assert.IsNotNull(_campaign.CampaignData.CampaignUnlocks);
            Assert.IsNotNull(_campaign.CampaignData.UnlockedItems);

            Assert.AreEqual(Campaignname, _campaign.CampaignData.Name);
            Assert.AreEqual(1, _campaign.CampaignData.CityProsperity);
            Assert.AreEqual(0, _campaign.CampaignData.DonatedGold);
            Assert.AreNotEqual("", _campaign.CampaignData.RoadEventDeckString);
            Assert.AreNotEqual("", _campaign.CampaignData.CityEventDeckString);

            Assert.AreEqual("1,2,3,4,5,6", _campaign.CampaignData.CampaignUnlocks.UnlockedClassesIds);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            var camp = new DL_Campaign
            {
                Name = "testCamp",
                CityProsperity = 3,
                DonatedGold = 50,
                Parties = new List<DL_CampaignParty>(),
                GlobalAchievements = new List<DL_CampaignGlobalAchievement>(),
                UnlockedScenarios = new List<DL_CampaignUnlockedScenario>(),
                CampaignUnlocks = new DL_CampaignUnlocks()
            };

            _campaign = new Campaign(camp);

            Assert.AreEqual(3, _campaign.CampaignData.CityProsperity);
            Assert.AreEqual(50, _campaign.CampaignData.DonatedGold);
        }

        #endregion

        [TestMethod]
        public void CampaignUnlocksTest()
        {
            _campaign = Campaign.NewInstance("testcampaign");
            _campaign.AddUnlockedClass(7);
            _campaign.AddUnlockedClass(10);
            _campaign.AddUnlockedClass(15);
            _campaign.CampaignData.CampaignUnlocks.EnvelopeAUnlocked = true;
            _campaign.CampaignData.CampaignUnlocks.EnvelopeBUnlocked = true;
            _campaign.CampaignData.CampaignUnlocks.ReputationMinus10Unlocked = true;
            _campaign.CampaignData.CampaignUnlocks.ReputationMinus20Unlocked = true;
            _campaign.CampaignData.CampaignUnlocks.ReputationPlus10Unlocked = true;
            _campaign.CampaignData.CampaignUnlocks.ReputationPlus20Unlocked = true;
            _campaign.CampaignData.CampaignUnlocks.TheDrakePartyAchievementsUnlocked = true;
            _campaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked = true;

            Assert.IsTrue(_campaign.CampaignData.CampaignUnlocks.EnvelopeAUnlocked);
            Assert.IsTrue(_campaign.CampaignData.CampaignUnlocks.EnvelopeBUnlocked);
            Assert.IsTrue(_campaign.CampaignData.CampaignUnlocks.ReputationMinus10Unlocked);
            Assert.IsTrue(_campaign.CampaignData.CampaignUnlocks.ReputationMinus20Unlocked);
            Assert.IsTrue(_campaign.CampaignData.CampaignUnlocks.ReputationPlus10Unlocked);
            Assert.IsTrue(_campaign.CampaignData.CampaignUnlocks.ReputationPlus20Unlocked);
            Assert.IsTrue(_campaign.CampaignData.CampaignUnlocks.TheDrakePartyAchievementsUnlocked);
            Assert.IsTrue(_campaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked);


            // Unlocked Class Ids
            List<int> expectedUnlockedClassesIds = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 10, 15 };
            Assert.IsTrue(_campaign.UnlockedClassesIds.SequenceEqual(expectedUnlockedClassesIds));

            // add twice
            _campaign.AddUnlockedClass(15);
            Assert.IsTrue(_campaign.UnlockedClassesIds.SequenceEqual(expectedUnlockedClassesIds));

            // remove
            expectedUnlockedClassesIds.Remove(7);
            _campaign.RemoveUnlockedClass(7);
            Assert.IsTrue(_campaign.UnlockedClassesIds.SequenceEqual(expectedUnlockedClassesIds), "7 removed");

            // remove item not in list
            _campaign.RemoveUnlockedClass(13);
            Assert.IsTrue(_campaign.UnlockedClassesIds.SequenceEqual(expectedUnlockedClassesIds), "Nothing removed");
        }
        %
        [TestMethod]
        public void AddGlobalAchievementTest()
        {
            _campaign = Campaign.NewInstance("testcampaign");

            // test global achievement
            Assert.IsFalse(_campaign.HasGlobalAchievement(42));

            var ga42 = new DL_AchievementType()
            {
                InternalNumber = 42,
                Name = "testGA42",
                Steps = 1,
            };

            _campaign.AddGlobalAchievement(ga42);

            Assert.IsTrue(_campaign.HasGlobalAchievement(42));

            // Test global achievements with subachievements
            Assert.IsFalse(_campaign.HasGlobalAchievement(4712));

            var ga4711 = new DL_AchievementType()
            {
                InternalNumber = 4711,
                Name = "testGA4711",
                Steps = 1,
            };

            ga4711.Achievements = new List<DL_Achievement>()
            {
                new DL_Achievement()
                {
                    Name = "testSubA4711_1",
                    AchievementType = ga4711,
                    InternalNumber = 1
                },
                new DL_Achievement()
                {
                    Name = "testSubA4711_2",
                    AchievementType = ga4711,
                    InternalNumber = 2
                }
            };

            _campaign.AddGlobalAchievement(ga4711);

            Assert.IsTrue(_campaign.HasGlobalAchievement(4712));

            // test global achievements with steps
            Assert.IsFalse(_campaign.HasGlobalAchievement(1338));

            var ga1337 = new DL_AchievementType()
            {
                InternalNumber = 1337,
                Name = "testGA1337",
                Steps = 3,
            };

            _campaign.AddGlobalAchievement(ga1337);

            Assert.IsTrue(_campaign.HasGlobalAchievement(1338));
        }

        [TestMethod]
        public void RemoveGlobalAchievementTest()
        {
            _campaign = Campaign.NewInstance("testcampaign");

            var ga42 = new DL_AchievementType()
            {
                InternalNumber = 42,
                Name = "testGA42",
                Steps = 1,
            };

            var cga = _campaign.AddGlobalAchievement(ga42);

            Assert.IsTrue(_campaign.HasGlobalAchievement(42));

            _campaign.RemoveGlobalAchievement(cga);

            Assert.IsFalse(_campaign.HasGlobalAchievement(42));
        }

        #region "Party"

        [TestMethod]
        public void AddPartyAchievementTest()
        {
            DataServiceCollection.PartyDataService = new DataServiceFake<DL_CampaignParty>();
            _campaign = Campaign.NewInstance("testcampaign");
            _campaign.AddParty("TestParty");

            // test global achievement
            Assert.IsFalse(_campaign.HasPartyAchievement(42));

            var pa42 = new DL_PartyAchievement()
            {
                InternalNumber = 42,
                Name = "testPA42"
            };

            _campaign.AddPartyAchievement(pa42);

            Assert.IsTrue(_campaign.HasPartyAchievement(42));

        }

        [TestMethod]
        public void PartyTest()
        {
            _campaign = Campaign.NewInstance("Testcampaign");

            DataServiceCollection.PartyDataService = new DataServiceFake<DL_CampaignParty>();

            Assert.AreEqual(0, _campaign.CampaignData.Parties.Count);

            _campaign.AddParty("Testparty");

            Assert.IsNotNull(_campaign.CampaignData.Parties);

            var party = _campaign.CampaignData.Parties.First();

            Assert.AreEqual("Testparty", party.Name);
            Assert.AreEqual(party, _campaign.CurrentParty);
            Assert.AreEqual(1, _campaign.CampaignData.Parties.Count);
        }
        #endregion

        #region "EventDecks"

        [TestMethod]
        public void SetEventDeckStringTest()
        {
            Assert.AreEqual(30, _campaign.CityEventDeck.GetItems().Count);
            _campaign.CityEventDeck.FromString("1,2,3,4");
            _campaign.SetEventDeckString(GloomhavenCampaignTracker.Shared.EventTypes.CityEvent);
            Assert.AreEqual(4, _campaign.CityEventDeck.GetItems().Count);
        }

        //[TestMethod]
        //public void AddEventToDeck()
        //{
        //    _campaign.CityEventDeck.FromString("");
        //    Assert.AreEqual(0, _campaign.CityEventDeck.GetItems().Count);

        //    _campaign.AddEventToDeck(45, GloomhavenCampaignTracker.Shared.EventTypes.CityEvent);

        //    Assert.AreEqual(1, _campaign.CityEventDeck.GetItems().Count);
        //    Assert.AreEqual(45, _campaign.CityEventDeck.GetItems().First());

        //    Assert.AreEqual(1, _campaign.CampaignData.EventDeckHistory.Count);
        //    Assert.AreEqual("Added, Shuffled", _campaign.CampaignData.EventDeckHistory.First().Action);
        //    Assert.AreEqual(1, _campaign.CampaignData.EventDeckHistory.First().EventType);
        //    Assert.AreEqual(45, _campaign.CampaignData.EventDeckHistory.First().ReferenceNumber);

        //}

        #endregion

        [TestMethod]
        public void SaveTest()
        {
            _campaign = Campaign.NewInstance("testcampaign");
            _campaign.AddUnlockedClass(7);
            _campaign.AddUnlockedClass(10);
            _campaign.AddUnlockedClass(15);

            _campaign.Save();

            Assert.AreEqual("1,2,3,4,5,6,7,10,15", _campaign.CampaignData.CampaignUnlocks.UnlockedClassesIds);
        }

        #region "Party Achievements"

        #endregion
    }
}
