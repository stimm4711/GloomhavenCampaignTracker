using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    /// <summary>
    /// Adapter for Character perks
    /// </summary>
    internal class CharacterPerksAdapter : CharacterDetailsAdapterBase
    {
        public override int Count
        {
            get
            {
                if (_character.CharacterPerks == null) return 0;
                return _character.CharacterPerks.Count;
            }
        }

        public void AddItem(DL_ClassPerk item)
        {
            _character.CharacterPerks.Add(item);
        }

        public CharacterPerksAdapter(Context context, DL_Character character) : base(context, character)
        {
        }

        internal class PerkAdapterViewHolder : Object
        {
            public TextView DescriptionTextView { get; set; }
            public ImageView OptionsButton { get; set; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            PerkAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as PerkAdapterViewHolder;

            if (holder == null)
            {
                holder = new PerkAdapterViewHolder();
                var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                view = inflater.Inflate(Resource.Layout.listviewitem_characterperk, parent, false);
                holder.DescriptionTextView = view.FindViewById<TextView>(Resource.Id.descriptionTextView);
                holder.OptionsButton = view.FindViewById<ImageView>(Resource.Id.optionsImageView);              
                view.Tag = holder;
                holder.DescriptionTextView.AfterTextChanged += (sender, args) => SpannableTools.AddIcons(_context, args.Editable, holder.DescriptionTextView.LineHeight);
            }

            if (!(_character?.CharacterPerks.Count > position)) return view;

            var perk = _character.CharacterPerks[position];

            if (perk == null) return view;
            
            holder.DescriptionTextView.Text = perk.Perktext;

            // options button  
            if (!holder.OptionsButton.HasOnClickListeners)
            {
                holder.OptionsButton.Click += (sender, e) =>
                {
                    ConfirmDeleteDialog(position);
                };
            }

            return view;
        }        

        /// <summary>
        /// Confirm delete perk
        /// </summary>
        /// <param name="position"></param>
        private void ConfirmDeleteDialog(int position)
        {
            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetTitle(_context.Resources.GetString(Resource.String.Confirm))
                .SetMessage("Delete Perk?")
                .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
                {
                    _character.CharacterPerks.Remove(_character.CharacterPerks[position]);
                    SaveCharacter();
                    NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }   
    }
}