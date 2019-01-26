using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using Object = Java.Lang.Object;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

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
        private readonly Dictionary<string, List<CampaignUnlockedScenario>> _listDataChild;

        public ExpandableScenarioListAdapter(Activity context, List<string> listDataHeader, Dictionary<string, List<CampaignUnlockedScenario>> listChildData)
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
            return _listDataChild[_listDataHeader[groupPosition]][childPosition];
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public void Add(CampaignUnlockedScenario s)
        {
            if(s.Completed)
            {
                _listDataChild[_completed].Add(s);
            }
            if (s.IsAvailable())
            {
                _listDataChild[_unlocked].Add(s);
            }
            else if (s.IsBlocked())
            {
                _listDataChild[_blocked].Add(s);
            }
            else if (!s.IsAvailable())
            {
                _listDataChild[_unavailable].Add(s);
            }
        }

        public void Remove(CampaignUnlockedScenario s)
        {
            if (s.Completed)
            {
                _listDataChild[_completed].Remove(s);
            }
            if (s.IsAvailable())
            {
                _listDataChild[_unlocked].Remove(s);
            }
            else if (s.IsBlocked())
            {
                _listDataChild[_blocked].Remove(s);
            }
            else if (!s.IsAvailable())
            {
                _listDataChild[_unavailable].Remove(s);
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
        /// click on scenario item shows which achievements are blocking it or makeing it unavailable 
        /// </summary>
        /// <param name="campScenario"></param>
        private void ScenarioItemClick(CampaignUnlockedScenario campScenario)
        {
            if (campScenario.IsAvailable() || campScenario.Completed) return;

            // collect achievement names
            List<string> achievementNames;
            var alerttitle = "";

            if (campScenario.IsBlocked())
            {
                // blocking achievements
                alerttitle = (string.Format(_context.Resources.GetString(Resource.String.BlockingAchievement), campScenario.ScenarioName));
                achievementNames = campScenario.GetBlockingGlobalAchievements();
                achievementNames.AddRange(campScenario.GetBlockingPartyAchievements());
            }
            else
            {
                // required achievements
                alerttitle = (string.Format(_context.Resources.GetString(Resource.String.RequiredAchievements), campScenario.ScenarioName));
                achievementNames = campScenario.GetNeededGlobalAchievements();
                achievementNames.AddRange(campScenario.GetNeededPartyAchievements());
            }

            var sb = new StringBuilder();
            foreach (var achievementname in achievementNames)
            {
                sb.Append(achievementname + "\n");
            }

            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetTitle(alerttitle)
                .SetMessage(sb.ToString())
                .Show();
        }

        private void SetScenarioCompletedStatus(CampaignUnlockedScenario campScenario, bool status, CheckBox checkb)
        {
            if (status)
            {
                _isCompleting = true;
                               
                List<CampaignUnlockedScenario> unlockedScenarios = new List<CampaignUnlockedScenario>();
                if (campScenario.Completed) return;

                campScenario.Completed = true;

                var unlockedScenarioNumbers = campScenario.GetUnlockedScenarios();
                var currentCampaign = GCTContext.CampaignCollection.CurrentCampaign;                                               

                if (campScenario.Scenario.ScenarioNumber == 13)
                {
                    // Choose the Scenario to unlock

                    // Show dialog with selectable scenarios and radio buttons
                    var view = _context.LayoutInflater.Inflate(Resource.Layout.alertdialog_listview, null);
                    var listview = view.FindViewById<ListView>(Resource.Id.listView);
                    listview.ItemsCanFocus = true;
                    listview.ChoiceMode = ChoiceMode.Single;

                    IEnumerable<Scenario> selectableScenarios = GCTContext.ScenarioCollection.Items.Where(x=> unlockedScenarioNumbers.Contains(x.ScenarioNumber));

                    var adapter = new ArrayAdapter<string>(_context, Android.Resource.Layout.SimpleListItemSingleChoice, selectableScenarios.Select(x => $"# {x.ScenarioNumber}   {x.ScenarioName}").ToArray());
                    listview.Adapter = adapter;

                    new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                        .SetCustomView(view)
                        .SetTitle("Select unlocked scenario")
                        .SetMessage($"Choose the scenario you want to unlock.")
                        .SetPositiveButton("Unlock scenario", (senderAlert, args) =>
                        {
                            if (listview.CheckedItemPosition == -1) return;

                            var scenario = selectableScenarios.ElementAt(listview.CheckedItemPosition);

                            if (scenario == null) return;

                            if (_listDataChild.Values.Any(x =>x.Any(y=>y.Scenarionumber == scenario.ScenarioNumber))) return;

                            var unlockedcampscenario = currentCampaign.AddUnlockedScenario(scenario.ScenarioNumber);                                   

                            _listDataChild[_unlocked].Remove(campScenario);
                            _listDataChild[_completed].Add(campScenario);

                            Add(unlockedcampscenario);

                            NotifyDataSetChanged();

                            _isCompleting = false;
                        })
                        .Show();
                }
                else
                {
                    foreach (var scenarioNumber in unlockedScenarioNumbers)
                    {
                        if (_listDataChild.Values.Any(x => x.Any(y => y.Scenarionumber == scenarioNumber))) continue;
                        unlockedScenarios.Add(currentCampaign.AddUnlockedScenario(scenarioNumber));
                    }

                    _listDataChild[_unlocked].Remove(campScenario);
                    _listDataChild[_completed].Add(campScenario);

                    // add unlocked scenarios
                    foreach (var s in unlockedScenarios)
                    {
                        Add(s);
                    }

                    NotifyDataSetChanged();

                    _isCompleting = false;
                }

                campScenario.Save();
            }
            else
            {
                var removecampScenarios = SetIncomplete(campScenario);

                var alertView = _context.LayoutInflater.Inflate(Resource.Layout.alertdialog_listview, null);
                var lv = alertView.FindViewById<ListView>(Resource.Id.listView);
                lv.Adapter = new ArrayAdapter<string>(_context, Android.Resource.Layout.SimpleListItem1, removecampScenarios.Select(x=>$"# {x.Scenarionumber}   {x.ScenarioName}").ToArray());

                new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                    .SetCustomView(alertView)
                    .SetTitle($"Set the scenario {campScenario.ScenarioName} incomplete?")
                    .SetMessage("The following scenarios will be removed from the list.")
                    .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => {
                        checkb.Checked = true;
                    })
                    .SetPositiveButton(_context.Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                    {
                        foreach (var cus in removecampScenarios)
                        {
                            GCTContext.CurrentCampaign.RemoveUnlockedScenario(cus);
                            Remove(cus);
                        }

                        campScenario.Completed = false;

                        _listDataChild[_completed].Remove(campScenario);
                        _listDataChild[_unlocked].Add(campScenario);

                        campScenario.Save();
                        NotifyDataSetChanged();
                    })
                    .Show();
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
                if (_listDataChild[_completed].Any(x => x.Scenarionumber != cs.Scenarionumber && 
                                                        !removededScenarios.Contains(x) && 
                                                        x.GetUnlockedScenarios().Contains(scenarioNumber))) continue;

                var campScenarioList = _listDataChild.Values.FirstOrDefault(x=>x.Any(y=>y.Scenarionumber == scenarioNumber));
                if(campScenarioList != null)
                {
                    var campScenario = campScenarioList.FirstOrDefault(y => y.Scenarionumber == scenarioNumber);

                    if (campScenario != null)
                    {
                        removededScenarios.Add(campScenario);

                        foreach (var cus in SetIncomplete(campScenario, removededScenarios))
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