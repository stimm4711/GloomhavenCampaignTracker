using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using static GloomhavenCampaignTracker.Droid.Adapter.AbilitiesEnhancementDeleteableAdapter;

namespace GloomhavenCampaignTracker.Droid.Fragments.character
{
    public class CharacterDetailAbilitiesFragmentOld : CharacterDetailFragmentBase
    {
        private ListView _lv;
        private AbilitiesAdapterOld _abilitiesAdapter;

        internal static CharacterDetailAbilitiesFragmentOld NewInstance(DL_Character character)
        {
            var frag = new CharacterDetailAbilitiesFragmentOld { Arguments = new Bundle() };
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
                _abilitiesAdapter  = new AbilitiesAdapterOld(Context, Character);
                _lv.Adapter = _abilitiesAdapter;
            }            

            if (!fab.HasOnClickListeners)
            {
                fab.Click += (senderx, ex) =>
                {
                    AddNewAbility(_lv);
                };
            }     
            
            if(!_lv.HasOnClickListeners)
            {
                _lv.ItemClick += Lv_ItemClick;
            }

            return _view;
        }

        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var inflater = Context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_editability, null);
            var refNumberText = convertView.FindViewById<EditText>(Resource.Id.inputText);
            var levelText = convertView.FindViewById<EditText>(Resource.Id.inputNumber);
            var nameText = convertView.FindViewById<EditText>(Resource.Id.abilityName);
            var enhancementListTop = convertView.FindViewById<ListView>(Resource.Id.listViewtop);
            var fabTop = convertView.FindViewById<FloatingActionButton>(Resource.Id.fabtop);
            var enhancementListBottom = convertView.FindViewById<ListView>(Resource.Id.listViewbottom);
            var fabBottom = convertView.FindViewById<FloatingActionButton>(Resource.Id.fabbottom);

            var ability = Character.Abilities[e.Position];               

            if (ability == null) return;

            refNumberText.Text = ability.ReferenceNumber.ToString();
            levelText.Text = ability.Level.ToString();
            nameText.Text = ability.AbilityName;

            enhancementListTop.Adapter = new AbilitiesEnhancementDeleteableAdapterOld(Context, ability, true);
            enhancementListBottom.Adapter = new AbilitiesEnhancementDeleteableAdapterOld(Context, ability, false);

            if (!fabTop.HasOnClickListeners)
            {
                fabTop.Click += (s, arg) =>
                {
                    AddEnhancement(inflater, enhancementListTop, ability, true);
                };
            }

            if (!fabBottom.HasOnClickListeners)
            {
                fabBottom.Click += (s, arg) =>
                {
                    AddEnhancement(inflater, enhancementListBottom, ability, false);
                };
            }

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle("Edit Ability")
                .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .SetPositiveButton("Save changes", (senderAlert, args) =>
                {
                    if (string.IsNullOrEmpty(nameText.Text)) return;

                    ability.AbilityName = nameText.Text;

                    if (!string.IsNullOrEmpty(levelText.Text))
                    {
                        int.TryParse(levelText.Text, out int level);
                        if (level < 1 || level > 9)
                        {
                            Toast.MakeText(Context, Resources.GetString(Resource.String.WrongAbilityLevel), ToastLength.Short).Show();
                            return;
                        }
                        ability.Level = level;
                    }

                    if (!string.IsNullOrEmpty(levelText.Text))
                    {
                        if (int.TryParse(refNumberText.Text, out int number))
                        {
                            ability.ReferenceNumber = number;
                        }
                        else
                        {
                            ability.ReferenceNumber = 0;
                        }
                    }

                    SaveCharacter();

                    _lv.Adapter = new AbilitiesAdapterOld(Context, Character);
                })
                .Show();
        }

        private void AddEnhancement(LayoutInflater inflater, ListView enhancementListTop, DL_Ability ability, bool isTop)
        {
            var view = inflater.Inflate(Resource.Layout.alertdialog_addenhancement, null);
            var ehanceedittext = view.FindViewById<EditText>(Resource.Id.enhancementEditText1);
            var slotedittext = view.FindViewById<EditText>(Resource.Id.textInputEditText3);

            DL_Enhancement enhan = null;

            ehanceedittext.AfterTextChanged += (se, args) => SpannableTools.AddIcons(Context, args.Editable, ehanceedittext.LineHeight);

            if (!ehanceedittext.HasOnClickListeners)
            {
                ehanceedittext.Click += (x, y) =>
                {
                    var enhancementselectionview = inflater.Inflate(Resource.Layout.alertdialog_listview, null);
                    var listview = enhancementselectionview.FindViewById<ListView>(Resource.Id.listView);

                    listview.ItemsCanFocus = true;
                    listview.ChoiceMode = ChoiceMode.Single;

                    var enhancements = EnhancementRepository.Get();

                    var itemadapter = new AbilitiesEnhancementSelectableAdapter(Context, enhancements);
                    listview.Adapter = itemadapter;

                    new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                        .SetCustomView(enhancementselectionview)
                        .SetTitle("Select enhancement")
                        .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel),
                            (senderAlert, args) => { })
                        .SetPositiveButton("Set selected enhancement", (senderAlert, args) =>
                        {
                            var enhancement = itemadapter.GetSelected();

                            if (enhancement == null) return;
                            if (ability.Enhancements == null)
                                ability.Enhancements = new List<DL_AbilityEnhancement>();

                            enhan = enhancement;
                            ehanceedittext.Text = enhancement.EnhancementCode;
                        })
                        .Show();
                };
            }

            new CustomDialogBuilder(base.Context, Resource.Style.MyDialogTheme)
                .SetCustomView(view)
                .SetTitle("Add enhancement")
                .SetNegativeButton(base.Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .SetPositiveButton("Add", (senderAlert, args) =>
                {
                    if (enhan == null) return;

                    int slotnumber = 0;
                    if (!string.IsNullOrEmpty(slotedittext.Text))
                    {
                        int.TryParse(slotedittext.Text, out slotnumber);
                    }

                    var abEn = new DL_AbilityEnhancement()
                    {
                        Ability = ability,
                        Enhancement = enhan,
                        ID_Ability = ability.Id,
                        ID_Enhancement = enhan.Id,
                        SlotNumber = slotnumber,
                        IsTop = isTop
                    };

                    ability.Enhancements.Add(abEn);

                    SaveCharacter();
                    enhancementListTop.Adapter = new AbilitiesEnhancementDeleteableAdapterOld(Context, ability, isTop);
                })
                .Show();
        }

        /// <summary>
        /// show alert dialog to add a new ability
        /// </summary>
        /// <param name="lv"></param>
        private void AddNewAbility(ListView lv)
        {
            var inflater = Context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_addability, null);
            var refNumberText = convertView.FindViewById<EditText>(Resource.Id.inputText);
            var levelText = convertView.FindViewById<EditText>(Resource.Id.inputNumber);
            var nameText = convertView.FindViewById<EditText>(Resource.Id.abilityName);

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle(Context.Resources.GetString(Resource.String.AddAbilityAlertTitle))
                .SetPositiveButton(Context.Resources.GetString(Resource.String.AddAbility), (senderAlert, args) =>
                {
                    if (string.IsNullOrEmpty(nameText.Text)) return;

                    var newAbility = new DL_Ability()
                    {
                        Character = Character,
                        ID_Character = Character.Id,
                        AbilityName = nameText.Text
                    };

                    if (!string.IsNullOrEmpty(levelText.Text))
                    {
                        int.TryParse(levelText.Text, out int level);
                        if (level < 1 || level > 9)
                        {
                            Toast.MakeText(Context, Resources.GetString(Resource.String.WrongAbilityLevel), ToastLength.Short).Show();
                            return;
                        }
                        newAbility.Level = level;
                    }

                    if (!string.IsNullOrEmpty(levelText.Text))
                    {
                        if (int.TryParse(refNumberText.Text, out int number))
                        {
                            newAbility.ReferenceNumber = number;
                        }
                        else
                        {
                            newAbility.ReferenceNumber = 0;
                        }
                    }

                    Character.Abilities.Add(newAbility);
                    SaveCharacter();
                    lv.Adapter = new AbilitiesAdapterOld(Context, Character);
                })
                .Show();
        }       
    }
}