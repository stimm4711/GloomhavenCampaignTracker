using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign
{
    [Table("DL_Class")]
    public class DL_Class : Java.Lang.Object, IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ClassId { get; set; }

        [MaxLength(100)]
        public string ClassName { get; set; }              

        public string ClassIcon { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_ClassPerks> Perks { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_Character> Characters { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_ClassAbility> Abilities { get; set; }

    }
}