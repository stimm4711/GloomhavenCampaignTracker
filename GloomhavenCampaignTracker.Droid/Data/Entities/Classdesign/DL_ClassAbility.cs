using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign
{
    [Table("DL_ClassAbility")]
    public class DL_ClassAbility : Java.Lang.Object, IEntity, ISelectable
    {
        [PrimaryKey, AutoIncrement, JsonIgnore] 
        public int Id { get; set; }

        [MaxLength(250)]
        public string AbilityName { get; set; }

        public int ReferenceNumber { get; set; }

        public int Level { get; set; }

        public string Image_url { get; set; }

        [ForeignKey(typeof(DL_Class))]
        public int ID_Class { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Class DL_Class { get; set; }

        [Ignore]
        public bool IsSelected { get; set; }
    }
}