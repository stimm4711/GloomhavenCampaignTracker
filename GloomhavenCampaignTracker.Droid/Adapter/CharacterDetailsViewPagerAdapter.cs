using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Fragments.character;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class CharacterDetailsViewPagerAdapter : FragmentStatePagerAdapter
    {
        private readonly Context _context;
        private readonly int PageCount = 4;
        private readonly Fragment[] frags;
        private readonly DL_Character _character;
       
        public CharacterDetailsViewPagerAdapter(Context context, FragmentManager fm, DL_Character character) : base(fm)
        {
            _context = context;
            FragManager = fm;
            if (GCTContext.Settings.IsShowOldAbilitySheet) PageCount = 5;
            frags = new Fragment[PageCount];
            _character = character;
        }

        public override int Count => PageCount;

        public FragmentManager FragManager { get; }
        
        public override ICharSequence GetPageTitleFormatted(int position)
        {
            var title = GetTitle(position);
            return new String(title);
        }
        
        private string GetTitle(int position)
        {
            var title = _context.Resources.GetString(Resource.String.Character);
            switch (position)
            {
                case 0:
                    title = _context.Resources.GetString(Resource.String.Character);
                    if (PageCount == 5) title = "Char";
                    break;
                case 1:
                    title = _context.Resources.GetString(Resource.String.Items);
                    break;
                case 2:
                    title = _context.Resources.GetString(Resource.String.Abilities);
                    if (PageCount == 5) title = $"{_context.Resources.GetString(Resource.String.Abilities)} 2.0"; ;
                    break;                
                case 3:
                    title = _context.Resources.GetString(Resource.String.Perks);                   
                    break;
                case 4:
                    title = $"{_context.Resources.GetString(Resource.String.Abilities)} 1.0";
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
                    frag = CharacterDetailsStatsFragment.NewInstance(_character);
                    break;
                case 1:
                    frag = CharacterDetailItemsFragment.NewInstance(_character);
                    break;
                case 2:
                    frag = CharacterDetailAbilitiesFragment.NewInstance(_character);
                    break;
                case 3:
                    frag = CharacterDetailPerks2Fragment.NewInstance(_character);                  
                    break;
                case 4:
                    frag = CharacterDetailAbilitiesFragmentOld.NewInstance(_character);
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
            if (frag is CharacterDetailsStatsFragment f)
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