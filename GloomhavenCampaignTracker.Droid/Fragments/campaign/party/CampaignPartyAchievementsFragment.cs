using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.Threading.Tasks;
using GloomhavenCampaignTracker.Business.Network;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign.party
{
    public class CampaignPartyAchievementsFragment : CampaignDetailsFragmentBase
    {
        CampaignPartyAchievementAdapter _listViewAdapter;

        public static CampaignPartyAchievementsFragment NewInstance()
        {
            var frag = new CampaignPartyAchievementsFragment { Arguments = new Bundle() };
            return frag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {            
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_globalachievements, container, false);
            _listViewAdapter = new CampaignPartyAchievementAdapter(Context);

            _dataChanged = false;
            UpdateData();

            var fab = _view.FindViewById<FloatingActionButton>(Resource.Id.fab);                            

            if (!fab.HasOnClickListeners)
            {
                fab.Click += (sender, e) =>
                {
                    ShowAssignAchievementDialog(inflater);
                };
            }

            return _view;
        }

        private void UpdateData()
        {
            if (GloomhavenClient.IsClientRunning())
            {
                Task.Run(async () =>
                {
                    await GloomhavenClient.Instance.UpdatePartyAchievements();

                    Activity.RunOnUiThread(() =>
                    {
                        var achievementsListView = _view.FindViewById<ListView>(Resource.Id.campaignWorld_gaListViewListView);
                        achievementsListView.Adapter = _listViewAdapter;
                    });
                });
            }
            else
            {
                var achievementsListView = _view.FindViewById<ListView>(Resource.Id.campaignWorld_gaListViewListView);
                achievementsListView.Adapter = _listViewAdapter;
            }
        }

        private void ShowAssignAchievementDialog(LayoutInflater inflater)
        {
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_textinputlayoutwithspinner, null);
            var spinner = convertView.FindViewById<Spinner>(Resource.Id.spinner);

            IEnumerable<PartyAchievement> selectableAchievements = GCTContext.AchievementCollectiom.PartyAchievements.OrderBy(x => x.PartyAchievementData.Name);

            if (!Campaign.HasGlobalAchievement(GlobalAchievementsInternalNumbers.EndOfGloom) || !GCTContext.Settings.IsFCActivated)
                selectableAchievements = selectableAchievements.Where(x => x.PartyAchievementData.ContentOfPack == 1);

            if (GCTContext.CurrentCampaign.CurrentParty.PartyAchievements != null)
            {
                var assignedAchievementIds = GCTContext.CurrentCampaign.CurrentParty.PartyAchievements.Select(y => y.ID_PartyAchievement);
                selectableAchievements = selectableAchievements.Where(x => !(assignedAchievementIds.Contains(x.PartyAchievementData.Id)));
            }

            var adapter = new SpinnerAdapter(Context);
            adapter.AddItems(selectableAchievements);
            spinner.Adapter = adapter;

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
               .SetCustomView(convertView)
                .SetTitle(Resources.GetString(Resource.String.AssignPartyAchievement))  
                .SetPositiveButton(Resources.GetString(Resource.String.Assign), (senderAlert, args) =>
                {
                    var achType = selectableAchievements.ElementAt(spinner.SelectedItemPosition);

                    if (achType == null) return;

                    if (GCTContext.CurrentCampaign.CurrentParty.PartyAchievements.Any(x => x.ID_PartyAchievement == achType.PartyAchievementData.Id))
                    {
                        Toast.MakeText(Context, Resources.GetString(Resource.String.PartyAchievementsAlreadyAssigned), ToastLength.Short).Show();
                    }
                    else
                    {
                        _dataChanged = true;
                        GCTContext.CurrentCampaign.AddPartyAchievement(achType.PartyAchievementData);
                        _listViewAdapter.NotifyDataSetChanged();
                        CheckForDrakesUnlock();
                    }
                })
                .Show();
        }

        private void CheckForDrakesUnlock()
        {
            if (GCTContext.CurrentCampaign.HasTheDrakesPartyAchievements())
            {
                if(!GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.TheDrakePartyAchievementsUnlocked)
                {
                    GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.TheDrakePartyAchievementsUnlocked = true;
                    GCTContext.CurrentCampaign.AddEventToDeck(Context, 75, EventTypes.CityEvent);
                    GCTContext.CurrentCampaign.AddEventToDeck(Context, 66, EventTypes.RoadEvent);

                    if (GCTContext.CurrentCampaign.HasGlobalAchievement(GlobalAchievementsInternalNumbers.TheDrake_Slain))
                    {
                        GCTContext.CurrentCampaign.RemoveGlobalAchievement(GlobalAchievementsInternalNumbers.TheDrake_Slain);
                    }

                    if (!GCTContext.CurrentCampaign.HasGlobalAchievement(GlobalAchievementsInternalNumbers.TheDrake_Aided))
                    {
                        var gaType = AchievementTypeRepository.Get().FirstOrDefault(x => x.InternalNumber == 100);
                        if (gaType != null)
                        {
                            var ach = gaType.Achievements.FirstOrDefault(x => x.InternalNumber == 2);
                            GCTContext.CurrentCampaign.AddGlobalAchievement(gaType, ach);
                        }
                    }

                    new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                        .SetTitle(Resources.GetString(Resource.String.Congratulation))
                        .SetMessage(Resources.GetString(Resource.String.DrakesPartyachievementsUnlockGained))
                        .SetPositiveButton(Resources.GetString(Resource.String.OK), (sa, arg) => { })
                        .Show();
                }
            }           
        }
    }
}