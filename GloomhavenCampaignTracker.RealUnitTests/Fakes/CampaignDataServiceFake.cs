using System.Collections.Generic;
using System.Linq;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.RealUnitTests.Fakes
{
    public class CampaignDataServiceFake : ICampaignDataService
    {
        private readonly List<DL_Campaign> _items = new List<DL_Campaign>();

        public void Delete(DL_Campaign item)
        {
            _items.Remove(item);
        }

        public List<DL_Item> GetUnlockableItems(int campaignId)
        {
            return null;
        }

        public List<DL_Scenario> GetUnlockedScenarios(int campaignId)
        {
            return null;
        }

        public List<DL_Campaign> GetCampaignsFlat()
        {
            return null;
        }

        public List<DL_CampaignUnlockedScenario> GetUnlockedCampaignScenarios(int campaignId)
        {
            return null;
        }

        public List<DL_CampaignParty> GetParties(int campaignId)
        {
            return null;
        }

        public List<DL_Character> GetRetiredCharacters(int campaignId)
        {
            return null;
        }

        public DL_Campaign Get(long id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public List<DL_Campaign> Get()
        {
            return _items;
        }

        public void InsertOrReplace(DL_Campaign item)
        {
            var existingItem = _items.FirstOrDefault(y => y.Id == item.Id);                

            if (existingItem != null)
            {
                _items.Remove(existingItem);
            }
            else
            {
                var maxId = _items.Any() ? _items.Max(x => x.Id) : 0;
                item.Id = maxId + 1;
            }
               
            _items.Add(item);
        }

        public List<DL_AchievementType> GetAchievementTypesFlat()
        {
            return new List<DL_AchievementType>();
        }

        public DL_Campaign Get(long id, bool recursive = true)
        {
            throw new System.NotImplementedException();
        }
    }
}