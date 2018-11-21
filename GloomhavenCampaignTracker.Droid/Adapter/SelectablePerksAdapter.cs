using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Droid.Adapter
{

    internal class SelectablePerksAdapter : BaseAdapter
    {
        readonly Context _context;
        private readonly List<DL_ClassPerk> _perks;

        public SelectablePerksAdapter(Context context, List<DL_ClassPerk> perks)
        {
            _context = context;
            _perks = perks;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return _perks[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return GetPerkView(position, convertView, parent);
        }

        private View GetPerkView(int position, View convertView, ViewGroup parent)
        {
            PerksAdapterViewHolder holder = null;

            if (convertView != null)
                holder = convertView.Tag as PerksAdapterViewHolder;

            if (holder == null)
            {
                holder = new PerksAdapterViewHolder();

                var inflater = _context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                convertView = inflater.Inflate(Resource.Layout.listviewitem_perk_selectable, parent, false);
                holder.Title = convertView.FindViewById<TextView>(Resource.Id.perktext);
                holder.Selected = convertView.FindViewById<CheckBox>(Resource.Id.selected);
                holder.Title.AfterTextChanged += (sender, args) => SpannableTools.AddIcons(_context, args.Editable, holder.Title.LineHeight);
                
                // Perk CheckCHanged Event
                holder.Selected.CheckedChange += (sender, e) =>
                {
                    var chkBx = (CheckBox)sender;
                    var thisPerk = (DL_ClassPerk)chkBx.Tag;

                    if (thisPerk == null || thisPerk.IsSelected == chkBx.Checked) return;
                    thisPerk.IsSelected = chkBx.Checked;
                    NotifyDataSetChanged();
                };

                convertView.Tag = holder;
            }           

            var perk = _perks[position];
            holder.Title.Text = perk.Perktext;
            holder.Selected.Tag = perk;
            holder.Selected.Checked = perk.IsSelected;            

            return convertView;
        }

        public override int Count => _perks.Count();

        internal IEnumerable<DL_ClassPerk> GetSelected()
        {
            return _perks.Where(x => x.IsSelected);
        }

        private class PerksAdapterViewHolder : Java.Lang.Object
        {
            public TextView Title { get; set; }

            public CheckBox Selected { get; set; }
        }
    }

    
}