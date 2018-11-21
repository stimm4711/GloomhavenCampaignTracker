using Android.Content;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    /// <summary>
    /// Adapter for Character abilities
    /// </summary>
    internal abstract class CharacterDetailsAdapterBase : BaseAdapter
    {
        protected readonly Context _context;
        protected readonly DL_Character _character;

        public CharacterDetailsAdapterBase(Context context, DL_Character character)
        {
            _context = context;
            _character = character;
        }

        public override long GetItemId(int position)
        {
            return position;
        }      

        public override Object GetItem(int position)
        {
            return position;
        }

        protected void SaveCharacter()
        {
            DataServiceCollection.CharacterDataService.InsertOrReplace(_character);
        }
    }
}