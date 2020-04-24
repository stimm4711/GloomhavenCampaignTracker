using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using System;
using System.Linq;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using GloomhavenCampaignTracker.Droid.Adapter;
using System.Collections.Generic;
using System.IO;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;

namespace GloomhavenCampaignTracker.Droid
{
    [Activity(Label = "BackupActivity")]
    public class BackupActivity : AppCompatActivity
    {
        private string _backupfilepath = "ghcampaigntracker/backup/";
        private ListView _listviewbackups;
        private BackupAdapter _listviewbackupadapter;
        private EditText _filepath;

        private const int LOAD_BACKUPFILE_CODE = 42;

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
            var createbackupbutton = FindViewById<Button>(Resource.Id.button_create_backup);
            var loadbackupbutton = FindViewById<Button>(Resource.Id.button_load_backup);

            _listviewbackups.ItemsCanFocus = true;
            _listviewbackups.ChoiceMode = ChoiceMode.Single;

            _backupfilepath = GetExternalFilesDir(null).AbsolutePath;
            _filepath.Text = _backupfilepath;

            SetListviewAdapter();

            if (!createbackupbutton.HasOnClickListeners)
            {
                createbackupbutton.Click += Createbackupbutton_Click;
            }

            if (!loadbackupbutton.HasOnClickListeners)
            {
                loadbackupbutton.Click += Loadbackupbutton_Click; ;
            }              
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
            var i = BaseContext.PackageManager.GetLaunchIntentForPackage(BaseContext.PackageName);
            i.AddFlags(ActivityFlags.ClearTop);
            StartActivity(i);
        }
                

        private void Loadbackupbutton_Click(object sender, EventArgs e)
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

                var filename = BackupHandler.CopyFileToBackupStorage(fileData, _backupfilepath);
                var file = new Java.IO.File(filename);
                if (file == null) return;

                SetListviewAdapter();

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception choosing file: " + ex.ToString());
            }
        }

        private Java.IO.File[] GetBackupFiles()
        {
            try
            {
                var path = new Java.IO.File(_backupfilepath);
                if (!path.Exists()) return new Java.IO.File[0];
                return path.ListFiles().Where(x => x != null && x.IsFile).ToArray();
            }
            catch
            {
                return new Java.IO.File[0];
            }            
        }

        private void Createbackupbutton_Click(object sender, System.EventArgs e)
        {
            var buname = "";
            var success = BackupHandler.CreateDBBackup(ref buname, _backupfilepath);
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

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var id = item.ItemId;
            if (id != Android.Resource.Id.Home) return base.OnOptionsItemSelected(item);
            Finish();
            return true;
        }
    }
}