using System;
using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Support.V4.Content;

namespace GloomhavenCampaignTracker.Droid.CustomControls
{
    /// <inheritdoc />
    /// <summary>
    /// https://github.com/xamarin/monodroid-samples/blob/master/AnimationDemo/KarmaMeter.cs
    /// </summary>
    internal class ProsperityMeter : View
    {
        private const int DefaultHeight = 30;
        private const int DefaultWidth = 200;

        private Paint _negativePaint;
        private double _position = 0.5;
        private Paint _positivePaint;

        public ProsperityMeter(Context context, IAttributeSet attrs)
            : this(context, attrs, 0)
        {
            Initialize();
        }

        public ProsperityMeter(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Initialize();
        }

        public double KarmaValue
        {
            get => _position;
            set
            {
                _position = Math.Max(0f, Math.Min(value, 1f));
                Invalidate();
            }
        }

        public void SetProsperityValue(double value, bool animate)
        {
            if (!animate)
            {
                KarmaValue = value;
                return;
            }

            var animator = ValueAnimator.OfFloat((float)_position, (float)Math.Max(0f, Math.Min(value, 1f)));
            animator.SetDuration(500);

            animator.Update += (sender, e) => KarmaValue = (double)e.Animation.AnimatedValue;
            animator.Start();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            var middle = canvas.Width * (float)_position;

            canvas.DrawPaint(_negativePaint);

            canvas.DrawRect(0, 0, middle, canvas.Height, _positivePaint);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var width = MeasureSpec.GetSize(widthMeasureSpec);
            SetMeasuredDimension(width < DefaultWidth ? DefaultWidth : width, DefaultHeight);
        }

        private void Initialize()
        {
            _positivePaint = new Paint
            {
                AntiAlias = true,
                Color = new Color(ContextCompat.GetColor(Context, Resource.Color.gloom_secondary))
            };
            _positivePaint.SetStyle(Paint.Style.FillAndStroke);

            _negativePaint = new Paint
            {
                AntiAlias = true,
                Color = new Color(ContextCompat.GetColor(Context, Resource.Color.gloom_primaryDarker))
            };
            _negativePaint.SetStyle(Paint.Style.FillAndStroke);
        }
    }
}