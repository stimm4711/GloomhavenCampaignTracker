using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Business;
using Java.Lang;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    public class CharacterAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<DL_Character> _characters;
        private readonly bool _justParty;
        private readonly bool _hideRetired;

        public CharacterAdapter(Context context, List<DL_Character> characters, bool justParty = false, bool hideRetired = false)
        {
            _context = context;
            _characters = characters.Where(x => !(x.Retired && _hideRetired)).OrderBy(x=>!x.Retired).ToList();
            _justParty = justParty;
            _hideRetired = hideRetired;
        }

        public override int Count => _characters.Count;

        public override Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public int GetCharacterId(int position)
        {
            return _characters[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            CharacterAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as CharacterAdapterViewHolder;

            if (holder == null)
            {
                holder = new CharacterAdapterViewHolder();
                var inflater = _context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                view = inflater.Inflate(Resource.Layout.listviewitem_character, parent, false);
                holder.Charactername = view.FindViewById<TextView>(Resource.Id.charnameTextView);
                holder.Characterlevel = view.FindViewById<TextView>(Resource.Id.charLevelText);
                holder.CharacterClass = view.FindViewById<ImageView>(Resource.Id.chaclassImage);
                holder.CharacterRetiredImage = view.FindViewById<ImageView>(Resource.Id.retiredImage);
                holder.CharacterOptionsButton = view.FindViewById<ImageView>(Resource.Id.optionsImageView);
                holder.CharacterExperience = view.FindViewById<TextView>(Resource.Id.xptext);
                holder.CharacterGold = view.FindViewById<TextView>(Resource.Id.goldText);
                holder.PartyImage = view.FindViewById<ImageView>(Resource.Id.partyImage);
                view.Tag = holder;
            }

            var partymember = _characters[position];

            holder.Charactername.Text = partymember.Name;
            holder.Characterlevel.Text = partymember.Level.ToString();
            holder.CharacterGold.Text = partymember.Gold.ToString();
            holder.CharacterExperience.Text = partymember.Experience.ToString();
            holder.CharacterClass.SetImageResource(ResourceHelper.GetClassIconRessourceId(_characters[position].ClassId - 1));

            holder.CharacterRetiredImage.Visibility = partymember.Retired ? ViewStates.Visible : ViewStates.Invisible;           

            holder.PartyImage.Visibility = partymember.ID_Party > 0 ? ViewStates.Visible : ViewStates.Invisible;

            holder.Charactername.Tag = new CharacterWrapper(partymember);

            // options button            
            if (!holder.CharacterOptionsButton.HasOnClickListeners)
            {
                holder.CharacterOptionsButton.Click += (sender, e) =>
                {
                    var charWrapper = (CharacterWrapper)holder.Charactername.Tag;
                    ShowCharacterPopupMenu(position, holder, charWrapper.Character);
                };
            }

            return view;
        }

        private void ShowCharacterPopupMenu(int position, CharacterAdapterViewHolder holder, DL_Character character)
        {
            if (position >= Count) return;

            // open a popup menu 
            var menu = new PopupMenu(_context, holder.CharacterOptionsButton);
            menu.Inflate(Resource.Menu.characterPopupMenu);  
            
            var delete = menu.Menu.FindItem(Resource.Id.c_popup_delete);
            delete.SetEnabled(!_justParty);
            delete.SetVisible(!_justParty);

            var remove = menu.Menu.FindItem(Resource.Id.c_popup_remove);
            remove.SetEnabled(_justParty);
            remove.SetVisible(_justParty);

            menu.MenuItemClick += (s, a) =>
            {
                if (character == null) return;

                switch (a.Item.ItemId)
                {
                    case Resource.Id.c_popup_delete:
                        DeleteCharacter(character);
                        break;
                    case Resource.Id.c_popup_remove:
                        // remove character from party
                        RemoveCharacterFromParty(character);
                        break;
                }

                NotifyDataSetChanged();
            };

            menu.Show();
        }

        /// <summary>
        /// Just remove the character from the party
        /// </summary>
        /// <param name="character"></param>
        private void RemoveCharacterFromParty(DL_Character character)
        {            
            if (GCTContext.CurrentCampaign == null) return;
            if (GCTContext.CurrentCampaign.CurrentParty == null) return;
            if (GCTContext.CurrentCampaign.CurrentParty.Id != character.ID_Party) return;

            character.Party = null;
            character.ID_Party = 0;
            DataServiceCollection.CharacterDataService.InsertOrReplace(character);
            _characters.Remove(character);

            NotifyDataSetChanged();
        }

        /// <summary>
        /// Delete character
        /// </summary>
        /// <param name="character"></param>
        private void DeleteCharacter(DL_Character character)
        {           
            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetMessage($"Delete Character {character.Name}?")
                .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
                {
                    // delete the character                   
                    DataServiceCollection.CharacterDataService.Delete(character);

                    _characters.Remove(character);

                    NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();          
        }
    }

    internal class CharacterAdapterViewHolder : Object
    {
        public TextView Charactername { get; set; }
        public TextView Characterlevel { get; set; }
        public TextView CharacterExperience { get; set; }
        public TextView CharacterGold { get; set; }
        public ImageView CharacterClass { get; set; }
        public ImageView CharacterRetiredImage { get; set; }
        public ImageView CharacterOptionsButton { get; set; }
        public ImageView PartyImage { get; set; }
    }
}