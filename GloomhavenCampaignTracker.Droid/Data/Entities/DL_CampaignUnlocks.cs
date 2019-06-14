using GloomhavenCampaignTracker.Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_CampaignUnlocks")]
    public class DL_CampaignUnlocks : IEntity, ICampaignUnlocksExchange
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }

        [ForeignKey(typeof(DL_Campaign))]
        public int ID_Campaign { get; set; }

        public bool EnvelopeAUnlocked { get; set; }

        public bool EnvelopeBUnlocked { get; set; }

        public bool TownRecordsBookUnlocked { get; set; }

        public string UnlockedClassesIds { get; set; } = "1,2,3,4,5,6";

        public string UnlockedItemDesignNumbers { get; set; } = "";

        public bool ReputationPlus10Unlocked { get; set; }

        public bool ReputationPlus20Unlocked { get; set; }

        public bool ReputationMinus10Unlocked { get; set; }

        public bool ReputationMinus20Unlocked { get; set; }

        public bool TheDrakePartyAchievementsUnlocked { get; set; }

        public bool EnvelopeXUnlocked { get; set; }

        public bool EnvelopeXEncrypted { get; set; }

        public string EnvelopeXSolution { get; set; } 

        public bool HiddenClassUnlocked { get; set; }

        public bool CampaignCompleted { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Campaign Campaign { get; set; }
    }
}