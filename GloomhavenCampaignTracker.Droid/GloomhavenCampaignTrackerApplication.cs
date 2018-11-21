using Android.App;
using Android.Preferences;
using Android.Runtime;
using Calligraphy;
using System;

namespace GloomhavenCampaignTracker.Droid
{
    [Application]
    public class GloomhavenCampaignTrackerApplication : Application
    {
        private const string enableGloomFont = "enableGloomFont";

        public GloomhavenCampaignTrackerApplication(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();

            //var isEnableFonts = true;

            //try
            //{
            //    var prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
            //    isEnableFonts = prefs.GetBoolean(enableGloomFont, true);
            //}
            //catch
            //{
            //    isEnableFonts = true;
            //}          
            
            //if(isEnableFonts)
            //{
                // initalize Calligraphy
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    CalligraphyConfig.InitDefault(
                        new CalligraphyConfig.Builder()
                            .SetDefaultFontPath("fonts/PirataOne_Gloomhaven.ttf")
                            .SetFontAttrId(Resource.Attribute.fontPath)                                
                            .Build());
                }
            //}                  
        }
    }

}