using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class ProsperityItemAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<DL_Item> _items = new List<DL_Item>();

        public override int Count => _items.Count;

        public ProsperityItemAdapter(Context context, List<DL_Item> items, int filter) 
        {
            _context = context;
            if(filter >= 0 && filter <= 5)
            {
                _items = items.Where(x=>x.Itemcategorie == filter).ToList();
            }
            else
            {
                _items = items;
            }            
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
                convertView = inflater.Inflate(Resource.Layout.listviewitem_item_withprice, parent, false);
                holder.ItemName = convertView.FindViewById<TextView>(Resource.Id.itemnameTextView);
                holder.ItemNumber = convertView.FindViewById<TextView>(Resource.Id.itemnumber);
                holder.ItemPrice = convertView.FindViewById<TextView>(Resource.Id.itemcostsTextView);
                holder.ItemCategoryImage = convertView.FindViewById<ImageView>(Resource.Id.categorieImageView);
                convertView.Tag = holder;

                // Set Item Click Event
                convertView.Click += (sender, e) =>
                {
                    var v = (View)sender;
                    var numberTxtView = v.FindViewById<TextView>(Resource.Id.itemnumber);
                    if (numberTxtView == null) return;
                    ItemClick((DL_Item)numberTxtView.Tag);
                };
            }           

            var item = _items[position];

            // Set Data
            holder.ItemName.Text = item.Itemname;
            holder.ItemNumber.Text = $"# {item.Itemnumber}";
            holder.ItemPrice.Text = $"{item.Itemprice} {_context.Resources.GetString(Resource.String.CharacterGold)}";
            holder.ItemCategoryImage.SetImageResource(ResourceHelper.GetItemCategorieIconRessourceId(item.Itemcategorie));
            holder.ItemNumber.Tag = item;

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
            public TextView ItemPrice { get; set; }
            public ImageView ItemCategoryImage { get; set; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return _items[position];
        }
    }
}