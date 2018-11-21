using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_PersonalQuest")]
    public class DL_PersonalQuest : Java.Lang.Object, IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int QuestNumber { get; set; }

        [MaxLength(250)]
        public string QuestName { get; set; }

        [MaxLength(250)]
        public string QuestGoal { get; set; }

        [MaxLength(250)]
        public string QuestReward { get; set; }

        public int QuestRewardClassId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.None)]
        public List<DL_Character> Characters { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<DL_PersonalQuestCounter> Counters { get; set; }
    }
}