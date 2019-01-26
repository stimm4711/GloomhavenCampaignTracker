using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Data.ViewEntities;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using Java.Lang;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class PartyAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<DL_VIEW_CampaignParties> _parties;

        public override int Count => _parties.Count;

        public PartyAdapter(Context context, List<DL_VIEW_CampaignParties> parties) 
        {
            _context = context;                
            _parties = parties;       
        }

        public DL_VIEW_CampaignParties GetParty(int position)
        {
             return _parties[position]; 
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;              
            
            if (view == null)
            {
                var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                view = inflater.Inflate(Resource.Layout.listviewitem_party, null);
            }

            var campaignParty = _parties[position];

            // partyname
            var listItemText = (TextView)view.FindViewById(Resource.Id.partyViewItem_nameTextView);
            listItemText.Text = campaignParty.PartyName;

            // Character count
            var charcountText = (TextView)view.FindViewById(Resource.Id.partyViewItem_charCount);
            charcountText.Text = string.Format(_context.Resources.GetString(Resource.String.PartymemberCount), campaignParty.CharacterCount);

            // party achievement count
            var achCountText = (TextView)view.FindViewById(Resource.Id.partyViewItem_achCount);
            achCountText.Text = string.Format(_context.Resources.GetString(Resource.String.PartyachievementsCount), campaignParty.PartyachievementCount);

            // options button
            var optionsButton = view.FindViewById<ImageView>(Resource.Id.optionsImageView);
            if (!optionsButton.HasOnClickListeners)
            {
                optionsButton.Click += (sender, e) =>
                {
                    if (_parties.Count == 1)
                    {
                        Toast.MakeText(_context, _context.Resources.GetString(Resource.String.PartyCantbeDeletedIsLastOfCampaign), ToastLength.Short).Show();
                    }
                    else
                    {
                        ConfirmDeleteDialog(position);
                    }
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
                .SetMessage(_context.Resources.GetString(Resource.String.DeleteParty))
                .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
                {
                    var p = GCTContext.CurrentCampaign.CampaignData.Parties.FirstOrDefault(x => x.Id == _parties[position].PartyId);
                    if(p != null)
                    {
                        GCTContext.CurrentCampaign.CampaignData.Parties.Remove(p);
                        CampaignPartyRepository.Delete(p);
                    }
                   
                    _parties.Remove(_parties[position]);
                    NotifyDataSetChanged();                
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