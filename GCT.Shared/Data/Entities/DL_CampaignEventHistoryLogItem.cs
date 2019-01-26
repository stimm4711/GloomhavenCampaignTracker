using GloomhavenCampaignTracker.Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_CampaignEventHistoryLogItem")]
    public class DL_CampaignEventHistoryLogItem : IEntity, ICampaignEventHistoryLogItemExchange
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }

        public int ReferenceNumber { get; set; }

        public string Action { get; set; } 

        public string Outcome { get; set; }

        public int Position { get; set; } = -1;

        public int Decision { get; set; }

        public int EventType { get; set; }

        [ForeignKey(typeof(DL_Campaign)), JsonIgnore]
        public int ID_Campaign { get; set; }

    }
}