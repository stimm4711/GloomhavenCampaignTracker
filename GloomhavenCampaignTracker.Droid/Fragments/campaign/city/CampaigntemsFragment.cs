using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.Linq;

namespace GloomhavenCampaignTracker.Droid.Fragments.campaign.city
{
    public class CampaignItemsFragment : CampaignFragmentBase
    {
        private readonly int _prosperityLevel;
        private ListView _lv;

        public CampaignItemsFragment(int prosperitylevel)
        {
            _prosperityLevel = prosperitylevel;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _dataChanged = false;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            if (_prosperityLevel >= 0 && _prosperityLevel <= 9)
            {
                _view = inflater.Inflate(Resource.Layout.fragment_itemstore_prosperityitems, container, false);
            }
            else
            {
                _view = inflater.Inflate(Resource.Layout.fragment_itemstore_unlockedIiems, container, false);
            }

            _lv = _view.FindViewById<ListView>(Resource.Id.itemsListView);
            var fab = _view.FindViewById<FloatingActionButton>(Resource.Id.fab);

            var itemfilter = _view.FindViewById<ItemFilterBar>(Resource.Id.itemfilter);

            itemfilter.ItemClicked += Itemfilter_ItemClicked;

            if (fab != null)
            {
                if (!fab.HasOnClickListeners)
                {
                    fab.Click += (senderx, ex) =>
                    {
                        AddNewItemDialog();
                    };
                }
            }

            FillListView();

            return _view;
        }

        private void Itemfilter_ItemClicked(object sender, ItemFilterBar.ItemClickedEventArgs e)
        {
            FillListView(e.Itemcategorie);
        }

        private void FillListView(int filter = -1)
        {
            if (_prosperityLevel == 0)
            {
                var prosp = Helper.GetProsperityLevel(GCTContext.CurrentCampaign.CityProsperity);
                var items = DataServiceCollection.ItemDataService.GetSelectableItems(prosp, GCTContext.CurrentCampaign.CampaignData.Id);
                _lv.Adapter = new ProsperityItemAdapter(Context, items, filter);
            }
            if (_prosperityLevel >= 1 && _prosperityLevel <= 9)
            {
                var items = DataServiceCollection.ItemDataService.GetByProsperity(_prosperityLevel);
                _lv.Adapter = new ProsperityItemAdapter(Context, items, filter);
            }
            if (_prosperityLevel > 9)
            {
                var items = GCTContext.CurrentCampaign.CampaignData.UnlockedItems;
                foreach (var item in items)
                {
                    if (item.Prosperitylevel <= GCTContext.CurrentCampaign.GetProsperityLevel()) item.IsHide = true;
                    if (item.Prosperitylevel > GCTContext.CurrentCampaign.GetProsperityLevel() && item.IsHide) item.IsHide = false;

                    if (item.Itemnumber == 151 && !GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.HiddenClassUnlocked) item.IsHide = true;
                }

                var adapter = new UnlockedItemAdapter(Context, items, filter);
                adapter.DataModified += Adapter_DataModified;

                _lv.Adapter = adapter;
            }
        }

        /// <summary>
        /// Add an item to a character
        /// </summary>
        /// <param name="lv"></param>
        private void AddNewItemDialog()
        {
            var view = _inflater.Inflate(Resource.Layout.alertdialog_listview, null);
            var listview = view.FindViewById<ListView>(Resource.Id.listView);
            listview.ItemsCanFocus = true;
            listview.ChoiceMode = ChoiceMode.Multiple;

            var selectableItems = DataServiceCollection.CampaignDataService.GetUnlockableItems(GCTContext.CurrentCampaign.CampaignData.Id);

            if (!GCTContext.CurrentCampaign.CampaignData.CampaignUnlocks.HiddenClassUnlocked)
            {
                var item151 = selectableItems.FirstOrDefault(x => x.Itemnumber == 151);
                selectableItems.Remove(item151);
            }

            var itemadapter = new SelectableItemAdapter(Context, selectableItems, false);
            listview.Adapter = itemadapter;

            new CustomDialogBuilder(base.Context, Resource.Style.MyDialogTheme)
                .SetCustomView(view)
                .SetTitle($"Select unlocked items")
                .SetNegativeButton(base.Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .SetPositiveButton("Add selected items to itemstore", (senderAlert, args) =>
                {
                    var selectedItems = itemadapter.GetSelected();
                    foreach (DL_Item si in selectedItems)
                    {
                        var item = new DL_CampaignUnlockedItem()
                        {
                            Campaign = GCTContext.CurrentCampaign.CampaignData,
                            ID_Campaign = GCTContext.CurrentCampaign.CampaignData.Id,
                            ID_Item = si.Id,
                            Item = si
                        };

                        CampaignUnlockedItemRepository.InsertOrReplace(item);

                        GCTContext.CurrentCampaign.CampaignData.UnlockedItems.Add(si);
                    }

                    SetModified();
                    FillListView();
                })
                .Show();
        }

        public override void OnStop()
        {
            _dataChanged = false;
            base.OnStop();
        }
    }
}