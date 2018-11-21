using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Data.ViewEntities;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Droid.Fragments.campaign;
using GloomhavenCampaignTracker.Shared;
using GloomhavenCampaignTracker.Shared.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    /// <summary>
    /// Adapter for campaigns
    /// </summary>
    internal class CampaignAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<DL_VIEW_Campaign> _campaigns;
        private readonly bool _readonly;

        public override int Count => _campaigns.Count;

        public CampaignAdapter(Context context)
        {
            _context = context;
            _campaigns = DataServiceCollection.CampaignDataService.GetCampaigns();
            _readonly = false;
        }

        public CampaignAdapter(Context context, List<DL_VIEW_Campaign> campaigns)
        {
            _context = context;
            _campaigns = campaigns;
            _readonly = true;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        internal DL_VIEW_Campaign GetCampaign(int position)
        {
            return _campaigns[position];
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
            {
                var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                view = inflater.Inflate(Resource.Layout.listviewitem_campaign, null);
            }

            var campaign = _campaigns[position];

            // Campaignname
            var listItemText = (TextView)view.FindViewById(Resource.Id.campaignViewItem_nameTextView);
            listItemText.Text = campaign.Campaignname;

            // global achievements count
            var achCountText = (TextView)view.FindViewById(Resource.Id.campaignViewItem_achCount);
            achCountText.Text = 
                string.Format(
                    _context.Resources.GetString(Resource.String.NameValue), 
                    _context.Resources.GetString(Resource.String.GlobalAchievements), 
                    campaign.GlobalAchievementCount
                );

            // unclocked scenarios count
            var scenCountText = (TextView)view.FindViewById(Resource.Id.campaignViewItem_scenCount);           
            scenCountText.Text = string.Format(
                    _context.Resources.GetString(Resource.String.NameValue),
                    _context.Resources.GetString(Resource.String.UnlockedScenarios),
                    campaign.ScenarioCount
                );

            // options button
            var optionsButton = view.FindViewById<ImageView>(Resource.Id.optionsImageView);

            if (_readonly)
            {
                optionsButton.Visibility = ViewStates.Gone;
            }
            else
            {
                if (!optionsButton.HasOnClickListeners)
                {
                    optionsButton.Click += (sender, e) =>
                    {
                        ConfirmDeleteDialog(position);
                    };
                }
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
            CampaignSelectionFragment _frag = (CampaignSelectionFragment)((DetailsActivity)_context).SupportFragmentManager.FindFragmentById(Resource.Id.detailframe_container);

            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
               .SetMessage(_context.Resources.GetString(Resource.String.DeleteCampaign))
                .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
                {
                    var c = GCTContext.CampaignCollection.Campaigns.FirstOrDefault(x => x.CampaignData.Id == _campaigns[position].CampaignId);

                    if(c!= null)
                    {
                        GCTContext.CampaignCollection.DeleteCampaign(c.CampaignData);
                        _campaigns.Remove(_campaigns[position]);
                        NotifyDataSetChanged();
                        Toast.MakeText(_context, _context.Resources.GetString(Resource.String.CampaignDeleted), ToastLength.Short).Show();                       
                    }
                    _frag.CampaignSelected();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        public override Object GetItem(int position)
        {
            return position;
        }
    }
}