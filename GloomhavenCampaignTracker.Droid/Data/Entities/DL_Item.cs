using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_Item")]
    public class DL_Item : Java.Lang.Object, IEntity, ISelectable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public int Itemnumber { get; set; }

        [MaxLength(250), Unique]
        public string Itemname { get; set; }

        public int Itemcategorie { get; set; }

        public int Itemcount { get; set; }

        public string Itemtext { get; set; }

        public int Itemprice { get; set; }

        public int Prosperitylevel { get; set; }

        public int ContentOfPack { get; set; } // 1 = gloomhaven, 2 = forgotten circles

        [ManyToMany(typeof(DL_CharacterItem))]
        public List<DL_Character> Characters { get; set; }

        [ManyToMany(typeof(DL_CampaignUnlockedItem))]
        public List<DL_Campaign> Campaign { get; set; }


        [Ignore]
        public bool IsSelected { get; set; }

        [Ignore]
        public bool IsHide { get; set; } = false;

        public override bool Equals(object obj)
        {
            if (!(obj is DL_Item)) return false;
            return ((DL_Item)obj).Id == Id;
        }

        public override int GetHashCode()
        {
            var hashCode = -689818818;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + Itemnumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<DL_Campaign>>.Default.GetHashCode(Campaign);
            return hashCode;
        }

        public string GetNumberText()
        {
            if (Itemnumber < 10) return $"00{Itemnumber}";
            if (Itemnumber < 100) return $"0{Itemnumber}";
            else return $"{Itemnumber}";
        }
    }
}