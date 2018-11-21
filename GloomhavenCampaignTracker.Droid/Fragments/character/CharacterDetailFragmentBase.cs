using Android.OS;
using Android.Views;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using Android.Support.V4.App;
using GloomhavenCampaignTracker.Shared.Business;
using System.Linq;

namespace GloomhavenCampaignTracker.Droid.Fragments.character
{
    public abstract class CharacterDetailFragmentBase : Fragment
    {
        public static readonly string charID = "CharacterID";

        protected LayoutInflater _inflater;
        protected View _view;
        protected DL_Character _character;
        protected int _characterId = -1;

        public DL_Character Character
        {
            get
            {
                if (_character != null) return _character;

                if (_characterId > 0)
                {
                    _character = GCTContext.CharacterCollection.FirstOrDefault(x => x.Id == _characterId);
                }

                if (Arguments != null)
                {
                    _characterId = Arguments.GetInt(charID, 0);
                }

                _character = GCTContext.CharacterCollection.FirstOrDefault(x => x.Id == _characterId);
                
                return _character;
            }
            set
            {
                _character = value;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {           
            _inflater = inflater;
            return _view;
        }
        
    
        protected void SaveCharacter()
        {
            if(_character != null)  DataServiceCollection.CharacterDataService.InsertOrReplace(_character);
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            if (_character != null) outState.PutInt(charID, _character.Id);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (savedInstanceState != null)
            {
                _characterId = savedInstanceState.GetInt(charID);
            }          
        }
    }
}  