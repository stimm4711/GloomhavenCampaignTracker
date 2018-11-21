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
    internal class PerksAdapter : CharacterDetailsAdapterBase
    {
        public override int Count => _character.Perks?.Count ?? 0;

        public void AddItem(DL_Perk item)
        {
            _character.Perks.Add(item);
        }

        public PerksAdapter(Context context, DL_Character character) : base(context, character)
        {
        }

        internal class PerkAdapterViewHolder : Object
        {
            public TextView CheckboxTextView { get; set; }
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
                view = inflater.Inflate(Resource.Layout.listviewitem_perk, parent, false);
                holder.CheckboxTextView = view.FindViewById<TextView>(Resource.Id.perkcheckboxnumber);
                holder.DescriptionTextView = view.FindViewById<TextView>(Resource.Id.descriptionTextView);
                holder.OptionsButton = view.FindViewById<ImageView>(Resource.Id.optionsImageView);              
                view.Tag = holder;
            }

            if (_character?.Perks.Count > position)
            {
                var perk = _character.Perks[position];
                
                if (perk != null)
                {
                    holder.CheckboxTextView.Text = $"# {perk.Checkboxnumber}";
                    holder.DescriptionTextView.Text = perk.Perkcomment;

                    // options button  
                    if (!holder.OptionsButton.HasOnClickListeners)
                    {
                        holder.OptionsButton.Click += (sender, e) =>
                        {
                            ConfirmDeleteDialog(position);
                        };
                    }
                }                
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
                    _character.Perks.Remove(_character.Perks[position]);
                    SaveCharacter();
                    NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }   
    }
}