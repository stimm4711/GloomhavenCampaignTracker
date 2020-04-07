using System.Linq;
using Android.Content;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid
{
    internal class EventhistoryHelper
    {
        private static int GetLastAddedEventHistoryPosition(int eventtype)
        {
            var position = -1;
            var history = CampaignEventHistoryLogItemRepository.GetEvents(GCTContext.CurrentCampaign.CampaignData.Id, eventtype);
            if (history.Any())
            {
                position = history.Max(x => x.Position);
            }

            return position;
        }

        public static void DrawnEventHistory(Context context, int eventnumber, int eventtype, int selectedOption, string outcome = "No effect")
        {
            if(GCTContext.CurrentCampaign != null)
            {
                int position = GetLastAddedEventHistoryPosition(eventtype);
                CampaignEventHistoryLogItemRepository.InsertOrReplace(new DL_CampaignEventHistoryLogItem()
                {
                    Action = context.Resources.GetString(Resource.String.Drawn),
                    EventType = eventtype,
                    ID_Campaign = GCTContext.CurrentCampaign.CampaignData.Id,
                    ReferenceNumber = eventnumber,
                    Outcome = outcome,
                    Decision = selectedOption,
                    Position = position + 1
                });
            }            
        }

        public static void AddEventHistory(Context context,int eventnumber, int eventtype, bool doShuffle = true)
        {
            var comment = (doShuffle ? context.Resources.GetString(Resource.String.AddedShuffled) : context.Resources.GetString(Resource.String.Added));
            int position = GetLastAddedEventHistoryPosition(eventtype);

            CampaignEventHistoryLogItemRepository.InsertOrReplace(new DL_CampaignEventHistoryLogItem()
            {
                Action = comment,
                EventType = eventtype,
                ID_Campaign = GCTContext.CurrentCampaign.CampaignData.Id,
                ReferenceNumber = eventnumber,
                Position = position + 1
            });
        }

        public static void RemoveEventHistory(Context context, int eventnumber, int eventtype)
        {
            int position = GetLastAddedEventHistoryPosition(eventtype);

            CampaignEventHistoryLogItemRepository.InsertOrReplace(new DL_CampaignEventHistoryLogItem()
            {
                Action = context.Resources.GetString(Resource.String.Removed),
                EventType = eventtype,
                ID_Campaign = GCTContext.CurrentCampaign.CampaignData.Id,
                ReferenceNumber = eventnumber,
                Position = position + 1
            });
        }
    }
}