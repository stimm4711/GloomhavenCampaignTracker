using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_ClassPerk")]
    public class DL_ClassPerk : Java.Lang.Object, IEntity, ISelectable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ClassId { get; set; }

        public int Checkboxnumber { get; set; }

        [MaxLength(500)]
        public string Perktext { get; set; }

        [ManyToMany(typeof(DL_CharacterPerk), CascadeOperations = CascadeOperation.None)]
        public List<DL_Character> Characters { get; set; }

        [Ignore]
        public bool IsSelected { get; set; }
    }
}