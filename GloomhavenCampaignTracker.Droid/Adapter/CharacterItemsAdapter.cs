using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Droid.Fragments.character;
using GloomhavenCampaignTracker.Shared;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using Java.Lang;
using System.Threading.Tasks;
using Android.Graphics;
using System.Net.Http;

namespace GloomhavenCampaignTracker.Droid.Adapter
{
    internal class CharacterItemsAdapter : CharacterDetailsAdapterBase
    {
        private readonly CharacterDetailItemsFragment _characteritemsfrag;

        public override int Count => _character.Items?.Count ?? 0;

        public CharacterItemsAdapter(Context context, DL_Character character, CharacterDetailItemsFragment characteritemsfrag) : base(context, character)
        {
            _characteritemsfrag = characteritemsfrag;
        }
        
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _character.Items[position];

            DL_ItemHolder holder = null;

            if (convertView != null)
                holder = convertView.Tag as DL_ItemHolder;

            if (holder == null)
            {
                // Create new holder
                holder = new DL_ItemHolder();

                var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.listviewitem_item_withprice_deleteable, parent, false);
                holder.ItemName = convertView.FindViewById<TextView>(Resource.Id.itemnameTextView);
                holder.ItemNumber = convertView.FindViewById<TextView>(Resource.Id.itemnumber); 
                holder.Options = convertView.FindViewById<ImageView>(Resource.Id.optionsImageView);
                holder.ItemCategoryImage = convertView.FindViewById<ImageView>(Resource.Id.categorieImageView);
                holder.ItemPrice = convertView.FindViewById<TextView>(Resource.Id.itemcostsTextView);
                convertView.Tag = holder;

                // Set Item Click Event
                convertView.Click += (sender, e) =>
                {
                    var v = (View)sender;
                    var numberTxtView = v.FindViewById<TextView>(Resource.Id.itemnumber);
                    if (numberTxtView == null) return;
                    ItemClick((DL_Item)numberTxtView.Tag);
                };
            }

            // Set Data
            holder.ItemName.Text = item.Itemname;
            holder.ItemNumber.Text = $"# {item.Itemnumber}";
            holder.ItemPrice.Text = $"{item.Itemprice} G";
            holder.ItemCategoryImage.SetImageResource(ResourceHelper.GetItemCategorieIconRessourceId(item.Itemcategorie));
            holder.ItemNumber.Tag = item;

            if (!holder.Options.HasOnClickListeners)
            {
                holder.Options.Click += (sender, e) =>
                {
                    ShowOptionsButtonPopupMenu(position, holder.Options);
                };
            }

            return convertView;
        }

        private void ItemClick(DL_Item item)
        {
            var numbertext = item.GetNumberText();
            if (string.IsNullOrEmpty(numbertext)) return;
            var diag = new ItemImageViewDialogBuilder(_context, Resource.Style.MyTransparentDialogTheme)
                        .SetItemNumber(numbertext)
                        .Show();
        }       

        private class DL_ItemHolder : Object
        {
            public TextView ItemName { get; set; }
            public TextView ItemNumber { get; set; }
            public TextView ItemPrice { get; set; }
            public ImageView Options { get; set; }
            public ImageView ItemCategoryImage { get; set; }
        }

        /// <summary>
        /// Show opions button menu
        /// </summary>
        /// <param name="position"></param>
        /// <param name="optionsButton"></param>
        private void ShowOptionsButtonPopupMenu(int position, ImageView optionsButton)
        {
            if (position >= Count) return;

            // open a popup menu with delete option
            var menu = new PopupMenu(_context, optionsButton);
            Helper.ForcePopupmenuToShowIcons(menu);
            menu.Inflate(Resource.Menu.characterItemsPopupMenu);

            menu.MenuItemClick += (s, a) =>
            {
                switch (a.Item.ItemId)
                {
                    case Resource.Id.popup_sell:
                        // sell
                        ConfirmSellDialog(position);
                        break;
                    case Resource.Id.popup_delete:
                        // delete
                        ConfirmDeleteDialog(position);
                        break;

                }
            };

            menu.Show();
        }

        /// <summary>
        /// Confirm delete
        /// </summary>
        /// <param name="position"></param>
        private void ConfirmSellDialog(int position)
        {
            var item = _character.Items[position];
            var sellvalue = (int) Math.Floor(item.Itemprice / 2);

            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetMessage($"Sell Item for {sellvalue} gold? ")

                .SetPositiveButton("Sell", (senderAlert, args) =>
                {
                    if (position >= Count) return;
                   
                    if(_character.Party != null)
                    {
                        var campaign = _character.Party.Campaign;
                        if (campaign.Id == GCTContext.CurrentCampaign.CampaignData.Id) campaign = GCTContext.CurrentCampaign.CampaignData;

                        campaign.CityProsperity = System.Math.Max(1, campaign.CityProsperity);
                        var prosperityLevel = Helper.GetProsperityLevel(campaign.CityProsperity);

                        if (_character.Items[position].Prosperitylevel > prosperityLevel)
                        {
                            if (campaign.UnlockedItems == null) campaign = CampaignRepository.Get(campaign.Id);
                            if (!campaign.UnlockedItems.Contains(_character.Items[position]))
                            {
                                // Add as unlock to store                       
                                campaign.UnlockedItems.Add(_character.Items[position]);
                                CampaignRepository.InsertOrReplace(campaign);
                            }
                        }
                    }

                    _character.Items.Remove(_character.Items[position]);
                    _characteritemsfrag.UpdateCharacterGold(sellvalue);

                    SaveCharacter();
                    NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }

        /// <summary>
        /// Confirm delete
        /// </summary>
        /// <param name="position"></param>
        private void ConfirmDeleteDialog(int position)
        {
            var item = _character.Items[position];

            new CustomDialogBuilder(_context, Resource.Style.MyDialogTheme)
                .SetMessage("Delete Item?")

                .SetPositiveButton(_context.Resources.GetString(Resource.String.YesDelete), (senderAlert, args) =>
                {
                    if (position >= Count) return;
                    _character.Items.Remove(item);
                    SaveCharacter();
                    NotifyDataSetChanged();
                })
                .SetNegativeButton(_context.Resources.GetString(Resource.String.NoCancel), (senderAlert, args) => { })
                .Show();
        }
    }
}