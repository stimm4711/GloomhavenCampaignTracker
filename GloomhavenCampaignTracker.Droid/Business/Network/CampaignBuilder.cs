using GloomhavenCampaignTracker.Business.Network.Messages;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GloomhavenCampaignTracker.Business.Network
{
    public class CampaignBuilder
    {
        private CampaignExchangeData _campaignExchangeData;

        public CampaignBuilder(CampaignExchangeData campaignExchangeData)
        {
            _campaignExchangeData = campaignExchangeData;
        }

        public void BuildCampaign()
        {
            if (_campaignExchangeData == null) return;
            DL_Campaign campaign = SaveCampaign();

            SaveScenarios(campaign);                       
        }

        private void SaveScenarios(DL_Campaign campaign)
        {
            var time = DateTime.Now;

            // unlocked scenarios
            foreach (ICampaignUnlockedScenarioExchange cus in _campaignExchangeData.CampaignUnlockedScenarios)
            {
                var scenario = ScenarioRepository.Get().FirstOrDefault(x => x.Id == cus.ID_Scenario);

                if (scenario == null) continue;

                DL_CampaignUnlockedScenario unlockedScenario = new DL_CampaignUnlockedScenario()
                {
                    Completed = cus.Completed,
                    ID_Scenario = scenario.Id,
                    Campaign = campaign,
                    ID_Campaign = campaign.Id,
                    Scenario = scenario,
                    ScenarioTreasures = new List<DL_Treasure>(),
                    LastSync = time
                };

                CampaignUnlockedScenarioRepository.InsertOrReplace(unlockedScenario);

                foreach (ITreasureExchange st in _campaignExchangeData.ScenarioTreasures.Where(x => x.CampaignScenario_ID == cus.Id))
                {
                    DL_Treasure t = new DL_Treasure()
                    {
                        CampaignScenario_ID = unlockedScenario.Id,
                        Looted = st.Looted,
                        Number = st.Number,
                        UnlockedScenario = unlockedScenario,
                        Content = st.Content
                    };

                    unlockedScenario.ScenarioTreasures.Add(t);
                }              

                campaign.UnlockedScenarios.Add(unlockedScenario);
                DataServiceCollection.CampaignDataService.InsertOrReplace(campaign, recursive: true);
            }
        }

        private DL_Campaign SaveCampaign()
        {
            // camppaign
            DL_Campaign campaign = new DL_Campaign()
            {
                CityEventDeckString = _campaignExchangeData.Campaign.CityEventDeckString,
                CityProsperity = _campaignExchangeData.Campaign.CityProsperity,
                DonatedGold = _campaignExchangeData.Campaign.DonatedGold,
                GlobalAchievements = new List<DL_CampaignGlobalAchievement>(),
                Name = _campaignExchangeData.Campaign.Name,
                Parties = new List<DL_CampaignParty>(),
                RoadEventDeckString = _campaignExchangeData.Campaign.RoadEventDeckString,
                UnlockedItems = new List<DL_Item>(),
                UnlockedScenarios = new List<DL_CampaignUnlockedScenario>()
            };

            // global achievements
            foreach (DL_CampaignGlobalAchievement globalachievement in _campaignExchangeData.CampaignGlobalAchievements)
            {
                var achType = AchievementTypeRepository.Get().FirstOrDefault(x => x.Id == globalachievement.ID_AchievementType);
                var ach = AchievementRepository.Get().FirstOrDefault(x => x.Id == globalachievement.ID_Achievement);

                if (achType == null) continue;

                campaign.GlobalAchievements.Add( new DL_CampaignGlobalAchievement()
                {
                    Achievement = ach,
                    AchievementType = achType,
                    Campaign = campaign,
                    ID_AchievementType = achType.Id,
                    Step = globalachievement.Step
                });
            }

            // Campaign Unlocks
            campaign.CampaignUnlocks = (DL_CampaignUnlocks)_campaignExchangeData.CampaignUnlocks;

            // unlocked items
            foreach (ICampaignUnlockedItemExchange unlockedItem in _campaignExchangeData.CampaignUnlockedItems)
            {
                campaign.UnlockedItems.Add(ItemRepository.Get(unlockedItem.ID_Item));
            }           

            AddCampaignParties(campaign);

            DataServiceCollection.CampaignDataService.InsertOrReplace(campaign, true);

            foreach (ICampaignEventHistoryLogItemExchange campEventHistoryLogItem in _campaignExchangeData.CampaignEventHistoryLogItems)
            {
                DL_CampaignEventHistoryLogItem cehi = new DL_CampaignEventHistoryLogItem()
                {
                    Action = campEventHistoryLogItem.Action,
                    ID_Campaign = campaign.Id,
                    Decision = campEventHistoryLogItem.Decision,
                    EventType = campEventHistoryLogItem.EventType,
                    Outcome = campEventHistoryLogItem.Outcome,
                    Position = campEventHistoryLogItem.Position,
                    ReferenceNumber = campEventHistoryLogItem.ReferenceNumber
                };

                CampaignEventHistoryLogItemRepository.InsertOrReplace(cehi);
            }

            return campaign;
        }

        private void AddCampaignParties(DL_Campaign campaign)
        {
            foreach (IPartyExchange party in _campaignExchangeData.CampaignParties)
            {
                DL_CampaignParty campParty = new DL_CampaignParty()
                {
                    Campaign = campaign,
                    Name = party.Name,
                    Notes = party.Notes,
                    Reputation = party.Reputation,
                    CurrentLocationNumber = party.CurrentLocationNumber,
                    PartyAchievements = new List<DL_CampaignPartyAchievement>()
                };

                AddPartyachievements(party, campParty);

                DataServiceCollection.PartyDataService.InsertOrReplace(campParty);

                AddCharacters(party, campParty);

                campaign.Parties.Add(campParty);
            }
        }

        private void AddPartyachievements(IPartyExchange party, DL_CampaignParty campParty)
        {
            foreach (IPartyAchievementExchange partyachievements in _campaignExchangeData.PartyAchievements.Where(x => x.ID_Party == party.Id))
            {
                var pa = PartyAchievementRepository.Get(partyachievements.ID_PartyAchievement);

                var cpa = new DL_CampaignPartyAchievement()
                {
                    ID_PartyAchievement = pa.Id,
                    Party = campParty,
                    PartyAchievement = pa
                };

                campParty.PartyAchievements.Add(cpa);
            }
        }

        private void AddCharacters(IPartyExchange party, DL_CampaignParty campParty)
        {
            foreach (ICharacterExchange character in _campaignExchangeData.Characters.Where(x => x.ID_Party == party.Id))
            {

                DL_PersonalQuest pq = null;                

                if (character.ID_PersonalQuest > 0)
                    pq = PersonalQuestRepository.Get(character.ID_PersonalQuest, false);

                var chara = new DL_Character()
                {
                    Abilities = new List<DL_Ability>(),
                    ID_PersonalQuest = (pq == null) ? 0 : pq.Id,
                    Items = new List<DL_Item>(),
                    Level = character.Level,
                    LifegoalNumber = character.LifegoalNumber,
                    Name = character.Name,
                    Notes = character.Notes,
                    Party = campParty,
                    CharacterPerks = new List<DL_ClassPerk>(),
                    Checkmarks = character.Checkmarks,
                    ClassId = character.ClassId,
                    Experience = character.Experience,
                    Gold = character.Gold,
                    Perks = new List<DL_Perk>(),
                    PersonalQuest = pq,
                    Playername = character.Playername,
                    Retired = character.Retired,
                    ID_Party = campParty.Id,
                   
                };

                AddCharacterAbilities(character, chara);

                AddCharacterPerks(character, chara);

                AddPerks(character, chara);

                AddCharacterItems(character, chara);

                CharacterRepository.InsertOrReplace(chara);
            }
        }

        private void AddCharacterItems(ICharacterExchange character, DL_Character chara)
        {
            foreach (ICharacterItemExchange itemEx in _campaignExchangeData.CharacterItems.Where(x => x.ID_Character == character.Id))
            {
                var itemn = ItemRepository.Get(itemEx.ID_Item);
                chara.Items.Add(itemn);
            }
        }

        private void AddPerks(ICharacterExchange character, DL_Character chara)
        {
            foreach (IPerkExchange perkEx in _campaignExchangeData.Perks.Where(x => x.ID_Character == character.Id))
            {
                var p = new DL_Perk()
                {
                    Character = chara,
                    Perkcomment = perkEx.Perkcomment,
                    Checkboxnumber = perkEx.Checkboxnumber
                };

                chara.Perks.Add(p);
            }
        }

        private void AddCharacterPerks(ICharacterExchange character, DL_Character chara)
        {
            foreach (ICharacterPerkExchange charPerk in _campaignExchangeData.CharacterPerks.Where(x => x.ID_Character == character.Id))
            {
                chara.CharacterPerks.Add(ClassPerkRepository.Get(charPerk.ID_ClassPerk));
            }
        }

        private void AddCharacterAbilities(ICharacterExchange character, DL_Character chara)
        {
            foreach (ICharacterAbilityExchange characterabilities in _campaignExchangeData.CharacterAbilities.Where(x => x.ID_Character == character.Id))
            {
                var ab = new DL_Ability()
                {
                    AbilityName = characterabilities.AbilityName,
                    Character = chara,
                    Enhancements = new List<DL_AbilityEnhancement>(),
                    Level = characterabilities.Level,
                    Name = characterabilities.Name,
                    ReferenceNumber = characterabilities.ReferenceNumber
                };

                foreach (IAbilityEnhancementExchange enhance in _campaignExchangeData.AbilityEnhancements.Where(x => x.ID_Ability == characterabilities.Id))
                {
                    var enh = EnhancementRepository.Get(enhance.ID_Enhancement);
                    var abEnh = new DL_AbilityEnhancement()
                    {
                        Ability = ab,
                        Enhancement = enh,
                        ID_Enhancement = enh.Id,
                        IsTop = enhance.IsTop,
                        SlotNumber = enhance.SlotNumber
                    };

                    ab.Enhancements.Add(abEnh);
                }

                chara.Abilities.Add(ab);
            }
        }
    }
}
