using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Business.Network;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.city;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.party;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.unlocks;
using GloomhavenCampaignTracker.Droid.Fragments.campaign.world;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class CampaignViewPagerAdapter : FragmentStatePagerAdapter
    {
        private readonly FragmentManager _fragManager;
        private readonly Context _context;
        private const int PageCount = 4;
        private readonly Fragment[] frags;

        public CampaignViewPagerAdapter(Context context, FragmentManager fm ) : base(fm)
        {
            _context = context;
            _fragManager = fm;
            frags = new Fragment[PageCount];
        }

        public override int Count => PageCount;

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            var title = GetTitle(position);
            return new String(title);
        }
        
        private string GetTitle(int position)
        {
            var title = _context.Resources.GetString(Resource.String.World);
            switch (position)
            {
                case 0:
                    title = _context.Resources.GetString(Resource.String.World);
                    break;
                case 1:
                    title = _context.Resources.GetString(Resource.String.City);
                    break;
                case 2:
                    title = _context.Resources.GetString(Resource.String.CamapignParty);
                    break;
                case 3:
                    title = _context.Resources.GetString(Resource.String.CampaignUnlocks);
                    break;
            }
            return title;
        }

        public override Fragment GetItem(int position)
        {          
            Fragment frag = new CampaignWorldFragment( _fragManager);
            switch (position)
            {
                case 0:
                    frag = new CampaignWorldFragment( _fragManager);
                    break;
                case 1:
                    frag = new CampaignCityFragment( _fragManager);                    
                    break;
                case 2:
                    frag = new CampaignPartyFragment( _fragManager);
                    break;
                case 3:
                    frag = new CampaignUnlocksFragment();
                    break;
            }

            return frag;
        }

        public override Object InstantiateItem(ViewGroup container, int position)
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

        public override int GetItemPosition(Object frag)
        {
            if (frag is CampaignCityFragment f)
            {
                if (f != null)
                {
                    f.Update();
                }

                return PositionNone;
            }

            return base.GetItemPosition(frag);
        }
    }
}