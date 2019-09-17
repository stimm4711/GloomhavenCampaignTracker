using GloomhavenCampaignTracker.Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_CampaignScenarioTreasure")]
    public class DL_CampaignScenarioTreasure : Java.Lang.Object, IEntity
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }
        
        public bool Looted { get; set; }

        [ForeignKey(typeof(DL_ScenarioTreasure))]
        public int ScenarioTreasure_ID { get; set; }

        [ForeignKey(typeof(DL_CampaignUnlockedScenario))]
        public int CampaignScenario_ID { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public DL_CampaignUnlockedScenario UnlockedScenario { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public DL_ScenarioTreasure ScenarioTreasure { get; set; }
    }
}