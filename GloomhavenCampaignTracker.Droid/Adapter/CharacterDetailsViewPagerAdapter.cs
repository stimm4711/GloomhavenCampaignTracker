using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Fragments.character;
using GloomhavenCampaignTracker.Shared.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using Java.Lang;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class CharacterDetailsViewPagerAdapter : FragmentStatePagerAdapter
    {
        private readonly FragmentManager _fragManager;
        private readonly Context _context;
        private readonly int PageCount = 4;
        private readonly Fragment[] frags;
        private readonly DL_Character _character;
       
        public CharacterDetailsViewPagerAdapter(Context context, FragmentManager fm, DL_Character character) : base(fm)
        {
            _context = context;
            _fragManager = fm;
            if (GCTContext.ShowOldPerkSheet) PageCount = 5;
            frags = new Fragment[PageCount];
            _character = character;
        }

        public override int Count => PageCount;

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            var title = GetTitle(position);
            return new String(title);
        }
        
        private string GetTitle(int position)
        {
            var title = "Character";
            switch (position)
            {
                case 0:
                    title = "Character";
                    if (PageCount == 5) title = "Char";
                    break;
                case 1:
                    title = "Items";
                    break;
                case 2:
                    title = "Abilities";
                    break;                
                case 3:
                    title = "Perks";
                    if (PageCount == 5) title = "Perks.2"; ;                    
                    break;
                case 4:
                    title = "Perks.1";
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
                case 2:
                    frag = CharacterDetailAbilitiesFragment.NewInstance(_character);
                    break;
                case 1:
                    frag = CharacterDetailItemsFragment.NewInstance(_character);
                    break;
                case 3:
                        frag = CharacterDetailPerks2Fragment.NewInstance(_character);                  
                    break;
                case 4:
                    frag = CharacterDetailPerksFragment.NewInstance(_character);
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