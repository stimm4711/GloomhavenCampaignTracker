using Android.OS;
using Android.Views;
using GloomhavenCampaignTracker.Shared.Business;
using GloomhavenCampaignTracker.Droid.Adapter;
using Android.Support.V4.View;
using System.Collections.Generic;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign.party
{
    public class CharacterDetailFragmentViewPager : CampaignFragmentBase
    {
        private List<DL_Character> _characters = new List<DL_Character>();
        private ViewPager _viewPager;

        internal static CharacterDetailFragmentViewPager NewInstance(int characterid, bool justParty )
        {
            var frag = new CharacterDetailFragmentViewPager { Arguments = new Bundle() };
            frag.Arguments.PutInt(DetailsActivity.SelectedCharacterId, characterid);
            frag.Arguments.PutBoolean(DetailsActivity.JustParty, justParty);
            return frag;
        }

        private DL_Character GetCharacter()
        {
            if (Arguments == null) return null;
            if (_characters == null || _characters.Count == 0) return null;

            var id = Arguments.GetInt(DetailsActivity.SelectedCharacterId, 0);
            if (id <= 0) return null;

            var cSearch = new CharacterSearch(id);
            return _characters.Find(cSearch.FindId);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.fragment_charactersviewpager, container, false);

            _viewPager = _view.FindViewById<ViewPager>(Resource.Id.viewpager);

            _characters = GCTContext.CharacterCollection;

            var adapter = new CharacterViewPagerAdapter(Context, ChildFragmentManager, _characters);
            _viewPager.Adapter = adapter;

            var character = GetCharacter();

            if (character != null)
            {
                SetPage(character);
            }

            var pagerTabStrip = (PagerTabStrip)_view.FindViewById(Resource.Id.pts);
            pagerTabStrip.DrawFullUnderline = true;
            pagerTabStrip.TabIndicatorColor = ContextCompat.GetColor(Context, Resource.Color.gloom_secondary);

            var fontTypeFace = Typeface.CreateFromAsset(Context.Assets, "fonts/PirataOne_Gloomhaven.ttf");

            for (var i = 0; i < pagerTabStrip.ChildCount; ++i)
            {
                var nextChild = pagerTabStrip.GetChildAt(i);
                var view = nextChild as TextView;
                if (view != null)
                {
                    var textViewToConvert = view;
                    textViewToConvert.Typeface = fontTypeFace;
                }

                nextChild.LongClick += NextChild_LongClick;
            }

            return _view;
        }

        private void NextChild_LongClick(object sender, View.LongClickEventArgs e)
        {
            if (_characters == null || _characters.Count == 0) return;

            var index = _viewPager.CurrentItem;
            if (_characters.Count <= index) return;

            var inflater = Activity.LayoutInflater;
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_addpartymember, null);
            var charNameEdit = (EditText)convertView.FindViewById(Resource.Id.character_name);
            var spinner = convertView.FindViewById<Spinner>(Resource.Id.characterClassSpinner);
            var adapter = new CharacterClassAdapter(Context);
            spinner.Adapter = adapter;

            var character = _characters[index];

            charNameEdit.Text = character.Name;
            var indexOfClass =  adapter.GetIndexOfClass(character.ClassId);
            spinner.SetSelection(indexOfClass);

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle("Edit Character")
                .SetNegativeButton(Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .SetPositiveButton(Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                {
                    var classId = (int)spinner.Adapter.GetItem(spinner.SelectedItemPosition);

                    if (string.IsNullOrEmpty(charNameEdit.Text)) return;
                    character.Name = charNameEdit.Text;
                    character.ClassId = classId;
                    SaveCharacter(character);

                    UpdateAdapter();
                })
               .Show();
        }

        private static void SaveCharacter(DL_Character character)
        {
            if (character == null) return;

            DataServiceCollection.CharacterDataService.InsertOrReplace(character);
        }

        internal void UpdateAdapter()
        {
            if (_characters == null || _characters.Count == 0) return;
            _viewPager.Adapter = new CharacterViewPagerAdapter(Context, ChildFragmentManager, _characters);
        }

        internal void SetPage(DL_Character character)
        {
            if (character == null) return;
            var cSearch = new CharacterSearch(character.Id);
            var index = _characters.FindIndex(cSearch.FindId);
            _viewPager.SetCurrentItem(index, true);
        }
    }
}