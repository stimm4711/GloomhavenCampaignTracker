using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using Java.Lang;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class TreasureAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly CampaignUnlockedScenario _campScenario;
        private readonly ExpandableScenarioListAdapter _campScenarioAdapter;

        public override int Count
        {
            get
            {
                if(_campScenario != null && _campScenario.Treasures != null)
                {
                    return _campScenario.Treasures.Count;
                }
                else
                {
                    return 0;
                }
            }            
        }

        public TreasureAdapter(Context context, CampaignUnlockedScenario campScenario, ExpandableScenarioListAdapter campScenAdapter)
        {
            _context = context;
            _campScenario = campScenario;
            _campScenarioAdapter = campScenAdapter;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
         
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (_campScenario?.Treasures?.Count > position)
            {
                var scenarioTreasure = _campScenario.Treasures[position];

                ScenarioTreasureHolder holder = null;

                if (convertView != null)
                    holder = convertView.Tag as ScenarioTreasureHolder;

                if (holder == null)
                {
                    // Create new holder
                    holder = new ScenarioTreasureHolder();

                    var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                    convertView = inflater.Inflate(Resource.Layout.listviewitem_checkbox, null);
                    holder.TreasureNumber = (TextView)convertView.FindViewById(Resource.Id.treasureTileTextView);
                    holder.TreasureLooted = (CheckBox)convertView.FindViewById(Resource.Id.treasureLootedCheck);
                    holder.TresureContent = (TextView)convertView.FindViewById(Resource.Id.treasureContent);
                    holder.OptionsButton = convertView.FindViewById<Button>(Resource.Id.optionsButton);

                    // Looted CheckCHanged Event
                    holder.TreasureLooted.CheckedChange += (sender, e) =>
                    {
                        var chkBx = (CheckBox)sender;
                        var treasure = (CampaignTreasureWrapper)chkBx.Tag;

                        if (treasure == null || treasure.Treasure.Looted == chkBx.Checked) return;
                        treasure.Treasure.Looted = chkBx.Checked;
                        NotifyDataSetChanged();
                        CampaignUnlockedScenarioRepository.InsertOrReplace(treasure.Treasure.UnlockedScenario);
                        _campScenarioAdapter.NotifyDataSetChanged();
                    };

                    // Options Event
                    if (!holder.OptionsButton.HasOnClickListeners)
                    {
                        holder.OptionsButton.Click += (sender, e) =>
                        {
                            ConfirmDeleteDialog(position);
                        };
                    }

                    // Set Item Click Event
                    convertView.Click += (sender, e) =>
                    {
                        var v = (View)sender;
                        var chb = v.FindViewById<CheckBox>(Resource.Id.treasureLootedCheck);
                        var t = (CampaignTreasureWrapper)chb.Tag;

                        TreasureItemClick(t);
                    };
                }

                // Set Data
                holder.TreasureNumber.Text = $"# {scenarioTreasure.ScenarioTreasure.TreasureNumber}";
                holder.TresureContent.Text = "---";
                holder.TreasureLooted.Tag = new CampaignTreasureWrapper(scenarioTreasure);
                holder.TreasureLooted.Checked = scenarioTreasure.Looted;              
            }

            return convertView;
        }

        private void TreasureItemClick(CampaignTreasureWrapper treasure)
        {
            var inflater = _context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_addtreasure, null);
            var refNumberText = convertView.FindViewById<EditText>(Resource.Id.treasure_ref_number);
            var content = convertView.FindViewById<EditText>(Resource.Id.treasure_content);

            content.Text = treasure.Treasure.ScenarioTreasure.TreasureContent;
            refNumberText.Text = $"{treasure.Treasure.ScenarioTreasure.TreasureNumber}";

            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle(_context.Resources.GetString(Resource.String.EditTreasure))
                .SetPositiveButton(_context.Resources.GetString(Resource.String.Save), (senderAlert, args) =>
                {
                    if (int.TryParse(refNumberText.Text, out int refNumber))
                    {
                        treasure.Treasure.ScenarioTreasure.TreasureNumber = refNumber;
                    }

                    treasure.Treasure.ScenarioTreasure.TreasureContent = content.Text;

                    NotifyDataSetChanged();
                    _campScenario.Save();
                })
                .Show();
        }

        private class ScenarioTreasureHolder : Java.Lang.Object
        {
            public TextView TreasureNumber { get; set; }
            public CheckBox TreasureLooted { get; set; }
            public TextView TresureContent { get; set; }
            public Button OptionsButton { get; set; }
        }

        /// <summary>
        /// Confirm delete
        /// </summary>
        /// <param name="position"></param>
        private void ConfirmDeleteDialog(int position)
        {
            var inflater = _context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
               .SetMessage(_context.Resources.GetString(Resource.String.DeleteTreasure))
                .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
                {
                    _campScenario.Treasures.Remove(_campScenario.Treasures[position]);
                    _campScenario.Save();
                    NotifyDataSetChanged();
                    _campScenarioAdapter.NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        public override Object GetItem(int position)
        {
            return position;
        }
    }
}