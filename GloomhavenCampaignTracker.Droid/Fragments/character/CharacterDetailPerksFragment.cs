using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Droid.Fragments.character
{
    public class CharacterDetailPerksFragment : CharacterDetailFragmentBase
    {
        private ListView _lv;

        internal static CharacterDetailPerksFragment NewInstance(DL_Character character)
        {
            var frag = new CharacterDetailPerksFragment() { Arguments = new Bundle() };
            frag.Arguments.PutInt(charID, character.Id);
            return frag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            _view = inflater.Inflate(Resource.Layout.fragment_characterdetails_listwithfab, container, false);
           
            var fab = _view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            _lv = _view.FindViewById<ListView>(Resource.Id.ListView);
          
            if(Character != null)
            {
                _lv.Adapter = new PerksAdapter(Context, Character);
            }            

            if (!fab.HasOnClickListeners)
            {
                fab.Click += (senderx, ex) =>
                {
                    AddNewperk(_lv);
                };
            }

            if (!_lv.HasOnClickListeners)
            {
                _lv.ItemClick += Lv_ItemClick;
            }

            return _view;
        }

        /// <summary>
        /// show alert dialog to add a new perk
        /// </summary>
        /// <param name="lv"></param>
        private void AddNewperk(ListView lv)
        {
            var inflater = Context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_addperk, null);
            var checkboxNumberTextView = convertView.FindViewById<EditText>(Resource.Id.checkboxnumber);
            var perkdescription = convertView.FindViewById<EditText>(Resource.Id.perkcomment);

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle("Add Perk")
                .SetPositiveButton("Add perk", (senderAlert, args) =>
                {
                    if (string.IsNullOrEmpty(checkboxNumberTextView.Text)) return;

                    if (!int.TryParse(checkboxNumberTextView.Text, out int checkboxnumber)) return;

                    var newPerk = new DL_Perk()
                    {
                        Character = Character,
                        ID_Character = Character.Id,
                        Checkboxnumber = checkboxnumber,
                        Perkcomment = perkdescription.Text
                    };

                    Character.Perks.Add(newPerk);
                    SaveCharacter();
                    lv.Adapter = new PerksAdapter(Context, Character);
                })
                .Show();
        }

        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var inflater = Context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_addperk, null);
            var checkboxnumber = convertView.FindViewById<EditText>(Resource.Id.checkboxnumber);
            var perkcomment = convertView.FindViewById<EditText>(Resource.Id.perkcomment);

            var perk = Character.Perks[e.Position];

            if (perk == null) return;

            checkboxnumber.Text = perk.Checkboxnumber.ToString();
            perkcomment.Text = perk.Perkcomment;

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle("Edit Perk")
                .SetPositiveButton("Save changes", (senderAlert, args) =>
                {
                    if (!string.IsNullOrEmpty(checkboxnumber.Text))
                    {
                        if(int.TryParse(checkboxnumber.Text, out int checkboxNumber))
                        {
                            perk.Checkboxnumber = checkboxNumber;
                        }                        
                    }

                    if (!string.IsNullOrEmpty(perkcomment.Text))
                    {
                        perk.Perkcomment = perkcomment.Text;
                    }

                    SaveCharacter();

                    _lv.Adapter = new PerksAdapter(Context, Character);
                })
                .Show();
        }
    }
}