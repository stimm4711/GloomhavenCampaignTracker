using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Fragments.character;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign
{
    public class SzenarioCharacterRewardsFragment : CharacterDetailFragmentBase
    {
        private TextView _charactername;
        private EditText _xpEditText;
        private EditText _goldEditText;
        private CheckBox _check1;
        private CheckBox _check2;
        private TextView _xpTotalText;
        private TextView _goldTotalText;
        private TextView _checkTotalText;

        internal static SzenarioCharacterRewardsFragment NewInstance(int characterId)
        {
            var frag = new SzenarioCharacterRewardsFragment() { Arguments = new Bundle() };
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
            _view = inflater.Inflate(Resource.Layout.fragment_campaign_scenario_rewards_character, container, false);
            _charactername = _view.FindViewById<TextView>(Resource.Id.charactername);
            _xpEditText = _view.FindViewById<EditText>(Resource.Id.xp);
            _goldEditText = _view.FindViewById<EditText>(Resource.Id.gold);
            _check1 = _view.FindViewById<CheckBox>(Resource.Id.checkMark1);
            _check2 = _view.FindViewById<CheckBox>(Resource.Id.checkMark2);
            _xpTotalText = _view.FindViewById<TextView>(Resource.Id.totalxp);
            _goldTotalText = _view.FindViewById<TextView>(Resource.Id.totalgold);
            _checkTotalText = _view.FindViewById<TextView>(Resource.Id.totalChecks);

            _xpEditText.FocusChange += _xpEditText_FocusChange;
            _goldEditText.FocusChange += _goldEditText_FocusChange;

            _xpTotalText.Text = Character.Experience.ToString();
            _goldTotalText.Text = Character.Gold.ToString();
            _checkTotalText.Text = Character.Checkmarks.ToString();

            if(!_check1.HasOnClickListeners)
            {
                _check1.CheckedChange += _check_CheckedChange;
            }

            if (!_check2.HasOnClickListeners)
            {
                _check2.CheckedChange += _check_CheckedChange;
            }

            _charactername.Text = Character.Name;

            return _view;
        }

        private void _check_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            SetTotalCheckmarks();
        }

        private void SetTotalCheckmarks()
        {
            var newChecks = 0;

            if (_check1.Checked) newChecks += 1;
            if (_check2.Checked) newChecks += 1;

            _checkTotalText.Text = (Character.Checkmarks + newChecks).ToString();
        }

        private void _goldEditText_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus) return;
            if (string.IsNullOrEmpty(_goldEditText.Text)) return;
            SetTotalGold();
        }

        private void SetTotalGold()
        {            
            if (!int.TryParse(_goldEditText.Text, out int newGoldp)) return;
            _goldTotalText.Text = (Character.Gold + newGoldp).ToString();
        }

        private void _xpEditText_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus) return;
            if (string.IsNullOrEmpty(_xpEditText.Text)) return;
            SetTotalXP();
        }

        private void SetTotalXP()
        {
            if (!int.TryParse(_xpEditText.Text, out int newXp)) return;
            _xpTotalText.Text = (Character.Experience + newXp).ToString();
        }

        internal int NewTotalXP()
        {
            if (!int.TryParse(_xpEditText.Text, out int newXp)) return Character.Experience;
            return Character.Experience + newXp;
        }

        internal int NewTotalGold()
        {
            if (!int.TryParse(_goldEditText.Text, out int newGold)) return Character.Gold;
            return Character.Gold + newGold;
        }

        internal int NewTotalCheckmarks()
        {
            var newChecks = 0;

            if (_check1.Checked) newChecks += 1;
            if (_check2.Checked) newChecks += 1;

            return Character.Checkmarks + newChecks;
        }

        public void Save()
        {
            Character.Checkmarks = NewTotalCheckmarks();
            Character.Gold = NewTotalGold();
            Character.Experience = NewTotalXP();

            CharacterRepository.InsertOrReplace(Character);
        }
    }
}