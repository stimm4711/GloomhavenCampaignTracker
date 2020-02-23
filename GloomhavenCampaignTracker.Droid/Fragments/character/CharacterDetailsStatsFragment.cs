using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System;
using System.Collections.Generic;
using Android.Graphics;
using Android.Support.V4.Content;
using System.Linq;
using GloomhavenCampaignTracker.Droid.Views;
using static GloomhavenCampaignTracker.Droid.Views.PersonalQuestCounterView;
using GloomhavenCampaignTracker.Business;
using System.Threading.Tasks;
using System.Net.Http;
using Plugin.Connectivity;

namespace GloomhavenCampaignTracker.Droid.Fragments.character
{
    public class CharacterDetailsStatsFragment : CharacterDetailFragmentBase
    {
        private EditText _levelEditText;
        private EditText _xpEditText;
        private EditText _goldEditText;       
        private EditText _notes;
        private EditText _lifegoalnumber;
        private readonly Dictionary<int, CheckBox> _checkmarks = new Dictionary<int, CheckBox>();
        private ImageButton _retireButton;
        private readonly Dictionary<int, int> _xpToLevelThresholds = new Dictionary<int, int>();
        private ImageView _levelUpImage;
        private TextView _xptonextlevel;
        private ImageView _donateToSactuaryButton;
        private LinearLayout _linearLayoutPersonalQuest;

        internal static CharacterDetailsStatsFragment NewInstance(DL_Character character)
        {
            var frag = new CharacterDetailsStatsFragment() { Arguments = new Bundle() };
            frag.Arguments.PutInt(charID, character.Id);
            return frag;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            if (_xpToLevelThresholds != null && _xpToLevelThresholds.Count == 0)
            {
                _xpToLevelThresholds.Add(2, 45);
                _xpToLevelThresholds.Add(3, 95);
                _xpToLevelThresholds.Add(4, 150);
                _xpToLevelThresholds.Add(5, 210);
                _xpToLevelThresholds.Add(6, 275);
                _xpToLevelThresholds.Add(7, 345);
                _xpToLevelThresholds.Add(8, 420);
                _xpToLevelThresholds.Add(9, 500);
            }

            if (Character == null)
            {
                var charID = savedInstanceState.GetInt("CharacterID");
                Character = CharacterRepository.Get(charID, recursive: true);
            }

            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            _view = inflater.Inflate(Resource.Layout.fragment_characterdetail_detail, container, false);
                       
            _levelEditText = _view.FindViewById<EditText>(Resource.Id.level);
            _xpEditText = _view.FindViewById<EditText>(Resource.Id.xp);
            _goldEditText = _view.FindViewById<EditText>(Resource.Id.gold);
            _notes = _view.FindViewById<EditText>(Resource.Id.notestext);
            _lifegoalnumber = _view.FindViewById<TextInputEditText>(Resource.Id.characterfrag_characterlivegoalid);

            _retireButton = _view.FindViewById<ImageButton>(Resource.Id.retireButton);

            _levelUpImage = _view.FindViewById<ImageView>(Resource.Id.levelupimage);
            _xptonextlevel = _view.FindViewById<TextView>(Resource.Id.xptonextlevel);

            _donateToSactuaryButton = _view.FindViewById<ImageView>(Resource.Id.donateicon);

            _linearLayoutPersonalQuest = _view.FindViewById<LinearLayout>(Resource.Id.linearLayoutPersonalQuest);

            LoadCheckmarks();
            SetCheckmarkEvents();

            _lifegoalnumber.Text = "";

            if (Character != null)
            {
                _levelEditText.Text = $"{Character.Level}";
                _xpEditText.Text = $"{Character.Experience}";
                _goldEditText.Text = $"{Character.Gold}";
                _notes.Text = Character.Notes;

                SetRetiredButton();
                SetCheckmarks();
                CheckForlevelUp();

                _retireButton.Enabled = Character.PersonalQuest != null;
            }

            _levelEditText.FocusChange += _levelEditText_FocusChange1; 
            _xpEditText.FocusChange += _xpEditText_FocusChange;
            _notes.FocusChange += _notes_FocusChange;
            _goldEditText.FocusChange += _goldEditText_FocusChange;
            
            if (_lifegoalnumber != null && !_lifegoalnumber.HasOnClickListeners)
            {
                _lifegoalnumber.Click += LgButton_Click;
            }

            if (_retireButton != null && !_retireButton.HasOnClickListeners)
            {
                _retireButton.Click += RetireButton_Click;
            }

            if (_levelUpImage != null && !_levelUpImage.HasOnClickListeners)
            {
                _levelUpImage.Click += _levelUpImage_Click;
            }

            if(_donateToSactuaryButton != null && !_donateToSactuaryButton.HasOnClickListeners)
            {
                _donateToSactuaryButton.Click += _donateToSactuaryButton_Click;
            }

            return _view;
        }

