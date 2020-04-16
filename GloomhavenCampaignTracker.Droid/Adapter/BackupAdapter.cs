using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Content;
using Android.Provider;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Droid.CustomControls;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class BackupAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<BackupListItem> _items = new List<BackupListItem>();
        public event EventHandler RestartApplication;

        public override int Count => _items.Count;

        public BackupAdapter(Context context, List<BackupListItem> items) 
        {
            _context = context;
            _items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
         
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return GetItemView(position, convertView, parent);
        }

        private View GetItemView(int position, View convertView, ViewGroup parent)
        {           
            Holder holder = null;

            if (convertView != null)
                holder = convertView.Tag as Holder;

            if (holder == null)
            {
                // Create new holder
                holder = new Holder();

                var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.listviewitem_backup, parent, false);
                holder.FileName = convertView.FindViewById<TextView>(Resource.Id.backupnameTextView);
                holder.Share = convertView.FindViewById<ImageView>(Resource.Id.shareButton);
                holder.Delete = convertView.FindViewById<ImageView>(Resource.Id.deleteButton);
                holder.BackupTime = convertView.FindViewById<TextView>(Resource.Id.backupTimeTextView);
                holder.RestoreButton = convertView.FindViewById<ImageView>(Resource.Id.img_load_backup);

                holder.RestoreButton.Click += (sender, e) =>
                {
                    var btn = (ImageView)sender;
                    var thisItem = (BackupListItem)btn.Tag;

                    if (thisItem == null) return;
                        
                    var dbfile = thisItem.BackupFile;

                    if (dbfile == null) return;

                    BackupConfirmDialog(dbfile);
                };               

                convertView.Tag = holder;
            }

            var item = _items[position];            

            // Set Data
            holder.FileName.Text = item.BackupFile.Name;
            holder.RestoreButton.Tag = item;
            holder.BackupTime.Text = File.GetCreationTime(item.BackupFile.AbsolutePath).ToString("MM/dd/yyyy HH:mm");

            if (!holder.Delete.HasOnClickListeners)
            {
                holder.Delete.Click += (sender, e) =>
                {
                    DeleteBackup(position, holder.FileName);
                };
            }

            if (!holder.Share.HasOnClickListeners)
            {
                holder.Share.Click += (sender, e) =>
                {
                    ShareBackup(position, holder.FileName);
                };
            }

            return convertView;
        }

        private void BackupConfirmDialog(Java.IO.File dbfile)
        {
            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetTitle("Confirm Restore")
                .SetMessage("WARNING: This will replace the current database. All changes will be lost!")
                .SetPositiveButton("Confirm", (sender, a) =>
                {
                    if (BackupHandler.RestoreBackup(dbfile.AbsolutePath))
                    {
                        Toast.MakeText(_context, "Databasebackup restored! Application will restart now", ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(_context, "Database import falied!", ToastLength.Short).Show();
                    }

                    OnRestartApp();                   
                })
                .SetNegativeButton("Cancel", (sender, a) => { })
                .Show();
        }

        private void OnRestartApp()
        {
            RestartApplication?.Invoke(this, null);
        }

        private void ShareBackup(int position, TextView fileName)
        {
            var item = _items[position];
            ShareFile(item.BackupFile);
        }

        public void ShareFile(Java.IO.File file)
        {
            Android.Net.Uri imageUri = Android.Support.V4.Content.FileProvider.GetUriForFile(
            _context,
            "de.timmcode.ghcampaigntracker.droid.provider",
            file);

            Intent sendIntent = new Intent();
            sendIntent.SetAction(Intent.ActionSend);
            sendIntent.PutExtra(Intent.ExtraStream, imageUri);
            sendIntent.SetType("text/plain");            
            _context.StartActivity(sendIntent);
        }

        private void DeleteBackup(int position, TextView fileName)
        {
            var item = _items[position];

            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetMessage(_context.Resources.GetString(Resource.String.DeleteBackupCommit))

                .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
                {
                    if (position >= Count) return;
                    _items.Remove(item);

                    item.BackupFile.Delete();

                    string where = MediaStore.MediaColumns.Data + "=?";
                    string[] selectionArgs = new string[] { item.BackupFile.AbsolutePath };
                    ContentResolver contentResolver = _context.ContentResolver;
                    Android.Net.Uri filesUri = MediaStore.Files.GetContentUri("external");

                    if (item.BackupFile.Exists())
                    {
                        contentResolver.Delete(filesUri, where, selectionArgs);
                    }

                    NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        private class Holder : Java.Lang.Object
        {
            public TextView FileName { get; set; }
            public ImageView Share { get; set; }
            public ImageView Delete { get; set; }
            public TextView BackupTime { get; set; }
            public ImageView RestoreButton { get; set; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return _items[position];
        }

        internal BackupListItem GetSelected()
        {
            return _items.FirstOrDefault(x => x.IsSelected);
        }
    }
}