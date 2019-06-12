using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign
{
    [Table("DL_CharacterAbilityEnhancement")]
    public class DL_CharacterAbilityEnhancement : Java.Lang.Object, IEntity
    {
        [PrimaryKey, AutoIncrement, JsonIgnore] 
        public int Id { get; set; }

        [ForeignKey(typeof(DL_Enhancement))]
        public int ID_Enhancement { get; set; }

        public int SlotNumber { get; set; }

        public bool IsTop { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Enhancement Enhancement { get; set; }

        [ForeignKey(typeof(DL_CharacterAbility))]
        public int ID_CharacterAbility { get; set; }      

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_CharacterAbility CharacterAbility { get; set; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() == typeof(DL_CharacterAbilityEnhancement))
            {
                return ((DL_CharacterAbilityEnhancement)obj).Id == Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1066060786;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + ID_CharacterAbility.GetHashCode();
            hashCode = hashCode * -1521134295 + ID_Enhancement.GetHashCode();
            hashCode = hashCode * -1521134295 + SlotNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + IsTop.GetHashCode();
            return hashCode;
        }
    }
}