        public override void OnResume()
        {
            base.OnResume();
            if (Character != null)
            {
                ShowPersonalQuest();
            }
        }

        private void _donateToSactuaryButton_Click(object sender, EventArgs e)
        {
            if(Character.Gold >= 10)
            {
                if (Character.Party != null && Character.Party.ID_Campaign == GCTContext.CurrentCampaign.CampaignData.Id)
                {
                    new CustomDialogBuilder(base.Context, Resource.Style.MyDialogTheme)
                       .SetTitle(Resources.GetString(Resource.String.CharacterStat_DonateToTheSanctuary))
                       .SetMessage(Resources.GetString(Resource.String.CharacterStat_DonateToTheSanctuary))
                       .SetPositiveButton(Resources.GetString(Resource.String.CharacterStat_DonateTenGold), (senderAlert, args) =>
                       {
                           if (GCTContext.CurrentCampaign.CampaignData.DonatedGold == 90 && !GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeBUnlocked)
                           {
                               new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                                   .SetTitle(Resources.GetString(Resource.String.Congratulation))
                                   .SetMessage(Resources.GetString(Resource.String.CharacterStat_UnlockedENvelopeBByDonatingHundredGold))
                                   .SetPositiveButton(Resources.GetString(Resource.String.CharacterStat_DonateAndReveal), (s, a) => {
                                       GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeBUnlocked = true;
                                       DoDonation();
                                   })
                                   .SetNegativeButton(Resources.GetString(Resource.String.CharacterStat_DontDonate), (s, a) =>
                                   { })
                                   .Show();
                           }
                           else
                           {
                               DoDonation();
                           }
                       })
                       .SetNegativeButton(Resources.GetString(Resource.String.Close), (senderAlert, args) => { })
                       .Show();                            
                }              
            }
            else
            {
                Toast.MakeText(Context, Resources.GetString(Resource.String.CharacterStat_NeedsTenGold), ToastLength.Long).Show();
            }
        }

        private void DoDonation()
        {
            Character.Gold -= 10;

            var text = String.Format(Resources.GetString(Resource.String.CharacterStat_donated), Character.Name);

            if (Helper.GetNextDonationValueForProsperity(GCTContext.CurrentCampaign.CampaignData.DonatedGold) == GCTContext.CurrentCampaign.CampaignData.DonatedGold + 10)
            {
                text += Resources.GetString(Resource.String.CharacterStat_ProsperityRaised);
                GCTContext.CurrentCampaign.CityProsperity += 1;
            }

            Toast.MakeText(Context, text, ToastLength.Long).Show();

            GCTContext.CurrentCampaign.AddDonationToTheSanctuary();

            GCTContext.CurrentCampaign.Save(false);
            _goldEditText.Text = Character.Gold.ToString();
            SaveCharacter();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("CharacterID", Character.Id);
    
            base.OnSaveInstanceState(outState);
        }

        private void _levelUpImage_Click(object sender, EventArgs e)
        {
            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetTitle($"Level {Character.Level +1} available!")
                .SetMessage("You earned enough experience to reach a new level! The next time you return to Gloomhaven select a new ability and perk!")
                .SetPositiveButton("Level up now", (senderAlert, args) =>
                {
                    Character.Level = Character.Level+1;
                    _levelEditText.Text = Character.Level.ToString();
                    CheckForlevelUp();
                    SaveCharacter();
                })
                .SetNegativeButton("Later", (senderAlert, args) => { })
                .Show();
        }

