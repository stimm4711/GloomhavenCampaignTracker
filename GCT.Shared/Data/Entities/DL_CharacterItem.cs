using Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_CharacterItem")]
    public class DL_CharacterItem : IEntity, ICharacterItemExchange
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }

        [ForeignKey(typeof(DL_Character))]
        public int ID_Character { get; set; }

        [ForeignKey(typeof(DL_Item))]
        public int ID_Item { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Item Item { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public DL_Character Character { get; set; }
    }
}