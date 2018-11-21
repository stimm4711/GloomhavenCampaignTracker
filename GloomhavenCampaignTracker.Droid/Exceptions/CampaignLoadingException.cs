using System;

namespace GloomhavenCampaignTracker.Droid.Exceptions
{
    class CampaignLoadingException : Exception
    {
        public CampaignLoadingException(string message) : base(message)
        {
        }
    }

    class PartyLoadingException : Exception
    {
        public PartyLoadingException(string message) : base(message)
        {
        }
    }
}