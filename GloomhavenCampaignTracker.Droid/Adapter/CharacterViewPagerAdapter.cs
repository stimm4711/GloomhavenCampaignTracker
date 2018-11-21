using System;
using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Fragments.character;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;
using Android.Support.V4.Content;
using Android.Text;
using Android.Text.Style;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class CharacterViewPagerAdapter : FragmentStatePagerAdapter
    {
        private readonly Context _context;
        private readonly List<DL_Character>  _characters;

        public CharacterViewPagerAdapter(Context context, FragmentManager fm, List<DL_Character> characters ) : base(fm)
        {
            _context = context;

            if (characters !=null )
            {
                Count = characters.Count;
            }

            _characters = characters;
        }

        public override int Count { get; }

        private string GetTitle(int position)
        {
            var character = _characters[position];        
            return $"  {character.Name} ";
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            var title = $"{GetTitle(position)}  ";
            var v = LayoutInflater.From(_context).Inflate(Resource.Layout.itemview_custom_tab_header, null);
            var tv = (TextView)v.FindViewById(Resource.Id.tabText);
            
            var drawable = ContextCompat.GetDrawable(_context, ResourceHelper.GetClassIconWhiteSmallRessourceId(_characters[position].ClassId - 1));
            var size = Convert.ToInt32(Math.Round(tv.LineHeight * 1.3, MidpointRounding.AwayFromZero));
            drawable.Bounds.Set(0, 0, size, size);

            var span = new ImageSpan(drawable);
            var span2 = new ImageSpan(drawable);

            var spannableString = new SpannableString(title);
            spannableString.SetSpan(span, title.Length - 1, title.Length, 0); //Add image at end of string
            spannableString.SetSpan(span2, 0, 1, 0);

            return spannableString;
        }

        public override Fragment GetItem(int position)
        {
            _characters[position] = DataServiceCollection.CharacterDataService.Get(_characters[position].Id);
            return CharacterDetailsViewPagerTabs.NewInstance(_characters[position]); 
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            var createdfrag = (Fragment)base.InstantiateItem(container, position);       
            return createdfrag;
        }

    }
}