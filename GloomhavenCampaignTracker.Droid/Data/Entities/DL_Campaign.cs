using System.Collections.Generic;
using GloomhavenCampaignTracker.Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_Campaign")]
    public class DL_Campaign : IEntity, ICampaignExchange
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }

        [MaxLength(250), Unique]
        public string Name { get; set; }

        public int CityProsperity { get; set; }

        public int DonatedGold { get; set; }

        public string RoadEventDeckString { get; set; }

        public string CityEventDeckString { get; set; }

        public string RiftEventDeckString { get; set; }

        public int CurrentParty_ID { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_CampaignParty> Parties { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_CampaignGlobalAchievement> GlobalAchievements { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public List<DL_CampaignUnlockedScenario> UnlockedScenarios { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public DL_CampaignUnlocks CampaignUnlocks { get; set; }

        [ManyToMany(typeof(DL_CampaignUnlockedItem)), JsonIgnore]
        public List<DL_Item> UnlockedItems { get; set; }


    }
}