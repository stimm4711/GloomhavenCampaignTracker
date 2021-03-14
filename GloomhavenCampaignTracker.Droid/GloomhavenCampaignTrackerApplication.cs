using Android.App;
using Android.Runtime;
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
        }
    }

}