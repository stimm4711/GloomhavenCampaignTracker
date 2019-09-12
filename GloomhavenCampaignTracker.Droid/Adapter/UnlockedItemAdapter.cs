using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class UnlockedItemAdapter : ChangeAwareAdapter
    {
        private readonly Context _context;
        private readonly List<DL_Item> _items;

        public override int Count => _items.Count;

        public UnlockedItemAdapter(Context context, List<DL_Item> items, int filter)
        {
            _context = context;
            if (filter >= 0 && filter <= 5)
            {
                _items = items.Where(x => x.Itemcategorie == filter).ToList();
            }
            else
            {
                _items = items;
            }
            _items = _items.Where(x => !x.IsHide).ToList();
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
                convertView = inflater.Inflate(Resource.Layout.listviewitem_item_withprice_deleteable, parent, false);
                holder.ItemName = convertView.FindViewById<TextView>(Resource.Id.itemnameTextView);
                holder.ItemNumber = convertView.FindViewById<TextView>(Resource.Id.itemnumber);
                holder.ItemPrice = convertView.FindViewById<TextView>(Resource.Id.itemcostsTextView);
                holder.Options = convertView.FindViewById<ImageView>(Resource.Id.optionsImageView);
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

            if (!holder.Options.HasOnClickListeners)
            {
                holder.Options.Click += (sender, e) =>
                {
                    ConfirmDeleteDialog(position);
                };
            }

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

        /// <summary>
        /// Confirm delete
        /// </summary>
        /// <param name="position"></param>
        private void ConfirmDeleteDialog(int position)
        {
            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetMessage(_context.Resources.GetString(Resource.String.DeleteItemCommit))
                .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
                {
                    if (position >= Count) return;
                    GCTContext.CurrentCampaign.CampaignData.UnlockedItems.Remove(_items[position]);
                    OnDataModified();
                    _items.Remove(_items[position]);
                    NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        private class DL_ItemHolder : Java.Lang.Object
        {
            public TextView ItemName { get; set; }
            public TextView ItemNumber { get; set; }
            public TextView ItemPrice { get; set; }
            public ImageView Options { get; set; }
            public ImageView ItemCategoryImage { get; set; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return _items[position];
        }
    }
}