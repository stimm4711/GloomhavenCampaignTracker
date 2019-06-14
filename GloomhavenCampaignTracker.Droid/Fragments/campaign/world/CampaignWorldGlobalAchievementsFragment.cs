using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.Threading.Tasks;
using GloomhavenCampaignTracker.Business.Network;
using GloomhavenCampaignTracker.Business;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign.world
{
    public class CampaignWorldGlobalAchievementsFragment : CampaignDetailsFragmentBase
    {
        ListView _achievementsListView;
        CampaignAchievementTypeAdapter _listViewAdapter;

        public static CampaignWorldGlobalAchievementsFragment NewInstance(int campId)
        {
            var frag = new CampaignWorldGlobalAchievementsFragment { Arguments = new Bundle() };
            return frag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_globalachievements, container, false);

            // achievements
            _achievementsListView = _view.FindViewById<ListView>(Resource.Id.campaignWorld_gaListViewListView);
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
                    await GloomhavenClient.Instance.UpdateGlobalAchievements();

                    Activity.RunOnUiThread(() =>
                    {
                        _listViewAdapter = new CampaignAchievementTypeAdapter(Context);
                        _achievementsListView.Adapter = _listViewAdapter;
                    });

                });
            }
            else
            {
                _listViewAdapter = new CampaignAchievementTypeAdapter(Context);
                _achievementsListView.Adapter = _listViewAdapter;
            }
        }

        private void ShowAssignAchievementDialog(LayoutInflater inflater)
        {
            // create view 
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_textinputlayoutwithspinner, null);           
            var spinner = convertView.FindViewById<Spinner>(Resource.Id.spinner);

            IEnumerable<DL_AchievementType> selectableAchievements = DataServiceCollection.CampaignDataService.GetAchievementTypesFlat();

            if (!Campaign.CampaignData.CampaignUnlocks.CampaignCompleted || !GCTContext.ActivateForgottenCiclesContent)
                selectableAchievements = selectableAchievements.Where(x => x.ContentOfPack == 1);

            if (Campaign.GlobalAchievements != null)
            {
                var assignedAchievementIds = Campaign.GlobalAchievements.Select(y => y.AchievementType.Id);
                selectableAchievements = selectableAchievements.Where(x => !(assignedAchievementIds.Contains(x.Id)));
            }

            var adapter = new SpinnerAdapter(Context);
            adapter.AddItems(selectableAchievements);
            spinner.Adapter = adapter;                     
           
            // create dialog
            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)            
                .SetCustomView(convertView)
                .SetMessage("Select a global achievement")
                .SetTitle(Resources.GetString(Resource.String.AssignGlobalAchievement))           
                .SetPositiveButton(Resources.GetString(Resource.String.Assign), (senderAlert, args) =>
                {
                    var achType = selectableAchievements.ElementAt(spinner.SelectedItemPosition);
                    achType = AchievementTypeRepository.Get(achType.Id);

                    if (achType == null) return;

                    var gaWithSameType = Campaign.GlobalAchievements.Where(x => x.AchievementType.Id == achType.Id).Select(x => x.AchievementType);
                    if (gaWithSameType.Any())
                    {
                        Toast.MakeText(Context, Resources.GetString(Resource.String.AchievementAlreadyAssigned), ToastLength.Short).Show();
                    }
                    else
                    {
                        _dataChanged = true;
                        Campaign.AddGlobalAchievement(achType);

                        if (achType.InternalNumber == (int)GlobalAchievementsInternalNumbers.EndOfGloom)
                        {
                            Campaign.CampaignData.CampaignUnlocks.CampaignCompleted = true;

                            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                                .SetMessage("Congratulatios! You have finished the campaign. Now you can play the expansion Forgotten Circles. Go to Settings to activate the content of the expansion.")
                                .SetTitle("Campaign Finished!")
                                .SetPositiveButton(Resources.GetString(Resource.String.OK), (s, a) => { })
                                .Show();
                        }
                       
                        _listViewAdapter.NotifyDataSetChanged();
                    }
                })
                .Show();
        }
    }
}