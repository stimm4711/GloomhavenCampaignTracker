using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign;
using Java.Lang;
using System.Collections.Generic;
using System.Linq;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    /// <summary>
    /// Adapter for Character abilities
    /// </summary>
    internal class AbilitiesAdapter : CharacterDetailsAdapterBase
    {
        private List<DL_CharacterAbility> _abilities;

        public override int Count
        {
            get
            {
                if (_abilities == null) return 0;
                return _abilities.Count;
                //if (_character.Abilities == null) return 0;
                //return _character.Abilities.Count;
            }
        }

        public AbilitiesAdapter(Context context, DL_Character character) : base(context, character)
        {
            _abilities = character.CharacterAbilities.OrderBy(x => x.Ability.ReferenceNumber).ToList();
        }
         
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            AbilityViewHolder holder = null;

            if (convertView != null)
                holder = convertView.Tag as AbilityViewHolder;

            if (holder == null)
            {
                holder = new AbilityViewHolder();

                var inflater = _context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                convertView = inflater.Inflate(Resource.Layout.listviewitem_ability, parent, false);
                holder.AbilityNumber = convertView.FindViewById<TextView>(Resource.Id.itemNumberTextView);
                holder.AbilityName = convertView.FindViewById<TextView>(Resource.Id.abilityNameText);
                holder.AbilityLevel = convertView.FindViewById<TextView>(Resource.Id.itemLevelTextView);
                holder.AbilityEnhancementsNumber = convertView.FindViewById<TextView>(Resource.Id.numberofenhancements);
                holder.OptionsButton = convertView.FindViewById<ImageView>(Resource.Id.optionsImageView);

                convertView.Tag = holder;
            }

            //var ability = _character.Abilities[position];
            var ability = _abilities[position];

            if (ability == null) return convertView;

            //holder.AbilityNumber.Text = $"# {ability.ReferenceNumber}";
            //holder.AbilityName.Text = ability.AbilityName;
            //holder.AbilityLevel.Text = $"{ability.Level}";

            holder.AbilityNumber.Text = $"# {ability.Ability.ReferenceNumber}";
            holder.AbilityName.Text = ability.Ability.AbilityName;
            holder.AbilityLevel.Text = $"{ability.Ability.Level}";

            var numberOfEnhancements = (ability?.AbilityEnhancements != null) ? ability.AbilityEnhancements.Count : 0;
            holder.AbilityEnhancementsNumber.Text = $"{numberOfEnhancements}";

            // options button  
            if (!holder.OptionsButton.HasOnClickListeners)
            {
                holder.OptionsButton.Click += (sender, e) =>
                {
                    ShowAbilityPopupMenu(position, holder.OptionsButton);
                };
            }
            return convertView;
        }

        /// <summary>
        /// Ability options menu
        /// </summary>
        /// <param name="position"></param>
        /// <param name="optionsButton"></param>
        private void ShowAbilityPopupMenu(int position, ImageView optionsButton)
        {
            if (position >= Count) return;

            // open a popup menu with delete option
            var menu = new PopupMenu(_context, optionsButton);
            menu.Inflate(Resource.Menu.popupDeleteMenu);

            menu.MenuItemClick += (s, a) =>
            {
                if (a.Item.ItemId == Resource.Id.popup_delete)
                    ConfirmDeleteDialog(position);
            };

            menu.Show();
        }

        /// <summary>
        /// Confirm delete ability
        /// </summary>
        /// <param name="position"></param>
        private void ConfirmDeleteDialog(int position)
        {
            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetTitle(_context.Resources.GetString(Resource.String.Confirm))
                .SetMessage(_context.Resources.GetString(Resource.String.DeleteAbility))
                .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
                {
                    //_character.Abilities.Remove(_character.Abilities[position]);
                    _character.CharacterAbilities.Remove(_abilities[position]);
                    _abilities.Remove(_abilities[position]);
                    SaveCharacter();
                    NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return _character.Abilities[position];
        }

        internal class AbilityViewHolder : Object
        {
            public TextView AbilityNumber { get; set; }
            public TextView AbilityName { get; set; }
            public TextView AbilityLevel { get; set; }
            public TextView AbilityEnhancementsNumber { get; set; }
            public ImageView OptionsButton { get; set; }
        }
    }
}