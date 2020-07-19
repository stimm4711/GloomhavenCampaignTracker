using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Collections.Generic;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.Linq;

namespace GloomhavenCampaignTracker.Droid.Fragments.character
{
    public class CharacterDetailPerks2Fragment : CharacterDetailFragmentBase
    {
        private ListView _lv;

        internal static CharacterDetailPerks2Fragment NewInstance(DL_Character character)
        {
            var frag = new CharacterDetailPerks2Fragment() { Arguments = new Bundle() };
            frag.Arguments.PutInt(charID, character.Id);
            return frag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            if (_view != null) return _view;

            // default
            _view = LayoutInflater.Inflate(Resource.Layout.fragment_characterdetails_listwithfab, container, false);
            var fab = _view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            _lv = _view.FindViewById<ListView>(Resource.Id.ListView);

            if (Character != null)
            {
                _lv.Adapter = new CharacterPerksAdapter(Context, Character);
            }

            if (!fab.HasOnClickListeners)
            {
                fab.Click += (senderx, ex) =>
                {
                    AddNewperk();
                };
            }

            return _view;
        }


        /// <summary>
        /// show alert dialog to add a new perk
        /// </summary>
        private void AddNewperk()
        {
            var inflater = Context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_listview, null);
            var listview = convertView.FindViewById<ListView>(Resource.Id.listView);
            var perks = DataServiceCollection.CharacterDataService.GetClassPerks(Character.ClassId-1, Character.Id);
            var perkadapter= new SelectablePerksAdapter(Context, perks);

            listview.ItemsCanFocus = true;
            listview.ChoiceMode = ChoiceMode.Multiple;
            listview.Adapter = perkadapter;

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle("Add Perks")
                .SetPositiveButton("Add perks", (senderAlert, args) =>
                {
                    var selectedPerks = perkadapter.GetSelected();
                    foreach (var sp in selectedPerks)
                    {
                        if (Character.CharacterPerks == null) Character.CharacterPerks = new List<DL_ClassPerk>();
                        Character.CharacterPerks.Add(sp);
                    }

                    SaveCharacter();
                    _lv.Adapter = new CharacterPerksAdapter(Context, Character);
                })
                .SetNegativeButton("Cancel", (senderAlert, args) => {})
                .Show();
        }
    }
}