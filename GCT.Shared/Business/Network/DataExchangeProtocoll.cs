namespace GloomhavenCampaignTracker.Shared.Business.Network
{
    public class DataExchangeProtocoll
    {
        public PAYLOADTYPES PayloadType { get; set; } 

        public long PayloadLength { get; set; }

        public string JSONPayload { get; set; }
    }
}