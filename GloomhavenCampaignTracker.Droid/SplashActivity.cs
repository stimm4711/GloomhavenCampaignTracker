using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Fragments.campaign;
using GloomhavenCampaignTracker.Shared;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using Fragment = Android.Support.V4.App.Fragment;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Calligraphy;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Support.V4.View;
using Android.Text.Method;
using GloomhavenCampaignTracker.Droid.Fragments;
using GloomhavenCampaignTracker.Droid.CustomControls;
using System.Linq;
using GloomhavenCampaignTracker.Droid.Exceptions;
using System.Text;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using Android.Util;
using System.Threading.Tasks;

namespace GloomhavenCampaignTracker.Droid
{
    [Activity(Label = "Gloomhaven Campaign Tracker", MainLauncher = true, Icon = "@drawable/ic_launcher", AlwaysRetainTaskState = true, Theme = "@style/MyTheme.Splash")]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { DoStartupWork(); });
            startupWork.Start();
        }

        async void DoStartupWork()
        {
            await Task.Run(()=>GloomhavenDbHelper.InitDb());
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        public override void OnBackPressed()
        {
            
        }
    }
}

