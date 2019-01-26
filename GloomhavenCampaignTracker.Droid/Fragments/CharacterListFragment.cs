using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Business;
using ListFragment = Android.Support.V4.App.ListFragment;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.party;
using System.Collections.Generic;
using Android.Content;
using GloomhavenCampaignTracker.Shared;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System;
using System.Linq;
using Android.Preferences;

namespace GloomhavenCampaignTracker.Droid.Fragments
{
    public class CharacterListFragment : ListFragment
    {
        protected List<DL_Character> _characters = new List<DL_Character>();
        private FloatingActionButton _fab;
        private CampaignPartyFragment _frag;
        private bool _hideRetired;
        private bool _showRetired = true;

        public static CharacterListFragment NewInstance(bool justCurrentParty)
        {
            var frag = new CharacterListFragment
            {
                Arguments = new Bundle(),
                Justparty = justCurrentParty
            };
            return frag;
        }

        public bool Justparty { get; set; } = false;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            FillListAdapter();
            HasOptionsMenu = true;
        }

        protected virtual void SetCharacters()
        {
            try
            {
                if (Justparty)
                {
                    _characters = new List<DL_Character>();
                    if (GCTContext.CurrentCampaign != null && GCTContext.CurrentCampaign.CurrentParty != null)
                    {
                        if (_hideRetired)
                        {
                            _characters = DataServiceCollection.CharacterDataService.GetPartymembersUnretiredFlat(GCTContext.CurrentCampaign.CurrentParty.Id);
                        }
                        else
                        {
                            _characters = DataServiceCollection.CharacterDataService.GetPartymembersFlat(GCTContext.CurrentCampaign.CurrentParty.Id);
                        }                                           
                    }
                }
                else
                {
                    if (_hideRetired)
                    {
                        _characters = DataServiceCollection.CharacterDataService.GetCharactersUnretiredFlat();
                    }
                    else
                    {
                        _characters = DataServiceCollection.CharacterDataService.GetCharactersFlat();
                    }                   
                }
               
                GCTContext.CharacterCollection = _characters;
            }
            catch
            {
                Toast.MakeText(Context, "Error on loading the character list.", ToastLength.Long);
            }            
        }

        private void FillListAdapter()
        {
            SetCharacters();
            ListAdapter = new CharacterAdapter(Context, _characters, Justparty, _hideRetired);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.listview_floatingactionbutton, container, false);
            _fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab);

            var prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
            _hideRetired = prefs.GetBoolean("FilterRetired", true);

            return view;
        }

        public override void OnResume()
        {
            FillListAdapter();
            base.OnResume();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            ListView.ItemClick += Listview_ItemClick;

            if (!_fab.HasOnClickListeners)
            {
                _fab.Click += (sender, e) =>
                {
                    AddNewCharacterDialog();
                };
            }
        }

        private void Listview_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var characterID = ((CharacterAdapter)ListAdapter).GetCharacterId(e.Position);
            ShowCharacterDetail(characterID);
        }

        private void ShowCharacterDetail(int id)
        {            
            if (_frag != null)
            {
                _frag.ShowCharacterDetail(id);
            }    
            else
            {
                var intent = new Intent();
                intent.SetClass(Context, typeof(DetailsActivity));
                intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.CharacterDetail);
                intent.PutExtra(DetailsActivity.SelectedCharacterId, id);
                intent.PutExtra(DetailsActivity.JustParty, Justparty);
                Context.StartActivity(intent);
            }
        }

        public void SetPartyFrag(CampaignPartyFragment frag)
        {
            _frag = frag;
        }

        private void AddNewCharacterDialog()
        {
            var inflater = Activity.LayoutInflater;
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_addpartymember, null);
            var spinner = convertView.FindViewById<Spinner>(Resource.Id.characterClassSpinner);
            var adapter = new CharacterClassAdapter(Context, showAll: !Justparty);
            spinner.Adapter = adapter;
            
            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)                     
                .SetCustomView(convertView)
                .SetTitle(Resources.GetString(Resource.String.AddCharacter))
                .SetPositiveButton(Resources.GetString(Resource.String.Add), (senderAlert, args) =>
                {
                    var charNameEdit = (EditText)convertView.FindViewById(Resource.Id.character_name);
                    
                    if (string.IsNullOrEmpty(charNameEdit.Text))
                    {
                        Toast.MakeText(Context, Resources.GetString(Resource.String.CharacterNameMissing), ToastLength.Short).Show();
                        return;
                    }
                    

                    var classId = (int)spinner.Adapter.GetItem(spinner.SelectedItemPosition);
                    AddCharacter(charNameEdit.Text, classId, -1);
                    FillListAdapter();                   
                })
                .Show();          
        }

        protected virtual void AddCharacter(string charname, int classId, int personalQuest)
        {
            var charac = new DL_Character()
            {
                Level = 1,
                Name = charname,
                ClassId = classId,
                Abilities = new List<DL_Ability>(),
                Experience = 0,
                Checkmarks = 0,
                Gold = 0,
                Retired = false,
                Party = null,
                Perks = new List<DL_Perk>(),
                LifegoalNumber = personalQuest,
                Items = new List<DL_Item>(),
                PersonalQuest = null,
                Notes = ""
            };            
            
            if (Justparty)
            {
                charac.Party = GCTContext.CurrentCampaign.CurrentParty;
                charac.ID_Party = GCTContext.CurrentCampaign.CurrentParty.Id;
            }
            else
            {
                _characters.Add(charac);
            }

            DataServiceCollection.CharacterDataService.InsertOrReplace(charac);
        }

        internal void ShowRetired()
        {
            _hideRetired = false;
            FillListAdapter();
        }

        internal void HideRetired()
        {
            _hideRetired = true;
            FillListAdapter();
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            //change main_compat_menu
            inflater.Inflate(Resource.Menu.characterlistMenu, menu);

            var item = menu.FindItem(Resource.Id.action_filter_retired);

            if (item != null)
            {
                var prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
                var isFilterRetired = prefs.GetBoolean("FilterRetired", true);

                item.SetIcon(isFilterRetired ? Resource.Drawable.ic_filter_retired : Resource.Drawable.ic_show_retired);

                _showRetired = !isFilterRetired;
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_filter_retired)
            {
                if (_showRetired)
                {
                    HideRetired();
                    item.SetIcon(Resource.Drawable.ic_filter_retired);
                    _showRetired = false;
                }
                else
                {
                    ShowRetired();
                    item.SetIcon(Resource.Drawable.ic_show_retired);
                    _showRetired = true;
                }

                var prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
                var editor = prefs.Edit();
                editor.PutBoolean("FilterRetired", !_showRetired);
                editor.Apply();

                return base.OnOptionsItemSelected(item);
            }
            return base.OnOptionsItemSelected(item);
        }

    }
}
