using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign
{
    [Table("DL_CharacterAbility")]
    public class DL_CharacterAbility : Java.Lang.Object, IEntity
    {
        [PrimaryKey, AutoIncrement, JsonIgnore] 
        public int Id { get; set; }
       
        [ForeignKey(typeof(DL_ClassAbility))]
        public int ID_ClassAbility { get; set; }

        [ForeignKey(typeof(DL_Character))]
        public int ID_Character { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_ClassAbility Ability { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Character Character { get; set; }

    }
}