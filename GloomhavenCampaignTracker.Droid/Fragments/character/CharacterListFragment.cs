using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Shared.Business;
using ListFragment = Android.Support.V4.App.ListFragment;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.party;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using GloomhavenCampaignTracker.Shared;

namespace GloomhavenCampaignTracker.Droid.Fragments.character
{
    public class CharacterListFragment : ListFragment
    {
        protected List<Character> _characters = new List<Character>();
        private FloatingActionButton _fab;
        private CampaignPartyFragment _frag;

        public static CharacterListFragment NewInstance(bool justCurrentParty)
        {          
            var frag = new CharacterListFragment { Arguments = new Bundle() };
            frag.Justparty = justCurrentParty;
            return frag;
        }

        public bool Justparty { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            FillListAdapter();
        }

        protected virtual void SetCharacters()
        {
            if (Justparty)
            {
                _characters = GCTContext.CharacterCollection.Characters.Where(x => x.CharacterData.ID_Party == GCTContext.CurrentCampaign.CurrentParty?.PartyData.Id).ToList();
            }
            else
            {
                _characters = GCTContext.CharacterCollection.Characters;
            }    
        }

        private void FillListAdapter()
        {
            SetCharacters();
            ListAdapter = new CharacterAdapter(Context, _characters, Justparty);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.listview_floatingactionbutton, container, false);
            _fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab);;          

            return view;
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
            var position = e.Position;
            var character = _characters[position];
            ShowCharacterDetail(character.CharacterData.Id);
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
            var adapter = new CharacterClassAdapter(Context);
            spinner.Adapter = adapter;
            
            new CustomDialogBuilder(base.Context, Resource.Style.MyDialogTheme)                     
                .SetCustomView(convertView)
                .SetTitle(Resources.GetString(Resource.String.AddCharacter))
                .SetPositiveButton(Resources.GetString(Resource.String.Add), (senderAlert, args) =>
                {
                    var charNameEdit = (EditText)convertView.FindViewById(Resource.Id.character_name);
                    var lifegoalEdit = convertView.FindViewById<EditText>(Resource.Id.characterfrag_characterlivegoalid);

                    if (string.IsNullOrEmpty(charNameEdit.Text))
                    {
                        Toast.MakeText(Context, Resources.GetString(Resource.String.CharacterNameMissing), ToastLength.Short).Show();
                        return;
                    }

                    int pq = -1;
                    if (!string.IsNullOrEmpty(lifegoalEdit.Text) && (!int.TryParse(lifegoalEdit.Text, out pq) || pq < 510 || pq > 533))
                    {
                        // if personal quest is set it must be a valid integer between 510 and 533
                        Toast.MakeText(Context, "The number of the personal quest must be the reference number and between 510 and 533", ToastLength.Short).Show();
                        return;
                    }

                    var classId = (int)spinner.Adapter.GetItem(spinner.SelectedItemPosition);
                    AddCharacter(charNameEdit.Text, classId, pq);
                    FillListAdapter();                   
                })
                .Show();          
        }

        protected virtual Character AddCharacter(string charname, int classId, int personalQuest)
        {
            var character = GCTContext.CharacterCollection.NewCharacter(charname, classId, personalQuest);

            if (Justparty)
            {               
                GCTContext.CurrentCampaign.CurrentParty.AddPartyMember(character);
            }

            return character;
        }
    }
}