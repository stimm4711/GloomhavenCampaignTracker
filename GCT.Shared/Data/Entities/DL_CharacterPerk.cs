using GloomhavenCampaignTracker.Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_CharacterPerk")]
    public class DL_CharacterPerk : IEntity, ICharacterPerkExchange
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }

        [ForeignKey(typeof(DL_Character))]
        public int ID_Character { get; set; }

        [ForeignKey(typeof(DL_ClassPerk))]
        public int ID_ClassPerk { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_ClassPerk Perk { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Character Character { get; set; }
    }
}