using System;
using System.Collections.Generic;
using System.Linq;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Business
{
    public class CampaignCollection
    {
        private readonly Lazy<List<Campaign>> m_campaigns;
        private readonly Dictionary<int, Campaign> m_campaignDict = new Dictionary<int, Campaign>();
        private int _currentCampaignId;


        public Campaign CurrentCampaign
        {
            get
            {
                if (_currentCampaignId <= 0) return null;

                if (m_campaignDict.ContainsKey(_currentCampaignId))
                {
                    return m_campaignDict[_currentCampaignId];
                }
                else
                {
                    try
                    {
                        DL_Campaign campData = DataServiceCollection.CampaignDataService.Get(_currentCampaignId);
                        if (campData != null)
                        {
                            Campaign cam = new Campaign(campData);
                            m_campaignDict[_currentCampaignId] = cam;

                            return cam;
                        }
                    }catch
                    {
                        return null;
                    }
                }
                    
                return null;
            }
        }

        public CampaignCollection()
        {
            m_campaigns = new Lazy<List<Campaign>>(() =>
            {
                var campaigns = new List<Campaign>();
                try
                {
                    foreach (DL_Campaign campData in DataServiceCollection.CampaignDataService.Get())
                    {
                        if (m_campaignDict.ContainsKey(campData.Id))
                        {
                            campaigns.Add(m_campaignDict[campData.Id]);
                        }
                        else
                        {
                            Campaign cam = new Campaign(campData);
                            campaigns.Add(cam);
                            m_campaignDict[campData.Id] = cam;
                        }
                    }
                }
                catch
                {
                    return campaigns;
                }
                

                return campaigns;
            });          
        }        

        public List<Campaign> Campaigns
        {
            get { return m_campaigns?.Value;  }            
        }

        public bool SetCurrentCampaign(int campaignId)
        {
            bool success = false;
            if(campaignId > 0)
            {
                DL_Campaign data = null;
                try
                {
                    data = DataServiceCollection.CampaignDataService.Get(campaignId);                    
                }
                catch
                {
                    // If an error occurs on loading the data, set campaign id to -1. then the current campaign is null
                    campaignId = -1;
                }

                if (data != null)
                {
                    var campaign = new Campaign(data);
                    m_campaignDict[campaignId] = campaign;
                    success = true;

                    if (campaign.CampaignData.Parties != null && campaign.CampaignData.Parties.Any())
                    {
                        campaign.SetCurrentParty(campaign.CampaignData.Parties.FirstOrDefault().Id);
                    }
                }                
            }

            _currentCampaignId = campaignId;

            return success;
        }

        public void DeleteCampaign(DL_Campaign campaign)
        {
            var campaignId = campaign.Id;

            if (m_campaignDict.ContainsKey(campaignId))
            {
                Campaigns.Remove(m_campaignDict[campaignId]);

                if (CurrentCampaign?.CampaignData.Id == campaignId)
                {
                    if (Campaigns.Any())
                    {
                        SetCurrentCampaign(Campaigns.FirstOrDefault().CampaignData.Id);
                    }                        
                }

                m_campaignDict[campaignId].Delete();
                m_campaignDict.Remove(campaignId);
            }
            else
            {
                DataServiceCollection.CampaignDataService.Delete(campaign);
            }
        }
    }
}