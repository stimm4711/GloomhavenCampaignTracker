using System;

namespace GloomhavenCampaignTracker.Shared.Business.Network
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