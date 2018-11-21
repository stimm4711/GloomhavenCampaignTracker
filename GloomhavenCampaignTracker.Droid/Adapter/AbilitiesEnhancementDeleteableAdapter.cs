using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using Java.Lang;
using Android.Runtime;
using GloomhavenCampaignTracker.Shared;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    /// <summary>
    /// Adapter for Character abilities
    /// </summary>
    internal class AbilitiesEnhancementDeleteableAdapter : BaseAdapter
    {
        private readonly DL_Ability _ability;
        private readonly Context _context;
        private readonly bool _isTop;
        private readonly List<DL_AbilityEnhancement> _enhancements;

        public override int Count => _enhancements.Count; 

        public AbilitiesEnhancementDeleteableAdapter(Context context, DL_Ability ability, bool istop)
        {
            _ability = ability;
            _context = context;
            _isTop = istop;

            if(_ability == null ||_ability.Enhancements == null)
            {
                _enhancements = new List<DL_AbilityEnhancement>();
            }
            else
            {
                _enhancements = _ability.Enhancements.Where(x => x.IsTop == istop).ToList();
            }            
        }

        public override Object GetItem(int position)
        {
            return _enhancements[position]; 
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            EnhancementViewHolder holder = null;

            if (convertView != null)
                holder = convertView.Tag as EnhancementViewHolder;

            if (holder == null)
            {
                holder = new EnhancementViewHolder();

                var inflater = _context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                convertView = inflater.Inflate(Resource.Layout.listviewitem_enhancement_deleteable, parent, false);
                holder.SlotNumber = convertView.FindViewById<TextView>(Resource.Id.slotnumber);
                holder.EnhancementCode = convertView.FindViewById<TextView>(Resource.Id.enhacement);
                holder.OptionsButton = convertView.FindViewById<ImageView>(Resource.Id.optionsImageView);
                holder.EnhancementCode.AfterTextChanged += (sender, args) => SpannableTools.AddIcons(_context, args.Editable, holder.EnhancementCode.LineHeight);

                convertView.Tag = holder;
            }

            var enhancement = _enhancements[position];

            if (enhancement == null) return convertView;

            holder.SlotNumber.Text = $"# {enhancement.SlotNumber}";
            holder.EnhancementCode.Text = enhancement.Enhancement.EnhancementCode;

            // options button  
            if (!holder.OptionsButton.HasOnClickListeners)
            {
                holder.OptionsButton.Click += (sender, e) =>
                {
                    ConfirmDeleteDialog(position);
                };
            }
            return convertView;
        }

        /// <summary>
        /// Confirm delete
        /// </summary>
        /// <param name="position"></param>
        private void ConfirmDeleteDialog(int position)
        {
            var item = _enhancements[position];

            if (item == null) return;

            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetMessage("Delete Enhancement?")
                .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
                {
                    if (position >= Count) return;
                    _ability?.Enhancements.Remove(item);
                    _enhancements.Remove(item);
                    DataServiceCollection.CharacterDataService.InsertOrReplace(_ability.Character);
                    NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        internal class EnhancementViewHolder : Object
        { 
            public TextView SlotNumber { get; set; }
            public TextView EnhancementCode { get; set; }
            public ImageView OptionsButton { get; set; }
            public CheckBox IsTopAction { get; set; }
        }

    }
}