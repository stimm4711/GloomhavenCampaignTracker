using GloomhavenCampaignTracker.Business.Network.Messages;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    [Table("DL_CampaignUnlockedScenario")]
    public class DL_CampaignUnlockedScenario : IEntity, ICampaignUnlockedScenarioExchange
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(DL_Campaign))]
        public int ID_Campaign { get; set; }

        [ForeignKey(typeof(DL_Scenario))]
        public int ID_Scenario { get; set; }

        public bool Completed { get; set; }

        public DateTime LastSync { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Campaign Campaign { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead), JsonIgnore]
        public DL_Scenario Scenario { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_Treasure> ScenarioTreasures { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All), JsonIgnore]
        public List<DL_CampaignScenarioTreasure> CampaignScenarioTreasures { get; set; }

        
    }
}