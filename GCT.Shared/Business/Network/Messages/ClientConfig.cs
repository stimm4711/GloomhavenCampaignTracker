using System;

namespace GloomhavenCampaignTracker.Shared.Business.Network
{
    public class ClientConfig
    {
        public Guid GUID { get; set; }

        public string DBVersion { get; set; }

        public string Clientname { get; set; }
    }
}