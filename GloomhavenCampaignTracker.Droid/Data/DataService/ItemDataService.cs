using System.Collections.Generic;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class ItemDataService : IDataService<DL_Item>
    {
        public void Delete(DL_Item item)
        {
            ItemRepository.Delete(item);
        }

        public DL_Item Get(long id)
        {
            return ItemRepository.Get(id);
        }

        public List<DL_Item> Get()
        {
            return ItemRepository.Get();
        }

        public void InsertOrReplace(DL_Item item)
        {
            ItemRepository.InsertOrReplace(item);
        }

        public List<DL_Item> GetByProsperity(int prosperity)
        {
            return ItemRepository.GetByProsperity(prosperity);
        }

        public List<DL_Item> GetByItemnumber(int itemnumber)
        {
            return ItemRepository.GetItemByItemNumber(itemnumber);
        }

        public List<DL_Item> GetUnlockableItems()
        {
            return ItemRepository.GetUnlockableItems();
        }        

        public List<DL_Item> GetSelectableItems(int prosperity, int campaignId)
        {
            return ItemRepository.GetSelectableItems(prosperity, campaignId);
        }
    }
}