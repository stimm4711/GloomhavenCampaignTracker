using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Fragments.campaign;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using Java.Lang;
using String = Java.Lang.String;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class ScenarioDetailsViewPagerAdapter : FragmentStatePagerAdapter
    {
        private readonly FragmentManager _fragManager;
        private readonly Context _context;
        private const int PageCount = 2;
        private readonly Fragment[] frags;
        private readonly DL_CampaignUnlockedScenario CampaignUnlockedScenario;

        public ScenarioDetailsViewPagerAdapter(Context context, FragmentManager fm, DL_CampaignUnlockedScenario scenario) : base(fm)
        {
            _context = context;
            _fragManager = fm;
            frags = new Fragment[PageCount];
            CampaignUnlockedScenario = scenario;
        }

        public override int Count => PageCount;

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            var title = GetTitle(position);
            return new String(title);
        }

        private string GetTitle(int position)
        {
            var title = "Details";
            switch (position)
            {
                case 0:
                    title = "Details";
                    break;
                case 1:
                    title = "Unlocks";
                    break;
            }
            return title;
        }

        public override Fragment GetItem(int position)
        {
            Fragment frag = null;
            switch (position)
            {
                case 0:
                    //frag = SzenarioDetailsFragment.NewInstance(CampaignUnlockedScenario);
                    break;
                case 1:
                    frag = SzenarioDetailsUnlocksFragment.NewInstance(CampaignUnlockedScenario);
                    break;
            }

            return frag;
        }

        public View GetTabView(int position)
        {
            //    Given you have a custom layout in `res / layout / custom_tab.xml` with a TextView
            var v = LayoutInflater.From(_context).Inflate(Resource.Layout.itemview_custom_tab_header, null);
            var tv = (TextView)v.FindViewById(Resource.Id.tabText);
            tv.Text = GetTitle(position);
            return tv;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            Fragment createdfrag = (Fragment)base.InstantiateItem(container, position);
            frags[position] = createdfrag;
            return createdfrag;
        }

    }
}