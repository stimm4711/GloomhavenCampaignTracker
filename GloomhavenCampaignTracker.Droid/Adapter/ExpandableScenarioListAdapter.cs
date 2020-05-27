using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using Object = Java.Lang.Object;
using Android.Content;
using GloomhavenCampaignTracker.Droid.Business;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    /// <summary>
    /// http://www.appliedcodelog.com/2016/06/expandablelistview-in-xamarin-android.html
    /// </summary>
    public class ExpandableScenarioListAdapter : BaseExpandableListAdapter
    {
        private readonly string _completed;
        private readonly string _unlocked;
        private readonly string _blocked; 
        private readonly string _unavailable;
        private readonly Activity _context;
        private bool _isCompleting;

        /// <summary>
        /// header titles
        /// </summary>
        private readonly List<string> _listDataHeader;

        /// <summary>
        /// child data in format of header title, child title
        /// </summary>
        private readonly Dictionary<string, SortedList<int,CampaignUnlockedScenario>> _listDataChild;

        public ExpandableScenarioListAdapter(Activity context, List<string> listDataHeader, Dictionary<string, SortedList<int, CampaignUnlockedScenario>> listChildData)
        {
            _context = context;
            _listDataHeader = listDataHeader;
            _listDataChild = listChildData;

            _unlocked = _context.Resources.GetString(Resource.String.Unlocked);
            _unavailable = _context.Resources.GetString(Resource.String.Unavailable);
            _completed = _context.Resources.GetString(Resource.String.Completed);
            _blocked = _context.Resources.GetString(Resource.String.Blocked);
        }

        /// <summary>
        /// for child item view 
        /// </summary>
        /// <param name="groupPosition"></param>
        /// <param name="childPosition"></param>
        /// <returns></returns>
        public override Object GetChild(int groupPosition, int childPosition)
        {
            return _listDataChild[_listDataHeader[groupPosition]].ElementAt(childPosition).Value;
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public void Add(CampaignUnlockedScenario s)
        {
            if(s.Completed)
            {
                _listDataChild[_completed].Add(s.Scenarionumber, s);
            }
            if (s.IsAvailable())
            {
                _listDataChild[_unlocked].Add(s.Scenarionumber, s);
            }
            else if (s.IsBlocked())
            {
                _listDataChild[_blocked].Add(s.Scenarionumber, s);
            }
            else if (!s.IsAvailable())
            {
                _listDataChild[_unavailable].Add(s.Scenarionumber, s);
            }
        }

        public void Remove(CampaignUnlockedScenario s)
        {
            if (s.Completed)
            {                
                _listDataChild[_completed].Remove(s.Scenarionumber);
            }
            if (s.IsAvailable())
            {
                _listDataChild[_unlocked].Remove(s.Scenarionumber);
            }
            else if (s.IsBlocked())
            {
                _listDataChild[_blocked].Remove(s.Scenarionumber);
            }
            else if (!s.IsAvailable())
            {
                _listDataChild[_unavailable].Remove(s.Scenarionumber);
            }
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            var campScenario = (CampaignUnlockedScenario)GetChild(groupPosition, childPosition);
            
            CampaignScenarioHolder holder = null;           

            if (convertView != null)
                holder = convertView.Tag as CampaignScenarioHolder;                       


            if (holder == null)
            {
                // Create new holder
                holder = new CampaignScenarioHolder();

                convertView = _context.LayoutInflater.Inflate(Resource.Layout.listviewitem_campaignscenario, parent, false);
                holder.ScenarioName = convertView.FindViewById<TextView>(Resource.Id.scenariViewItem_nameTextView);
                holder.ScenarioNumber = convertView.FindViewById<TextView>(Resource.Id.scenariViewItem_numberTextView);
                holder.ScenarioCompleted = convertView.FindViewById<CheckBox>(Resource.Id.scenarioItemView_completedCheckBox);
                holder.OptionsButton = convertView.FindViewById<Button>(Resource.Id.optionsButton);
                holder.TreasureText = convertView.FindViewById<TextView>(Resource.Id.scenarioTreasureStatus);
                holder.Region = convertView.FindViewById<TextView>(Resource.Id.campaignScenarioRegion);

                if(!GCTContext.Settings.IsShowTreasure)
                {
                    holder.TreasureText.Visibility = ViewStates.Gone;
                    var treasureImage = convertView.FindViewById<ImageView>(Resource.Id.treasureImage);
                    treasureImage.Visibility = ViewStates.Gone;
                }

                // Completed CheckCHanged Event
                holder.ScenarioCompleted.CheckedChange += (sender, e) =>
                {
                    var chkBx = (CheckBox)sender;
                    // http://www.appliedcodelog.com/2015/07/working-on-issues-with-listview-in.html
                    var cs = (CampaignUnlockedScenario)chkBx.Tag; //get the tagged position from the Control

                    if (cs == null || cs.Completed == chkBx.Checked) return;
                    if (_isCompleting) return;
                    SetScenarioCompletedStatus(cs, e.IsChecked, chkBx);                      
                };

                // Options Event
                if (!holder.OptionsButton.HasOnClickListeners)
                {
                    holder.OptionsButton.Click += (sender, e) =>
                    {
                        var bttn = (Button)sender;
                        var cs = (CampaignUnlockedScenario)bttn.Tag; //get the tagged position from the Control
                        if (cs == null) return;
                        ShowOptionsButtonPopupMenu(cs, bttn, holder.ScenarioCompleted);
                    };
                }

                // Set Item Click Event
                convertView.Click += (sender, e) =>
                {
                    var v = (View) sender;
                    var chb = v.FindViewById<CheckBox>(Resource.Id.scenarioItemView_completedCheckBox);
                    var cs = (CampaignUnlockedScenario)chb.Tag;

                    ScenarioItemClick(cs);
                };

                convertView.Tag = holder;
            }
            
            // Set Data
            holder.ScenarioName.Text = campScenario.ScenarioName;
            holder.ScenarioNumber.Text = $"# {campScenario.Scenarionumber}";
            holder.ScenarioCompleted.Tag = campScenario;
            holder.ScenarioCompleted.Checked = campScenario.Completed;
            holder.TreasureText.Text = $"{campScenario.GetTreasureText()}";
            holder.OptionsButton.Tag = campScenario;
            holder.Region.Text = Helper.GetRegionName(campScenario.UnlockedScenarioData.Scenario.Region_ID);

            // Set Backgroundcolor
            SetBackgroudColor(groupPosition, convertView);

            return convertView;
        }

        private class CampaignScenarioHolder : Object
        {
            public TextView ScenarioName { get; set; }
            public TextView ScenarioNumber { get; set; }
            public CheckBox ScenarioCompleted { get; set; }
            public TextView TreasureText { get; set; }
            public Button OptionsButton { get; set; }
            public TextView Region { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="campScenario"></param>
        private void ScenarioItemClick(CampaignUnlockedScenario campScenario)
        {
            var intent = new Intent();
            intent.SetClass(_context, typeof(DetailsActivity));
            intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.ScenarioDetails);
            intent.PutExtra(DetailsActivity.SelectedScenarioId, campScenario.UnlockedScenarioData.ID_Scenario);
            _context.StartActivity(intent);
        }

        private void ScenarioCompletion(CheckBox status, CampaignUnlockedScenario campScenario)
        {
            if (status.Checked)
            {
                campScenario.Completed = true;
                var lstUnlockedScenarios = new List<CampaignUnlockedScenario>();
                var currentCampaign = GCTContext.CampaignCollection.CurrentCampaign;
                var unlockedScenarioNumbers = campScenario.GetUnlockedScenarios().Where(x => !currentCampaign.IsScenarioUnlocked(x));
                var lstScenariosWithSection = new List<int>()
                {
                    98,100,99
                };

                if (lstScenariosWithSection.Contains(campScenario.Scenario.ScenarioNumber))
                {
                    // Choose Section for scenario unlock
                    var view = _context.LayoutInflater.Inflate(Resource.Layout.alertdialog_listview, null);
                    var listview = view.FindViewById<ListView>(Resource.Id.listView);
                    listview.ItemsCanFocus = true;
                    listview.ChoiceMode = ChoiceMode.Single;
                    var selectableSections = ScenarioHelper.GetSelectableSection(campScenario.Scenario.ScenarioNumber);

                    var adapter = new ArrayAdapter<string>(_context, Android.Resource.Layout.SimpleListItemSingleChoice, selectableSections.Select(x => $"Section {x}").ToArray());
                    listview.Adapter = adapter;

                    new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                        .SetCustomView(view)
                        .SetTitle(_context.Resources.GetString(Resource.String.SelectSection))
                        .SetMessage(_context.Resources.GetString(Resource.String.SelectSectionNumber))
                        .SetPositiveButton(_context.Resources.GetString(Resource.String.Select), (senderAlert, args) =>
                        {
                            if (listview.CheckedItemPosition == -1) return;

                            var selectedSection = selectableSections.ElementAt(listview.CheckedItemPosition);

                            lstUnlockedScenarios.AddRange(ScenarioHelper.GetUnlockedScenarioBySection(selectedSection));
                            UnlockScenarios(lstUnlockedScenarios, campScenario);
                        })
                        .Show();
                }
                else if (campScenario.Scenario.ScenarioNumber == 13)
                {
                    // Choose the Scenario to unlock

                    // Show dialog with selectable scenarios and radio buttons
                    var view = _context.LayoutInflater.Inflate(Resource.Layout.alertdialog_listview, null);
                    var listview = view.FindViewById<ListView>(Resource.Id.listView);
                    listview.ItemsCanFocus = true;
                    listview.ChoiceMode = ChoiceMode.Single;

                    IEnumerable<Scenario> selectableScenarios = GCTContext.ScenarioCollection.Items.Where(x => unlockedScenarioNumbers.Contains(x.ScenarioNumber));

                    var adapter = new ArrayAdapter<string>(_context, Android.Resource.Layout.SimpleListItemSingleChoice, selectableScenarios.Select(x => $"# {x.ScenarioNumber}   {x.ScenarioName}").ToArray());
                    listview.Adapter = adapter;

                    new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                        .SetCustomView(view)
                        .SetTitle(_context.Resources.GetString(Resource.String.SelectUnlockedScenario))
                        .SetMessage(_context.Resources.GetString(Resource.String.ChooseScenarioToUnlock))
                        .SetPositiveButton(_context.Resources.GetString(Resource.String.UnlockScenario), (senderAlert, args) =>
                        {
                            if (listview.CheckedItemPosition == -1) return;

                            var scenario = selectableScenarios.ElementAt(listview.CheckedItemPosition);

                            if (scenario == null) return;

                            lstUnlockedScenarios.Add(currentCampaign.AddUnlockedScenario(scenario.ScenarioNumber));

                            UnlockScenarios(lstUnlockedScenarios, campScenario);
                        })
                        .Show();
                }
                else 
                {
                    foreach (var scenarioNumber in unlockedScenarioNumbers)
                    {
                        lstUnlockedScenarios.Add(currentCampaign.AddUnlockedScenario(scenarioNumber));
                    }

                    UnlockScenarios(lstUnlockedScenarios, campScenario);
                }
            }
            else
            {
                var removecampScenarios = ScenarioHelper.SetIncomplete(campScenario.UnlockedScenarioData);

                var alertView = _context.LayoutInflater.Inflate(Resource.Layout.alertdialog_listview, null);
                var lv = alertView.FindViewById<ListView>(Resource.Id.listView);
                lv.Adapter = new ArrayAdapter<string>(_context, Android.Resource.Layout.SimpleListItem1, removecampScenarios.Select(x => $"# {x.Scenario.ScenarioNumber}   {x.Scenario.ScenarioName}").ToArray());

                new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                    .SetCustomView(alertView)
                    .SetTitle(String.Format(_context.Resources.GetString(Resource.String.SetScenarioIncomplete), campScenario.ScenarioName))
                    .SetMessage(_context.Resources.GetString(Resource.String.ScenariosWillBeRemoved))
                    .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) =>
                    {
                        if (status != null) status.Checked = true;
                        _isCompleting = false;
                    })
                    .SetPositiveButton(_context.Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                    {
                        if (status != null) status.Checked = false;

                        foreach (var cus in removecampScenarios)
                        {
                            GCTContext.CurrentCampaign.RemoveScenario(cus.UnlockedScenarioData);
                            Remove(cus);
                        }

                        campScenario.Completed = false;
                        campScenario.Save();

                        NotifyDataSetChanged();
                        _isCompleting = false;
                    })
                    .Show();
            }  
        }

        private void UnlockScenarios(List<CampaignUnlockedScenario> lstUnlockedScenarios, CampaignUnlockedScenario campScenario)
        {
            foreach (var cus in lstUnlockedScenarios)
            {
                Add(cus);
            }

            campScenario.Save();

            NotifyDataSetChanged();

            _isCompleting = false;
        }

        private void SetScenarioCompletedStatus(CampaignUnlockedScenario campScenario, bool status, CheckBox checkb)
        {
            _isCompleting = true;

            if (!campScenario.Completed)
            {
                _listDataChild[_unlocked].Remove(campScenario.Scenarionumber);
                _listDataChild[_completed].Add(campScenario.Scenarionumber, campScenario);    

                new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                  .SetTitle("Scenarios completed!")
                  .SetMessage($"You've completed the scenarion. Do you want to enter rewards now?")
                  .SetPositiveButton(_context.Resources.GetString(Resource.String.Yes), (senderAlert, AssemblyLoadEventArgs) =>
                  {
                      var intent = new Intent(_context, typeof(DetailsActivity));                     
                      intent.PutExtra(DetailsActivity.SelectedScenarioId, campScenario.ScenarioId);
                      intent.PutExtra(DetailsActivity.CasualMode, false);
                      intent.PutExtra(DetailsActivity.SelectedFragId, (int)DetailFragmentTypes.ScenarioRewards);
                      _context.StartActivity(intent);
                  })
                  .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) =>
                  {
                      ScenarioCompletion(checkb, campScenario);                     
                      NotifyDataSetChanged();
                  })
                  .Show();
            }
            else
            {
                _listDataChild[_completed].Remove(campScenario.Scenarionumber);
                _listDataChild[_unlocked].Add(campScenario.Scenarionumber, campScenario);

                ScenarioCompletion(checkb, campScenario);
                SetIncomplete(campScenario);
                NotifyDataSetChanged();
            }           
        }

        internal HashSet<CampaignUnlockedScenario> SetIncomplete(CampaignUnlockedScenario cs, HashSet<CampaignUnlockedScenario> removededScenarios = null)
        {
            if (removededScenarios == null) removededScenarios = new HashSet<CampaignUnlockedScenario>();

            if (!cs.Completed) return removededScenarios;

            var unlockedScenarioNumbers = cs.GetUnlockedScenarios();
            var currentCampaign = GCTContext.CampaignCollection.CurrentCampaign;

            var followingScenarios = new List<CampaignUnlockedScenario>();
            foreach (var scenarioNumber in unlockedScenarioNumbers)
            {
                // Check if the scenario was unlocked by any other completed scenario
                if (_listDataChild[_completed].Any(x => x.Value.Scenarionumber != cs.Scenarionumber && 
                                                        !removededScenarios.Contains(x.Value) && 
                                                        x.Value.GetUnlockedScenarios().Contains(scenarioNumber))) continue;

                var campScenarioList = _listDataChild.Values.FirstOrDefault(x=>x.Any(y=>y.Value.Scenarionumber == scenarioNumber));
                if(campScenarioList != null)
                {
                    var campScenario = campScenarioList.FirstOrDefault(y => y.Value.Scenarionumber == scenarioNumber);

                    if (campScenario.Value != null)
                    {
                        removededScenarios.Add(campScenario.Value);

                        foreach (var cus in SetIncomplete(campScenario.Value, removededScenarios))
                        {
                            removededScenarios.Add(cus);
                        }
                    }
                }               
            }

            return removededScenarios;
        }

        /// <summary>
        /// Show opions button menu
        /// </summary>
        /// <param name="campScenario"></param>
        /// <param name="optionsButton"></param>
        private void ShowOptionsButtonPopupMenu(CampaignUnlockedScenario campScenario, Button optionsButton, CheckBox check)
        {
            // open a popup menu with delete option
            var menu = new PopupMenu(_context, optionsButton);
            Helper.ForcePopupmenuToShowIcons(menu);
            menu.Inflate(Resource.Menu.scenarioPopupMenu);

            var completedItem = menu.Menu.FindItem(Resource.Id.sc_popup_completed);
            var undoCompletedItem = menu.Menu.FindItem(Resource.Id.sc_popup_undo_completed);

            completedItem.SetEnabled(campScenario.IsAvailable());
            completedItem.SetVisible(!campScenario.Completed);
            undoCompletedItem.SetVisible(campScenario.Completed);
           
            menu.MenuItemClick += (sender, args) =>
            {
                switch (args.Item.ItemId)
                {
                    case Resource.Id.sc_popup_delete:
                        // delete scenario
                        ConfirmDeleteDialog(campScenario);
                        break;
                    case Resource.Id.sc_popup_completed:
                        // scenario completed
                        SetScenarioCompletedStatus(campScenario, true, check);
                        break;
                    case Resource.Id.sc_popup_undo_completed:
                        // scenario undo complete
                        SetScenarioCompletedStatus(campScenario, false, check);
                        break;
                    case Resource.Id.sc_popup_treasures:
                        // Show Treasures                        
                        ShowTreasuresDialog(campScenario);
                        break;
                }
            };

            menu.Show();
        }

        #region "Treasures"

        /// <summary>
        /// Show Treasure dialog for a scenario
        /// </summary>
        /// <param name="campScenario"></param>
        private void ShowTreasuresDialog(CampaignUnlockedScenario campScenario)
        {            
            var alertView = _context.LayoutInflater.Inflate(Resource.Layout.alertdialog_listview_floatingactionbutton, null);
            var fab = alertView.FindViewById<FloatingActionButton>(Resource.Id.fab);
            var lv = alertView.FindViewById<ListView>(Resource.Id.listView);
            TreasureAdapter treasureAdapter = new TreasureAdapter(_context, campScenario, this);
            lv.Adapter = treasureAdapter;

            if (!fab.HasOnClickListeners)
            {
                fab.Click += (senderx, ex) =>
                {
                    AddNewTreasureDialog(campScenario, lv);
                };
            }

            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
               .SetCustomView(alertView)
                .SetTitle(string.Format(_context.Resources.GetString(Resource.String.Treasures), campScenario.ScenarioName))
                .SetPositiveButton(_context.Resources.GetString(Resource.String.Close), (senderAlert, args) => { })
                .Show();
        }

        /// <summary>
        /// Add a new treasure to scenario
        /// </summary>
        /// <param name="campScenario"></param>
        /// <param name="lv"></param>
        private void AddNewTreasureDialog(CampaignUnlockedScenario campScenario, ListView lv)
        {
            var convertView = _context.LayoutInflater.Inflate(Resource.Layout.alertdialog_addtreasure, null);
            var refNumberText = convertView.FindViewById<EditText>(Resource.Id.treasure_ref_number);
            var content = convertView.FindViewById<EditText>(Resource.Id.treasure_content);

            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle(_context.Resources.GetString(Resource.String.AddTreasure))
                .SetPositiveButton(_context.Resources.GetString(Resource.String.AddTreasureAlertTitle), (senderAlert, args) =>
                {
                    if (int.TryParse(refNumberText.Text, out int refNumber))
                    {
                        campScenario.AddTreasure(refNumber, content.Text);
                        NotifyDataSetChanged();
                        lv.Adapter = new TreasureAdapter(_context, campScenario, this);
                    }

                    campScenario.Save();
                })
                .Show();
        }

        #endregion

        /// <summary>
        /// Sets the background color for scenario items by their status
        /// </summary>
        /// <param name="groupPosition"></param>
        /// <param name="view"></param>
        private void SetBackgroudColor(int groupPosition, View view)
        {
            var chk = view.FindViewById<CheckBox>(Resource.Id.scenarioItemView_completedCheckBox);

            var color = ContextCompat.GetColor(_context, Resource.Color.gloom_primaryLighter);
            var enableCheckbox = true;

            if (groupPosition != 0)
            {
                enableCheckbox = false;

                switch (groupPosition)
                {
                    case 3:
                        // Blocked = red
                        color = ContextCompat.GetColor(_context, Resource.Color.gloom_scenarioBlockedItemBackground);
                        break;
                    case 2:
                        // Completed = green
                        enableCheckbox = true;
                        color = ContextCompat.GetColor(_context, Resource.Color.gloom_scenarioCompletedItemBackground);
                        break;
                    default:
                        // unavailable blue
                        color = ContextCompat.GetColor(_context, Resource.Color.gloom_scenarioUnavailableItemBackground);
                        break;
                }
            }

            view.SetBackgroundColor(new Color(color));
            chk.Enabled = enableCheckbox;
        }

        /// <summary>
        /// Confirm delete
        /// </summary>
        /// <param name="campscenario"></param>
        private void ConfirmDeleteDialog(CampaignUnlockedScenario campscenario)
        {
            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetMessage(string.Format(_context.Resources.GetString(Resource.String.DeleteUnlockedScenario), campscenario.ScenarioName))
                .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
                {
                    GCTContext.CurrentCampaign.RemoveUnlockedScenario(campscenario);                    
                    Remove(campscenario);
                    NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return _listDataChild[_listDataHeader[groupPosition]].Count;
        }

        //For header view
        public override Object GetGroup(int groupPosition)
        {
            return _listDataHeader[groupPosition];
        }

        public override int GroupCount => _listDataHeader.Count;

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var headerTitle = (string)GetGroup(groupPosition);

            convertView = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.listview_expandable_groupheader_customlayout, null);
            var lblListHeader = (TextView)convertView.FindViewById(Resource.Id.lblListHeader);
            lblListHeader.Text = $"{headerTitle} ({GetChildrenCount(groupPosition)})";

            return convertView;
        }

        public override bool HasStableIds => false;

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}