using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class PartyAchievementSpinnerAdapter : ArrayAdapter
    {
        private readonly Context _context;
        private readonly List<DL_PartyAchievement> _achievementtypes = new List<DL_PartyAchievement>();

        public override int Count => _achievementtypes.Count;

        public void AddItems(IEnumerable<DL_PartyAchievement> at)
        {
            _achievementtypes.AddRange(at);
        }

        public PartyAchievementSpinnerAdapter(Context context, int textViewResourceId): base(context, textViewResourceId)
        {
            _context = context;
        }

        public new DL_PartyAchievement GetItem(int position)
        {
            return _achievementtypes.Count > position ? _achievementtypes[position] : null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
         
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = GetView(position, convertView);
            return view;
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            var view = GetView(position, convertView);
            return view;
        }

        private View GetView(int position, View convertView)
        {
            var view = convertView;

            if (view == null)
            {
                var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                view = inflater.Inflate(Resource.Layout.itemview, null);
            }

            //Handle TextView and display string from your list
            var listItemText = (TextView)view.FindViewById(Resource.Id.itemTextView);
            listItemText.Text = _achievementtypes[position].Name;
            return view;
        }
    }
}