using Android.App;
using Android.Content;
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
    public class ScenarioListFragment : Android.Support.V4.App.Fragment
    { 
        View fragmentView;
        LayoutInflater inflater;
        ListView listView;
        ScenarioAdapter listViewAdapter;
        Toolbar toolbar;
        ImageView toolbarAddImageView;
        protected bool isDualPane;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            fragmentView = inflater.Inflate(Resource.Layout.listViewWithToolbar, container, false);
            this.inflater = inflater;
            listView = fragmentView.FindViewById<ListView>(Resource.Id.listviewWithToolbar_listView);
            toolbar = fragmentView.FindViewById<Toolbar>(Resource.Id.listviewWithToolbar_toolbar);
            toolbarAddImageView = fragmentView.FindViewById<ImageView>(Resource.Id.listviewWithToolbar_toolbarImage);

            toolbar.Title = "Scenarios";

            // Init ListView
            FillListVew();
            listView.ItemClick += listView_ItemClick;

            var editFragment = FragmentManager.FindFragmentById(Resource.Id.scenarioManagement_container);
            isDualPane = editFragment != null;

            if (!toolbarAddImageView.HasOnClickListeners)
            {
                toolbarAddImageView.Click += (sender, e) =>
                {
                    UpdateDetail(null);                  
                };
            }

            return fragmentView;
        }

        private void UpdateDetail(DL_Scenario item)
        {
            if (isDualPane)
            {
                var editFragment = (ScenarioEditFragment)FragmentManager.FindFragmentById(Resource.Id.scenarioManagement_container);
                editFragment.UpdateContent(item);
            }
            else
            {
                int scenarioID = 0;
                if(item != null)
                {
                    scenarioID = item.Id;
                }
                var intent = new Intent();
                intent.SetClass(Activity, typeof(DetailsActivity)); 
                intent.PutExtra("current_scenario_id", scenarioID);
                intent.PutExtra("selected_fragid", (int)DetailFragmentTypes.Scenario);
                StartActivity(intent);
            }
        }

        private void FillListVew()
        {
            listViewAdapter = new ScenarioAdapter(Context, Resource.Layout.AchievementTypeItemView, null);
            listViewAdapter.AddItems(GenericRepository<DL_Scenario>.Get().ToList());
            listView.Adapter = listViewAdapter;
        }

        void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Get our item from the list adapter
            DL_Scenario item = listViewAdapter.GetItem(e.Position);
            // Update the Detail view
            UpdateDetail(item);
        }

        public void UpdateList()
        {
            listViewAdapter.NotifyDataSetChanged();
        }

        internal void AddNewItem(DL_Scenario item)
        {
            listViewAdapter.AddItem(item);
        }    
    }
}