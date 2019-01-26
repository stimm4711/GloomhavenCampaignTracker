using System.Collections.Generic;
using GloomhavenCampaignTracker.Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_CampaignParty")]
    public class DL_CampaignParty : IEntity, IPartyExchange
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        public int Reputation { get; set; }

        public int CurrentLocationNumber { get; set; }

        public string Notes { get; set; }

        [ForeignKey(typeof(DL_Campaign))]
        public int ID_Campaign { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Campaign Campaign { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_CampaignPartyAchievement> PartyAchievements { get; set; }

        //[OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        //public List<DL_Character> PartyMember { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is DL_CampaignParty)) return false;
            return ((DL_CampaignParty)obj).Id == Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}