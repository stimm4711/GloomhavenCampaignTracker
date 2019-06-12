using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Linq;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class ScenarioDBAccess : IDBAccess<DL_Scenario>
    {
        static object locker = new object();
        private SQLiteConnection Connection;

        public ScenarioDBAccess()
        {
            Connection = GloomhavenDbHelper.Connection;
        }

        public List<DL_Scenario> GetAllWithChildren(bool recursive = true)
        {
            lock (locker)
            {
               return Connection.GetAllWithChildren<DL_Scenario>(recursive: recursive).OrderBy(x=>x.Scenarionumber).ToList();
            }
        }

        public List<DL_Scenario> Get(bool recursive = true)
        {
            lock (locker)
            {
                var query = $"Select * from DL_Scenario order by Scenarionumber";
                return Connection.Query<DL_Scenario>(query);
            }
        }

        public DL_Scenario Get(long id, bool recursive = true)
        {
            lock (locker)
            {
                return Connection.GetWithChildren<DL_Scenario>(id, recursive: recursive);
            }
        }      

        internal DL_Scenario GetScenarioByScenarioNumber(int number)
        {
            lock (locker)
            {
                var query = $"Select * from DL_Scenario where Scenarionumber = ?";
                return Connection.Query<DL_Scenario>(query, number).FirstOrDefault();
            }
        }       

        public void InsertOrReplace(DL_Scenario item)
        {
            lock (locker)
            {
                Connection.InsertOrReplaceWithChildren(item, recursive: true);
            }
        }

        internal void InsertOrReplace(IEnumerable<DL_Scenario> items)
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

        internal void Insert(IEnumerable<DL_Scenario> items)
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

        public void Delete(DL_Scenario item)
        {
            lock (locker)
            {
                Connection.Delete(item, true);
            }
        }

        public void UpdateScenarioRegion(int scenarioID, int regionId)
        {
            lock (locker)
            {
                var query = "UPDATE DL_Scenario SET Region_ID = ? WHERE Id = ?";
                Connection.Query<DL_Scenario>(query, regionId, scenarioID);
            }
        }

        internal void UpdateScenarioUnlockedScenarios(int scenarioNumber, string commaSeparatedScenarioNumbers)
        {
            lock (locker)
            {
                var query = "UPDATE DL_Scenario SET UnlockedScenarios = ? WHERE Scenarionumber = ?";
                Connection.Query<DL_Scenario>(query, commaSeparatedScenarioNumbers, scenarioNumber);
            }
        }
    }
}