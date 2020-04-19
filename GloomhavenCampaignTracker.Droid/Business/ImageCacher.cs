using System;
using System.Drawing;
using System.IO;
using Android.OS;

namespace GloomhavenCampaignTracker.Droid.Business
{
    public class ImageCacher
    {
        private ImageCacher _instance;

        private ImageCacher()
        {
            _instance = new ImageCacher();
        }

        public ImageCacher Instance
        {
            get
            {
                if (_instance is null) {
                    _instance = new ImageCacher();
                }
                return _instance;
            }
        }

        internal string ImageCacheFileBasePath
        {
            get
            {
                // Just use whatever directory SpecialFolder.Personal returns
                var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var path = Path.Combine(libraryPath, "/ImagesCache");
                return path;
            }
        }

        internal Bitmap GetAbilityImage(string abilityname)
        {


            return null;
        }

    }
}