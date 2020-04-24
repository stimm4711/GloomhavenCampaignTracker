using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System.Collections.Generic;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.Linq;
using GloomhavenCampaignTracker.Business;

namespace GloomhavenCampaignTracker.Droid.Fragments.character
{
    public class CharacterDetailItemsFragment : CharacterDetailFragmentBase
    {     
        internal static CharacterDetailItemsFragment NewInstance(DL_Character character)
        {
            var frag = new CharacterDetailItemsFragment() { Arguments = new Bundle() };
            frag.Arguments.PutInt(charID, character.Id);
            return frag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            _view = inflater.Inflate(Resource.Layout.fragment_characterdetails_items, container, false);
            
            var fabstore = _view.FindViewById<FloatingActionButton>(Resource.Id.fabStore);
            var fabnumber = _view.FindViewById<FloatingActionButton>(Resource.Id.fabItemnumber);

            var lv = _view.FindViewById<ListView>(Resource.Id.characteritemsListView);

            if(Character != null)
            {
                lv.Adapter = new CharacterItemsAdapter(Context, Character, this);
            }            

            if (!fabstore.HasOnClickListeners)
            {
                fabstore.Click += (senderx, ex) =>
                {
                    AddNewItemDialogStore(lv);
                };
            }

            if (!fabnumber.HasOnClickListeners)
            {
                fabnumber.Click += (senderx, ex) =>
                {
                    AddNewItemDialogNumber(lv);
                };
            }

            return _view;
        }

        private void AddNewItemDialogNumber(ListView lv)
        {
            var inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
            var convertView = inflater.Inflate(Resource.Layout.alertdialog_additem, null);
            var refNumberText = convertView.FindViewById<EditText>(Resource.Id.item_ref_number);

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(convertView)
                .SetTitle(Resources.GetString(Resource.String.AddItemToCharacter))
                .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .SetPositiveButton(Resources.GetString(Resource.String.AddItemToCharacter), (senderAlert, args) =>
                {
                    if (!int.TryParse(refNumberText.Text, out int refNumber)) return;
                    var item = DataServiceCollection.ItemDataService.GetByItemnumber(refNumber).FirstOrDefault();
                        
                    if (item != null)
                    {
                        Character.Items.Add(item);
                    }
                    else
                    {
                        Toast.MakeText(Context, $"Couldn't find item with number {refNumber}.", ToastLength.Short).Show();
                    }

                    SaveCharacter();
                    lv.Adapter = new CharacterItemsAdapter(Context, Character, this);
                })
                .Show();
        }

        /// <summary>
        /// Add an item to a character
        /// </summary>
        /// <param name="lv"></param>
        private void AddNewItemDialogStore(ListView lv)
        {
            var inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(Resource.Layout.alertdialog_listview_itemfilter, null);
            var listview = view.FindViewById<ListView>(Resource.Id.listView);
            var itemfilter = view.FindViewById<ItemFilterBar>(Resource.Id.itemfilter);
            List<DL_Item> selectableItems = new List<DL_Item>();

            itemfilter.ItemClicked += (sender, e) =>
            {
                if (selectableItems != null)
                {
                    IEnumerable<DL_Item> filteredItems;
                    if (e.Itemcategorie >= 0 && e.Itemcategorie <= 5)
                    {
                        filteredItems = selectableItems.Where(x => x.Itemcategorie == e.Itemcategorie);
                    }
                    else
                    {
                        filteredItems = selectableItems;
                    }

                    var adapter = new SelectableItemAdapter(Context, filteredItems.ToList(), true);
                    listview.Adapter = adapter;
                }
            };

            listview.ItemsCanFocus = true;
            listview.ChoiceMode = ChoiceMode.Multiple;
                        
            if (Character.Party?.Campaign != null)
            {
                var prosp = Helper.GetProsperityLevel(Character.Party.Campaign.CityProsperity);
                var items = DataServiceCollection.ItemDataService.GetSelectableItems(prosp, Character.Party.Campaign.Id);
                foreach(var item in items)
                {
                    if(!selectableItems.Any(x=>x.Itemnumber == item.Itemnumber))
                    {
                        selectableItems.Add(item);
                    }                    
                }               
            }
            else
            {
                selectableItems = DataServiceCollection.ItemDataService.Get();
            }

            if (Character.Items != null)
            {
                selectableItems = selectableItems.Except(Character.Items).ToList();

                foreach (var i in Character.Items)
                {
                    var si = selectableItems.FirstOrDefault(x => x.Itemnumber == i.Itemnumber);
                    if (si != null) selectableItems.Remove(si);
                }
            }          

            var itemadapter = new SelectableItemAdapter(Context, selectableItems, true);
            listview.Adapter = itemadapter;

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(view)
                .SetTitle("Select Items")
                .SetNeutralButton(Context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .SetNegativeButton("Add items", (senderAlert, args) =>
                {
                    var selectedItems = itemadapter.GetSelected();
                    foreach (var si in selectedItems)
                    {
                        if (Character.Items == null) Character.Items = new List<DL_Item>();
                        Character.Items.Add(si);
                    }

                    SaveCharacter();
                    lv.Adapter = new CharacterItemsAdapter(Context, Character, this);
                })
                .SetPositiveButton("Buy items", (senderAlert, args) =>
                {
                    var selectedItems = itemadapter.GetSelected();
                    var costs = selectedItems.Sum(x => x.Itemprice);

                    if(Character.Party != null)
                    {
                        var modifier = selectedItems.Count() * Helper.GetShopPriceModifier(Character.Party.Reputation);
                        costs += modifier;
                    }

                    if(Character.Gold < costs)
                    {
                        new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                            .SetTitle("Checkout")
                            .SetMessage($"{Character.Name} does not have enough gold.")
                            .SetPositiveButton(Context.Resources.GetString(Resource.String.OK), (s, a) => { })
                            .Show();
                    }
                    else
                    {
                        var message = $"Buy items for {costs} gold? You have {Character.Gold}.";
                        new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                           .SetTitle("Checkout")
                           .SetMessage(message)
                           .SetNegativeButton(Context.Resources.GetString(Resource.String.NoCancel), (s, a) => { })
                           .SetPositiveButton("Buy items", (s, a) =>
                           {                               
                               foreach (var si in selectedItems)
                               {
                                   if (Character.Items == null) Character.Items = new List<DL_Item>();
                                   Character.Items?.Add(si);
                               }

                               UpdateCharacterGold(-costs);
                               lv.Adapter = new CharacterItemsAdapter(Context, Character, this);
                           })
                           .Show();
                    }
                })
                .Show();           
        }

        public void UpdateCharacterGold(int value)
        {
            ((CharacterDetailsViewPagerTabs) ParentFragment).UpdateCharacterGolds(value);
        }
    }
}