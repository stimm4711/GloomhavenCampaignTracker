using SQLite;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_GlommhavenSettings")]
    public class DL_GlommhavenSettings : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Value { get; set; }
    }
}