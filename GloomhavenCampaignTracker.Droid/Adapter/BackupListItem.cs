using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Provider;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using Xamarin.Essentials;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class BackupListItem : Java.Lang.Object
    {
        public Java.IO.File BackupFile { get; set; }
        public bool IsSelected { get; set; }

    }

    internal class SelectableBackupAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<BackupListItem> _items = new List<BackupListItem>();

        public override int Count => _items.Count;

        public SelectableBackupAdapter(Context context, List<BackupListItem> items) 
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
                convertView = inflater.Inflate(Resource.Layout.listviewitem_backup_selectable, parent, false);
                holder.FileName = convertView.FindViewById<TextView>(Resource.Id.backupnameTextView);
                holder.Share = convertView.FindViewById<ImageView>(Resource.Id.shareButton);
                holder.Selected = convertView.FindViewById<CheckBox>(Resource.Id.selected);
                holder.Delete = convertView.FindViewById<ImageView>(Resource.Id.deleteButton);
                holder.BackupTime = convertView.FindViewById<TextView>(Resource.Id.backupTimeTextView);

                // CheckCHanged Event
                holder.Selected.CheckedChange += (sender, e) =>
                {
                    var chkBx = (CheckBox)sender;
                    var thisItem = (BackupListItem)chkBx.Tag;

                    if (thisItem == null || thisItem.IsSelected == chkBx.Checked) return;

                    foreach(var i in _items.Where(x=>x.IsSelected))
                    {
                        i.IsSelected = false;
                    }

                    thisItem.IsSelected = chkBx.Checked;                                       

                    NotifyDataSetChanged();
                };               

                convertView.Tag = holder;
            }

            var item = _items[position];            

            // Set Data
            holder.FileName.Text = item.BackupFile.Name;
            holder.Selected.Tag = item;
            holder.Selected.Checked = item.IsSelected;
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

                    //NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        private class Holder : Java.Lang.Object
        {
            public TextView FileName { get; set; }
            public ImageView Share { get; set; }
            public CheckBox Selected { get; set; }
            public ImageView Delete { get; set; }
            public TextView BackupTime { get; set; }
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