        private void ShowPersonalQuest()
        {
            if (Character?.PersonalQuest == null) return;
            _lifegoalnumber.Text = GCTContext.Settings.IsShowPq ? $"# {Character.PersonalQuest.QuestNumber}   {Character.PersonalQuest.QuestName}" : $"# {Character.PersonalQuest.QuestNumber}";

            if(GCTContext.Settings.IsShowPq)
            {
                _linearLayoutPersonalQuest.RemoveAllViews();

                foreach (var pqc in Character.CharacterQuestCounters.Where(x => x.PersonalQuestCounter.PersonalQuest_ID == Character.PersonalQuest.Id))
                {
                    if(pqc.PersonalQuestCounter.CounterValue > 1)
                    {
                        var cpqcLayout = new PersonalQuestCounterView(Context);

                        if(pqc.PersonalQuestCounter.CounterScenarioUnlock > 50)
                        {
                            cpqcLayout.ThresholdReached += CpqcLayout_ThresholdReached;
                            cpqcLayout.ThresholdNotReached += CpqcLayout_ThresholdNotReached;
                        }

                        cpqcLayout.SetCounter(pqc);

                        _linearLayoutPersonalQuest.AddView(cpqcLayout);
                    }
                    else
                    {
                        var counterForUnlockingScenario = 
                            Character.CharacterQuestCounters.FirstOrDefault(x => x.PersonalQuestCounter.PersonalQuest_ID == pqc.PersonalQuestCounter.PersonalQuest_ID && 
                                                                                 x.PersonalQuestCounter.CounterScenarioUnlock > 50);

                        if(counterForUnlockingScenario == null || counterForUnlockingScenario.Value == counterForUnlockingScenario.PersonalQuestCounter.CounterValue)
                        {
                            var cpqcLayout = new PersonalQuestCheckView(Context);
                            _linearLayoutPersonalQuest.AddView(cpqcLayout);
                            cpqcLayout.SetCounter(pqc);                           
                        }                       
                    }                   
                }
            }           
        }

        private void CpqcLayout_ThresholdNotReached(object sender, ThresholdReachedEventArgs e)
        {
            var cus = GCTContext.CurrentCampaign.CampaignData.UnlockedScenarios.FirstOrDefault(x=>x.Scenario.Scenarionumber == e.ScenarioUnlocked);

            if (cus != null)
            {
                Toast.MakeText(Context, $"Unlocked scenario #{e.ScenarioUnlocked} is locked again.", ToastLength.Long).Show();
                GCTContext.CurrentCampaign.RemoveScenario(cus);
            }
            
            ShowPersonalQuest();
        }

        private void CpqcLayout_ThresholdReached(object sender, ThresholdReachedEventArgs e)
        {
            Toast.MakeText(Context, $"You have unlocked scenario #{e.ScenarioUnlocked} for your personal quest.", ToastLength.Long).Show();
            GCTContext.CurrentCampaign.AddUnlockedScenario(e.ScenarioUnlocked);
            ShowPersonalQuest();
        }

        private void _goldEditText_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus) return;
            if (string.IsNullOrEmpty(_goldEditText.Text)) return;
            if (!int.TryParse(_goldEditText.Text, out int newGold)) return;
            if (newGold == Character.Gold) return;

            if (newGold < 0)
            {
                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                    .SetTitle("Unvalid value")
                    .SetMessage("The gold can not be less than 0.")
                    .SetPositiveButton(Context.Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                    {
                        newGold = 0;
                        Character.Gold = newGold;
                        _goldEditText.Text = Character.Gold.ToString();
                    })
                    .Show();
            }
            else
            {
                Character.Gold = newGold;
            }

