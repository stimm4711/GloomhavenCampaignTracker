using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Business
{
    public class CharacterSearch
    {
        private readonly int _id;

        public CharacterSearch(int id)
        {
            _id = id;
        }

        public bool FindId(DL_Character c)
        {
            return c.Id == _id;
        }
    }
}