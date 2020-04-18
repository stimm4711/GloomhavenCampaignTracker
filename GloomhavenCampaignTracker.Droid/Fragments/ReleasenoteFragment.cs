using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Text.Method;

namespace GloomhavenCampaignTracker.Droid.Fragments
{
    public class ReleasenoteFragment : Fragment
    {
        public static ReleasenoteFragment NewInstance()
        {
            var frag = new ReleasenoteFragment { Arguments = new Bundle() };
            return frag;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view =  inflater.Inflate(Resource.Layout.alertdialog_release_notes, container, false);

            return view;
        }
    }
}