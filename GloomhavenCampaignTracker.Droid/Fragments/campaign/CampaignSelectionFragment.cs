using System;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using System.Linq;
using System.Text;
using ListFragment = Android.Support.V4.App.ListFragment;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    /// <summary>
    /// Select and add campaigns
    /// </summary>
    public class CampaignSelectionFragment : ListFragment
    { 
        public static CampaignSelectionFragment NewInstance()
        {
            return new CampaignSelectionFragment { Arguments = new Bundle() };
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ListAdapter = new CampaignAdapter(Context);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.listview_floatingactionbutton, container, false);
            var fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab);

            if (!fab.HasOnClickListeners)
            {
                fab.Click += (sender, e) =>
                {
                    AddNewCampaignDialog();                   
                };
            }

            return view;
        }

        /// <summary>
        /// Close Details Activity after campaign is selected
        /// </summary>
        public void CampaignSelected()
        {
            ListAdapter = null;
            if (Activity.GetType() == typeof(DetailsActivity))
            {
                Activity.Finish();
            }
        }

        /// <summary>
        /// Campaign selected
        /// </summary>
        /// <param name="lValue"></param>
        /// <param name="vValue"></param>
        /// <param name="position"></param>
        /// <param name="id"></param>
        public override void OnListItemClick(ListView lValue, View vValue, int position, long id)
        {
            var selectedCampaign = ((CampaignAdapter)ListAdapter).GetCampaign(position);

            // Set current campaign and current party
            //GCTContext.CurrentCampaign = selectedCampaign;
            if (GCTContext.CampaignCollection.SetCurrentCampaign(selectedCampaign.CampaignId))
            {
                if (GCTContext.CurrentCampaign != null)
                {
                    GCTContext.CurrentCampaign.SetCurrentParty(GCTContext.CurrentCampaign.CampaignData.CurrentParty_ID);
                }
            }
            else
            {
                Toast.MakeText(Context, "Error on loading campaign.", ToastLength.Long).Show();
            }           
            
            CampaignSelected();
        }      
        
        /// <summary>
        /// Add a new campaign in a dialog
        /// </summary>
        private void AddNewCampaignDialog()
        {
            var inflater = Activity.LayoutInflater;
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_addcampaign, null);

            var dialog = new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle(Resources.GetString(Resource.String.AddCampaign))
                .SetCancelable(false)
                .SetPositiveButton(Resources.GetString(Resource.String.StartCampaign), (senderAlert, args) => { })
                .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();

            var dialogPositiveButton = dialog.GetButton((int)DialogButtonType.Positive);
            var dialogNegativeButton = dialog.GetButton((int)DialogButtonType.Negative);

            dialogPositiveButton.Click += (senderAlert, args) =>
            {
                if (AddNewCampaign(convertView)) dialog.Dismiss();
            };

            dialogNegativeButton.Click += (senderAlert, args) => { dialog.Dismiss(); };
        }

        private bool AddNewCampaign(View convertView)
        {
            // get campaign name
            var campaignNameEdit = (EditText)convertView.FindViewById(Resource.Id.campaigname);
            if (string.IsNullOrEmpty(campaignNameEdit.Text))
            {
                // No campaign name 
                Snackbar.Make(convertView, Resources.GetString(Resource.String.CampaignNameMissing), Snackbar.LengthShort).Show();
            }
            else if (GCTContext.CampaignCollection.Campaigns.Any(x => string.Equals(x.CampaignData.Name, campaignNameEdit.Text, StringComparison.CurrentCultureIgnoreCase)))
            {
                // existing campaign name 
                Snackbar.Make(convertView, Resources.GetString(Resource.String.CampaignnameExist), Snackbar.LengthShort).Show();
            }
            else
            {
                // get party name
                var partyNameEdit = (EditText)convertView.FindViewById(Resource.Id.partyname);
                if (string.IsNullOrEmpty(partyNameEdit.Text))
                {
                    // no party name
                    Snackbar.Make(convertView, Resources.GetString(Resource.String.PartyNameMissing), Snackbar.LengthShort).Show();
                }
                else
                {

                    // create new campaign and add party
                    Campaign campaign = Campaign.NewInstance(campaignNameEdit.Text);
                    campaign.AddUnlockedScenario(1);
                    var messages = campaign.GetLoadingMessages();
                    if (messages.Any())
                    {
                        var sb = new StringBuilder();
                        foreach (string message in messages)
                        {
                            sb.Append(message);
                            sb.Append("\n");
                        }

                        new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                               .SetMessage(sb.ToString())
                               .SetTitle("Campaign loading errors")
                                .SetPositiveButton(Context.Resources.GetString(Resource.String.OK), (senderAlert, args) => { GCTContext.CurrentCampaign.ClearLoadingMessages(); })
                               .Show();                       
                    }

                    // Add party
                    campaign.AddParty(partyNameEdit.Text);

                    GCTContext.CampaignCollection.Campaigns.Add(campaign);

                    // set current campaign
                    if(!GCTContext.CampaignCollection.SetCurrentCampaign(campaign.CampaignData.Id))
                    {
                        Toast.MakeText(Context, "Error on loading campaign.", ToastLength.Long).Show();
                    }

                    CampaignSelected();

                    return true;
                }
            }

            return false;
        }
    }
}