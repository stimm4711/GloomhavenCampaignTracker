using SQLite;
using SQLiteNetExtensions.Attributes;

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

        public int ContentOfPack { get; set; } // 1 = gloomhaven, 2 = forgotten circles
    }
}