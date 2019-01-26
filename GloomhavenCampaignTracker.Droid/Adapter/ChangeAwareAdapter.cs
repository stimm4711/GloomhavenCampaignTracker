using System;
using Android.Widget;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    public abstract class ChangeAwareAdapter : BaseAdapter
    {
        public event EventHandler<EventArgs> DataModified;

        protected void OnDataModified()
        {
            DataModified?.Invoke(this, new EventArgs());
        }
    }

}