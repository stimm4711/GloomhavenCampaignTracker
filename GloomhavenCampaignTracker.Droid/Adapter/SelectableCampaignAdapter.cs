using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using Data.ViewEntities;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    /// <summary>
    /// Adapter for campaigns
    /// </summary>
    internal class SelectableCampaignAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<DL_VIEW_Campaign_selectable> _campaigns;

        public override int Count => _campaigns.Count;

        public SelectableCampaignAdapter(Context context, List<DL_VIEW_Campaign> campaigns)
        {
            _context = context;
            _campaigns = campaigns.Select(x=> new DL_VIEW_Campaign_selectable(x)).ToList();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        internal DL_VIEW_Campaign_selectable GetCampaign(int position)
        {
            return _campaigns[position];
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            DL_VIEW_Campaign_Selectable_holder holder = null;

            if (convertView != null)
                holder = convertView.Tag as DL_VIEW_Campaign_Selectable_holder;

            if (holder == null)
            {
                // Create new holder
                holder = new DL_VIEW_Campaign_Selectable_holder();

                var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.listviewitem_selectable_campaign, parent, false);
                holder.Campaignname = convertView.FindViewById<TextView>(Resource.Id.campaignViewItem_nameTextView);
                holder.GlobalAchievementCount = convertView.FindViewById<TextView>(Resource.Id.campaignViewItem_achCount);
                holder.Selected = convertView.FindViewById<CheckBox>(Resource.Id.selected);
                holder.ScenarioCount = convertView.FindViewById<TextView>(Resource.Id.campaignViewItem_scenCount);

                // Looted CheckCHanged Event
                holder.Selected.CheckedChange += (sender, e) =>
                {
                    var chkBx = (CheckBox)sender;
                    var thisItem = (DL_VIEW_Campaign_selectable)chkBx.Tag;

                    if (thisItem == null || thisItem.IsSelected == chkBx.Checked) return;
                    thisItem.IsSelected = chkBx.Checked;
                    NotifyDataSetChanged();
                };

                convertView.Tag = holder;
            }

            var campaign = _campaigns[position];

            // Set Data
            holder.Campaignname.Text = campaign.Campaignname;
            holder.GlobalAchievementCount.Text = string.Format(
                    _context.Resources.GetString(Resource.String.NameValue),
                    _context.Resources.GetString(Resource.String.GlobalAchievements),
                    campaign.GlobalAchievementCount
                );
            holder.Selected.Tag = campaign;
            holder.Selected.Checked = campaign.IsSelected;
            holder.ScenarioCount.Text = string.Format(
                    _context.Resources.GetString(Resource.String.NameValue),
                    _context.Resources.GetString(Resource.String.UnlockedScenarios),
                    campaign.ScenarioCount
                );

            return convertView;
        }

        private class DL_VIEW_Campaign_Selectable_holder : Java.Lang.Object
        {
            public TextView Campaignname { get; set; }
            public TextView ScenarioCount { get; set; }
            public CheckBox Selected { get; set; }
            public TextView GlobalAchievementCount { get; set; }
        }

        internal DL_VIEW_Campaign_selectable GetSelected()
        {
            return _campaigns.FirstOrDefault(x => x.IsSelected);
        }

        public override Object GetItem(int position)
        {
            return position;
        }
    }
}