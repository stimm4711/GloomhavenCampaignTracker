using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class CampaignAchievementTypeAdapter : BaseAdapter
    {
        private readonly Context _context;
        private LayoutInflater _inflater;
        private readonly List<CampaignGlobalAchievement> _globalachievements;

        public override int Count => _globalachievements.Count;

        public CampaignAchievementTypeAdapter(Context context)
        {
            _context = context;
            _globalachievements = new List<CampaignGlobalAchievement>();
            if (GCTContext.CurrentCampaign != null )
            {
                _globalachievements = GCTContext.CurrentCampaign.GlobalAchievements;
            }            
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view;
            _inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);

            var campAchievement = _globalachievements[position];
            var achType = campAchievement.AchievementType;

            if (achType.Steps > 1)
            {
                // take campaignAchievementWithSteps layout
                view = GetViewAchievementSteps(campAchievement);                
            }
            else if (achType.Achievements != null && achType.Achievements.Any())
            {
                // take campaignAchievementWithAchievements layout with spinner for achievements
                view = GetViewAchievementSubAchievements(campAchievement);
            }
            else
            {
                // take campaignAchievement layout
                view = _inflater.Inflate(Resource.Layout.listviewitem_campaignachievement, null);
            }

            //Handle TextView and display string from your list
            var listItemText = (TextView)view.FindViewById(Resource.Id.campaignAchievement_name);
            listItemText.Text = achType.Name;

            var optionsButton = view.FindViewById<Button>(Resource.Id.optionsButton);                   
            if (!optionsButton.HasOnClickListeners)
            {
                optionsButton.Click += (sender, e) =>
                {
                    ConfirmDeleteDialog(position);
                };
            }    

            return view;
        }

        /// <summary>
        /// Confirm delete achievement
        /// </summary>
        /// <param name="position"></param>
        private void ConfirmDeleteDialog(int position)
        {
            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
               .SetMessage(_context.Resources.GetString(Resource.String.DeleteGlobalAchievement))
                .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
                {
                    GCTContext.CurrentCampaign.RemoveGlobalAchievement(_globalachievements[position]);
                    NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        #region "Subachievements"

        private View GetViewAchievementSubAchievements(CampaignGlobalAchievement campAchievement)
        {
            var achType = campAchievement.AchievementType;
            var view = _inflater.Inflate(Resource.Layout.listviewitem_campaignachievement_with_achievements, null);

            var spinner = view.FindViewById<Spinner>(Resource.Id.campaignAchievement_achievementSpinner);
            var achievementListAdapter = new CampaignAchievementAdapter(_context, achType.Achievements);
            spinner.Adapter = achievementListAdapter;

            if (campAchievement.Achievement != null)
            {
                spinner.SetSelection(achievementListAdapter.GetPosition(campAchievement.Achievement));
            }

            spinner.ItemSelected += (sender, e) =>
            {
                var ach = achievementListAdapter.GetAchievement(spinner.SelectedItemPosition) ?? achType.Achievements.First();
                campAchievement.SetAchievement(ach);
            };

            return view;
        }

        #endregion

        #region "Achievements with steps"

        private View GetViewAchievementSteps(CampaignGlobalAchievement campAchievement)
        {
            var view = _inflater.Inflate(Resource.Layout.listviewitem_campaignachievement_withsteps, null);
            var stepText = (TextView)view.FindViewById(Resource.Id.campaignAchievement_steps);
            var decreaseStepsButton = view.FindViewById<Button>(Resource.Id.decreaseAchStepsButton);
            var raiseStepsButton = view.FindViewById<Button>(Resource.Id.raiseAchStepsbutton);

            SetStepsText(campAchievement, stepText);

            if (!decreaseStepsButton.HasOnClickListeners)
            {
                decreaseStepsButton.Click += (sender, e) =>
                {
                    campAchievement.DecreaseAchievementSteps();
                    SetStepsText(campAchievement, stepText);
                };
            }

            if (!raiseStepsButton.HasOnClickListeners)
            {
                raiseStepsButton.Click += (sender, e) =>
                {
                    campAchievement.IncreaseAchievementSteps();
                    SetStepsText(campAchievement, stepText);
                    CheckConditionAncientTechnologyStep5(campAchievement);
                };
            }

            return view;
        }

        private void SetStepsText(CampaignGlobalAchievement campAchievement, TextView stepText)
        {
            stepText.Text = string.Format(_context.Resources.GetString(Resource.String.PartsOf), campAchievement.Step, campAchievement.AchievementType.Steps);
        }

        private void CheckConditionAncientTechnologyStep5(CampaignGlobalAchievement campAchievement)
        {
            if (campAchievement.AchievementEnumNumber == (int)GlobalAchievementsInternalNumbers.AncientTechnology_Step5 &&
                                    !campAchievement.Campaign.CampaignUnlocks.EnvelopeAUnlocked)
            {
                new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)                
                .SetTitle(_context.Resources.GetString(Resource.String.Congratulation))
                .SetMessage(_context.Resources.GetString(Resource.String.UnlockedEnvelopeA))
                .SetPositiveButton(_context.Resources.GetString(Resource.String.OK), (senderAlert, args) =>
                {
                    GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.EnvelopeAUnlocked = true;
                })
                .Show();
            }
        }

        #endregion

        public override Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
    }
}