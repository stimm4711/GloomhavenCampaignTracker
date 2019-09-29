using System;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class SzenarioDetailsFragment : CampaignDetailsFragmentBase
    {
        private CampaignUnlockedScenario _campaignScenario;
        private TextView _scenarioname;
        private TextView _scenarionumber;
        private CheckBox _scenariostatus;
        private ProgressBar _progBarLoadingTreasure;
        private GridView _grid;

        internal static SzenarioDetailsFragment NewInstance(int scenarioId)
        {
            var frag = new SzenarioDetailsFragment { Arguments = new Bundle() };
            frag.Arguments.PutInt(DetailsActivity.SelectedScenarioId, scenarioId);
            return frag;
        }

        private CampaignUnlockedScenario GetUnlockedScenario()
        {
            if (Arguments == null) return null;

            var id = Arguments.GetInt(DetailsActivity.SelectedScenarioId, 0);
            if (id <= 0) return null;

            var campScenario = CampaignUnlockedScenarioRepository.Get(id, recursive: true);

            return new CampaignUnlockedScenario(campScenario);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_scenariodetails_details, container, false);
            _scenarioname = _view.FindViewById<TextView>(Resource.Id.scenarionametextview);
            _scenarionumber = _view.FindViewById<TextView>(Resource.Id.scenarionumbertextview);
            _scenariostatus = _view.FindViewById<CheckBox>(Resource.Id.scenariostatuscheckbox);
            _grid = _view.FindViewById<GridView>(Resource.Id.imagesGridView);
            _progBarLoadingTreasure = _view.FindViewById<ProgressBar>(Resource.Id.pbSpinningCircle);

            _campaignScenario = GetUnlockedScenario();

            if (_campaignScenario != null)
            {
                if (_campaignScenario.Scenario != null)
                {
                    _scenarioname.Text = _campaignScenario.ScenarioName;
                    _scenarionumber.Text = $"# {_campaignScenario.Scenarionumber}";
                    _scenariostatus.Checked = _campaignScenario.Completed;
                }
            }

            UpdateRequirementsListView();

            SetBackgroundOfTopLayout();

            _grid.Adapter = new CampaignScenarioTreasureAdapter(Context, _campaignScenario.UnlockedScenarioData);

            return _view;
        }

        private void UpdateRequirementsListView()
        {
            var lv_requirements = _view.FindViewById<ListView>(Resource.Id.lv_requirements);

            var adapter = new ArrayAdapter<string>(Context, Resource.Layout.listviewitem_singleitem, _campaignScenario.GetAllRequirements());

            lv_requirements.Adapter = adapter;
        }

        private void SetBackgroundOfTopLayout()
        {
            var enableCheckbox = true;
            var color = ContextCompat.GetColor(Context, Resource.Color.gloom_primaryLighter);
            if (_campaignScenario.IsBlocked())
            {
                enableCheckbox = false;
                color = ContextCompat.GetColor(Context, Resource.Color.gloom_scenarioBlockedItemBackground);
            }
            else if (!_campaignScenario.Completed && !_campaignScenario.IsAvailable())
            {
                enableCheckbox = false;
                color = ContextCompat.GetColor(Context, Resource.Color.gloom_scenarioUnavailableItemBackground);
            }
            else if (_campaignScenario.Completed)
            {
                color = ContextCompat.GetColor(Context, Resource.Color.gloom_scenarioCompletedItemBackground);
            }

            var layoutTop = _view.FindViewById<RelativeLayout>(Resource.Id.layout_top);
            layoutTop.SetBackgroundColor(new Color(color));

            var chk = _view.FindViewById<CheckBox>(Resource.Id.scenariostatuscheckbox);
            chk.Enabled = enableCheckbox;
        }
    }
}