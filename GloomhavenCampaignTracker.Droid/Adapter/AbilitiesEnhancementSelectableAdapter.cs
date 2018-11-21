using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using Java.Lang;
using Android.Runtime;


namespace GloomhavenCampaignTracker.Droid.Adapter
{
    /// <summary>
    /// Adapter for Character abilities
    /// </summary>
    internal class AbilitiesEnhancementSelectableAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<DL_Enhancement> _enhancements;
        private DL_Enhancement _selected;

        public override int Count => _enhancements.Count;

        public AbilitiesEnhancementSelectableAdapter(Context context, List<DL_Enhancement> enhancements)
        {
            _enhancements = enhancements;
            _context = context;
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
                convertView = inflater.Inflate(Resource.Layout.listviewitem_enhancement_selectable, parent, false);
                holder.EnhancementCode = convertView.FindViewById<TextView>(Resource.Id.enhancementtext);
                holder.Selected = convertView.FindViewById<CheckBox>(Resource.Id.selected);
                holder.EnhancementCode.AfterTextChanged += (sender, args) => SpannableTools.AddIcons(_context, args.Editable, holder.EnhancementCode.LineHeight);

                // CheckCHanged Event
                holder.Selected.CheckedChange += (sender, e) =>
                {
                    var chkBx = (CheckBox)sender;
                    var thisItem = (DL_Enhancement)chkBx.Tag;

                    if (thisItem == null || thisItem.IsSelected == chkBx.Checked) return;
                    thisItem.IsSelected = chkBx.Checked;
                    if (_selected != null) _selected.IsSelected = false;
                    _selected = thisItem;
                    NotifyDataSetChanged();
                };


                convertView.Tag = holder;
            }

            var enhancement = _enhancements[position];

            if (enhancement == null) return convertView;
            
            holder.EnhancementCode.Text = enhancement.EnhancementCode;
            holder.Selected.Tag = enhancement;
            holder.Selected.Checked = enhancement.IsSelected;

            return convertView;
        }

        internal class EnhancementViewHolder : Object
        {
            public CheckBox Selected { get; set; }

            public TextView EnhancementCode { get; set; }
        }

        internal DL_Enhancement GetSelected()
        {
            return _enhancements.FirstOrDefault(x => x.IsSelected);
        }

    }
}