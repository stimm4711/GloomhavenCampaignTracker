using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Network;
using Business.Network.Messages;
using Data;
using Data.ViewEntities;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using Newtonsoft.Json;
using Sockets.Plugin.Abstractions;

namespace GloomhavenCampaignTracker.Shared.Business.Network
{
    public enum PAYLOADTYPES
    {
        CONNECTIONLOST = 0,
        SERVERCONFIG,
        CLIENTCONFIG,
        CAMPAIGNLISTREQUEST,
        CAMPAIGNLISTRESPONSE,
        SELECTEDCAMPAIGNREQUEST,
        SELECTEDCAMPAIGNRESPONSE,
        CAMPAIGNDATA_REQUEST,
        CAMPAIGNDATA_RESPOMSE,
        PARTYDATA_REQUEST,
        PARTYDATA_RESPONSE,
        PARTYACHIEVEMENTS_REQUEST,
        PARTYACHIEVEMENTS_RESPONSE,
        CAMPAIGNGLOBALACHIEVEMENTS_REQUEST,
        CAMPAIGNGLOBALACHIEVEMENTS_RESPONSE,
        CAMPAIGNUNLOCKS_REQUEST,
        CAMPAIGNUNLOCKS_RESPONSE,
        CAMPAIGNEVENTHISTORY_REQUEST,
        CAMPAIGNEVENTHISTORY_RESPONSE,
        CAMPAIGNUNLOCKEDITEMS_REQUEST,
        CAMPAIGNUNLOCKEDITEMS_RESPONSE,
        CAMPAIGN_UNLOCKEDSCENARIOS_REQUEST,
        CAMPAIGN_UNLOCKEDSCENARIOS_RESPONSE,
        CAMPAIGN_TREASURES_REQUEST,
        CAMPAIGN_TREASURES_RESPONSE,
        CAMPAIGN_CHARACTER_REQUEST,
        CAMPAIGN_CHARACTER_RESPONSE,
        CAMPAIGN_CHARACTERABILITY_REQUEST,
        CAMPAIGN_CHARACTERABILITY_RESPONSE,
        CAMPAIGN_CHARACTERITEMS_REQUEST,
        CAMPAIGN_CHARACTERITEMS_RESPONSE,
        CAMPAIGN_CHARACTERPERK_REQUEST,
        CAMPAIGN_CHARACTERPERK_RESPONSE,
        CAMPAIGN_PERK_REQUEST,
        CAMPAIGN_PERK_RESPONSE,
        CAMPAIGN_ABILITYENHANCEMENTS_REQUEST,
        CAMPAIGN_ABILITYENHANCEMENTS_RESPONSE,
        CAMPAIGN_CITY_REQUEST,
        CAMPAIGN_CITY_RESPONSE,
        NULL_RESPONSE
    }

    public class GloomhavenServer : Server
    {        
        private Guid _serverFingerPrint;
        private Dictionary<Guid, ITcpSocketClient> _clients = new Dictionary<Guid, ITcpSocketClient>();
        private Dictionary<Guid, ClientConfig> _clientConfigs = new Dictionary<Guid, ClientConfig>();
        public event Func<object, ClientConnectedEventArgs, Task> ClientConnected;

        public static Guid Serverguid;
        public static int Port = 54218;

        private static GloomhavenServer _server = null;
        
        public static GloomhavenServer Instance
        {
            get
            {
                if (_server != null) return _server;

                if (Serverguid == null) throw new Exception("No Serverguid set");

                _server = new GloomhavenServer(Serverguid);

                return _server;
            }
        }

        public Dictionary<Guid, ITcpSocketClient> Clients { get => _clients; set => _clients = value; }

        private  GloomhavenServer(Guid serverguid) : base(Port)
        {
            _serverFingerPrint = serverguid;
        } 
        
        public static bool IsServerRunning()
        {
            return _server != null;
        }

