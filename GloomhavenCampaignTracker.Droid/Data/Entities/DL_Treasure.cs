using GloomhavenCampaignTracker.Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_Treasure")]
    public class DL_Treasure : IEntity, ITreasureExchange
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }
        
        public int Number { get; set; }

        public bool Looted { get; set; }

        public string Content { get; set; }

        [ForeignKey(typeof(DL_CampaignUnlockedScenario))]
        public int CampaignScenario_ID { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public DL_CampaignUnlockedScenario UnlockedScenario { get; set; }
    }
}