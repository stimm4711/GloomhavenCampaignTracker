using Android.App;
using Android.Runtime;
using Calligraphy;
using System;

namespace GloomhavenCampaignTracker.Droid
{
    [Application]
    public class GloomhavenCampaignTrackerApplication : Application
    {
        public GloomhavenCampaignTrackerApplication(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();

            // initalize Calligraphy
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                CalligraphyConfig.InitDefault(
                    new CalligraphyConfig.Builder()
                        .SetDefaultFontPath("fonts/PirataOne_Gloomhaven.ttf")
                        .SetFontAttrId(Resource.Attribute.fontPath)                                
                        .Build());
            } 
        }
    }

}