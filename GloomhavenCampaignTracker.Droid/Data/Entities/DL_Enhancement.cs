using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_Enhancement")]
    public class DL_Enhancement : Java.Lang.Object, IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string EnhancementCode { get; set; }

        public int Basecosts { get; set; }

        [ManyToMany(typeof(DL_AbilityEnhancement), CascadeOperations = CascadeOperation.None)]
        public List<DL_Ability> Abilities { get; set; }

        [Ignore]
        public bool IsSelected { get; set; }
    }
}