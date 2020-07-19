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

        [MaxLength(5)]
        public string ClassShorty { get; set; }

        public int ContentOfPack { get; set; } // 1 = gloomhaven, 2 = forgotten circles, 3 = Jaws of the Lion

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_ClassPerks> Perks { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_Character> Characters { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_ClassAbility> Abilities { get; set; }

    }
}