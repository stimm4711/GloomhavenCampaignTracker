using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign
{
    [Table("DL_ClassPerks")]
    public class DL_ClassPerks : Java.Lang.Object, IEntity, ISelectable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(DL_Class))]
        public int ID_Class { get; set; }

        public int Checkboxnumber { get; set; }

        [MaxLength(500)]
        public string Perktext { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Class DL_Class { get; set; }

        [Ignore]
        public bool IsSelected { get; set; }
    }
}