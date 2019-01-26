using System;
using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Text.Method;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared;
using GloomhavenCampaignTracker.Business;

namespace GloomhavenCampaignTracker.Droid.Fragments
{
    public class SupportDevFragment : Fragment
    {
        private List<string> _patronsList;
        private LayoutInflater _inflater;

        public static SupportDevFragment NewInstance()
        {
            var frag = new SupportDevFragment { Arguments = new Bundle() };
            return frag;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _patronsList = Helper.Patronnames();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _inflater = inflater;

            // Use this to return your custom view for this Fragment
            var view =  inflater.Inflate(Resource.Layout.fragment_support_developer, container, false);
            
             var bggTxt = view.FindViewById<TextView>(Resource.Id.bggtext);
            var playstore = view.FindViewById<TextView>(Resource.Id.playstorelink);
            var patronsButton = view.FindViewById<Button>(Resource.Id.patronsButton);
            var helmetimage = view.FindViewById<ImageView>(Resource.Id.helmetimage);

            if (!helmetimage.HasOnClickListeners)
            {
                helmetimage.Click += Helmetimage_Click;
            }

            if (!patronsButton.HasOnClickListeners)
            {
                patronsButton.Click += PatronsButton_Click;
            }
                    
            bggTxt.MovementMethod = LinkMovementMethod.Instance;
            playstore.MovementMethod = LinkMovementMethod.Instance;

            return view;
        }

        private void PatronsButton_Click(object sender, EventArgs e)
        {
            var alertView = _inflater.Inflate(Resource.Layout.alertdialog_listview, null);
            var lv = alertView.FindViewById<ListView>(Resource.Id.listView);
            var adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleListItem1, _patronsList);
            lv.Adapter = adapter;

            new CustomDialogBuilder(Context, Resource.Style.MyDialogTheme)
                .SetCustomView(alertView)
                .SetTitle("Patrons")
                .SetMessage("Thanks to all who helped me improving this app with feedback, suggestions and sending me some gold.")                
                .SetPositiveButton(Context.Resources.GetString(Resource.String.OK), (senderAlert, args) =>{})
                .Show();
        }

        private void Helmetimage_Click(object sender, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse("https://www.paypal.me/GHCampaignTracker");
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }
    }
}