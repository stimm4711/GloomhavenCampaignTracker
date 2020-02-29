using Android.Content;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Fragments.campaign;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using Java.Lang;
using System;
using System.Collections.Generic;
using String = Java.Lang.String;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class ScenarioRewardsViewPagerAdapter : FragmentStatePagerAdapter
    {
        private readonly FragmentManager _fragManager;
        private readonly Context _context;
        private int PageCount = 2;
        private readonly Fragment[] frags;
        private List<DL_Character> _characters;

        public ScenarioRewardsViewPagerAdapter(Context context, FragmentManager fm) : base(fm)
        {
            _context = context;
            _fragManager = fm;
            
            _characters = CharacterRepository.GetPartymembers(GloomhavenCampaignTracker.Business.GCTContext.CurrentCampaign.CurrentParty.Id);
            PageCount = _characters.Count;
            
            frags = new Fragment[PageCount];

        }

        public override int Count => PageCount;

        //public override ICharSequence GetPageTitleFormatted(int position)
        //{
        //    var title = $"  ";
        //    var v = LayoutInflater.From(_context).Inflate(Resource.Layout.itemview_custom_tab_header, null);
        //    var tv = (TextView)v.FindViewById(Resource.Id.tabText);

        //    var drawable = ContextCompat.GetDrawable(_context, ResourceHelper.GetClassIconWhiteSmallRessourceId(_characters[position].ClassId - 1));
        //    var size = Convert.ToInt32(System.Math.Round(tv.LineHeight * 1.3, MidpointRounding.AwayFromZero));
        //    drawable.Bounds.Set(0, 0, size, size);

        //    var span = new ImageSpan(drawable);

        //    var spannableString = new SpannableString(title);
        //    spannableString.SetSpan(span, title.Length - 1, title.Length, 0); //Add image at end of string

        //    return spannableString;
        //}

        public override Fragment GetItem(int position)
        {
            Fragment frag = null;
            switch (position)
            {
                case 0:

                    break;
                case 1:

                    break;
            }

            return frag;
        }

        //public View GetTabView(int position)
        //{
        //    //    Given you have a custom layout in `res / layout / custom_tab.xml` with a TextView
        //    var v = LayoutInflater.From(_context).Inflate(Resource.Layout.itemview_custom_tab_header, null);
        //    var tv = (TextView)v.FindViewById(Resource.Id.tabText);
        //    tv.Text = GetPageTitleFormatted(position);
        //    return tv;
        //}

        //public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        //{
            //Fragment createdfrag = (Fragment)base.InstantiateItem(container, position);
            //frags[position] = createdfrag;
            //return createdfrag;
        //}

    }
}