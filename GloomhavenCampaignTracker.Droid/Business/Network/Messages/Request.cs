using System;

namespace GloomhavenCampaignTracker.Business.Network
{
    public class Request
    {
        public Guid GUID { get; set; }
    }

    public class CampaignRequest : Request
    {
        public int CampaignId { get; set; }
    }

    public class CampaignUpdateRequest : Request
    {
        public string CampaignName { get; set; }
    }

}