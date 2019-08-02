﻿using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class SelectableClassAbilitiesAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<DL_ClassAbility> _items = new List<DL_ClassAbility>();
        private readonly bool _isCharacterDetailView;

        public override int Count => _items.Count;

        public SelectableClassAbilitiesAdapter(Context context, List<DL_ClassAbility> items) 
        {
            _context = context;
            _items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
         
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return GetItemView(position, convertView, parent);
        }

        private View GetItemView(int position, View convertView, ViewGroup parent)
        {
            AbilityViewHolder holder = null;

            if (convertView != null)
                holder = convertView.Tag as AbilityViewHolder;

            if (holder == null)
            {
                // Create new holder
                holder = new AbilityViewHolder();

                var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.listviewitem_classability_selectable, parent, false);
                holder.AbilityName = convertView.FindViewById<TextView>(Resource.Id.abilityNameText);
                holder.AbilityNumber = convertView.FindViewById<TextView>(Resource.Id.itemNumberTextView);
                holder.Selected = convertView.FindViewById<CheckBox>(Resource.Id.selected);
                holder.AbilityLevel = convertView.FindViewById<TextView>(Resource.Id.itemLevelTextView);   

                holder.Selected.CheckedChange += (sender, e) =>
                {
                    var chkBx = (CheckBox)sender;
                    var thisItem = (DL_ClassAbility)chkBx.Tag;

                    if (thisItem == null || thisItem.IsSelected == chkBx.Checked) return;
                    thisItem.IsSelected = chkBx.Checked;
                    NotifyDataSetChanged();
                };

                //todo: add ability images
                // Set Item Click Event
                //convertView.Click += (sender, e) =>
                //{
                //    var v = (View)sender;
                //    var checkview = v.FindViewById<CheckBox>(Resource.Id.selected);
                //    if (checkview == null) return;
                //    ItemClick((DL_ClassAbility)checkview.Tag);
                //};

                convertView.Tag = holder;
            }

            var item = _items[position];            

            // Set Data
            holder.AbilityName.Text = (_isCharacterDetailView || GCTContext.ShowItemNames) ? item.AbilityName : "";
            holder.AbilityNumber.Text = $"# {item.ReferenceNumber}";
            holder.Selected.Tag = item;
            holder.Selected.Checked = item.IsSelected;
            holder.AbilityLevel.Text = $"Lv. {item.Level}";

            return convertView;
        }

        //private void ItemClick(DL_ClassAbility item)
        //{
        //    var numbertext = item.GetNumberText();
        //    if (string.IsNullOrEmpty(numbertext)) return;
        //    var diag = new ItemImageViewDialogBuilder(_context, Resource.Style.MyTransparentDialogTheme)
        //                .SetItemNumber(numbertext)
        //                .Show();
        //}

        internal class AbilityViewHolder : Object
        {
            public CheckBox Selected { get; set; }
            public TextView AbilityNumber { get; set; }
            public TextView AbilityName { get; set; }
            public TextView AbilityLevel { get; set; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return _items[position];
        }

        internal IEnumerable<DL_ClassAbility> GetSelected()
        {
            return _items.Where(x => x.IsSelected);
        }
    }
}