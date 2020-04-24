using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class EventCharacterOutcomeFragment : CharacterRewardsFragment
    {
        internal static EventCharacterOutcomeFragment NewInstance(int characterId)
        {
            var frag = new EventCharacterOutcomeFragment() { Arguments = new Bundle() };
            frag.Arguments.PutInt(charID, characterId);
            return frag;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            if (Character == null)
            {
                var charID = savedInstanceState.GetInt("CharacterID");
                Character = CharacterRepository.Get(charID, recursive: true);
            }

            base.OnCreate(savedInstanceState);           
        }

        public override void OnResume()
        {
            base.OnResume();
            SetTotalCheckmarks();
            SetTotalGold();
            SetTotalXP();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_event_rewards_character, container, false);            
            _xpEditText = _view.FindViewById<EditText>(Resource.Id.xp);
            _goldEditText = _view.FindViewById<EditText>(Resource.Id.gold);
            _check1 = _view.FindViewById<CheckBox>(Resource.Id.checkMark1);
            _check2 = _view.FindViewById<CheckBox>(Resource.Id.checkMark2);
            _xpTotalText = _view.FindViewById<TextView>(Resource.Id.totalxp);
            _goldTotalText = _view.FindViewById<TextView>(Resource.Id.totalgold);
            _checkTotalText = _view.FindViewById<TextView>(Resource.Id.totalChecks);
            var charactername = _view.FindViewById<TextView>(Resource.Id.charactername);

            _xpEditText.FocusChange += XpEditText_FocusChange;
            _goldEditText.FocusChange += GoldEditText_FocusChange;

            _xpTotalText.Text = Character.Experience.ToString();
            _goldTotalText.Text = Character.Gold.ToString();
            _checkTotalText.Text = Character.Checkmarks.ToString();

            if(!_check1.HasOnClickListeners)
            {
                _check1.CheckedChange += Check_CheckedChange;
            }

            if (!_check2.HasOnClickListeners)
            {
                _check2.CheckedChange += Check_CheckedChange;
            }

            charactername.Text = Character.Name;

            return _view;
        }

        override protected int NewTotalGold()
        {            
            if (!int.TryParse(_goldEditText.Text, out int newGold)) return Character.Gold;    
            if (newGold < 0) _goldEditText.SetTextColor(Android.Graphics.Color.Red);
            if (newGold >= 0 && Context != null) _goldEditText.SetTextColor(new Android.Graphics.Color(ContextCompat.GetColor(Context, Resource.Color.gloom_primaryText)));

            return base.NewTotalGold();
        }

        protected override int CalcNewCHeckmarks()
        {
            var newChecks = 0;

            if (_check1.Checked) newChecks += 1;
            if (_check2.Checked) newChecks -= 1;

            return newChecks;
        }
    }
}