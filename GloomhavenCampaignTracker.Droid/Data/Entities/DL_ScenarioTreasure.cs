using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_ScenarioTreasure")]
    public class DL_ScenarioTreasure : IEntity
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }
        
        public int TreasureNumber { get; set; }

        public string TreasureContent { get; set; }

        [ForeignKey(typeof(DL_Scenario))]
        public int Scenario_ID { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Scenario Scenario { get; set; }
    }
}