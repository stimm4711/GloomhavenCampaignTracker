using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.CustomControls
{
    /// <summary>
    /// https://github.com/danoz73/QustomDialog/blob/master/src/com/qustom/dialog/QustomDialogBuilder.java
    /// </summary>
    public sealed class CustomDialogBuilder : AlertDialog.Builder
    {
        /** The custom_body layout */
        private readonly View mDialogView;
        /** optional dialog title layout */
        private readonly TextView mTitle;
        /** optional alert dialog image */
        private readonly ImageView mIcon;
        /** optional message displayed below title if title exists*/
        private readonly TextView mMessage;

        public CustomDialogBuilder(Context context, int themeResId) : base(context, themeResId)
        {
            mDialogView = View.Inflate(context, Resource.Layout.qustom_dialog_layout, null);
            SetView(mDialogView);
            mDialogView.SetBackgroundColor(new Color(ContextCompat.GetColor(Context, Resource.Color.gloom_background)));
            mTitle = (TextView)mDialogView.FindViewById(Resource.Id.alertTitle);
            mMessage = (TextView)mDialogView.FindViewById(Resource.Id.message);
            mIcon = (ImageView)mDialogView.FindViewById(Resource.Id.icon);

            mTitle.SetTextSize(Android.Util.ComplexUnitType.Dip, 20);
            mMessage.SetTextSize(Android.Util.ComplexUnitType.Dip, 18);
        }       

        public override AlertDialog.Builder SetTitle(ICharSequence title)
        {
            mTitle.SetText(title, TextView.BufferType.Normal);
            return this;
        }

        public CustomDialogBuilder SetTitleColor(Color color)
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

        public CustomDialogBuilder SetCustomView(View view)
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
            pbutton.SetTextSize(Android.Util.ComplexUnitType.Dip,20);          

            return alert;
        }

    }
}