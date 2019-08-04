using System.Collections.Generic;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class ClassAbilityDataService : IDataService<DL_ClassAbility>
    {
        public void Delete(DL_ClassAbility item)
        {
            ClassAbilitiesRepository.Delete(item);
        }

        public DL_ClassAbility Get(long id)
        {
            return ClassAbilitiesRepository.Get(id);
        }

        public List<DL_ClassAbility> Get()
        {
            return ClassAbilitiesRepository.Get();
        }

        public void InsertOrReplace(DL_ClassAbility item)
        {
            ClassAbilitiesRepository.InsertOrReplace(item);
        }
        public List<DL_ClassAbility> GetSelectableAbilities(int ID_class, int characterlevel)
        {
            return ClassAbilitiesRepository.GetSelectableAbilities(ID_class, characterlevel);
        }
    }
}