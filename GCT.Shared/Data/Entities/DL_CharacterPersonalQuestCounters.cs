using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_CharacterPersonalQuestCounter")]
    public class DL_CharacterPersonalQuestCounter :  IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(DL_PersonalQuestCounter))]
        public int PersonalQuestCounter_Id { get; set; }

        [ForeignKey(typeof(DL_Character))]
        public int Character_Id { get; set; }

        public int Value { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public DL_Character Character { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public DL_PersonalQuestCounter PersonalQuestCounter { get; set; }
    }
}