using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign
{
    [Table("DL_CharacterPerks")]
    public class DL_CharacterPerks : Java.Lang.Object, IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(DL_ClassPerks))]
        public int ID_ClassPerks { get; set; }

        [ForeignKey(typeof(DL_Character))]
        public int ID_Character { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Character Character { get; set; }
       
        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_ClassPerks ClassPerk { get; set; }
    }
}