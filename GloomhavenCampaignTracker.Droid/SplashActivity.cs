using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using Android.Util;
using System.Threading.Tasks;
using Data;

namespace GloomhavenCampaignTracker.Droid
{
    [Activity(Label = "Campaign Tracker", MainLauncher = true, Icon = "@drawable/ic_launcher",  Theme = "@style/MyTheme.Splash", NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;
        private TextView _loadingDetails;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_splash);
            FindViewById<TextView>(Resource.Id.txtAppVersion).Text = $"Version {PackageManager.GetPackageInfo(PackageName, 0).VersionName}";
            _loadingDetails = FindViewById<TextView>(Resource.Id.txtLoadingDetails);
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
            GloomhavenDbHelper.UpdateLoadingStep += UpdateLoadingStep;
            DatabaseUpdateHelper.UpdateLoadingStep += UpdateLoadingStep;

            await Task.Run(()=>GloomhavenDbHelper.InitDb());
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        private void UpdateLoadingStep(object sender, UpdateSplashScreenLoadingInfoEVentArgs e)
        {
            RunOnUiThread(() =>
            {
                _loadingDetails.Text = e.Stepname;
            });           
        }

        public override void OnBackPressed()
        {
            
        }
    }
}

