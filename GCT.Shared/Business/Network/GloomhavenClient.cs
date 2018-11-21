using Business.Network;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloomhavenCampaignTracker.Shared.Business.Network
{
    public class GloomhavenClient  : Client
    {
        private Guid _guid;

        private static GloomhavenClient _gclient;
        
        public static Guid ClientGuid { get; set; }
        public static string ServerIPAddress { get; set; }
        public static int Port = 54218;

        public static GloomhavenClient Instance
        {
            get
            {
                if (_gclient != null) return _gclient;

                if (string.IsNullOrEmpty(ServerIPAddress)) throw new Exception("Server IP address not set");
                if (ClientGuid == null) throw new Exception("Client Guid not set");

                _gclient = new GloomhavenClient(ServerIPAddress, ClientGuid);

                return _gclient;
            }
        }

        public Guid ServerGuid { get; internal set; }

        private GloomhavenClient(string address, Guid clientguid) : base(address, Port)
        {
            _guid = clientguid;
        }

        public static bool IsClientRunning()
        {
            return _gclient != null;
        }

        internal void SendClientConfig(string username, string dbversion)
        {
            var clientConfig = JsonConvert.SerializeObject(new ClientConfig()
            {
                Clientname = username,
                DBVersion = dbversion,
                GUID = _guid
            });

            var clienthello = new DataExchangeProtocoll()
            {
                PayloadType = PAYLOADTYPES.CLIENTCONFIG,
                JSONPayload = clientConfig,
                PayloadLength = clientConfig.Length
            };

            Send(clienthello);
        }

        internal void SendRequest(PAYLOADTYPES payload)
        {
            var req = JsonConvert.SerializeObject(new Request()
            {
                GUID = _guid
            });

            var package = new DataExchangeProtocoll()
            {
                PayloadType = payload,
                JSONPayload = req,
                PayloadLength = req.Length
            };

            Send(package);
        }

        internal void SendCampaignRequest(int campaignId)
        {
            var req = JsonConvert.SerializeObject(new CampaignRequest()
            {
                GUID = _guid,
                CampaignId = campaignId
            });

            var package = new DataExchangeProtocoll()
            {
                PayloadType = PAYLOADTYPES.CAMPAIGNDATA_REQUEST,
                JSONPayload = req,
                PayloadLength = req.Length
            };

            Send(package);
        }

        internal void SendCampaignUpdateRequest(string campaignName, PAYLOADTYPES plType)
        {
            var req = JsonConvert.SerializeObject(new CampaignUpdateRequest()
            {
                GUID = _guid,
                CampaignName = campaignName
            });

            var package = new DataExchangeProtocoll()
            {
                PayloadType = plType,
                JSONPayload = req,
                PayloadLength = req.Length
            };

            Send(package);
        }

        internal static void StopClient()
        {
            _gclient = null;
        }

        public async Task UpdatePartyAchievements()
        {
            // Before we sync the scenario data we must sync the achievements
            SendCampaignUpdateRequest(GCTContext.CurrentCampaign.CampaignData.Name, PAYLOADTYPES.PARTYACHIEVEMENTS_REQUEST);

            var responsePA = await Recieve();

            if (responsePA.PayloadType == PAYLOADTYPES.PARTYACHIEVEMENTS_RESPONSE)
            {
                var data = JsonConvert.DeserializeObject<IEnumerable<DL_CampaignPartyAchievement>>(responsePA.JSONPayload);

                if (data == null) return;

                var currentPAs = GCTContext.CurrentCampaign.CurrentParty.PartyAchievements;
                foreach (DL_CampaignPartyAchievement cpa in data)
                {
                    var currentPA = currentPAs.FirstOrDefault(y => cpa.ID_PartyAchievement == y.ID_PartyAchievement);

                    // Add not existing Global Achievement Type
                    if (currentPA == null)
                    {
                        var pa = PartyAchievementRepository.Get(cpa.Id, false);
                        if (pa != null)
                        {
                            GCTContext.CurrentCampaign.AddPartyAchievement(pa);
                        }
                    }
                }
            }
        }

        public async Task UpdateScenarios(List<CampaignUnlockedScenario> scenarios)
        {
            SendCampaignUpdateRequest(GCTContext.CurrentCampaign.CampaignData.Name, PAYLOADTYPES.CAMPAIGN_UNLOCKEDSCENARIOS_REQUEST);

            var resonse = await Recieve();

            if (resonse.PayloadType == PAYLOADTYPES.CAMPAIGN_UNLOCKEDSCENARIOS_RESPONSE)
            {
                var scenariosOnServer = JsonConvert.DeserializeObject<IEnumerable<DL_CampaignUnlockedScenario>>(resonse.JSONPayload);

                if (scenariosOnServer == null) return;

                // Get the Treasure Data
                SendCampaignUpdateRequest(GCTContext.CurrentCampaign.CampaignData.Name, PAYLOADTYPES.CAMPAIGN_TREASURES_REQUEST);

                var resonseTreasures = await Recieve();

                IEnumerable<DL_Treasure> scenariotreasures = null;

                if (resonseTreasures.PayloadType == PAYLOADTYPES.CAMPAIGN_TREASURES_RESPONSE)
                {
                    scenariotreasures = JsonConvert.DeserializeObject<IEnumerable<DL_Treasure>>(resonseTreasures.JSONPayload);
                }

                // update existing scenarios
                foreach (DL_CampaignUnlockedScenario cs in scenariosOnServer)
                {
                    var currentScenario = scenarios.FirstOrDefault(x => x.ScenarioId == cs.ID_Scenario);

                    if (currentScenario != null)
                    {
                        currentScenario.Completed = cs.Completed;

                        if (scenariotreasures != null)
                        {
                            foreach (DL_Treasure st in scenariotreasures.Where(x => x.CampaignScenario_ID == cs.Id))
                            {
                                if (!currentScenario.Treasures.Any(x => x.Number == st.Number))
                                {
                                    currentScenario.AddTreasure(st.Number, st.Content, st.Looted);
                                }
                            }
                        }

                        currentScenario.Save();
                    }
                }

                // new scenarios on server
                foreach (DL_CampaignUnlockedScenario cs in scenariosOnServer.Where(x => !scenarios.Any(y => y.ScenarioId == x.ID_Scenario)))
                {
                    var newScenario = GCTContext.CampaignCollection.CurrentCampaign.AddUnlockedScenario(cs.ID_Scenario);
                    if (scenariotreasures != null)
                    {
                        foreach (DL_Treasure st in scenariotreasures.Where(x => x.CampaignScenario_ID == cs.Id))
                        {
                            newScenario.AddTreasure(st.Number, st.Content, st.Looted);
                        }
                    }
                    scenarios.Add(newScenario);
                }

                GCTContext.CurrentCampaign.Save();
            }
        }

        public async Task UpdateGlobalAchievements()
        {
            // Before we sync the scenario data we must sync the achievements
            SendCampaignUpdateRequest(GCTContext.CurrentCampaign.CampaignData.Name, PAYLOADTYPES.CAMPAIGNGLOBALACHIEVEMENTS_REQUEST);

            var responseGA = await Recieve();

            if (responseGA.PayloadType == PAYLOADTYPES.CAMPAIGNGLOBALACHIEVEMENTS_RESPONSE)
            {
                var data = JsonConvert.DeserializeObject<IEnumerable<DL_CampaignGlobalAchievement>>(responseGA.JSONPayload);

                if (data == null) return;

                var currentGAs = GCTContext.CurrentCampaign.GlobalAchievements;
                foreach (DL_CampaignGlobalAchievement cga in data)
                {
                    var currentGA = currentGAs.FirstOrDefault(y => cga.ID_AchievementType == y.AchievementType.Id);

                    // Add not existing Global Achievement Type
                    if (currentGA == null)
                    {
                        GCTContext.CurrentCampaign.AddGlobalAchievement(cga.AchievementType, cga.Achievement);
                    }
                    else
                    {
                        // Update existing Achievements Type
                        currentGA.Achievement = cga.Achievement;
                        currentGA.ID_Achievement = cga.ID_Achievement;
                        currentGA.Step = cga.Step;

                        GCTContext.CurrentCampaign.Save();
                    }
                }
            }
        }

        public async Task UpdateCity()
        {
            SendCampaignUpdateRequest(GCTContext.CurrentCampaign.CampaignData.Name, PAYLOADTYPES.CAMPAIGN_CITY_REQUEST);

            var resonse = await Recieve();

            if (resonse.PayloadType == PAYLOADTYPES.CAMPAIGN_CITY_RESPONSE)
            {
                var data = JsonConvert.DeserializeObject<CityExchangeData>(resonse.JSONPayload);

                if (data == null) return;

                GCTContext.CurrentCampaign.CampaignData.DonatedGold = data.Donations;
                GCTContext.CurrentCampaign.CampaignData.CityProsperity = data.Prosperity;
            }
        }

        public async Task UpdateCampaignUnlocks()
        {
           SendCampaignUpdateRequest(GCTContext.CurrentCampaign.CampaignData.Name, PAYLOADTYPES.CAMPAIGNUNLOCKS_REQUEST);

            var resonse = await Recieve();

            if (resonse.PayloadType == PAYLOADTYPES.CAMPAIGNUNLOCKS_RESPONSE)
            {
                var data = JsonConvert.DeserializeObject<DL_CampaignUnlocks>(resonse.JSONPayload);

                if (data == null) return;

                GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks = data;
                GCTContext.CurrentCampaign.UnlockedClassesIds = Helper.StringToIntList(GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.UnlockedClassesIds);
            }
        }

        public async Task UpdateCampaignItemStore()
        {
            SendCampaignUpdateRequest(GCTContext.CurrentCampaign.CampaignData.Name, PAYLOADTYPES.CAMPAIGNUNLOCKEDITEMS_REQUEST);

            var resonse = await Recieve();

            if (resonse.PayloadType == PAYLOADTYPES.CAMPAIGNUNLOCKEDITEMS_RESPONSE)
            {
                var data = JsonConvert.DeserializeObject<List<DL_CampaignUnlockedItem>>(resonse.JSONPayload);

                if (data == null) return;

                foreach(DL_CampaignUnlockedItem cui in data)
                {
                    if (!GCTContext.CurrentCampaign.CampaignData.UnlockedItems.Any(x => x.Id == cui.ID_Item))
                    {
                        var item = ItemRepository.Get(cui.Id);
                        if(item != null)
                        {
                            GCTContext.CurrentCampaign.CampaignData.UnlockedItems.Add(item);
                        }
                    }
                }
            }
        }
    }
}
