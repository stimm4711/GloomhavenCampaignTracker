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

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view =  inflater.Inflate(Resource.Layout.alertdialog_release_notes, container, false);

            //var bggTxt = view.FindViewById<TextView>(Resource.Id.githubtext);

            //bggTxt.MovementMethod = LinkMovementMethod.Instance;

            return view;
        }
    }
}