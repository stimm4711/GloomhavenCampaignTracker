using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using GloomhavenCampaignTracker.Shared.Business;
using System.Threading.Tasks;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public abstract class CampaignDetailsFragmentBase : Fragment
    {
        protected View _view;
        protected bool _dataChanged = true;

        protected Campaign Campaign => GCTContext.CampaignCollection.CurrentCampaign;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                return null;
            }
                       
            return _view;
        }

        public override void OnStop()
        {   
            try
            {
                if (_dataChanged)
                {
                    GCTContext.CurrentCampaign?.Save();
                }               
            }
            catch
            {                       
                    
            }             

            base.OnStop();
        }
    }
}