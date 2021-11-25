using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System;
using System.Linq;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.party;

namespace GloomhavenCampaignTracker.Droid.Fragments.character
{
    public class CharacterDetailsViewPagerTabs : Android.Support.V4.App.Fragment
    {
        private View _view;
            
        private TextView _partyname;
        private TextView _playername;

        private TabLayout _tabLayout;
        private ViewPager _viewPager;
        private CharacterDetailsViewPagerAdapter _adapter;

        public DL_Character Character;

        internal static CharacterDetailsViewPagerTabs NewInstance(DL_Character character)
        {
            var frag = new CharacterDetailsViewPagerTabs
            {
                Arguments = new Bundle(),
                Character = character
            };
            return frag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.fragment_character_detail_tabbed, container, false);
            _viewPager = _view.FindViewById<ViewPager>(Resource.Id.characterdetailsviewpager);
            _tabLayout = _view.FindViewById<TabLayout>(Resource.Id.characterdetails_tabs);

            _partyname = _view.FindViewById<TextView>(Resource.Id.partynametextview);
            _playername = _view.FindViewById<TextView>(Resource.Id.playernametextview);

            var partyimage = _view.FindViewById<ImageView>(Resource.Id.partyimgview);
            var playerImage = _view.FindViewById<ImageView>(Resource.Id.playerimgview);

            _partyname.Text = "";
            _playername.Text = "";

            UpdateView();

            _viewPager.Adapter = _adapter;

            _tabLayout.SetupWithViewPager(_viewPager);

            UpdateTabViews();

            if (!partyimage.HasOnClickListeners)
            {
                partyimage.Click += Partyimage_Click;
            }

            if (!playerImage.HasOnClickListeners)
            {
                playerImage.Click += PlayerImage_Click; ;
            }

            return _view;
        }

        private void UpdateView()
        {
            if (Character == null) return;
            if (Character.Party != null)
            {
                _partyname.Text = Character.Party.Name;
            }

            _playername.Text = Character.Playername;

            _adapter = new CharacterDetailsViewPagerAdapter(Context, ChildFragmentManager, Character);
        }

        private void PlayerImage_Click(object sender, EventArgs e)
        {
            if (Character == null) return;

            var convertView = LayoutInflater.Inflate(Resource.Layout.alertdialog_editplayername, null);
            var playerNameEdit = convertView.FindViewById<TextInputEditText>(Resource.Id.player_name);

            playerNameEdit.Text = $"{Character.Playername}";

            new CustomDialogBuilder(base.Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle("Edit Playername")
                .SetNegativeButton(Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .SetPositiveButton(Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                {                    
                    if (string.IsNullOrEmpty(playerNameEdit.Text)) return;
                    Character.Playername = playerNameEdit.Text;

                    SaveCharacter();
                    _adapter = new CharacterDetailsViewPagerAdapter(base.Context, ChildFragmentManager, Character);

                    CharacterDetailFragmentViewPager parentFragment = ((CharacterDetailFragmentViewPager)ParentFragment);
                    parentFragment.UpdateAdapter();
                    parentFragment.SetPage(Character);
                })
                .Show();
        }

        internal void SaveCharacter()
        {
            if (Character != null)
            {
                DataServiceCollection.CharacterDataService.InsertOrReplace(Character);
            }
        }

        private void Partyimage_Click(object sender, EventArgs e)
        {
            if (Character == null) return;

            var view = LayoutInflater.Inflate(Resource.Layout.alertdialog_listview, null);
            var listview = view.FindViewById<ListView>(Resource.Id.listView);
            listview.ItemsCanFocus = true;
            listview.ChoiceMode = ChoiceMode.Single;

            var parties = DataServiceCollection.PartyDataService.Get();
            var adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleListItemSingleChoice, parties.Select(x => x.Name).ToArray());
            listview.Adapter = adapter;

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(view)
                .SetTitle("Assign to Party")
                .SetMessage($"Select a new party for {Character.Name}")
                .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .SetPositiveButton(Context.Resources.GetString(Resource.String.Assign), (senderAlert, args) =>
                {
                    if (listview.CheckedItemPosition == -1) return;
                    var partyname = adapter.GetItem(listview.CheckedItemPosition);

                    if (string.IsNullOrEmpty(partyname)) return;
                    if (Character.Party != null)
                    {
                        Character.Party = null;
                        Character.ID_Party = 0;
                    }

                    // Assign character to a new party
                    var party = parties.FirstOrDefault(x => x.Name == partyname);
                    if(party?.ID_Campaign == GCTContext.CurrentCampaign?.CampaignData.Id)
                    {
                        party = GCTContext.CurrentCampaign?.CampaignData.Parties.FirstOrDefault(x => x.Id == party.Id);
                    }

                    if (party != null)
                    {   
                        Character.Party = party;
                        Character.ID_Party = party.Id;
                    }
                    _partyname.Text = partyname;

                    SaveCharacter();

                })
                .Show();
        }

        public override void OnStop()
        {
            if (Character != null)
            {
                DataServiceCollection.CharacterDataService.InsertOrReplace(Character);
            }

            base.OnStop();
        }

        public void UpdateCharacterGolds(int sellvalue)
        {
            if (Character == null) return;
            Character.Gold += sellvalue;
            SaveCharacter();
            _adapter.NotifyDataSetChanged();
            UpdateTabViews();
        }

        private void UpdateTabViews()
        {
            // Iterate over all tabs and set the custom view
            for (var i = 0; i < _tabLayout.TabCount; i++)
            {
                var tab = _tabLayout.GetTabAt(i);
                tab.SetCustomView(_adapter.GetTabView(i));
            }
        }
    }
}