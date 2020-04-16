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
using Java.IO;
using System.IO;
using Android.Provider;

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
            try
            {
                // start a file picker
                Intent intent = new Intent(Intent.ActionOpenDocument);
                intent.AddCategory(Intent.CategoryOpenable);
                intent.SetType("application/db3");
                StartActivityForResult(intent, LOAD_BACKUPFILE_CODE);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception choosing file: " + ex.ToString());
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
           Android.Net.Uri currentUri;

            if (requestCode == LOAD_BACKUPFILE_CODE)
            {
                // User picked a file
                if (data != null)
                {
                    // uri of file
                    currentUri = data.Data;

                    try
                    {                
                        // make sure backupfolder exists
                        var backupfolder = new Java.IO.File(_backupfilepath);
                        if (!backupfolder.Exists()) backupfolder.Mkdirs();

                        // get the name of the selected file
                        var returnCursor = ContentResolver.Query(currentUri, null, null, null, null);
                        int nameIndex = returnCursor.GetColumnIndex(OpenableColumns.DisplayName);
                        returnCursor.MoveToFirst();
                        var name = returnCursor.GetString(nameIndex);

                        // create the destination file in the backupfolder
                        var filename = Path.Combine(backupfolder.Path, name);
                        var destFile = new Java.IO.File(filename);     

                        // Get the Filedescriptor and transfer the selected file to the destination file
                        ParcelFileDescriptor pfd = ContentResolver.OpenFileDescriptor(currentUri, "r");
                        FileDescriptor fd = pfd.FileDescriptor;

                        BackupHandler.TransferFileStreams(fd, destFile);
                        var src = new FileInputStream(fd).Channel;
                        var dst = new FileOutputStream(destFile).Channel;
                        dst.TransferFrom(src, 0, src.Size());
                        src.Close();
                        dst.Close();                      

                        pfd.Close();

                        // Update the llist
                        SetListviewAdapter();

                    }
                    catch (Java.IO.IOException e)
                    {
                        // do nothing
                    }
                }
            }

            base.OnActivityResult(requestCode, resultCode, data);
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