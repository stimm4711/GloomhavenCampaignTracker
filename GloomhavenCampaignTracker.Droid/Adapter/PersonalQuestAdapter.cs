using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class PersonalQuestAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<DL_PersonalQuest> _items = new List<DL_PersonalQuest>();
        private readonly bool _showDetails;

        public override int Count => _items.Count;

        public PersonalQuestAdapter(Context context, List<DL_PersonalQuest> items, bool showDetails) 
        {
            _context = context;
            _items = items;
            _showDetails = showDetails;
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
            Holder holder = null;

            if (convertView != null)
                holder = convertView.Tag as Holder;

            if (holder == null)
            {
                // Create new holder
                holder = new Holder();

                var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.listviewitem_personalquest, parent, false);
                holder.Questname = convertView.FindViewById<TextView>(Resource.Id.questnameTextView);
                holder.Questnumber = convertView.FindViewById<TextView>(Resource.Id.questnumber);
                convertView.Tag = holder;
            }

            var item = _items[position];

            // Set Data              
            holder.Questname.Text = (GCTContext.ShowPersonalQuestDetails || _showDetails) ? item.QuestName : "";
            holder.Questnumber.Text = $"# {item.QuestNumber}";

            return convertView;
        }

        private class Holder : Java.Lang.Object
        {
            public TextView Questname { get; set; }
            public TextView Questnumber { get; set; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return _items[position];
        }
    }
}