using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign.party
{
    public class CampaignPartyNotesFragment : CampaignDetailsFragmentBase
    {
        private EditText _partynotes;

        public static CampaignPartyNotesFragment NewInstance()
        {
            var frag = new CampaignPartyNotesFragment { Arguments = new Bundle() };
            return frag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {            
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_party_notes, container, false);
            _partynotes = _view.FindViewById<EditText>(Resource.Id.partynotestext);

            if (_partynotes != null)
                _partynotes.AfterTextChanged += _partynotes_AfterTextChanged;

            UpdateView();

            return _view;
        }

        private void _partynotes_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            GCTContext.CurrentCampaign.CurrentParty.Notes = _partynotes.Text;
            DataServiceCollection.PartyDataService.InsertOrReplace(GCTContext.CurrentCampaign.CurrentParty);
        }

        internal void UpdateView()
        {
            if (GCTContext.CurrentCampaign.CurrentParty == null) return;
            _partynotes.Text = GCTContext.CurrentCampaign.CurrentParty.Notes;
        }

    }
}