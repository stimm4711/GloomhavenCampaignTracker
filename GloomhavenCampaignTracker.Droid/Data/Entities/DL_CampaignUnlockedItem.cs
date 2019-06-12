using GloomhavenCampaignTracker.Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_CampaignUnlockedItem")]
    public class DL_CampaignUnlockedItem : IEntity, ICampaignUnlockedItemExchange
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }

        [ForeignKey(typeof(DL_Campaign))]
        public int ID_Campaign { get; set; }

        [ForeignKey(typeof(DL_Item))]
        public int ID_Item { get; set; }

        public int InStore  { get; set; }       

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Campaign Campaign { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Item Item { get; set; }
    }
}