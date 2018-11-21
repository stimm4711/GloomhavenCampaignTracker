using Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_CampaignPartyAchievement")]
    public class DL_CampaignPartyAchievement : IEntity, IPartyAchievementExchange
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }

        [ForeignKey(typeof(DL_CampaignParty))]
        public int ID_Party { get; set; }

        [ForeignKey(typeof(DL_PartyAchievement))]
        public int ID_PartyAchievement { get; set; }     

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_CampaignParty Party { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_PartyAchievement PartyAchievement { get; set; }
    }
}