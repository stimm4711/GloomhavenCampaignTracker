using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class EventWrapper : Java.Lang.Object
    {
        private DL_CampaignEventHistoryLogItem _eventitem;

        public EventWrapper(DL_CampaignEventHistoryLogItem eventitem)
        {
            _eventitem = eventitem;
        }

        public int ReferenceNumber
        {
            get { return _eventitem.ReferenceNumber; }
            set { _eventitem.ReferenceNumber = value; }
        }

        public string Action
        {
            get { return _eventitem.Action; }
            set { _eventitem.Action = value; }
        }

        public string Outcome
        {
            get { return _eventitem.Outcome; }
            set { _eventitem.Outcome = value; }
        }

        public int Position
        {
            get { return _eventitem.Position; }
            set { _eventitem.Position = value; }
        }

        public int Decision
        {
            get { return _eventitem.Decision; }
            set { _eventitem.Decision = value; }
        }

        public int EventType
        {
            get { return _eventitem.EventType; }
            set { _eventitem.EventType = value; }
        }
    }
}