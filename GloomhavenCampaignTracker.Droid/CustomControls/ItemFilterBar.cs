using System;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace GloomhavenCampaignTracker.Droid.CustomControls
{
    public class ItemFilterBar : LinearLayout
    {
        private ImageView _helmetImgView;
        private ImageView _armorImgView;
        private ImageView _bootsImgView;
        private ImageView _onehandImgView;
        private ImageView _twoHandmgView;
        private ImageView _smallImgView;

        private int _filterSelected = -1;
       
        public event EventHandler<ItemClickedEventArgs> ItemClicked;

        public ItemFilterBar(Context context) : base(context)
        {
            Initialize(context);
        }

        public ItemFilterBar(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(context);
        }

        public ItemFilterBar(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize(context);
        }

        public ItemFilterBar(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize(context);
        }

        public void Initialize(Context context)
        {
            LayoutInflater inflater = LayoutInflater.FromContext(context);
            var view = inflater.Inflate(Resource.Layout.custom_itemfilter, this);
                      
            _helmetImgView = view.FindViewById<ImageView>(Resource.Id.headImageView);
            _armorImgView = view.FindViewById<ImageView>(Resource.Id.armorImageView);
            _bootsImgView = view.FindViewById<ImageView>(Resource.Id.bootsImageView);
            _onehandImgView = view.FindViewById<ImageView>(Resource.Id.singlehandweaponImageView);
            _twoHandmgView = view.FindViewById<ImageView>(Resource.Id.twohandweaponImageView);
            _smallImgView = view.FindViewById<ImageView>(Resource.Id.smallitemImageView);

            _helmetImgView.Click += HelmetImgView_Click;
            _armorImgView.Click += ArmorImgView_Click;
            _bootsImgView.Click += BootsImgView_Click;
            _onehandImgView.Click += OnehandImgView_Click;
            _twoHandmgView.Click += TwoHandmgView_Click;
            _smallImgView.Click += SmallImgView_Click;
        }

        private void SmallImgView_Click(object sender, EventArgs e)
        {
            ItemClickedEventArgs args = new ItemClickedEventArgs
            {
                Itemcategorie = _filterSelected == 5 ? -1 : 5
            };

            _filterSelected = args.Itemcategorie;

            OnItemFilterClicked(args);
            UpdateView();
        }

        private void TwoHandmgView_Click(object sender, EventArgs e)
        {
            ItemClickedEventArgs args = new ItemClickedEventArgs
            {
                Itemcategorie = _filterSelected == 3 ? -1 : 3
            };

            _filterSelected = args.Itemcategorie;

            OnItemFilterClicked(args);
            UpdateView();
        }

        private void OnehandImgView_Click(object sender, EventArgs e)
        {
            ItemClickedEventArgs args = new ItemClickedEventArgs
            {
                Itemcategorie = _filterSelected == 2 ? -1 : 2
            };

            _filterSelected = args.Itemcategorie;

            OnItemFilterClicked(args);
            UpdateView();
        }

        private void BootsImgView_Click(object sender, EventArgs e)
        {
            ItemClickedEventArgs args = new ItemClickedEventArgs
            {
                Itemcategorie = _filterSelected == 4 ? -1 : 4
            };

            _filterSelected = args.Itemcategorie;

            OnItemFilterClicked(args);
            UpdateView();
        }

        private void ArmorImgView_Click(object sender, EventArgs e)
        {
            ItemClickedEventArgs args = new ItemClickedEventArgs
            {
                Itemcategorie = _filterSelected == 1 ? -1 : 1
            };

            _filterSelected = args.Itemcategorie;

            OnItemFilterClicked(args);
            UpdateView();
        }

        private void HelmetImgView_Click(object sender, EventArgs e)
        {
            ItemClickedEventArgs args = new ItemClickedEventArgs
            {
                Itemcategorie = _filterSelected == 0 ? -1 : 0
            };

            _filterSelected = args.Itemcategorie;

            OnItemFilterClicked(args);
            UpdateView();
        }

        private void UpdateView()
        {
            var colorSelected = ContextCompat.GetColor(Context, Resource.Color.gloom_secondary);

            _helmetImgView.SetBackgroundColor(Color.Transparent);
            _armorImgView.SetBackgroundColor(Color.Transparent);
            _bootsImgView.SetBackgroundColor(Color.Transparent);
            _onehandImgView.SetBackgroundColor(Color.Transparent);
            _twoHandmgView.SetBackgroundColor(Color.Transparent);
            _smallImgView.SetBackgroundColor(Color.Transparent);

            switch (_filterSelected)
            {
                case 0:
                    _helmetImgView.SetBackgroundColor(new Color(colorSelected));
                    break;
                case 1:
                    _armorImgView.SetBackgroundColor(new Color(colorSelected));
                    break;
                case 4:
                    _bootsImgView.SetBackgroundColor(new Color(colorSelected));
                    break;
                case 2:
                    _onehandImgView.SetBackgroundColor(new Color(colorSelected));
                    break;
                case 3:
                    _twoHandmgView.SetBackgroundColor(new Color(colorSelected));
                    break;
                case 5:
                    _smallImgView.SetBackgroundColor(new Color(colorSelected));
                    break;
            }           
        }

        protected virtual void OnItemFilterClicked(ItemClickedEventArgs e)
        {
            ItemClicked?.Invoke(this, e);
        }

        public class ItemClickedEventArgs : EventArgs
        {
            public int Itemcategorie { get; set; }
        }


    }
}