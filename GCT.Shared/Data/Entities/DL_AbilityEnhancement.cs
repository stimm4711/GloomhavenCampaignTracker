using GloomhavenCampaignTracker.Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_AbilityEnhancement")]
    public class DL_AbilityEnhancement : Java.Lang.Object, IEntity, IAbilityEnhancementExchange
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }

        [ForeignKey(typeof(DL_Ability))]
        public int ID_Ability { get; set; }

        [ForeignKey(typeof(DL_Enhancement))]
        public int ID_Enhancement { get; set; }

        public int SlotNumber { get; set; }

        public bool IsTop { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public DL_Ability Ability { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Enhancement Enhancement { get; set; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() == typeof(DL_AbilityEnhancement))
            {
                return ((DL_AbilityEnhancement)obj).Id == Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1066060786;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + ID_Ability.GetHashCode();
            hashCode = hashCode * -1521134295 + ID_Enhancement.GetHashCode();
            hashCode = hashCode * -1521134295 + SlotNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + IsTop.GetHashCode();
            return hashCode;
        }
    }
}