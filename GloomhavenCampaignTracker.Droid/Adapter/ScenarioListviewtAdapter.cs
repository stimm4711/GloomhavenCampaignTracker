using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class ScenarioListviewtAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<DL_Scenario> _scenarios;

        public override int Count => _scenarios.Count;

        public ScenarioListviewtAdapter(Context context, List<DL_Scenario> scenarios ) 
        {
            _context = context;
            _scenarios = scenarios;            
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
            var scenario = _scenarios[position];
            var view = inflater.Inflate(Resource.Layout.listviewitem_campaignscenario_numberAndName, null);

            var scenarionumberText = (TextView)view.FindViewById(Resource.Id.scenariViewItem_numberTextView);
            scenarionumberText.Text = $"#{scenario.Scenarionumber.ToString()}";

            var scenarionameText = (TextView)view.FindViewById(Resource.Id.scenariViewItem_nameTextView);
            scenarionameText.Text = scenario.Name;           

            return view;
        }       

        public override Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
    }
}