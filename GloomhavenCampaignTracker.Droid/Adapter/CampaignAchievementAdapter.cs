using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class CampaignAchievementAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<DL_Achievement> _achievements;

        public override int Count => _achievements.Count;

        public CampaignAchievementAdapter(Context context, List<DL_Achievement> achievements)
        {
            _context = context;
            _achievements = achievements;
        }

        public DL_Achievement GetAchievement(int position)
        {
             return _achievements[position]; 
        }

        public override Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public int GetPosition(DL_Achievement item)
        {
            return _achievements.IndexOf(item);
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
                var inflater = (LayoutInflater) _context.GetSystemService(Context.LayoutInflaterService);
                view = inflater.Inflate(Resource.Layout.itemview_spinnerAchievement, null);
            }

            //Handle TextView and display string from your list
            var listItemText = (TextView) view.FindViewById(Resource.Id.itemTextView);
            listItemText.Text = _achievements[position] == null ? _context.Resources.GetString(Resource.String.Empty) : _achievements[position].Name;

            return view;
        }      
    }
}