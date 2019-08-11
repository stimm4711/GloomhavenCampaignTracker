using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Fragments.campaign;
using GloomhavenCampaignTracker.Business;
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

namespace GloomhavenCampaignTracker.Droid
{
    [Activity(Label = "Gloomhaven Campaign Tracker", Icon = "@drawable/ic_launcher", AlwaysRetainTaskState = true)]
    public class MainActivity : AppCompatActivity
    {
        private DrawerLayout _drawerLayout;
        private NavigationView _navigationView;
        private const string LastLoadedCampaign = "lastloadedcampaign";
        private const string LastloadedParty = "lastloadedparty";
        private const string SelectedFragId = "selected_fragid";
        private Fragment _currentFragment;
        private const string _showItemnames = "showItemnames";
        private const string _showPQNames = "showPQDetails";
        private const string _showOldAbilitySheet = "showOldAbilitySheet";
        private const string _activateFC = "activateFC";

        protected override void OnResume()
        {
            try
            {
                if (GCTContext.CampaignCollection?.CurrentCampaign != null)
                {
                    ShowCampaign();
                }
                else
                {
                    if (_currentFragment is CampaignViewPagerFragmentTabs)
                    {
                        SupportFragmentManager.BeginTransaction().Remove(_currentFragment).Commit();
                        SupportActionBar.Title = Resources.GetString(Resource.String.ApplicationName);
                    }
                }
            }
            catch (CampaignLoadingException ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Error on resuming application", ToastLength.Short).Show();
                GCTContext.CampaignCollection?.SetCurrentCampaign(-1);
            }

            base.OnResume();
        }

