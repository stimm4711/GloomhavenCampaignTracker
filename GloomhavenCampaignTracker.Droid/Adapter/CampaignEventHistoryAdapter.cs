using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using GloomhavenCampaignTracker.Droid.Fragments.campaign;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class CampaignEventHistoryAdapter : DraggableListAdapter
    {
        private readonly Context _context;
        private readonly List<DL_CampaignEventHistoryLogItem> _eventDeckHistory;

        public CampaignEventHistoryAdapter(Context context, EventTypes eventtype)
        {
            _context = context;
            _eventDeckHistory = CampaignEventHistoryLogItemRepository.GetEvents(GCTContext.CurrentCampaign.CampaignData.Id, (int)eventtype);           
            mMobileCellPosition = int.MinValue;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return new EventWrapper(_eventDeckHistory.ElementAt(position));
        }

        public int GetLastHistoryId()
        {
            return _eventDeckHistory.Max(x=>x.Position);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            CampaignEventHistoryAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as CampaignEventHistoryAdapterViewHolder;

            if (holder == null)
            {
                holder = new CampaignEventHistoryAdapterViewHolder();
                var inflater = _context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                view = inflater.Inflate(Resource.Layout.itemview_eventdeckhistoryitem, parent, false);
                holder.ReferenceNumber = view.FindViewById<TextView>(Resource.Id.eventnumberTextView);
                holder.Action = view.FindViewById<TextView>(Resource.Id.eventaction);
                holder.Outcome = view.FindViewById<TextView>(Resource.Id.eventresult);
                holder.SelectedOption = view.FindViewById<TextView>(Resource.Id.selectedoption);
                holder.OptionsButton = view.FindViewById<ImageView>(Resource.Id.optionsImageView);
                view.Tag = holder;
            }

            var campaignEventHistoryLogItem = _eventDeckHistory[position];

            holder.ReferenceNumber.Text = $"# {campaignEventHistoryLogItem.ReferenceNumber}";
            holder.Action.Text = campaignEventHistoryLogItem.Action;
            
            if (campaignEventHistoryLogItem.Action == _context.Resources.GetString(Resource.String.Drawn))
            {
                view.FindViewById<LinearLayout>(Resource.Id.eventhistorybottom).Visibility = ViewStates.Visible;
                holder.Outcome.Text = campaignEventHistoryLogItem.Outcome;
                var decision = campaignEventHistoryLogItem.Decision == 1 ? "A" : "B";
                holder.SelectedOption.Text = $"{_context.Resources.GetString(Resource.String.Option)} {decision}";
            }
            else
            {
               view.FindViewById<LinearLayout>(Resource.Id.eventhistorybottom).Visibility = ViewStates.Gone;
            }

            // options button  
            if (!holder.OptionsButton.HasOnClickListeners)
            {
                holder.OptionsButton.Click += (sender, e) =>
                {
                    ConfirmDeleteDialog(position);
                };
            }

            if (mMobileCellPosition == position)
            {
                view.Visibility = ViewStates.Invisible;
            }
            else
            {
                view.Visibility = ViewStates.Visible;
            }
            view.TranslationY = 0;

            return view;
        }

        public override int Count => _eventDeckHistory.Count;

        /// <summary>
        /// Confirm delete ability
        /// </summary>
        /// <param name="position"></param>
        private void ConfirmDeleteDialog(int position)
        {
            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetTitle(_context.Resources.GetString(Resource.String.Confirm))
                .SetMessage(_context.Resources.GetString(Resource.String.DeleteEventhistoryEntry))
                .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
                {                    
                    CampaignEventHistoryLogItemRepository.Delete(_eventDeckHistory[position]);
                    _eventDeckHistory.Remove(_eventDeckHistory[position]);
                    NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        public override int mMobileCellPosition { get ; set ; }

        public override void SwapItems(int from, int to)
        {
            var fromPosition = _eventDeckHistory[from].Position;
            var toPosition = _eventDeckHistory[to].Position;
            _eventDeckHistory[from].Position = toPosition;
            _eventDeckHistory[to].Position = fromPosition;
           
            CampaignEventHistoryLogItemRepository.InsertOrReplace(_eventDeckHistory[to]);
            CampaignEventHistoryLogItemRepository.InsertOrReplace(_eventDeckHistory[from]);

            var oldValue = _eventDeckHistory[from];
            _eventDeckHistory[from] = _eventDeckHistory[to];
            _eventDeckHistory[to] = oldValue;
            mMobileCellPosition = to;

            NotifyDataSetChanged();
        }
    }

    internal class CampaignEventHistoryAdapterViewHolder : Java.Lang.Object
    {
        public TextView ReferenceNumber { get; set; }
        public TextView Action { get; set; }

        public TextView SelectedOption { get; set; }

        public TextView Outcome { get; set; }
        public ImageView OptionsButton { get; set; }
    }
}