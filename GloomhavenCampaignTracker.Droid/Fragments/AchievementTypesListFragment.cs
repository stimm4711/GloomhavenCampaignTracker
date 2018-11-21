using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using GloomhavenSaveGame_Droid.Adapter;
using GloomhavenSaveGame_Droid.Orm.Entities;
using GloomhavenSaveGame_Droid.Orm.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace GloomhavenSaveGame_Droid.Fragments
{
    public class AchievementTypesListFragment : Android.Support.V4.App.Fragment
    { 
        View achievementTypesView;
        LayoutInflater inflater;
        ListView achievementTypesListView;
        AchievementTypeAdapter listViewAdapter;
        Toolbar toolbar;
        ImageView toolbarAddImageView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            achievementTypesView = inflater.Inflate(Resource.Layout.listViewWithToolbar, container, false);
            this.inflater = inflater;
            achievementTypesListView = achievementTypesView.FindViewById<ListView>(Resource.Id.listviewWithToolbar_listView);
            toolbar = achievementTypesView.FindViewById<Toolbar>(Resource.Id.listviewWithToolbar_toolbar);
            toolbarAddImageView = achievementTypesView.FindViewById<ImageView>(Resource.Id.listviewWithToolbar_toolbarImage);

            toolbar.Title = "Achievementtypes";

            // Init ListView
            FillListVew();
            achievementTypesListView.ItemClick += listView_ItemClick;
            
            if (!toolbarAddImageView.HasOnClickListeners)
            {
                toolbarAddImageView.Click += (sender, e) =>
                {
                    EditAchievementTypeFragment editAchievementTypeFragment = (EditAchievementTypeFragment)FragmentManager.FindFragmentById(Resource.Id.achievementManagement_container);
                    editAchievementTypeFragment.UpdateContent(null);
                };
            }

            return achievementTypesView;
        }

        private void FillListVew()
        {
            listViewAdapter = new AchievementTypeAdapter(Context, Resource.Layout.AchievementTypeItemView);
            listViewAdapter.AddItems(GenericRepository<DL_AchievementType>.Get().ToList());
            achievementTypesListView.Adapter = listViewAdapter;
        }

        void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Get our item from the list adapter
            DL_AchievementType achievementType = listViewAdapter.GetItem(e.Position);
            // Update the Detail view
            EditAchievementTypeFragment editAchievementTypeFragment = (EditAchievementTypeFragment)FragmentManager.FindFragmentById(Resource.Id.achievementManagement_container);
            editAchievementTypeFragment.UpdateContent(achievementType);
        }

        public void UpdateList()
        {
            listViewAdapter.NotifyDataSetChanged();
        }

        internal void AddNewItem(DL_AchievementType achievmentType)
        {
            listViewAdapter.AddItem(achievmentType);
        }    
    }
}