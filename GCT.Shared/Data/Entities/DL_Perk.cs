using GloomhavenCampaignTracker.Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_Perk")]
    public class DL_Perk : IEntity, IPerkExchange
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }
               
        public int Checkboxnumber { get; set; }

        [MaxLength(250)]
        public string Perkcomment { get; set; }

        [ForeignKey(typeof(DL_Character))]
        public int ID_Character { get; set; }

        [ManyToOne, JsonIgnore]
        public DL_Character Character { get; set; }
    }
}