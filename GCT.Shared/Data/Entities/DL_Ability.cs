using GloomhavenCampaignTracker.Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_Ability")]
    public class DL_Ability : Java.Lang.Object, IEntity, ICharacterAbilityExchange
    {
        
        [PrimaryKey, AutoIncrement, JsonIgnore] 
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string AbilityName { get; set; }

        public int ReferenceNumber { get; set; }

        public int Level { get; set; }

        [ForeignKey(typeof(DL_Character))]
        public int ID_Character { get; set; }

        [ManyToOne, JsonIgnore]
        public DL_Character Character { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_AbilityEnhancement> Enhancements { get; set; }


    }
}