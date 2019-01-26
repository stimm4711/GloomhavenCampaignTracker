using GloomhavenCampaignTracker.Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_GlobalAchievement")]
    public class DL_CampaignGlobalAchievement : IEntity, ICampaignGlobalAchievementExchange
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }

        [ForeignKey(typeof(DL_Campaign))]
        public int ID_Campaign { get; set; }

        [ForeignKey(typeof(DL_AchievementType))]
        public int ID_AchievementType { get; set; }

        [ForeignKey(typeof(DL_Achievement))]
        public int ID_Achievement { get; set; }

        public int Step  { get; set; }       

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Campaign Campaign { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_AchievementType AchievementType { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Achievement Achievement { get; set; }
    }
}