using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_Achievement")]
    public class DL_Achievement : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250), Unique]
        public string Name { get; set; }

        public int InternalNumber { get; set; }

        [ForeignKey(typeof(DL_AchievementType))]
        public int ID_AchievementType { get; set; }
        

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public DL_AchievementType AchievementType { get; set; }

        public override bool Equals(object obj)
        {
            if(obj != null && obj.GetType() == typeof(DL_Achievement))
            {
                return ((DL_Achievement)obj).Id == Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1919740922;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            if (Name != null) hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}