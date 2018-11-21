using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using GloomhavenCampaignTracker.Shared.Business;
using System.Text;
using GloomhavenCampaignTracker.Droid.CustomControls;
using Android.Support.V4.Content;
using Android.Graphics;
using Android.Support.Design.Widget;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    /// <summary>
    /// http://www.appliedcodelog.com/2016/06/expandablelistview-in-xamarin-android.html
    /// </summary>
    public class ExpandableScenarioListAdapter : BaseExpandableListAdapter
    {
        private const string completed = "Completed";
        private const string unlocked = "Unlocked";
        private const string blocked = "Blocked";
        private const string unavailable = "Unavailable";
        private Activity _context;

        /// <summary>
        /// header titles
        /// </summary>
        private List<string> _listDataHeader;

        /// <summary>
        /// child data in format of header title, child title
        /// </summary>
        private Dictionary<string, List<CampaignUnlockedScenario>> _listDataChild;

        public ExpandableScenarioListAdapter(Activity context, List<string> listDataHeader, Dictionary<String, List<CampaignUnlockedScenario>> listChildData)
        {
            _context = context;
            _listDataHeader = listDataHeader;
            _listDataChild = listChildData;
        }

        /// <summary>
        /// for child item view 
        /// </summary>
        /// <param name="groupPosition"></param>
        /// <param name="childPosition"></param>
        /// <returns></returns>
        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
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
                _listDataChild[completed].Add(s);
            }
            if (s.IsAvailable())
            {
                _listDataChild[unlocked].Add(s);
            }
            else if (s.IsBlocked())
            {
                _listDataChild[blocked].Add(s);
            }
            else if (!s.IsAvailable())
            {
                _listDataChild[unavailable].Add(s);
            }
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            convertView = _context.LayoutInflater.Inflate(Resource.Layout.listviewitem_campaignscenario, null);

            convertView.Click += (sender, e) =>
            {
                // show reasons for blocked and not available scenarios
                ScenarioItemClick(groupPosition, childPosition);
            };

            CampaignScenarioHolder holder = null;           

            if (convertView != null)
                holder = convertView.Tag as CampaignScenarioHolder;                       

            if (holder == null)
            {
                holder = new CampaignScenarioHolder();

                convertView = _context.LayoutInflater.Inflate(Resource.Layout.listviewitem_character, parent, false);
                holder.ScenarioName = convertView.FindViewById<TextView>(Resource.Id.scenariViewItem_nameTextView);
                holder.ScenarioNumber = convertView.FindViewById<TextView>(Resource.Id.scenariViewItem_numberTextView);
                holder.ScenarioCompleted = convertView.FindViewById<CheckBox>(Resource.Id.scenarioItemView_completedCheckBox);
                holder.OptionsButton = convertView.FindViewById<Button>(Resource.Id.optionsButton);
                holder.TreasureText = convertView.FindViewById<Button>(Resource.Id.scenarioTreasureStatus);
                convertView.Tag = holder;
            }

            var campScenario = (CampaignUnlockedScenario)GetChild(groupPosition, childPosition);

            holder.ScenarioName.Text = campScenario.ScenarioName;
            holder.ScenarioNumber.Text = campScenario.Scenarionumber.ToString();
            holder.ScenarioCompleted.Checked = campScenario.Completed;
            holder.TreasureText.Text = string.Format(_context.Resources.GetString(Resource.String.TreasuresPartOf), campScenario.TreasureTilesLooted?.Count, campScenario.Treasures?.Count);
            
            holder.ScenarioCompleted.CheckedChange += (sender, e) =>
            {
                // checkbox changed
                if (e.IsChecked == campScenario.Completed) return;
                SetScenarioCompletedStatus(convertView, holder.ScenarioCompleted, groupPosition, childPosition, e.IsChecked);
            };

            if (!holder.OptionsButton.HasOnClickListeners)
            {
                holder.OptionsButton.Click += (sender, e) =>
                {
                    ShowOptionsButtonPopupMenu(groupPosition, childPosition, holder.OptionsButton, convertView, holder.ScenarioCompleted);
                };
            }

            SetBackgroudColor(groupPosition, childPosition, convertView, holder.ScenarioCompleted);

            return convertView;
        }

        private class CampaignScenarioHolder : Java.Lang.Object
        {
            public TextView ScenarioName { get; set; }
            public TextView ScenarioNumber { get; set; }
            public CheckBox ScenarioCompleted { get; set; }
            public TextView TreasureText { get; set; }
            public Button OptionsButton { get; set; }
        }

        /// <summary>
        /// click on scenario item shows which achievements are blocking it or makeing it unavailable 
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="campScenario"></param>
        private void ScenarioItemClick(int groupPosition, int childPosition)
        {
            var campScenario = (CampaignUnlockedScenario)GetChild(groupPosition, childPosition);

            if (!campScenario.IsAvailable() && !campScenario.Completed)
            {
                // collect achievement names
                List<string> achievementNames = new List<string>();
                var alerttitle = "";

                if (campScenario.IsBlocked())
                {
                    // blocking achievements
                    alerttitle = (string.Format(_context.Resources.GetString(GloomhavenCampaignTracker.Droid.Resource.String.BlockingAchievement), campScenario.ScenarioName));
                    achievementNames = campScenario.GetBlockingGlobalAchievements();
                    achievementNames.AddRange(campScenario.GetBlockingPartyAchievements());
                }
                else
                {
                    // required achievements
                    alerttitle = (string.Format(_context.Resources.GetString(GloomhavenCampaignTracker.Droid.Resource.String.RequiredAchievements), campScenario.ScenarioName));
                    achievementNames = campScenario.GetNeededGlobalAchievements();
                    achievementNames.AddRange(campScenario.GetNeededPartyAchievements());
                }

                StringBuilder sb = new StringBuilder();
                foreach (string achievementname in achievementNames)
                {
                    sb.Append(achievementname + "\n");
                }

                new CustomDialogBuilder(_context, GloomhavenCampaignTracker.Droid.Resource.Style.MyDialogTheme)
                       .SetTitle(alerttitle)
                        .SetMessage(sb.ToString())
                        .Show();
            }
        }

        private void SetScenarioCompletedStatus(View view, CheckBox checkbox, int groupPosition, int childPosition, bool status)
        {
            var campScenario = (CampaignUnlockedScenario)GetChild(groupPosition, childPosition);
            var unlockedScenarios = campScenario.SetCompleted(status);                       

            _listDataChild[unlocked].Remove(campScenario);
            _listDataChild[completed].Add(campScenario);

            // add unlocked scenarios
            foreach (var s in unlockedScenarios)
            {
                Add(s);
            }

            // change background color
            SetBackgroudColor(groupPosition, childPosition, view, checkbox);
            NotifyDataSetChanged();
        }

        /// <summary>
        /// Show opions button menu
        /// </summary>
        /// <param name="position"></param>
        /// <param name="optionsButton"></param>
        private void ShowOptionsButtonPopupMenu(int groupPosition, int childPosition, Button optionsButton, View view, CheckBox checkbox)
        {
            if (childPosition >= GetChildrenCount(groupPosition)) return;

            var campScenario = (CampaignUnlockedScenario)GetChild(groupPosition, childPosition);

            // open a popup menu with delete option
            PopupMenu menu = new PopupMenu(_context, optionsButton);
            GloomhavenCampaignTracker.Shared.Helper.ForcePopupmenuToShowIcons(menu);
            menu.Inflate(Resource.Menu.scenarioPopupMenu);

            var completedItem = menu.Menu.FindItem(Resource.Id.sc_popup_completed);
            completedItem.SetEnabled(campScenario.IsAvailable());

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
                        SetScenarioCompletedStatus(view, checkbox, groupPosition, childPosition, true);
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
            var alertView = _context.LayoutInflater.Inflate(GloomhavenCampaignTracker.Droid.Resource.Layout.alertWithListViewFloatingActionButton, null);
            var fab = alertView.FindViewById<FloatingActionButton>(GloomhavenCampaignTracker.Droid.Resource.Id.fab);
            var lv = alertView.FindViewById<ListView>(GloomhavenCampaignTracker.Droid.Resource.Id.scenariorequirementsListView);
            lv.Adapter = new TreasureAdapter(_context, campScenario, null);

            if (!fab.HasOnClickListeners)
            {
                fab.Click += (senderx, ex) =>
                {
                    AddNewTreasureDialog(campScenario, lv);
                };
            }

            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
               .SetCustomView(alertView)
                .SetTitle(string.Format("{0} Treasures", campScenario.ScenarioName))
                .SetPositiveButton(_context.Resources.GetString(GloomhavenCampaignTracker.Droid.Resource.String.Close), (senderAlert, args) => { })
                .Show();
        }

        /// <summary>
        /// Add a new treasure to scenario
        /// </summary>
        /// <param name="campScenario"></param>
        /// <param name="lv"></param>
        private void AddNewTreasureDialog(CampaignUnlockedScenario campScenario, ListView lv)
        {
            var convertView = _context.LayoutInflater.Inflate(GloomhavenCampaignTracker.Droid.Resource.Layout.alertdialog_addtreasure, null);
            var refNumberText = convertView.FindViewById<EditText>(GloomhavenCampaignTracker.Droid.Resource.Id.treasure_ref_number);

            new CustomDialogBuilder(_context, GloomhavenCampaignTracker.Droid.Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle("Add new treasure")
                .SetPositiveButton(_context.Resources.GetString(GloomhavenCampaignTracker.Droid.Resource.String.AddTreasureAlertTitle), (senderAlert, args) =>
                {
                    if (int.TryParse(refNumberText.Text, out int refNumber))
                    {
                        campScenario.AddTreasure(refNumber);
                        NotifyDataSetChanged();
                        lv.Adapter = new TreasureAdapter(_context, campScenario, null);
                    }

                    campScenario.Save();
                })
                .Show();
        }

        #endregion

        /// <summary>
        /// Sets the background color for scenario items by their status
        /// </summary>
        /// <param name="campScenario"></param>
        /// <param name="view"></param>
        /// <param name="checkbox"></param>
        private void SetBackgroudColor(int groupPosition, int childPosition, View view, CheckBox checkbox)
        {
            var campScenario = (CampaignUnlockedScenario)GetChild(groupPosition, childPosition);
            var color = ContextCompat.GetColor(_context, Resource.Color.gloom_scenarioItemBackground);

            if (groupPosition != 0)
            {
                var status = (TextView)view.FindViewById(Resource.Id.campaignScenariostatus);

                if (groupPosition == 3)
                {
                    // Blocked = red
                    status.Text = _context.Resources.GetString(Resource.String.Blocked);
                    color = ContextCompat.GetColor(_context,Resource.Color.gloom_scenarioBlockedItemBackground);
                    checkbox.Enabled = false;
                }
                else if (groupPosition == 2)
                {
                    // Completed = green
                    color = ContextCompat.GetColor(_context, Resource.Color.gloom_scenarioCompletedItemBackground);
                }
                else
                {
                    // unavailable blue
                    status.Text = _context.Resources.GetString(Resource.String.Unavailable);
                    color = ContextCompat.GetColor(_context, Resource.Color.gloom_scenarioUnavailableItemBackground);
                    checkbox.Enabled = false;
                }
            }

            view.SetBackgroundColor(new Color(color));
        }

        /// <summary>
        /// Confirm delete
        /// </summary>
        /// <param name="position"></param>
        private void ConfirmDeleteDialog(CampaignUnlockedScenario campscenario)
        {
            new CustomDialogBuilder(_context, GloomhavenCampaignTracker.Droid.Resource.Style.MyDialogTheme)
                .SetMessage(string.Format("Delete {0}", campscenario.ScenarioName))
                .SetPositiveButton(_context.Resources.GetString(GloomhavenCampaignTracker.Droid.Resource.String.YesDelete), (senderAlert, args) =>
                {
                    //GCTContext.CurrentCampaign.RemoveScenario(campscenario);
                    
                    //.Remove(campscenario);
                    //NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(GloomhavenCampaignTracker.Droid.Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return _listDataChild[_listDataHeader[groupPosition]].Count;
        }

        //For header view
        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return _listDataHeader[groupPosition];
        }

        public override int GroupCount
        {
            get
            {
                return _listDataHeader.Count;
            }
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            string headerTitle = (string)GetGroup(groupPosition);

            convertView = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.HeaderCustomLayout, null);
            var lblListHeader = (TextView)convertView.FindViewById(Resource.Id.lblListHeader);
            lblListHeader.Text = headerTitle;

            return convertView;
        }

        public override bool HasStableIds
        {
            get
            {
                return false;
            }
        }
        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        class ViewHolderItem : Java.Lang.Object
        {
        }
    }
}