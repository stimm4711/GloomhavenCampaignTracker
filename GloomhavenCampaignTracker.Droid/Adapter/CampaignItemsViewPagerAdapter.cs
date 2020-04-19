using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.city;
using GloomhavenCampaignTracker.Business;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class CampaignItemsViewPagerAdapter : FragmentStatePagerAdapter
    {
        private readonly FragmentManager _fragManager;
        private readonly Context _context;
        private int _pageCount = 11;
        private readonly Fragment[] frags;
        private readonly Campaign _campaign;
        private readonly int _prosperity;

        public CampaignItemsViewPagerAdapter(Context context, FragmentManager fm, Campaign campaign) : base(fm)
        {
            _context = context;
            _fragManager = fm;
            _campaign = campaign;
            _prosperity = Helper.GetProsperityLevel(campaign.CityProsperity);
            _pageCount = _prosperity + 2;

            frags = new Fragment[_pageCount];            
        }

        public override int Count => _pageCount;

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            var title = GetTitle(position);
            return new Java.Lang.String(title);
        }
        
        private string GetTitle(int position)
        {
            var title = "";

            if (position == 0) return "*";
            if (position > _prosperity) return "UL";
            if (position < 10) return position.ToString();

            return title;
        }

        public override Fragment GetItem(int position)
        {
            if(position > _prosperity)
            {
                position += 11;
            }
            Fragment frag = new CampaignItemsFragment(position);  
            return frag;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            Fragment createdfrag = (Fragment) base.InstantiateItem(container, position);
            frags[position] = createdfrag;           
            return createdfrag;
        }

        public Fragment GetFragment(int position)
        {
            if (frags.Length > position)
            {
                return frags[position];
            }
            return null;
        }

        public View GetTabView(int position)
        {
            //    Given you have a custom layout in `res / layout / custom_tab.xml` with a TextView
            var v = LayoutInflater.From(_context).Inflate(Resource.Layout.itemview_custom_tab_header, null);
            var tv = (TextView)v.FindViewById(Resource.Id.tabText);
            tv.Text = GetTitle(position);
            return tv;
        }
    }
}