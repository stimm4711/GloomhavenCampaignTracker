using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    public class CharacterWrapper : Java.Lang.Object
    {
        private DL_Character _char;

        public CharacterWrapper(DL_Character character)
        {
            _char = character;
        }

        public DL_Character Character => _char;

    }

    public class TreasureWrapper : Java.Lang.Object
    {
        private DL_Treasure _treasure;

        public TreasureWrapper(DL_Treasure treasure)
        {
            _treasure = treasure;
        }

        public DL_Treasure Treasure => _treasure;

    }
}