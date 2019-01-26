using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Data.ViewEntities;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using ListFragment = Android.Support.V4.App.ListFragment;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class PartySelectionFragment : ListFragment
    {
        private const string lastloadedparty = "lastloadedparty";
        private View _view;

        public static PartySelectionFragment NewInstance()
        {
            return new PartySelectionFragment { Arguments = new Bundle() };
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            FillList();
        }

        private void FillList()
        {
            List<DL_VIEW_CampaignParties> parties = DataServiceCollection.CampaignDataService.GetParties(GCTContext.CurrentCampaign.CampaignData.Id);
            ListAdapter = new PartyAdapter(Context, parties);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.listview_floatingactionbutton, container, false);
            var fab = _view.FindViewById<FloatingActionButton>(Resource.Id.fab);

            if (!fab.HasOnClickListeners)
            {
                fab.Click += (sender, e) =>
                {
                    AddNewPartyDialog();                   
                };
            }

            return _view;
        }

        private void PartySelected()
        {
            var prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
            var editor = prefs.Edit();
            editor.PutInt(lastloadedparty, GCTContext.CurrentCampaign.CurrentParty.Id);
            editor.Apply();

            if (Activity.GetType() == typeof(DetailsActivity))
            {
                Activity.Finish();
            }
        }

        public override void OnListItemClick(ListView lValue, View vValue, int position, long id)
        {
            base.OnListItemClick(lValue, vValue, position, id);
            var selectedParty = ((PartyAdapter)ListAdapter).GetParty(position);
            GCTContext.CurrentCampaign.SetCurrentParty(selectedParty.PartyId);
            PartySelected();
        }      
        
        private void AddNewPartyDialog()
        {
            var inflater = Activity.LayoutInflater;
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_addparty, null);

            var dialog = new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle(Context.Resources.GetString(Resource.String.AddParty))
                .SetCancelable(false)  
                .SetPositiveButton(Context.Resources.GetString(Resource.String.Add), (senderAlert, args) => { })
                .SetNegativeButton(base.Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();

            var dialogPositiveButton = dialog.GetButton((int)DialogButtonType.Positive);
            var dialogNegativeButton = dialog.GetButton((int)DialogButtonType.Negative);

            dialogPositiveButton.Click += (senderAlert, args) =>
            {
                var partyNameEdit = convertView.FindViewById<EditText>(Resource.Id.partyname);
                if (string.IsNullOrEmpty(partyNameEdit.Text))
                {
                    // empty party name
                    Snackbar.Make(convertView, Resources.GetString(Resource.String.PartyNameMissing), Snackbar.LengthShort).Show();
                }
                else if (GCTContext.CurrentCampaign.CampaignData.Parties.Any(x => x.Name.ToLower().Equals(partyNameEdit.Text.ToLower())))
                {
                    // existing party name 
                    Snackbar.Make(convertView, Resources.GetString(Resource.String.PartynameAlreadyExists), Snackbar.LengthShort).Show();
                }
                else
                {
                    GCTContext.CurrentCampaign.AddParty(partyNameEdit.Text);
                    PartySelected();
                    dialog.Dismiss();
                }
            };

            dialogNegativeButton.Click += (senderAlert, args) => { dialog.Dismiss(); };            
        }     
    }
}