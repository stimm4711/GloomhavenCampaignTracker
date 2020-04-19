using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class SelectableItemAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<DL_Item> _items = new List<DL_Item>();
        private readonly bool _isCharacterDetailView;

        public override int Count => _items.Count;

        public SelectableItemAdapter(Context context, List<DL_Item> items, bool isCharacterDetailView) 
        {
            _context = context;
            _items = items;
            _isCharacterDetailView = isCharacterDetailView;  
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
            DL_ItemHolder holder = null;

            if (convertView != null)
                holder = convertView.Tag as DL_ItemHolder;

            if (holder == null)
            {
                // Create new holder
                holder = new DL_ItemHolder();

                var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.listviewitem_item_withprice_selectable, parent, false);
                holder.ItemName = convertView.FindViewById<TextView>(Resource.Id.itemnameTextView);
                holder.ItemNumber = convertView.FindViewById<TextView>(Resource.Id.itemnumber);
                holder.Selected = convertView.FindViewById<CheckBox>(Resource.Id.selected);
                holder.ItemPrice = convertView.FindViewById<TextView>(Resource.Id.itemcostsTextView);                
                holder.ItemCategoryImage = convertView.FindViewById<ImageView>(Resource.Id.categorieImageView);

                // Looted CheckCHanged Event
                holder.Selected.CheckedChange += (sender, e) =>
                {
                    var chkBx = (CheckBox)sender;
                    var thisItem = (DL_Item)chkBx.Tag;

                    if (thisItem == null || thisItem.IsSelected == chkBx.Checked) return;
                    thisItem.IsSelected = chkBx.Checked;
                    NotifyDataSetChanged();
                };

                // Set Item Click Event
                convertView.Click += (sender, e) =>
                {
                    var v = (View)sender;
                    var checkview = v.FindViewById<CheckBox>(Resource.Id.selected);
                    if (checkview == null) return;
                    ItemClick((DL_Item)checkview.Tag);
                };

                convertView.Tag = holder;
            }

            var item = _items[position];            

            // Set Data
            holder.ItemName.Text = (_isCharacterDetailView || GCTContext.Settings.IsShowItems) ? item.Itemname : "";
            holder.ItemNumber.Text = $"# {item.Itemnumber}";
            holder.Selected.Tag = item;
            holder.Selected.Checked = item.IsSelected;
            holder.ItemPrice.Text = $"{item.Itemprice} G";
            holder.ItemCategoryImage.SetImageResource(ResourceHelper.GetItemCategorieIconRessourceId(item.Itemcategorie));

            return convertView;
        }

        private void ItemClick(DL_Item item)
        {
            var numbertext = item.GetNumberText();
            if (string.IsNullOrEmpty(numbertext)) return;
            var diag = new ItemImageViewDialogBuilder(_context, Resource.Style.MyTransparentDialogTheme)
                        .SetItemNumber(numbertext)
                        .Show();
        }

        private class DL_ItemHolder : Java.Lang.Object
        {
            public TextView ItemName { get; set; }
            public TextView ItemNumber { get; set; }
            public CheckBox Selected { get; set; }
            public TextView ItemPrice { get; set; }
            public ImageView ItemCategoryImage { get; set; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return _items[position];
        }

        internal IEnumerable<DL_Item> GetSelected()
        {
            return _items.Where(x => x.IsSelected);
        }
    }
}