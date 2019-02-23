using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    /// <summary>
    /// Displays the character class icons
    /// </summary>
    internal class CharacterClassAdapter : BaseAdapter
    {
        private readonly Context _context;
        private readonly List<int> _availableClassIds; 

        /// <summary>
        /// class icons in ... .shared/resources/drawable
        /// </summary>
        private static readonly int[] ClassIcons =
        {
            Resource.Drawable.ic_class1icon_white_48,
            Resource.Drawable.ic_class2icon_white_48,
            Resource.Drawable.ic_class3icon_white_48,
            Resource.Drawable.ic_class4icon_white_48,
            Resource.Drawable.ic_class5icon_white_48,
            Resource.Drawable.ic_class6icon_white_48,
            Resource.Drawable.ic_class7icon_white_48,
            Resource.Drawable.ic_class8icon_white_48,
            Resource.Drawable.ic_class9icon_white_48,
            Resource.Drawable.ic_class10icon_white_48,
            Resource.Drawable.ic_class11icon_white_48,
            Resource.Drawable.ic_class12icon_white_48,
            Resource.Drawable.ic_class13icon_white_48,
            Resource.Drawable.ic_class14icon_white_48,
            Resource.Drawable.ic_class15icon_white_48,
            Resource.Drawable.ic_class16icon_white_48,
            Resource.Drawable.ic_class17icon_white_48,
            Resource.Drawable.ic_class18icon_white
        };

        public CharacterClassAdapter(Context context, bool showAll = false)
        {
            _context = context;
            _availableClassIds = (GCTContext.CurrentCampaign == null || showAll) ? new List<int>(new []{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17}) : GCTContext.CurrentCampaign.UnlockedClassesIds;
            if (GCTContext.CurrentCampaign != null && GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.HiddenClassUnlocked && !_availableClassIds.Contains(18)) _availableClassIds.Add(18);
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return _availableClassIds[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (parent.Id == Resource.Id.imagesGridView)
            {
                // used in campaign unlocks tab
                // shows small white icons in image views in a gridview
                ImageView imageView;
                if (convertView == null)
                {
                    // if it's not recycled, initialize some attributes
                    float scale = _context.Resources.DisplayMetrics.Density;
                    int pixels = (int)(48 * scale + 0.5f);

                    imageView = new ImageView(_context) { LayoutParameters = new AbsListView.LayoutParams(pixels, pixels) };
                    // fixed image size
                    imageView.SetScaleType(ImageView.ScaleType.FitCenter); // fit cell
                }
                else
                {
                    imageView = (ImageView)convertView;
                }

                // set icon, get the resource 
                imageView.SetImageResource(ResourceHelper.GetClassIconWhiteSmallRessourceId(_availableClassIds[position] - 1));

                // if a class is locked make the icon darker
                imageView.Alpha = (GCTContext.CurrentCampaign != null && GCTContext.CurrentCampaign.UnlockedClassesIds.Contains(_availableClassIds[position])) ? 0.8f : 0.3f;

                if (!imageView.HasOnClickListeners)
                {
                    imageView.Click += (sender, e) =>
                    {
                        var classId = _availableClassIds[position];
                        if (classId <= 6) return;

                        if (GCTContext.CurrentCampaign == null) return;

                        if (GCTContext.CurrentCampaign.UnlockedClassesIds.Contains(classId))
                        {
                            GCTContext.CurrentCampaign.RemoveUnlockedClass(classId);
                        }
                        else
                        {
                            GCTContext.CurrentCampaign.AddUnlockedClass(classId);
                        }

                        CampaignUnlocksRepository.InsertOrReplace(GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks);
                        NotifyDataSetChanged();
                    };
                }
                return imageView;
            }

            // used in character creation to fill a spinner view
            // use a holder for recycling
            CharacterClassAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as CharacterClassAdapterViewHolder;

            if (holder == null)
            {
                holder = new CharacterClassAdapterViewHolder();
                var inflater = _context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                view = inflater.Inflate(Resource.Layout.itemview_imagespinner, parent, false);
                holder.Image = view.FindViewById<ImageView>(Resource.Id.image);
                view.Tag = holder;
            }

            // set image, black icons 
            holder.Image.SetImageResource(ClassIcons[_availableClassIds[position] -1]);

            // if a class is locked make the icon darker
            if (GCTContext.CurrentCampaign != null)
            {
                holder.Image.Alpha = GCTContext.CurrentCampaign.UnlockedClassesIds.Contains(_availableClassIds[position]) ? 0.8f : 0.3f;
            }

            return view;
        }

        internal int GetIndexOfClass(int classid)
        {
            return _availableClassIds.IndexOf(classid);
        }

        public override int Count => _availableClassIds.Count;
    }

    internal class CharacterClassAdapterViewHolder : Java.Lang.Object
    {
        public ImageView Image { get; set; }
    }
}