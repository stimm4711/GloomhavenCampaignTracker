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
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Java.IO;

namespace GloomhavenCampaignTracker.Droid
{
    [Activity(Label = "BackupActivity")]
    public class BackupActivity : AppCompatActivity
    {
        private string _backupfilepath = "ghcampaigntracker/backup/";
        private ListView _listviewbackups;
        //private SelectableBackupAdapter _listviewbackupadapter;
        private BackupAdapter _listviewbackupadapter;
        private File _sd;
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
            SupportActionBar.Title = Resources.GetString(Resource.String.DatabaseBackupsTitle);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _listviewbackups = FindViewById<ListView>(Resource.Id.listviewbackups);
            _filepath = FindViewById<EditText>(Resource.Id.textViewbackuppath);
            var selectbackuppathbutton = FindViewById<Button>(Resource.Id.button_select_bu_path);
            var createbackupbutton = FindViewById<Button>(Resource.Id.button_create_backup);
            var loadbackupbutton = FindViewById<Button>(Resource.Id.button_load_backup);

            _listviewbackups.ItemsCanFocus = true;
            _listviewbackups.ChoiceMode = ChoiceMode.Single;

            _hasValidFilePath = GetFilePath();           

            SetListviewAdapter();

            if (!createbackupbutton.HasOnClickListeners)
            {
                createbackupbutton.Click += Createbackupbutton_Click;
            }

            if (!loadbackupbutton.HasOnClickListeners)
            {
                loadbackupbutton.Click += Loadbackupbutton_Click; ;
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

        /// <summary>
        /// Get files in Backuppath, fill BackupAdapter and assign it to listview
        /// </summary>
        private void SetListviewAdapter()
        {
            var files = GetBackupFiles();

            var listitems = new List<BackupListItem>();
            foreach (var file in files.OrderByDescending(x=>x.Name))
            {
                listitems.Add(new BackupListItem() { BackupFile = file });
            }
            
            _listviewbackupadapter = new BackupAdapter(this, listitems);
            _listviewbackupadapter.RestartApplication += _listviewbackupadapter_RestartApplication;

            _listviewbackups.Adapter = _listviewbackupadapter;          
        }

        private void _listviewbackupadapter_RestartApplication(object sender, EventArgs e)
        {
            RestartApp();
        }

        private void Selectbackuppathbutton_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Ignore this button. Just for now. Please.", ToastLength.Short).Show();
        }

        private void Loadbackupbutton_Click(object sender, System.EventArgs e)
        {
            PickFileAsync();              
        }

        /// <summary>
        /// Pick a file and copy it to the backup path
        /// </summary>
        private async void PickFileAsync()
        {
            try
            {
                FileData fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                    return; // user canceled file picking

                if (GetExternalStorageWritePermission())
                {
                    var filename = BackupHandler.CopyFileToBackupStorage(fileData);
                    var file = new File(filename);
                    if (file == null) return;

                }

                SetListviewAdapter();

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception choosing file: " + ex.ToString());
            }
        }
        

        private File[] GetBackupFiles()
        {
            if(_hasValidFilePath)
            {
                var path = new File(_backupfilepath);
                if (!path.Exists()) return new File[0];
                return path.ListFiles().Where(x => x != null && x.IsFile).ToArray();
            }

            return new File[] { };
        }

        private void RestartApp()
        {
            var i = BaseContext.PackageManager.GetLaunchIntentForPackage(BaseContext.PackageName);
            i.AddFlags(ActivityFlags.ClearTop);
            StartActivity(i);
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