using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using Java.Lang;
using Plugin.Connectivity;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    /// <summary>
    /// Displays the character class icons
    /// </summary>
    internal class CampaignScenarioTreasureAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<DL_CampaignScenarioTreasure> _scenarioTreasures;
        private readonly DL_CampaignUnlockedScenario _campaignScenario;

        public CampaignScenarioTreasureAdapter(Context context, DL_CampaignUnlockedScenario campaignScenario)
        {
            _context = context;
            _scenarioTreasures = campaignScenario.CampaignScenarioTreasures;
            _campaignScenario = campaignScenario;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return _scenarioTreasures[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        internal class TreasureViewHolder : Object
        {
            public ProgressBar ProgBar { get; set; }          
            public ImageView TreasureImageView { get; set; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            TreasureViewHolder holder = null;

            if (convertView != null)
                holder = convertView.Tag as TreasureViewHolder;

            var t = _scenarioTreasures[position];

            if (holder == null)
            {
                holder = new TreasureViewHolder();

                var inflater = _context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                convertView = inflater.Inflate(Resource.Layout.item_view2, parent, false);
                holder.ProgBar = convertView.FindViewById<ProgressBar>(Resource.Id.loadingPanel);
                holder.TreasureImageView = convertView.FindViewById<ImageView>(Resource.Id.itemimage);
                //// if it's not recycled, initialize some attributes
                float scale = _context.Resources.DisplayMetrics.Density;
                int pixelsHeight = (int)(164 * scale + 0.5f);
                int pixelsWidth = (int)(110 * scale + 0.5f);
                holder.TreasureImageView.LayoutParameters = new RelativeLayout.LayoutParams(pixelsWidth, pixelsHeight); //new ImageView(_context) { LayoutParameters = new AbsListView.LayoutParams(pixelsWidth, pixelsHeight) };
                //// fixed image size
                holder.TreasureImageView.SetScaleType(ImageView.ScaleType.FitCenter); // fit cell
                convertView.Tag = holder;
            }

            if (CrossConnectivity.Current.IsConnected)
            {
                holder.ProgBar.Visibility = ViewStates.Visible;

                var side = t.Looted ? "front" : "back";
                var number = t.ScenarioTreasure.TreasureNumber < 10 ? $"0{t.ScenarioTreasure.TreasureNumber}" : $"{t.ScenarioTreasure.TreasureNumber}";
                var treasurename = $"{side}-{number}.jpg";
                var url = Helper.GetTreasureUrl(treasurename); 

                GetImageBitmapFromUrlAsync(url, holder.TreasureImageView, holder.ProgBar);
            }

            if (!holder.TreasureImageView.HasOnClickListeners)
            {
                holder.TreasureImageView.Click += (sender, e) =>
                {
                    t.Looted = !t.Looted;

                    CampaignUnlockedScenarioRepository.InsertOrReplace(_campaignScenario);

                    holder.TreasureImageView.SetImageDrawable(null);
                    holder.ProgBar.Visibility = ViewStates.Visible;

                    var side = t.Looted ? "front" : "back";
                    var number = t.ScenarioTreasure.TreasureNumber < 10 ? $"0{t.ScenarioTreasure.TreasureNumber}" : $"{t.ScenarioTreasure.TreasureNumber}";
                    var treasurename = $"{side}-{number}.jpg";
                    var url = Helper.GetTreasureUrl(treasurename);

                    GetImageBitmapFromUrlAsync(url, holder.TreasureImageView, holder.ProgBar);
                };
            }
            return convertView;
        }

        private async void GetImageBitmapFromUrlAsync(string url, ImageView imagen, View view)
        {
            var image = await Helper.GetImageBitmapFromUrlAsync(url);
            imagen.SetImageBitmap(image);
            ((ProgressBar)view).Visibility = ViewStates.Gone;                 
        }

        public override int Count => _scenarioTreasures.Count;
    }

    internal class ScenarioTreasurHolder : Java.Lang.Object
    {
        public ImageView Image { get; set; }
    }
}