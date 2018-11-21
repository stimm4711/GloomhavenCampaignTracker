using System.Collections.Generic;
using Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_Character")]
    public class DL_Character :  IEntity, ISelectable, ICharacterExchange
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250), NotNull]
        public string Name { get; set; }

        [Column("Class")]
        public int ClassId { get; set; }

        public bool Retired { get; set; }

        public int Level { get; set; }

        public int Experience { get; set; }

        public int Gold { get; set; }

        public int LifegoalNumber { get; set; }
        
        public int Checkmarks { get; set; }

        public string Notes { get; set; }

        public string Playername { get; set; }

        public string PlayerGuid { get; set; }

        [ForeignKey(typeof(DL_CampaignParty))]
        public int ID_Party { get; set; }

        [ForeignKey(typeof(DL_PersonalQuest))]
        public int ID_PersonalQuest { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_CampaignParty Party { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_PersonalQuest PersonalQuest { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_Ability> Abilities { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_Perk> Perks { get; set; }

        [ManyToMany(typeof(DL_CharacterItem)), JsonIgnore]
        public List<DL_Item> Items { get; set; }

        [ManyToMany(typeof(DL_CharacterPerk)), JsonIgnore]
        public List<DL_ClassPerk> CharacterPerks { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_CharacterPersonalQuestCounter> CharacterQuestCounters { get; set; }


        public override bool Equals(object obj)
        {
            if (!(obj is DL_Character)) return false;
            if (((DL_Character)obj).Id == Id) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        [Ignore]
        public bool IsSelected { get; set; }
    }
}