using GloomhavenCampaignTracker.Shared.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GloomhavenCampaignTracker.UnitTests.Business
{
    public class PartyFixture
    {
        private Campaign _campaign;
        private Party _party;

        [SetUp]
        public void Setup()
        {
            DataServiceCollection.CampaignDataService = new DataServiceFake<DL_Campaign>();
            DataServiceCollection.PartyDataService = new DataServiceFake<DL_CampaignParty>();
            _campaign = Campaign.NewInstance("testcampaign");
        }

        [Test]
        public void NewInstanceTest()
        {
            _party = Party.NewInstance("testparty", _campaign);

            Assert.That(DataServiceCollection.PartyDataService.Get().Count.Equals(1));
            Assert.AreEqual(0, _party.PartyAchievements.Count);
            Assert.AreEqual(0, _party.Partymember.Count);
            Assert.AreEqual("testparty", _party.PartyName);
            Assert.AreEqual(0, _party.Reputation);
            Assert.NotNull(_party.PartyData);
        }

        [Test]
        public void Constructor()
        {
            var partyData = new DL_CampaignParty()
            {
                Id = 2,
                Name = "testparty",
                PartyAchievements = new List<DL_CampaignPartyAchievement>(),
                Reputation = 13,
                PartyMember = new List<DL_Character>()
            };

            partyData.PartyAchievements.Add(new DL_CampaignPartyAchievement()
            {
                Party = partyData,
                Id = 1,
                ID_Party = 2,
                ID_PartyAchievement = 1,
                PartyAchievement = new DL_PartyAchievement() { Id = 1, InternalNumber = 232, Name = "testpartyachievement" }
            });

            partyData.PartyMember.Add(new DL_Character()
            {
                Class = 1,
                Name = "testchar",
                Id = 1,
                ID_Party = 2,
                Level = 3,
                Party = partyData,
                Checkmarks = 0,
                Experience = 0,
                Gold = 0,
                ItemsCommaSeparted = "",
                Retired = false,
                Abilities = new List<DL_Ability>(),
                Perks = new List<DL_Perk>()
            });

            _party = new Party(partyData);

            Assert.AreEqual(13, _party.Reputation);
            Assert.AreEqual(2, _party.PartyData.Id);
            Assert.AreEqual(1, _party.Partymember.Count);
            Assert.AreEqual(1, _party.PartyAchievements.Count);
        }

        [Test]
        public void PartyAchievementsTest()
        {
            _party = Party.NewInstance("testparty", _campaign);

            Assert.IsFalse(_party.HasPartyAchievement(23));

            var pa = _party.AddPartyAchievement(new DL_PartyAchievement() { Id = 1, InternalNumber = 23, Name = "testachievement" });

            Assert.IsTrue(_party.HasPartyAchievement(23));

            _party.RemoveAchievement(pa);

            Assert.IsFalse(_party.HasPartyAchievement(23));
        }

        [Test]
        public void PartyMemberTest()
        {
            //DataServiceCollection.PartymemberDataService = new DataServiceFake<DL_Character>();

            //_party = Party.NewInstance("testparty", _campaign);

            //Assert.AreEqual(0, _party.Partymember.Count);

            //var character = GCTContext.CharacterCollection.NewCharacter("testpartymember", 2, 321);
            // _party.AddPartyMember(character);

            //Assert.AreEqual(1, _party.Partymember.Count);
            //Assert.AreEqual(1, _party.PartyData.PartyMember.Count);
            //Assert.AreEqual("testpartymember", _party.Partymember.First().Charactername);
            //Assert.AreEqual(2, _party.Partymember.First().CharacterClass);
            //Assert.AreEqual(1, _party.Partymember.First().Characterlevel);

            //_party.UnassignPartymember(character);

            //Assert.AreEqual(0, _party.Partymember.Count);
        }


    }
}