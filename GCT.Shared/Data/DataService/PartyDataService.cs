using System.Collections.Generic;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class PartyDataService : IDataService<DL_CampaignParty>
    {
        public void Delete(DL_CampaignParty item)
        {
            CampaignPartyRepository.Delete(item);
        }

        public DL_CampaignParty Get(long id)
        {
            return CampaignPartyRepository.Get(id);
        }

        public List<DL_CampaignParty> Get()
        {
            return CampaignPartyRepository.Get();
        }

        public void InsertOrReplace(DL_CampaignParty item)
        {
            CampaignPartyRepository.InsertOrReplace(item);
        }
       
    }
}