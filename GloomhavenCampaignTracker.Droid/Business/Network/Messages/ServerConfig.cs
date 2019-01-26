using System;

namespace GloomhavenCampaignTracker.Business.Network
{
    public class ServerConfig
    {
        public Guid ServerGuid { get; set; }

        public string DBVersion { get; set; }
    }
}