using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using System;
using System.Linq;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using GloomhavenCampaignTracker.Droid.Adapter;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Droid
{
    [Activity(Label = "BackupActivity")]
    public class BackupActivity : AppCompatActivity
    {
        private string _backupfilepath = "ghcampaigntracker/backup/";
        private ListView _listviewbackups;
        private SelectableBackupAdapter _listviewbackupadapter;
        private Java.IO.File _sd;
        private bool _hasValidFilePath;
        private EditText _filepath;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_backup);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Database Backups";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _listviewbackups = FindViewById<ListView>(Resource.Id.listviewbackups);
            _filepath = FindViewById<EditText>(Resource.Id.textViewbackuppath);
            var selectbackuppathbutton = FindViewById<Button>(Resource.Id.button_select_bu_path);
            var createbackupbutton = FindViewById<Button>(Resource.Id.button_create_backup);
            var restorebackupbutton = FindViewById<Button>(Resource.Id.button_restore_backup);

            _listviewbackups.ItemsCanFocus = true;
            _listviewbackups.ChoiceMode = ChoiceMode.Single;

            _hasValidFilePath = GetFilePath();           

            SetListviewAdapter();

            if (!createbackupbutton.HasOnClickListeners)
            {
                createbackupbutton.Click += Createbackupbutton_Click;
            }

            if (!restorebackupbutton.HasOnClickListeners)
            {
                restorebackupbutton.Click += Restorebackupbutton_Click; ;
            }

            if (!selectbackuppathbutton.HasOnClickListeners)
            {
                selectbackuppathbutton.Click += Selectbackuppathbutton_Click; ; ;
            }
               
        }

        private Boolean GetFilePath()
        {
            if (GetExternalStorageWritePermission())
            {
                _sd = Android.OS.Environment.ExternalStorageDirectory;

                if (Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState))
                {                    
                    var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                    _backupfilepath = prefs.GetString("Backuppath", $"{_sd}/ghcampaigntracker/backup/");
                    _filepath.Text = _backupfilepath;
                    return true;
                }
            }
            return false;
        }

        private void SetListviewAdapter()
        {
            var files = GetBackupFiles();

            var listitems = new List<BackupListItem>();
            foreach (var file in files.OrderBy(x=>x.Name))
            {
                listitems.Add(new BackupListItem() { BackupFile = file });
            }             

            _listviewbackupadapter = new SelectableBackupAdapter(this, listitems);
            _listviewbackups.Adapter = _listviewbackupadapter;
        }

        private void Selectbackuppathbutton_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Ignore this button. Just for now. Please.", ToastLength.Short).Show();
        }

        private void Restorebackupbutton_Click(object sender, System.EventArgs e)
        {
            if (GetExternalStorageReadPermission())
            {
                var sd = Android.OS.Environment.ExternalStorageDirectory;

                if (Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState))
                {  
                    var dbfile = _listviewbackupadapter.GetSelected();

                    if (dbfile == null) return;
                    
                    BackupConfirmDialog(sd, dbfile.BackupFile);

                }
            }
        }

        private Java.IO.File[] GetBackupFiles()
        {
            if(_hasValidFilePath)
            {
                var path = new Java.IO.File(_backupfilepath);
                if (!path.Exists()) return new Java.IO.File[0];
                return path.ListFiles().Where(x => x != null && x.IsFile).ToArray();
            }

            return new Java.IO.File[] { };
        }

        private void BackupConfirmDialog(IDisposable sd, Java.IO.File dbfile)
        {
            new CustomDialogBuilder(this, Resource.Style.MyDialogTheme)
                .SetTitle("Confirm Restore")
                .SetMessage("WARNING: This will replace the current database. All changes will be lost!")
                .SetPositiveButton("Confirm", (sender, a) =>
                {
                    if (BackupHandler.RestoreBackup(sd, dbfile))
                    {
                        Toast.MakeText(this, "Databasebackup restored! Application will restart now", ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "Database import falied!", ToastLength.Short).Show();
                    }

                    var i = BaseContext.PackageManager.GetLaunchIntentForPackage(BaseContext.PackageName);
                    i.AddFlags(ActivityFlags.ClearTop);
                    StartActivity(i);
                })
                .SetNegativeButton("Cancel", (sender, a) => { })
                .Show();
        }

        private void Createbackupbutton_Click(object sender, System.EventArgs e)
        {
            if (GetExternalStorageWritePermission())
            {
                var buname = "";
                var success = BackupHandler.CreateDBBackup(ref buname);
                if (success.HasValue)
                {
                    if (success.Value)
                    {
                        Toast.
                        MakeText(this, $"Databasebackup {buname} created!", ToastLength.Long).
                        Show();
                        SetListviewAdapter();
                    }
                    else
                    {
                        Toast.
                        MakeText(this, "Databasebackup failed!", ToastLength.Long).
                        Show();
                    }
                }
            }
        }

        private bool GetExternalStorageReadPermission()
        {
            var hasPermission2 = (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == Android.Content.PM.Permission.Granted);
            if (!hasPermission2)
            {
                ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.ReadExternalStorage }, 521);
            }

            return (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == Android.Content.PM.Permission.Granted);
        }

        private bool GetExternalStorageWritePermission()
        {
            var hasPermission = (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Android.Content.PM.Permission.Granted);
            if (!hasPermission)
            {
                ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.WriteExternalStorage }, 521);
            }

            return (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Android.Content.PM.Permission.Granted);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var id = item.ItemId;
            if (id != Android.Resource.Id.Home) return base.OnOptionsItemSelected(item);
            Finish();
            return true;
        }
    }
}