        protected override void HandleData(DataExchangeProtocoll data, ITcpSocketClient client)
        {
            string response = null;
            DL_Campaign campaign = null;
            CampaignExchangeData exchData = null;

            // recieved payloadtype
            switch (data.PayloadType)
            {               
                case PAYLOADTYPES.CLIENTCONFIG:
                    // Recieved client config
                    var cconf = JsonConvert.DeserializeObject<ClientConfig>(data.JSONPayload); 

                    if (cconf != null && cconf.DBVersion == DatabaseUpdateHelper.Dbversion.ToString())
                    {
                        System.Diagnostics.Debug.Write($"REQUEST: SERVERCONFIG from {cconf.Clientname}");
                        Clients[cconf.GUID] = client;
                        _clientConfigs[cconf.GUID] = cconf;

                        try
                        {
                            OnClientConnected(new ClientConnectedEventArgs($"Client connected: {cconf.Clientname}."));

                            System.Diagnostics.Debug.Write($"RESPONSE: SERVERCONFIG to {cconf.Clientname}");

                            response = JsonConvert.SerializeObject(new ServerConfig() { ServerGuid = _serverFingerPrint, DBVersion = "1.3.2" });

                            data.PayloadType = PAYLOADTYPES.SERVERCONFIG;
                            data.PayloadLength = response.Length;
                            data.JSONPayload = response;

                            Send(data, client);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex);
                            throw;
                        }
                    }                   

                    break;
                case PAYLOADTYPES.CAMPAIGNLISTREQUEST: 
                    // Recieved client request for campaignlist
                    var req = JsonConvert.DeserializeObject<Request>(data.JSONPayload); 

                    if(req != null && _clientConfigs.ContainsKey(req.GUID))
                    {
                        var conf =  _clientConfigs[req.GUID];
                        System.Diagnostics.Debug.Write($"REQUEST: CAMPAIGNLIST from {conf.Clientname}");

                        var campaigns = GetCampaignList();

                        response = JsonConvert.SerializeObject(campaigns);

                        data.PayloadType = PAYLOADTYPES.CAMPAIGNLISTRESPONSE;
                        data.PayloadLength = response.Length;
                        data.JSONPayload = response;

                        Send(data, client);
                    }   
                    break;

                case PAYLOADTYPES.CAMPAIGNDATA_REQUEST:
                    var campRequest = JsonConvert.DeserializeObject<CampaignRequest>(data.JSONPayload);

                    if (campRequest == null) SendNullResponse(client);
                    
                    campaign = CampaignRepository.Get(campRequest.CampaignId);
                    
                    if (campaign == null) SendNullResponse(client);

                    exchData = new CampaignExchangeData
                    {
                        Campaign = campaign,
                        CampaignEventHistoryLogItems = CampaignEventHistoryLogItemRepository.Get().Where(x => x.ID_Campaign == campRequest.CampaignId),
                        CampaignGlobalAchievements = CampaignGlobalAchievementRepository.Get().Where(x => x.ID_Campaign == campRequest.CampaignId),
                        CampaignUnlockedItems = CampaignUnlockedItemRepository.Get().Where(x => x.ID_Campaign == campRequest.CampaignId),
                        CampaignUnlocks = CampaignUnlocksRepository.Get().FirstOrDefault(x => x.ID_Campaign == campRequest.CampaignId)
                    };

                    var scenarios = CampaignUnlockedScenarioRepository.Get().Where(x => x.ID_Campaign == campRequest.CampaignId);
                    exchData.CampaignUnlockedScenarios = scenarios;
                    exchData.ScenarioTreasures = scenarios.SelectMany(x => x.ScenarioTreasures);

                    var parties = CampaignPartyRepository.Get().Where(x => x.ID_Campaign == campRequest.CampaignId);
                    exchData.CampaignParties = parties;
                    exchData.PartyAchievements = parties.SelectMany(x => x.PartyAchievements);

                    var characters = CharacterRepository.Get().Where(x => exchData.CampaignParties.Any(y => y.Id == x.ID_Party));
                    exchData.Characters = characters;

                    var abilities = characters.SelectMany(x => x.Abilities);
                    exchData.CharacterAbilities = abilities;
                    exchData.AbilityEnhancements = abilities.SelectMany(x => x.Enhancements);

                    exchData.CharacterItems = characters.SelectMany(x => x.Items.Select(y=> new DL_CharacterItem() { Character = x, ID_Character = x.Id, Item = y, ID_Item = y.Id }));
                    exchData.CharacterPerks = characters.SelectMany(x => x.CharacterPerks.Select(y => new DL_CharacterPerk() { Character = x, ID_Character = x.Id, ID_ClassPerk = y.Id, Perk = y }));
                    exchData.Perks = characters.SelectMany(x => x.Perks);

                    response = JsonConvert.SerializeObject(exchData);

                    data.PayloadType = PAYLOADTYPES.CAMPAIGNDATA_RESPOMSE;
                    data.PayloadLength = response.Length;
                    data.JSONPayload = response;

                    Send(data, client);

                    break;

                case PAYLOADTYPES.CAMPAIGN_CITY_REQUEST:
                    var campCityRequest = JsonConvert.DeserializeObject<CampaignUpdateRequest>(data.JSONPayload);

                    if (campCityRequest == null) SendNullResponse(client);

                    campaign = CampaignRepository.Get().FirstOrDefault(x=>x.Name == campCityRequest.CampaignName);

                    if (campaign == null) SendNullResponse(client);

                    var cityExchangeData = new CityExchangeData();

                    if (GCTContext.CurrentCampaign.CampaignData.Id == campaign.Id)
                    {
                        cityExchangeData.Donations = GCTContext.CurrentCampaign.CampaignData.DonatedGold;
                        cityExchangeData.Prosperity = GCTContext.CurrentCampaign.CityProsperity;
                    }
                    else
                    {
                        cityExchangeData.Donations = campaign.DonatedGold;
                        cityExchangeData.Prosperity = campaign.CityProsperity;
                    }

                    response = JsonConvert.SerializeObject(cityExchangeData);

                    data.PayloadType = PAYLOADTYPES.CAMPAIGN_CITY_RESPONSE;
                    data.PayloadLength = response.Length;
                    data.JSONPayload = response;

                    Send(data, client);

                    break;

                case PAYLOADTYPES.CAMPAIGNUNLOCKS_REQUEST:
                    var unlocksRequest = JsonConvert.DeserializeObject<CampaignUpdateRequest>(data.JSONPayload);

                    if (unlocksRequest == null) SendNullResponse(client);

                    campaign = CampaignRepository.Get().FirstOrDefault(x => x.Name == unlocksRequest.CampaignName);

                    if (campaign == null) SendNullResponse(client);

                    response = JsonConvert.SerializeObject(campaign.CampaignUnlocks);

                    data.PayloadType = PAYLOADTYPES.CAMPAIGNUNLOCKS_RESPONSE;
                    data.PayloadLength = response.Length;
                    data.JSONPayload = response;

                    Send(data, client);

                    break;
                case PAYLOADTYPES.CAMPAIGNGLOBALACHIEVEMENTS_REQUEST:
                    var gaRequest = JsonConvert.DeserializeObject<CampaignUpdateRequest>(data.JSONPayload);

                    if (gaRequest == null) SendNullResponse(client);

                    campaign = CampaignRepository.Get().FirstOrDefault(x => x.Name == gaRequest.CampaignName);

                    if (campaign == null) SendNullResponse(client);

                    response = JsonConvert.SerializeObject(campaign.GlobalAchievements);

                    data.PayloadType = PAYLOADTYPES.CAMPAIGNGLOBALACHIEVEMENTS_RESPONSE;
                    data.PayloadLength = response.Length;
                    data.JSONPayload = response;

                    Send(data, client);
                    break;

                case PAYLOADTYPES.PARTYACHIEVEMENTS_REQUEST:
                    var paRequest = JsonConvert.DeserializeObject<CampaignUpdateRequest>(data.JSONPayload);

                    if (paRequest == null) SendNullResponse(client);

                    campaign = CampaignRepository.Get().FirstOrDefault(x => x.Name == paRequest.CampaignName);

                    if (campaign == null) SendNullResponse(client);

                    var currentPartyAchievements = GCTContext.CurrentCampaign.CurrentParty.PartyAchievements;

                    response = JsonConvert.SerializeObject(currentPartyAchievements);

                    data.PayloadType = PAYLOADTYPES.PARTYACHIEVEMENTS_RESPONSE;
                    data.PayloadLength = response.Length;
                    data.JSONPayload = response;

                    Send(data, client);
                    break;
                case PAYLOADTYPES.CAMPAIGN_UNLOCKEDSCENARIOS_REQUEST:
                    var usRequest = JsonConvert.DeserializeObject<CampaignUpdateRequest>(data.JSONPayload);

                    if (usRequest == null) SendNullResponse(client);

                    campaign = CampaignRepository.Get().FirstOrDefault(x => x.Name == usRequest.CampaignName);

                    if (campaign == null) SendNullResponse(client);

                    response = JsonConvert.SerializeObject(campaign.UnlockedScenarios);

                    data.PayloadType = PAYLOADTYPES.CAMPAIGN_UNLOCKEDSCENARIOS_RESPONSE;
                    data.PayloadLength = response.Length;
                    data.JSONPayload = response;

                    Send(data, client);
                    break;
                case PAYLOADTYPES.CAMPAIGN_TREASURES_REQUEST:
                    var tRequest = JsonConvert.DeserializeObject<CampaignUpdateRequest>(data.JSONPayload);

                    if (tRequest == null) SendNullResponse(client);

                    campaign = CampaignRepository.Get().FirstOrDefault(x => x.Name == tRequest.CampaignName);

                    if (campaign == null) SendNullResponse(client);

                    List<DL_Treasure> xx = campaign.UnlockedScenarios.SelectMany(x => x.ScenarioTreasures).ToList();

                    response = JsonConvert.SerializeObject(xx);

                    data.PayloadType = PAYLOADTYPES.CAMPAIGN_TREASURES_RESPONSE;
                    data.PayloadLength = response.Length;
                    data.JSONPayload = response;

                    Send(data, client);
                    break;

            }
        }       

        private List<DL_VIEW_Campaign> GetCampaignList() => DataServiceCollection.CampaignDataService.GetCampaigns();

        protected async void OnClientConnected(ClientConnectedEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            Func<object, ClientConnectedEventArgs, Task> handler = ClientConnected;

            // Event will be null if there are no subscribers
            if (handler == null) return;

            Delegate[] invocationList = handler.GetInvocationList();            
            Task[] handlerTasks = new Task[invocationList.Length];

            for (int i = 0; i < invocationList.Length; i++)
            {
                handlerTasks[i] = ((Func<object, ClientConnectedEventArgs, Task>)invocationList[i])(this, e);
            }

            await Task.WhenAll(handlerTasks);            
        }

        internal static void StopServer()
        {
            _server = null;            
        }
    }
}
