using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_PersonalQuestCounter")]
    public class DL_PersonalQuestCounter : Java.Lang.Object, IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(DL_PersonalQuest))]
        public int PersonalQuest_ID { get; set; }

        [MaxLength(100)]
        public string CounterName { get; set; }

        public int CounterValue { get; set; }
        
        public int CounterScenarioUnlock { get; set; }       

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_PersonalQuest PersonalQuest { get; set; }
    }
}