            SaveCharacter();
        }

        private void _notes_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus) return;
            Character.Notes = _notes.Text;
            SaveCharacter();
        }
        
        private void _xpEditText_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus) return;
            if (string.IsNullOrEmpty(_xpEditText.Text)) return;
            if (!int.TryParse(_xpEditText.Text, out int newXp)) return;
            if (newXp < 0 || newXp > 500)
            {
                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                    .SetTitle("Unvalid value")
                    .SetMessage("The character experience must be between 0 and 500.")
                    .SetPositiveButton(Context.Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                    {
                        if (newXp < 0)
                        {
                            newXp = 0;
                        }
                        else if (newXp > 500)
                        {
                            newXp = 500;
                        }
                        Character.Experience = newXp;
                        _xpEditText.Text = Character.Experience.ToString();
                    })
                    .Show();
            }
            else
            {
                Character.Experience = newXp;

                CheckForlevelUp();
            }

            SaveCharacter();
        }

        private void CheckForlevelUp()
        {

            if (Character.Level > 8)
            {
                _levelUpImage.Visibility = ViewStates.Gone;
                _xptonextlevel.Text = "";
            }
            else
            {
                if (!_xpToLevelThresholds.ContainsKey(Character.Level + 1)) return;
                if (_levelUpImage == null) return;

                var nextLevelXpNeeded = _xpToLevelThresholds[Character.Level + 1];

                _levelUpImage.Visibility = Character.Experience >= nextLevelXpNeeded ? ViewStates.Visible : ViewStates.Gone;

                _xptonextlevel.Text = $"/ {nextLevelXpNeeded}";
            }           
        }

        private void _levelEditText_FocusChange1(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus) return;
            if (string.IsNullOrEmpty(_levelEditText.Text)) return;
            if (!int.TryParse(_levelEditText.Text, out int newLevel)) return;
            if (newLevel < 1 || newLevel > 9)
            {
                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                    .SetTitle("Unvalid value")
                    .SetMessage("The character level must be between 1 and 9.")
                    .SetPositiveButton(Context.Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                    {
                        if (newLevel < 1)
                        {
                            newLevel = 1;
                        }
                        else if (newLevel > 9)
                        {
                            newLevel = 9;
                        }
                        Character.Level = newLevel;
                        _levelEditText.Text = Character.Level.ToString();
                    })
                    .Show();
            }
            else
            {
                Character.Level = newLevel;
                CheckForlevelUp();
            }

            SaveCharacter();
        }

        private void SetRetiredButton()
        {
            if (Character.PersonalQuest == null) return;
            if (Character.Retired)
            {
                _retireButton.SetImageResource(Resource.Drawable.ic_retired_black);
                var color = ContextCompat.GetColor(Context, Resource.Color.gloom_rowBackground);
                _retireButton.SetBackgroundColor(new Color(color));
            }
            else
            {
                _retireButton.SetImageResource(Resource.Drawable.ic_flag_white_24dp);
                var color = ContextCompat.GetColor(Context, Resource.Color.gloom_secondary);
                _retireButton.SetBackgroundColor(new Color(color));
            }
        }

        private void RetireButton_Click(object sender, EventArgs e)
        {
            if (Character == null) return;
            if (Character.Retired)
            {
                // open alert
                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                    .SetTitle(Context.Resources.GetString(Resource.String.Confirm))
                    .SetMessage($"Call {Character.Name} back to work?")
                    .SetPositiveButton("There is more to do!", (senderAlert, args) =>
                    {
                        Character.Retired = false;
                        SaveCharacter();

                        if (Character.Party != null && DataServiceCollection.CampaignDataService.GetRetiredCharacters(Character.Party.ID_Campaign).Count == 0)
                        {
                            GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked = false;
                            CampaignUnlocksRepository.InsertOrReplace(GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks);
                        }            

                        Toast.MakeText(Context, string.Format($"{Character.Name} is ready for new adventures"), ToastLength.Short).Show();
                        SetRetiredButton();

                    })
                    .SetNegativeButton(Context.Resources.GetString(Resource.String.NotNow), (senderAlert, args) => { })
                    .Show();
            }
            else
            {
                //open alert
                new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                    .SetTitle(Context.Resources.GetString(Resource.String.Confirm))
                    .SetMessage($"Do you really want to retire {Character.Name}?")
                    .SetPositiveButton("Retire", (senderAlert, args) =>
                    {
                        Character.Retired = true;
                        if (Character.Party != null && DataServiceCollection.CampaignDataService.GetRetiredCharacters(Character.Party.ID_Campaign).Count == 0
                            && Character.Party.ID_Campaign == GCTContext.CurrentCampaign.CampaignData.Id
                            && !GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked)
                        {
                            FirstCharacterRetiredDialog();
                        }

                        SaveCharacter();

                        Toast.MakeText(Context, string.Format(Context.Resources.GetString(Resource.String.CharacterRetired), Character.Name), ToastLength.Short).Show();
                        SetRetiredButton();

                    })
                    .SetNegativeButton(Context.Resources.GetString(Resource.String.NotNow), (senderAlert, args) => { })
                    .Show();
            }            
        }

        /// <summary>
        /// Show an unlock alert if first character retires
        /// </summary>
        private void FirstCharacterRetiredDialog()
        {
            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetTitle(Context.Resources.GetString(Resource.String.Congratulation))
                .SetMessage(Context.Resources.GetString(Resource.String.UnlockedTownRecordBook))
                .SetPositiveButton(Context.Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                {
                    if (GCTContext.CurrentCampaign != null)
                    {
                        GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.TownRecordsBookUnlocked = true;
                        CampaignUnlocksRepository.InsertOrReplace(GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks);
                    }
                })
                .Show();
        }

        private void LgButton_Click(object sender, EventArgs e)
        {
            var inflater = LayoutInflater;
            var view = inflater.Inflate(Resource.Layout.alertdialog_personalquest_spinner_imageview, null);
            var connected = CrossConnectivity.Current.IsConnected;

            if (!connected)
            {
                // not connected to internet
                view = inflater.Inflate(Resource.Layout.alertdialog_personalquest_spinner, null);
            }

            var showPqImage = view.FindViewById<ImageButton>(Resource.Id.showpqimagebutton);
            var spinner = view.FindViewById<Spinner>(Resource.Id.personalquestselectionspinner);
            var pqimage = view.FindViewById<ImageView>(Resource.Id.itemimage);
            var goalText = view.FindViewById<TextView>(Resource.Id.goaltextview);
            var rewardtext = view.FindViewById<TextView>(Resource.Id.rewardtextview);
            var classImageView = view.FindViewById<ImageView>(Resource.Id.classImageView);

            SetPqSpinnerData(spinner, GCTContext.Settings.IsShowPq, true);

            if (!GCTContext.Settings.IsShowPq)
            {
                if (connected)
                {
                    pqimage.Visibility = ViewStates.Invisible;
                }
                else
                {
                    goalText.Visibility = ViewStates.Invisible;
                    rewardtext.Visibility = ViewStates.Invisible;
                    classImageView.Visibility = ViewStates.Invisible;                   
                }
               
                showPqImage.Visibility = ViewStates.Visible;
                var color = ContextCompat.GetColor(Context, Resource.Color.gloom_invisibleBackground);
                showPqImage.SetBackgroundColor(new Color(color));

                if (!showPqImage.HasOnClickListeners)
                {
                    showPqImage.Click += (s, ex) =>
                    {
                        var visible = true;
                        if (connected)
                        {
                            if (pqimage.Visibility == ViewStates.Invisible)
                            {                              
                                pqimage.Visibility = ViewStates.Visible;                                
                            }
                            else
                            {
                                pqimage.Visibility = ViewStates.Invisible;
                                visible = false;
                            }
                            SetPqSpinnerData(spinner, pqimage.Visibility == ViewStates.Visible, false);
                        }
                        else
                        {
                            if (goalText.Visibility == ViewStates.Invisible)
                            {
                                goalText.Visibility = ViewStates.Visible;
                                rewardtext.Visibility = ViewStates.Visible;
                                classImageView.Visibility = ViewStates.Visible;
                            }
                            else
                            {
                                goalText.Visibility = ViewStates.Invisible;
                                rewardtext.Visibility = ViewStates.Invisible;
                                classImageView.Visibility = ViewStates.Invisible;
                                visible = false;
                            }
                            SetPqSpinnerData(spinner, goalText.Visibility == ViewStates.Visible, false);                            
                        }    

                        if(visible)
                        {
                            color = ContextCompat.GetColor(Context, Resource.Color.gloom_secondary);
                            showPqImage.SetBackgroundColor(new Color(color));
                        }
                        else
                        {
                            color = ContextCompat.GetColor(Context, Resource.Color.gloom_invisibleBackground);
                            showPqImage.SetBackgroundColor(new Color(color));
                        }
                    }; 
                }
            }
            else
            {
                showPqImage.Visibility = ViewStates.Invisible;
            }

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(view)
                .SetTitle(Context.Resources.GetString(Resource.String.PersonalQuest))
                .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .SetPositiveButton(Context.Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                {
                    var pq = (DL_PersonalQuest)((PersonalQuestAdapter)spinner.Adapter).GetItem(spinner.SelectedItemPosition);
                    if (pq == null) return;
                    pq = PersonalQuestRepository.Get(pq.Id);

                    if(Character.PersonalQuest == null || pq.Id != Character.ID_PersonalQuest)
                    {
                        Character.PersonalQuest = pq;

                        foreach (var counter in PersonalQuestCountersRepository.Get(false).Where(x => x.PersonalQuest_ID == Character.PersonalQuest.Id))
                        {
                            if(!Character.CharacterQuestCounters.Any(x=>x.Character_Id == Character.Id && x.PersonalQuestCounter_Id == counter.Id))
                            {
                                var item = new DL_CharacterPersonalQuestCounter
                                {
                                    Character_Id = Character.Id,
                                    PersonalQuestCounter_Id = counter.Id,
                                    Character = Character,
                                    PersonalQuestCounter = counter,
                                    Value = 0
                                };

                                Character.CharacterQuestCounters.Add(item);
                            }                           
                        }

                        SaveCharacter();
                        ShowPersonalQuest();
                        _retireButton.Enabled = Character.PersonalQuest != null;
                        SetRetiredButton();
                    }                   
                })
                .Show();

            spinner.ItemSelected += (s, e2) =>
            {
                var pq = (DL_PersonalQuest)((PersonalQuestAdapter)spinner.Adapter).GetItem(e2.Position);
                if (pq == null) return;

                pq = PersonalQuestRepository.Get(pq.Id);

                if (connected)
                {
                    var imageBitmap = GetImageBitmapFromUrlAsync("https://raw.githubusercontent.com/stimm4711/gloomhaven/master/images/personal-goals/pg-" + pq.QuestNumber + ".png", pqimage, view);
                }
                else
                {
                    goalText.Text = pq.QuestGoal;
                    if (pq.QuestReward == "class")
                    {
                        if (GCTContext.Settings.IsShowPq) classImageView.Visibility = ViewStates.Visible;

                        classImageView.SetImageResource(ResourceHelper.GetClassIconWhiteSmallRessourceId(pq.QuestRewardClassId - 1));
                        rewardtext.Text = "Character";
                    }
                    else
                    {
                        classImageView.Visibility = ViewStates.Gone;
                        rewardtext.Text = $"{pq.QuestReward}";
                    }
                }                    
            };    
        }

        private async Task<Bitmap> GetImageBitmapFromUrlAsync(string url, ImageView imagen, View view)
        {
            Bitmap imageBitmap = null;

            using (var httpClient = new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            imagen.SetImageBitmap(imageBitmap);

            view.FindViewById<ProgressBar>(Resource.Id.loadingPanel).Visibility = ViewStates.Gone;

            return imageBitmap;
        }

        private void SetPqSpinnerData(Spinner spinner, bool showDetails, bool init)
        {            
            if (spinner == null) return;

            var pq = DataServiceCollection.CharacterDataService.GetPersonalQuestsFlat();

            var adapter = new PersonalQuestAdapter(Context, pq, showDetails);

            var indexQuest = 0;

            if(init)
            {
                if (Character.PersonalQuest != null)
                {
                    var pqSearch = new PersonalQuestSearch(Character.PersonalQuest.Id);
                    indexQuest = pq.FindIndex(pqSearch.FindId);
                }
            }
            else
            {
                indexQuest = spinner.SelectedItemPosition;
            }                    

            spinner.Adapter = adapter;
            spinner.SetSelection(indexQuest);
        }

        private class PersonalQuestSearch
        {
            private readonly int _id;

            public PersonalQuestSearch(int id)
            {
                _id = id;
            }

            public bool FindId(DL_PersonalQuest pq)
            {
                return pq.Id == _id;
            }
        }

        public void LoadCheckmarks()
        {
            _checkmarks.Clear();

            _checkmarks.Add(1, _view.FindViewById<CheckBox>(Resource.Id.checkMark1));
            _checkmarks.Add(2, _view.FindViewById<CheckBox>(Resource.Id.checkMark2));
            _checkmarks.Add(3, _view.FindViewById<CheckBox>(Resource.Id.checkMark3));
            _checkmarks.Add(4, _view.FindViewById<CheckBox>(Resource.Id.checkMark4));
            _checkmarks.Add(5, _view.FindViewById<CheckBox>(Resource.Id.checkMark5));
            _checkmarks.Add(6, _view.FindViewById<CheckBox>(Resource.Id.checkMark6));
            _checkmarks.Add(7, _view.FindViewById<CheckBox>(Resource.Id.checkMark7));
            _checkmarks.Add(8, _view.FindViewById<CheckBox>(Resource.Id.checkMark8));
            _checkmarks.Add(9, _view.FindViewById<CheckBox>(Resource.Id.checkMark9));
            _checkmarks.Add(10, _view.FindViewById<CheckBox>(Resource.Id.checkMark10));
            _checkmarks.Add(11, _view.FindViewById<CheckBox>(Resource.Id.checkMark11));
            _checkmarks.Add(12, _view.FindViewById<CheckBox>(Resource.Id.checkMark12));
            _checkmarks.Add(13, _view.FindViewById<CheckBox>(Resource.Id.checkMark13));
            _checkmarks.Add(14, _view.FindViewById<CheckBox>(Resource.Id.checkMark14));
            _checkmarks.Add(15, _view.FindViewById<CheckBox>(Resource.Id.checkMark15));
            _checkmarks.Add(16, _view.FindViewById<CheckBox>(Resource.Id.checkMark16));
            _checkmarks.Add(17, _view.FindViewById<CheckBox>(Resource.Id.checkMark17));
            _checkmarks.Add(18, _view.FindViewById<CheckBox>(Resource.Id.checkMark18));
        }

        public void SetCheckmarkEvents()
        {
            for (var i = 1; i <= 18; i++)
            {
                var checkmark = _checkmarks[i];
                
                checkmark.Click +=  (sender, e) =>
                {
                    if (checkmark.Checked)
                    {
                        if (Character.Checkmarks < 18) Character.Checkmarks++;
                    }
                    else
                    {
                        if (Character.Checkmarks > 0) Character.Checkmarks--;                           
                    }

                    SaveCharacter();

                    SetCheckmarks();
                };
            }
        }

        public void SetCheckmarks()
        {
            for (var i = 0; i <= Character.Checkmarks; i++)
            {
                if (i <= 0) continue;
                _checkmarks[i].Checked = true;
                _checkmarks[i].Enabled = false;
            }

            if (Character.Checkmarks > 0)
            {
                _checkmarks[Character.Checkmarks].Enabled = true;

                if (Character.Checkmarks < 18)
                {
                    _checkmarks[Character.Checkmarks+1].Enabled = true;
                }
            }                       
            
            if (Character.Checkmarks < 17 && Character.Checkmarks >= 0 && _checkmarks[Character.Checkmarks +2].Enabled)
            {
                _checkmarks[Character.Checkmarks + 2].Enabled = false;
            }
        }

        public void Update()
        {
            if (Character == null) return;
            if (_view == null) return;
            var goldEditText = _view.FindViewById<EditText>(Resource.Id.gold);
            goldEditText.Text = Character.Gold.ToString();
        }
    }
}