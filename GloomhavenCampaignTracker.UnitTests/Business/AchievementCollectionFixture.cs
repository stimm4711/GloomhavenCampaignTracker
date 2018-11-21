using GloomhavenCampaignTracker.Shared.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using NUnit.Framework;

namespace GloomhavenCampaignTracker.UnitTests.Business
{
    [TestFixture]
    public class AchievementCollectionFixture
    {
        private AchievementCollection _achievementCollection;
        
        [SetUp]
        public void Setup()
        {
            DataServiceCollection.PartyAchievementDataService = new DataServiceFake<DL_PartyAchievement>();
            _achievementCollection = new AchievementCollection();
        }

        [TearDown]
        public void TearDown()
        {
            _achievementCollection = null;
        }

        [Test]
        public void GlobalAchievementNames()
        {
            // Get some names for existing Global Achievements and for a not existing "Unknown"
            Assert.AreEqual("Ancient Technology: 1/5", _achievementCollection.GlobalAchievementInternalNumberToName(151));
            Assert.AreEqual("Ancient Technology: 2/5", _achievementCollection.GlobalAchievementInternalNumberToName(152));
            Assert.AreEqual("Ancient Technology: 3/5", _achievementCollection.GlobalAchievementInternalNumberToName(153));
            Assert.AreEqual("Ancient Technology: 4/5", _achievementCollection.GlobalAchievementInternalNumberToName(154));
            Assert.AreEqual("Ancient Technology: 5/5", _achievementCollection.GlobalAchievementInternalNumberToName(155));
            Assert.AreEqual("Annihilation of the Order", _achievementCollection.GlobalAchievementInternalNumberToName(16));
            Assert.AreEqual("Artifact: Cleansed", _achievementCollection.GlobalAchievementInternalNumberToName(303));
            Assert.AreEqual("Artifact: Lost", _achievementCollection.GlobalAchievementInternalNumberToName(302));
            Assert.AreEqual("Artifact: Recovered", _achievementCollection.GlobalAchievementInternalNumberToName(301));
            Assert.AreEqual("Unknown", _achievementCollection.GlobalAchievementInternalNumberToName(1501));
        }

        void PartyAchievementInternalNumberToName()
        {
            // Get some names for existing Party Achievements and for a not existing "Unknown"
            Assert.AreEqual("First Steps", _achievementCollection.PartyAchievementInternalNumberToName(8));
            Assert.AreEqual("High Sea Escort", _achievementCollection.PartyAchievementInternalNumberToName(12));
            Assert.AreEqual("Unknown", _achievementCollection.PartyAchievementInternalNumberToName(1501));
        }
    }
}