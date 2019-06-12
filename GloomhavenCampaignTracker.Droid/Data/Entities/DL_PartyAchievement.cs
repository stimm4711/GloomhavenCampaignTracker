using SQLite;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_PartyAchievement")]
    public class DL_PartyAchievement : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int InternalNumber { get; set; }

        [MaxLength(250), Unique]
        public string Name { get; set; }
    }
}