using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Linq;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class CampaignUnlockedScenariosDBAccess : IDBAccess<DL_CampaignUnlockedScenario>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public CampaignUnlockedScenariosDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_CampaignUnlockedScenario> Get(bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetAllWithChildren<DL_CampaignUnlockedScenario>(recursive: recursive);
            }
        }

        public DL_CampaignUnlockedScenario Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_CampaignUnlockedScenario>(id, recursive: recursive);
            }
        }           

        public void InsertOrReplace(DL_CampaignUnlockedScenario item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_CampaignUnlockedScenario> items)
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

        internal void Insert(IEnumerable<DL_CampaignUnlockedScenario> items)
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

        internal IEnumerable<DL_CampaignUnlockedScenario> CompletedScenario39AndScenario15NotUnlocked()
        {
            lock (locker)
            {
                try
                {
                    var query = "Select * From DL_CampaignUnlockedScenario cus Inner join DL_Scenario s On s.Id = cus.ID_Scenario Where s.Scenarionumber = 39 and cus.Completed = 1 and (Select Count(*) From DL_CampaignUnlockedScenario c Inner join DL_Scenario s On s.Id = c.ID_Scenario Where s.Scenarionumber = 15 and c.ID_Campaign = cus.ID_Campaign) = 0";

                    var result = Connection.Query<DL_CampaignUnlockedScenario>(query);
                   return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        internal IEnumerable<DL_CampaignUnlockedScenario> GetUnlockedScenariosOfCampaign(int campaignId)
        {
            lock (locker)
            {
                try
                {
                    return Get().Where(x => x.ID_Campaign == campaignId);
                }
                catch
                {
                    throw;
                }
            }
        }

        internal DL_CampaignUnlockedScenario GetUnlockedScenario(int scenarioNumber, int campaignId)
        {
            lock(locker)
            {
                try
                {
                    var query = "Select cus.* From DL_CampaignUnlockedScenario cus Inner join DL_Scenario s On s.Id = cus.ID_Scenario Where cus.ID_Campaign = ? and s.Scenarionumber = ?";
                    return Connection.ExecuteScalar<DL_CampaignUnlockedScenario>(query, campaignId, scenarioNumber); 
                } 
                catch
                {
                    throw;
                }              
            }
        }        

        public void Delete(DL_CampaignUnlockedScenario item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }


    }
}