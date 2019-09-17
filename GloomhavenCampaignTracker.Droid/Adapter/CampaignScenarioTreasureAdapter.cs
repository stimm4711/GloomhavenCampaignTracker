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

        public override View GetView(int position, View convertView, ViewGroup parent)
        {             
            ImageView imageView;
            if (convertView == null)
            {
                // if it's not recycled, initialize some attributes
                float scale = _context.Resources.DisplayMetrics.Density;
                int pixelsHeight = (int)(164 * scale + 0.5f);
                int pixelsWidth = (int)(110 * scale + 0.5f);
                imageView = new ImageView(_context) { LayoutParameters = new AbsListView.LayoutParams(pixelsWidth, pixelsHeight) };
                // fixed image size
                imageView.SetScaleType(ImageView.ScaleType.FitCenter); // fit cell
            }
            else
            {
                imageView = (ImageView)convertView;
            }

            var t = _scenarioTreasures[position];

            if (CrossConnectivity.Current.IsConnected)
            {
                var side = t.Looted ? "front" : "back" ;
                var number = t.ScenarioTreasure.TreasureNumber < 10 ? $"0{t.ScenarioTreasure.TreasureNumber}" : $"{t.ScenarioTreasure.TreasureNumber}";
                var treasurename = $"{side}-{number}.jpg";
                var url = "https://raw.githubusercontent.com/stimm4711/gloomhaven/master/images/treasure-chests/" + $"{treasurename}";
                var imageBitmap = GetImageBitmapFromUrlAsync(url, imageView);
            }

            if (!imageView.HasOnClickListeners)
            {
                imageView.Click += (sender, e) =>
                {
                    t.Looted = !t.Looted;

                    CampaignUnlockedScenarioRepository.InsertOrReplace(_campaignScenario);

                    var side = t.Looted ? "front" : "back";
                    var number = t.ScenarioTreasure.TreasureNumber < 10 ? $"0{t.ScenarioTreasure.TreasureNumber}" : $"{t.ScenarioTreasure.TreasureNumber}";
                    var treasurename = $"{side}-{number}.jpg";
                    var url = "https://raw.githubusercontent.com/stimm4711/gloomhaven/master/images/treasure-chests/" + $"{treasurename}";
                    var imageBitmap = GetImageBitmapFromUrlAsync(url, imageView);
                };
            }
            return imageView;
        }

        private async Task<Bitmap> GetImageBitmapFromUrlAsync(string url, ImageView imagen)
        {
            Bitmap imageBitmap = null;

            using (var httpClient = new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            imagen.SetImageBitmap(imageBitmap);

            NotifyDataSetChanged();
            return imageBitmap;
        }

        public override int Count => _scenarioTreasures.Count;
    }

    internal class ScenarioTreasurHolder : Java.Lang.Object
    {
        public ImageView Image { get; set; }
    }
}