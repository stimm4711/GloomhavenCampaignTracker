using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Business;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class StringToSpinnerDisplayValue : ISpinneritem
    {
        public StringToSpinnerDisplayValue(string item)
        {
            Spinnerdisplayvalue = item;
        }

        public string Spinnerdisplayvalue { get; }
    }

    internal class SpinnerAdapter : BaseAdapter
    {
        private readonly int _itemview = Resource.Layout.itemview;
        private readonly Context _context;
        private readonly List<ISpinneritem> _items = new List<ISpinneritem>();

        public override int Count => _items.Count;

        public void AddItems(IEnumerable<ISpinneritem> item)
        {
            _items.AddRange(item);
        }

        public void AddItems(string[] items)
        {
            foreach(var item in items)
            {
                _items.Add(new StringToSpinnerDisplayValue(item));
            }
        }

        public SpinnerAdapter(Context context, int itemView) 
        {
            _context = context;
            _itemview = itemView;
        }

        public SpinnerAdapter(Context context)
        {
            _context = context;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
         
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return GetView(position, convertView);
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            return GetView(position, convertView);
        }

        private View GetView(int position, View convertView)
        {
            var view = convertView;

            if (view == null)
            {
                var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                view = inflater.Inflate(_itemview, null);
            }

            var listItemText = (TextView)view.FindViewById(Resource.Id.itemTextView);
            listItemText.Text = _items[position].Spinnerdisplayvalue;

            return view;
        }

        public override Object GetItem(int position)
        {
            return _items[position].Spinnerdisplayvalue;
        }

        public ISpinneritem GetSpinnerItem(int position)
        {
            return _items[position];
        }
    }
}