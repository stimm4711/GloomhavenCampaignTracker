using System;
using System.Collections.Generic;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class CharacterDataService : IDataService<DL_Character>
    {
        public void Delete(DL_Character item)
        {
            CharacterRepository.Delete(item);
        }

        public DL_Character Get(long id)
        {
            return CharacterRepository.Get(id);
        }

        public List<DL_Character> Get()
        {
            return CharacterRepository.Get();
        }

        public void InsertOrReplace(DL_Character item)
        {
            CharacterRepository.InsertOrReplace(item);
        }

        internal List<DL_Item> GetItemsOfCharacter(int id)
        {
            return CharacterRepository.GetItemsOfCharacter(id);
        }

        internal DL_CampaignParty GetPartyOfCharacter(int id)
        {
            return CharacterRepository.GetPartyOfCharacter(id);
        }

        internal List<DL_Character> GetPartymembers(int id)
        {
            return CharacterRepository.GetPartymembers(id);
        }

        internal List<DL_Character> GetPartymembersFlat(int id)
        {
            return CharacterRepository.GetPartymembersFlat(id);
        }

        internal List<DL_Character> GetPartymembersUnretiredFlat(int id)
        {
            return CharacterRepository.GetPartymembersUnretiredFlat(id);
        }

        internal List<DL_Character> GetCharactersUnretiredFlat()
        {
            return CharacterRepository.GetCharactersUnretiredFlat();
        }

        internal List<DL_Character> GetCharactersFlat()
        {
            return CharacterRepository.GetCharactersFlat();
        }

        internal List<DL_ClassPerk> GetClassPerks(int classid, int characterid)
        {
            return CharacterRepository.GetClassPerks(classid, characterid);
        }

        internal List<DL_PersonalQuest> GetPersonalQuestsFlat()
        {
            return CharacterRepository.GetPersonalQuestsFlat();
        }
    }
}