        protected override void AttachBaseContext(Context context)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(context));
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = Resources.GetString(Resource.String.ApplicationName);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawerLayout);
            _navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            var drawerToggle = new Android.Support.V7.App.ActionBarDrawerToggle(this, _drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            _drawerLayout.AddDrawerListener(drawerToggle);
            drawerToggle.SyncState();
                        
            try
            {
                LoadLastCampaign();
            }
            catch(Exception ex)
            {
                GCTContext.CampaignCollection.SetCurrentCampaign(-1);
                new CustomDialogBuilder(this, Resource.Style.MyDialogTheme)
                    .SetTitle("Load last campaign error")
                    .SetMessage("Can't load the last opened campaign. Please try to open the campaign from the campaign selection. If that doesn't work please select crash & report to send informations.")
                    .SetPositiveButton("Crash & Report", (senderAlert, args) =>
                    {
                        throw ex;
                    })
                     .SetNegativeButton("Cancel", (senderAlert, args) => { })
                    .Show();                
            }           

            var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            var isShowItems = prefs.GetBoolean(_showItemnames, true);
            var isShowPq = prefs.GetBoolean(_showPQNames, true);
            var isShowOldAbilitySheet = prefs.GetBoolean(_showOldAbilitySheet, false);
            var isFCActivated = prefs.GetBoolean(_activateFC, false);

            GCTContext.ShowItemNames = isShowItems;
            GCTContext.ShowPersonalQuestDetails = isShowPq;
            GCTContext.ShowAbilitySheet = isShowOldAbilitySheet;
            GCTContext.ActivateForgottenCiclesContent = isFCActivated;

            _navigationView.NavigationItemSelected += NavigationItemSelected;

            _drawerLayout.CloseDrawers();
        }

        private void LoadLastCampaign()
        {
            try
            {
                if (GCTContext.CampaignCollection.CurrentCampaign == null)
                {
                    var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                    var currentCampaignId = prefs.GetInt(LastLoadedCampaign, -1);
                    if (currentCampaignId > 0)
                    {
                        var campaigns = DataServiceCollection.CampaignDataService.GetCampaignsFlat();
                        if(campaigns.Any(x=>x.Id == currentCampaignId))
                        {
                            if (!GCTContext.CampaignCollection.SetCurrentCampaign(currentCampaignId))
                            {
                                Toast.MakeText(this, "Error on loading campaign.", ToastLength.Long).Show();
                            }
                            else
                            {
                                var currentPartyId = prefs.GetInt(LastloadedParty, -1);
                                if (GCTContext.CurrentCampaign != null && currentPartyId > 0) GCTContext.CurrentCampaign.SetCurrentParty(currentPartyId);
                            }
                        }
                        else
                        {
                            Toast.MakeText(this, "Last opened campaign does not exists in database", ToastLength.Long).Show();
                            GCTContext.CampaignCollection.SetCurrentCampaign(-1);
                        }                        
                    }
                }

                ShowCampaign();
            }
            catch(CampaignLoadingException ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        public void SetActionBarTitle(string title)
        {
            SupportActionBar.Title = title;
        }

        #region "Navigation"

        private void NavigationItemSelected(object senderNav, NavigationView.NavigationItemSelectedEventArgs eNav)
        {
            switch (eNav.MenuItem.ItemId)
            {
                case Resource.Id.nav_campaign:
                    if (GCTContext.CurrentCampaign != null)
                    {
                        ShowCampaign();
                    }
                    else
                    {
                        var intent = new Intent(this, typeof(DetailsActivity));
                        intent.PutExtra(SelectedFragId, (int)DetailFragmentTypes.CampaignSelection);
                        StartActivity(intent);
                    }

                    _drawerLayout.CloseDrawers();
                    eNav.MenuItem.SetChecked(true);
                    break;
                case Resource.Id.nav_characters:
                    SupportActionBar.Title = "Characters";
                    _currentFragment = CharacterListFragment.NewInstance(false);
                    ShowFragment();
                    _drawerLayout.CloseDrawers();
                    eNav.MenuItem.SetChecked(true);
                    break;                
                case Resource.Id.nav_backup:
                    var backupactivityIntent = new Intent(this, typeof(BackupActivity));
                    StartActivity(backupactivityIntent);
                    _drawerLayout.CloseDrawers();
                    eNav.MenuItem.SetChecked(true);
                    break;
                case Resource.Id.nav_sharewifi:
                    var shareDaraIntent = new Intent(this, typeof(ShareDataActivity));
                    StartActivity(shareDaraIntent);
                    _drawerLayout.CloseDrawers();
                    eNav.MenuItem.SetChecked(true);
                    break;
                case Resource.Id.nav_about:
                    var convertView = LayoutInflater.Inflate(Resource.Layout.dialogfragment_about, null);
                    var cephaloTxt = convertView.FindViewById<TextView>(Resource.Id.cephalofairTxt);                    
                    var icons8Txt = convertView.FindViewById<TextView>(Resource.Id.icons8Txt);           

                    cephaloTxt.MovementMethod = LinkMovementMethod.Instance;                  
                    icons8Txt.MovementMethod = LinkMovementMethod.Instance;

                    new CustomDialogBuilder(this, Resource.Style.MyDialogTheme)
                        .SetCustomView(convertView)
                        .Show();
                    break;

                case Resource.Id.nav_settings:
                    var settigsintent = new Intent(this, typeof(DetailsActivity));
                    settigsintent.PutExtra(SelectedFragId, (int)DetailFragmentTypes.Settings);
                    StartActivity(settigsintent);  
                    _drawerLayout.CloseDrawers();
                    eNav.MenuItem.SetChecked(true);
                    break;
                case Resource.Id.nav_releasenotes:
                    var releasenotesintent = new Intent(this, typeof(DetailsActivity));
                    releasenotesintent.PutExtra(SelectedFragId, (int)DetailFragmentTypes.Releasenotes);
                    StartActivity(releasenotesintent);
                    _drawerLayout.CloseDrawers();
                    eNav.MenuItem.SetChecked(true);
                    break;
                case Resource.Id.nav_support:
                    var supportintent = new Intent(this, typeof(DetailsActivity));
                    supportintent.PutExtra(SelectedFragId, (int)DetailFragmentTypes.Support);
                    StartActivity(supportintent);
                    _drawerLayout.CloseDrawers();
                    eNav.MenuItem.SetChecked(true);
                    break;
            }
        }            

        public override bool OnOptionsItemSelected(IMenuItem item)
        {           
            if (item.ItemId != Android.Resource.Id.Home) return base.OnOptionsItemSelected(item);
            _drawerLayout.OpenDrawer(GravityCompat.Start);
            return true;
        }

        private void ShowCampaign()
        {
            if (GCTContext.CurrentCampaign == null) return;

            var messages = GCTContext.CurrentCampaign.GetLoadingMessages();
            if (messages.Any())
            {
                var sb = new StringBuilder();
                foreach (var message in messages)
                {
                    sb.Append(message);
                    sb.Append("\n");
                }

                new CustomDialogBuilder(this, Resource.Style.MyDialogTheme)
                       .SetMessage(sb.ToString())
                       .SetTitle("Campaign loading errors")
                        .SetPositiveButton(Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                        {
                            GCTContext.CurrentCampaign.ClearLoadingMessages();
                            _currentFragment = new CampaignViewPagerFragmentTabs();
                            ShowFragment();
                        })
                       .Show();                
            }
            else
            {
                _currentFragment = new CampaignViewPagerFragmentTabs();
                ShowFragment();
            }         

            var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            var editor = prefs.Edit();
            editor.PutInt(LastLoadedCampaign, GCTContext.CurrentCampaign.CampaignData.Id);
            if (GCTContext.CurrentCampaign.CurrentParty != null) editor.PutInt(LastloadedParty, GCTContext.CurrentCampaign.CurrentParty.Id);
            editor.Apply();
        }

        private void ShowFragment()
        {
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.frame_container, _currentFragment).Commit();
        }

        #endregion

        }
    }

