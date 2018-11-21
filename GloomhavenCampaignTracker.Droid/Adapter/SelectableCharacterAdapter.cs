using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class SelectableCharacterAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<CharacterWrapper> _items = new List<CharacterWrapper>();

        public override int Count => _items.Count;

        public SelectableCharacterAdapter(Context context, List<DL_Character> characters) 
        {
            _context = context;
            _items = characters.Select(x=> new CharacterWrapper(x)).ToList();
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
            CharacterAdapterViewHolder holder = null;

            if (convertView != null)
                holder = convertView.Tag as CharacterAdapterViewHolder;

            if (holder == null)
            {
                // Create new holder
                holder = new CharacterAdapterViewHolder();

                var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.listviewitem_character_selectable, parent, false);
                holder.Charactername = convertView.FindViewById<TextView>(Resource.Id.charnameTextView);
                holder.Characterlevel = convertView.FindViewById<TextView>(Resource.Id.charLevelText);
                holder.Selected = convertView.FindViewById<CheckBox>(Resource.Id.selected);
                holder.CharacterClass = convertView.FindViewById<ImageView>(Resource.Id.chaclassImage);

                // Looted CheckCHanged Event
                holder.Selected.CheckedChange += (sender, e) =>
                {
                    var chkBx = (CheckBox)sender;
                    var thisItem = (CharacterWrapper)chkBx.Tag;

                    if (thisItem == null || thisItem.Character.IsSelected == chkBx.Checked) return;
                    if (GetSelected().Any(x=>x.ClassId == thisItem.Character.ClassId && x.Id != thisItem.Character.Id))
                    {
                        Toast.MakeText(_context, "You can select just one character of each class.", ToastLength.Short).Show();
                        chkBx.Checked = false;
                    }
                    else
                    {
                        thisItem.Character.IsSelected = chkBx.Checked;
                        NotifyDataSetChanged();
                    }                   
                };

                convertView.Tag = holder;
            }

            // Set Data
            var partymember = _items[position].Character;

            holder.Charactername.Text = partymember.Name;
            holder.Characterlevel.Text = partymember.Level.ToString();
            holder.CharacterClass.SetImageResource(ResourceHelper.GetClassIconRessourceId(partymember.ClassId - 1));
            holder.Charactername.Tag = new CharacterWrapper(partymember);
            holder.Selected.Tag = new CharacterWrapper(partymember);
            holder.Selected.Checked = partymember.IsSelected;

            return convertView;
        }

        internal class CharacterAdapterViewHolder : Object
        {
            public TextView Charactername { get; set; }
            public TextView Characterlevel { get; set; }
            public ImageView CharacterClass { get; set; }
            public CheckBox Selected { get; set; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return _items[position];
        }

        internal IEnumerable<DL_Character> GetSelected()
        {
            return _items.Where(x => x.Character.IsSelected).Select(x=>x.Character);
        }
    }
}