using System.Collections.Generic;
using GloomhavenCampaignTracker.Business;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_Scenario")]
    public class DL_Scenario : IEntity, ISpinneritem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250), Unique]
        public string Name { get; set; }

        [Unique]
        public int Scenarionumber { get; set; }

        public string UnlockedScenarios { get; set; }

        public string RequiredGlobalAchievements { get; set; }

        public string RequiredPartyAchievements { get; set; }

        public string BlockingGlobalAchievements { get; set; }

        public string BlockingPartyAchievements { get; set; }

        public int Region_ID { get; set; }

        [ManyToMany(typeof(DL_CampaignUnlockedScenario), CascadeOperations = CascadeOperation.None)]
        public List<DL_Campaign> Campaigns { get; set; }

        public string Spinnerdisplayvalue
        {
            get
            {
                if (GCTContext.ShowScenarioNames)
                {
                    return $"# {Scenarionumber}   {Name}";
                }
                else
                {
                    return $"# {Scenarionumber}";
                }
            }
        }
    }
}