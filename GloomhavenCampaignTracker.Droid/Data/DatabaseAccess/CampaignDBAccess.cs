using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using Data.ViewEntities;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class CampaignDBAccess : IDBAccess<DL_Campaign>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public CampaignDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_Campaign> Get(bool recursive = true)
        {
            lock (locker)
            {
               return Connection.GetAllWithChildren<DL_Campaign>(recursive: recursive);
            }
        }

        public DL_Campaign Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_Campaign>(id, recursive: recursive);
            }
        }

        public void InsertOrReplace(DL_Campaign item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item);
            }
        }

        public void InsertOrReplace(DL_Campaign item, bool recursive)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: recursive);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_Campaign> items)
        {
            lock (locker)
            {
                Connection.BeginTransaction();
                try
                {
                    Connection.InsertOrReplaceAllWithChildren(items);
                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        internal void Insert(IEnumerable<DL_Campaign> items)
        {
            lock (locker)
            {
                Connection.BeginTransaction();
                try
                {
                    Connection.InsertAllWithChildren(items);
                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        internal List<DL_Character> GetRetiredCharacters(int campaignId)
        {
            lock(locker)
            {
                var query = "Select chara.* " +
                            "from DL_Character chara " +
                            "Inner join DL_CampaignParty party on party.Id = chara.ID_Party " +
                            "Where party.ID_Campaign = ? And chara.Retired = 1";
                return Connection.Query<DL_Character>(query, campaignId);
            }
        }

        public void Delete(DL_Campaign item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }

        internal List<DL_Campaign> GetCampaignsFlat()
        {
            lock (locker)
            {
                var query = "Select * from DL_Campaign";
                return Connection.Query<DL_Campaign>(query);
            }
        }

        internal List<DL_Item> GetUnlockableItems(int campaignId)
        {
            lock (locker)
            {
                var query = "Select * from DL_Item where Prosperitylevel > 9 Except Select i.* from DL_Item i inner join DL_CampaignUnlockedItem ci on i.Id = ci.ID_Item where ci.ID_Campaign = ? ";
                return Connection.Query<DL_Item>(query, campaignId);
            }
        }

        internal List<DL_Scenario> GetUnlockedScenarios(int campaignId)
        {
            lock (locker)
            {
                var query = "Select s.* From DL_CampaignUnlockedScenario cus " +
                            "Inner join DL_Scenario s On cus.ID_Scenario = s.Id " +
                            "Where ID_Campaign = ? " +
                            "Order by s.Scenarionumber";
                return Connection.Query<DL_Scenario>(query, campaignId);
            }
        }

        internal List<DL_CampaignUnlockedScenario> GetUnlockedCampaignScenarios(int campaignId)
        {
            lock (locker)
            {
                var query =
                    "Select * FROM DL_CampaignUnlockedScenario Where ID_Campaign = ? Order by ID_Scenario";
                var campScenarios = Connection.Query<DL_CampaignUnlockedScenario>(query, campaignId);

                for (int i = 0; i < campScenarios.Count; i++)
                {
                    campScenarios[i] = Connection.GetWithChildren<DL_CampaignUnlockedScenario>(campScenarios[i].Id, recursive: true);
                }

                return campScenarios;
            }
        }

        internal List<DL_VIEW_CampaignParties> GetParties(int campaignId)
        {
            lock (locker)
            {
                var query = "Select  p.id as PartyId, p.name as PartyName , cc.Charactercount as CharacterCount , ca.Partyachievementcount as PartyachievementCount " +
                    "From DL_CampaignParty p " +
                    "left join (Select ip.id as partyID , Count(c.id) As Charactercount  From DL_CampaignParty ip left join DL_Character c  on c.id_party = ip.id  group by ip.Id ) as cc " +
                    " on p.id = cc.partyid " +
                    "left join (Select ip.id as partyID , Count(pa.id) As Partyachievementcount From DL_CampaignParty ip left join DL_CampaignPartyAchievement pa on pa.id_party = ip.id group by ip.Id) as ca " +
                    "on p.id = ca.partyid " +
                    "Where p.ID_campaign = ?";
                var pa = Connection.Query<DL_VIEW_CampaignParties>(query, campaignId);
                return pa;
            }
        }

        internal List<DL_VIEW_Campaign> GetCampaigns()
        {
            lock (locker)
            {
                var query = "Select c.id as CampaignId, c.name as Campaignname, sc.ScenarioCount as ScenarioCount, ca.GlobalachievementCount as GlobalAchievementCount From DL_Campaign c "+
                    "left join (Select ic.id as CampaignId, Count(s.id) As ScenarioCount From DL_Campaign ic left join DL_CampaignUnlockedScenario s on s.id_campaign = ic.id group by ic.Id) as sc on c.id = sc.CampaignId "+
                    "left join (Select ic.id as CampaignId, Count(ga.id) As GlobalachievementCount From DL_Campaign ic left join DL_GlobalAchievement ga on ga.id_campaign = ic.id group by ic.Id) as ca on c.id = ca.CampaignId";
                var pa = Connection.Query<DL_VIEW_Campaign>(query);
                return pa;
            }
        }     

        internal List<DL_AchievementType> GetAchievementTypes()
        {
            lock (locker)
            {
                var query = "Select Id, Name , ContentOfPack FROM DL_AchievementType";
                var result = Connection.Query<DL_AchievementType>(query);

                return result;
            }
        }

    }
}