using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Text.Method;

namespace GloomhavenCampaignTracker.Droid.Fragments
{
    public class SupportDevFragment : Fragment
    {
        public static SupportDevFragment NewInstance()
        {
            var frag = new SupportDevFragment { Arguments = new Bundle() };
            return frag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view =  inflater.Inflate(Resource.Layout.fragment_support_developer, container, false);
            
             var bggTxt = view.FindViewById<TextView>(Resource.Id.bggtext);
            var playstore = view.FindViewById<TextView>(Resource.Id.playstorelink);           
                    
            bggTxt.MovementMethod = LinkMovementMethod.Instance;
            playstore.MovementMethod = LinkMovementMethod.Instance;

            return view;
        }
    }
}