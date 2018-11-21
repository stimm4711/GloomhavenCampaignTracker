using System;

namespace GloomhavenCampaignTracker.Shared.Business.Network
{
    public class ServerConfig
    {
        public Guid ServerGuid { get; set; }

        public string DBVersion { get; set; }
    }
}