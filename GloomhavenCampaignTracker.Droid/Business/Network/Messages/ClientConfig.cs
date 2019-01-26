using System;

namespace GloomhavenCampaignTracker.Business.Network.Messages
{
    public class ClientConfig
    {
        public Guid GUID { get; set; }

        public string DBVersion { get; set; }

        public string Clientname { get; set; }
    }
}