using System;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Net;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business;
using Java.Lang;
using Plugin.Connectivity;

namespace GloomhavenCampaignTracker.Droid.CustomControls
{
    /// <summary>
    /// https://github.com/danoz73/QustomDialog/blob/master/src/com/qustom/dialog/QustomDialogBuilder.java
    /// </summary>
    public sealed class EventFrontImageViewDialogBuilder : AlertDialog.Builder
    {
        /** The custom_body layout */
        private readonly View mDialogView;
        /** optional dialog title layout */
        private readonly TextView mTitle;
        /** optional alert dialog image */
        private readonly ImageView mIcon;
        /** optional message displayed below title if title exists*/
        private readonly TextView mMessage;

        private readonly ImageView _imagen;

        private string _eventnumber;
        private readonly View _eventview;
        private EventTypes _eventtype;

        public EventFrontImageViewDialogBuilder(Context context, int themeResId, EventTypes eventtype) : base(context, themeResId)
        {   
            mDialogView = View.Inflate(context, Resource.Layout.qustom_transparent_dialog_layout, null);
            SetView(mDialogView);
            mDialogView.SetBackgroundColor(Color.Transparent);
            mTitle = (TextView)mDialogView.FindViewById(Resource.Id.alertTitle);
            mMessage = (TextView)mDialogView.FindViewById(Resource.Id.message);
            mIcon = (ImageView)mDialogView.FindViewById(Resource.Id.icon);

            mTitle.SetTextSize(Android.Util.ComplexUnitType.Dip, 20);
            mMessage.SetTextSize(Android.Util.ComplexUnitType.Dip, 18);

            LayoutInflater inflater = LayoutInflater.FromContext(context);
            _eventview = inflater.Inflate(Resource.Layout.item_view, null);

            if (_eventview == null) return;

            _imagen = _eventview.FindViewById<ImageView>(Resource.Id.itemimage);
            _eventtype = eventtype;

            SetCustomView(_eventview);
        }

        public override AlertDialog.Builder SetTitle(ICharSequence title)
        {
            mTitle.SetText(title, TextView.BufferType.Normal);
            return this;
        }

        public EventFrontImageViewDialogBuilder SetTitleColor(Color color)
        {
            mTitle.SetTextColor(color);
            return this;
        }

        public override AlertDialog.Builder SetMessage(int textResId)
        {
            mMessage.SetText(textResId);
            return this;
        }


        public override AlertDialog.Builder SetMessage(ICharSequence text)
        {
            mMessage.SetText(text, TextView.BufferType.Normal);
            return this;
        }

        public override AlertDialog.Builder SetIcon(int drawableResId)
        {
            mIcon.SetImageResource(drawableResId);
            return this;
        }

        public override AlertDialog.Builder SetIcon(Drawable icon)
        {
            mIcon.SetImageDrawable(icon);
            return this;
        }

        public EventFrontImageViewDialogBuilder SetCustomView(View view)
        {
            View customView = view;
            ((FrameLayout)mDialogView.FindViewById(Resource.Id.customPanel)).AddView(customView);
            return this;
        }

        public override AlertDialog Show()
        {
            if (mTitle.Text.Equals("")) mDialogView.FindViewById(Resource.Id.topPanel).Visibility = ViewStates.Gone;
            if (mMessage.Text.Equals("")) mDialogView.FindViewById(Resource.Id.contentPanel).Visibility = ViewStates.Gone;           

            var alert = base.Show();

            Button nbutton = alert.GetButton((int)DialogButtonType.Negative);
            nbutton.SetTextSize(Android.Util.ComplexUnitType.Dip, 20);

            Button pbutton = alert.GetButton((int)DialogButtonType.Positive);
            pbutton.SetTextSize(Android.Util.ComplexUnitType.Dip, 20);

            string eventtypeurl = "https://raw.githubusercontent.com/stimm4711/gloomhaven/master/images/events/base/road/re-";           
            if (_eventtype == EventTypes.CityEvent)
            {
                eventtypeurl = "https://raw.githubusercontent.com/stimm4711/gloomhaven/master/images/events/base/city/ce-";
            }
            else if(_eventtype == EventTypes.RiftEvent)
            {
                eventtypeurl = "https://raw.githubusercontent.com/stimm4711/gloomhaven/master/images/events/base/rift/rf-";
            }

            var eventFront = GetImageBitmapFromUrlAsync(eventtypeurl + _eventnumber + "-f.png", _imagen, _eventview);

            return alert;
        }

        private async Task<Bitmap> GetImageBitmapFromUrlAsync(string url, ImageView imagen, View view)
        {           
            Bitmap imageBitmap = null;

            using (var httpClient = new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            imagen.SetImageBitmap(imageBitmap);

            view.FindViewById<ProgressBar>(Resource.Id.loadingPanel).Visibility = ViewStates.Gone;

            return imageBitmap;
        }

        public EventFrontImageViewDialogBuilder SetEventNumber(string numbertext)
        {
            _eventnumber = numbertext;
            return this;
        }
    }
}