using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    /// <summary>
    /// 
    /// </summary>
    internal class CampaignPartyAchievementAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<DL_CampaignPartyAchievement> _partyachievements;

        public override int Count => _partyachievements.Count;

        public CampaignPartyAchievementAdapter(Context context) 
        {
            _context = context;
            _partyachievements = new List<DL_CampaignPartyAchievement>();
            if (GCTContext.CurrentCampaign != null && GCTContext.CurrentCampaign.CurrentParty != null)
            {
                _partyachievements = GCTContext.CurrentCampaign.CurrentParty?.PartyAchievements;
            }           
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
            var partyAchievement = _partyachievements[position];
            var view = inflater.Inflate(Resource.Layout.listviewitem_partyachievement, null);

            //Handle TextView and display string from your list
            var listItemText = (TextView)view.FindViewById(Resource.Id.campaignAchievement_name);
            listItemText.Text = partyAchievement.PartyAchievement.Name;          

            // options button
            var optionsButton = view.FindViewById<Button>(Resource.Id.optionsButton);
            if (!optionsButton.HasOnClickListeners)
            {
                optionsButton.Click += (sender, e) =>
                {
                    ConfirmDeleteDialog(position);
                };
            }

            return view;
        }

        /// <summary>
        /// Confirm delete
        /// </summary>
        /// <param name="position"></param>
        private void ConfirmDeleteDialog(int position)
        {
            var inflater = _context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
            .SetMessage(_context.Resources.GetString(Resource.String.DeletePartyAchievement))
            .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
            {
                GCTContext.CurrentCampaign.RemoveAchievement(_partyachievements[position]);
                NotifyDataSetChanged();
            })
            .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
            .Show();
        }

        public override Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
    }
}