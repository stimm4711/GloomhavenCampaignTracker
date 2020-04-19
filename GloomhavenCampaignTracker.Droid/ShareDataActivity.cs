using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Data.ViewEntities;
using GloomhavenCampaignTracker.Droid.Adapter;
using GloomhavenCampaignTracker.Droid.CustomControls;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.Linq;
using Android.Support.V4.App;
using Android.Views;
using GloomhavenCampaignTracker.Business.Network;
using Data;
using GloomhavenCampaignTracker.Business;

namespace GloomhavenCampaignTracker.Droid
{
    [Activity(Label = "ShareDataActivity")]
    public class ShareDataActivity : AppCompatActivity
    {
        private Guid _guid;

        //private const string _guidpref = "deviceguid";
        //private const int Port = 54218;

        private TextView _serverstatus;
        private TextView _serveripaddress;
        private EditText _txtServerIP;
        private EditText _txtUsername;
        private Button _btnStartServer;
        private Button _btnConnectToServer;
        private Button _btnShowCampaign;

        private static readonly int ServerStartedNotificationId = 1000;
        private static readonly int ClientConnectedNotificationId = 2000;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_share);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Share Data";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _serverstatus = FindViewById<TextView>(Resource.Id.txt_serverstatus);
            _serveripaddress = FindViewById<TextView>(Resource.Id.txt_serveripadress);
            _txtServerIP = FindViewById<EditText>(Resource.Id.edit_serveripaddress);
            _txtUsername = FindViewById<EditText>(Resource.Id.edit_username);
            _btnConnectToServer = FindViewById<Button>(Resource.Id.btn_connect);
            _btnStartServer = FindViewById<Button>(Resource.Id.btn_serverstart);
            _btnShowCampaign = FindViewById<Button>(Resource.Id.btn_select_campaign);

            var username = GCTContext.Settings.Username; 

            _txtUsername.Text = username;

            if (!_btnStartServer.HasOnClickListeners)
            {
                _btnStartServer.Click += _btnStartServer_Click;
            }

            if (!_btnConnectToServer.HasOnClickListeners)
            {
                _btnConnectToServer.Click += _btnConnectToServer_Click;
            }

            _btnShowCampaign.Enabled = false;
            _btnShowCampaign.Alpha = 0.5f;

            if (!_btnShowCampaign.HasOnClickListeners)
            {
                _btnShowCampaign.Click += _btnShowCampaign_Click;
            }

            GetGuid();

            UpdateGUIServerIsRunning();

            UpdateConnectedToServer();
        }

        private void UpdateConnectedToServer()
        {
            if(GloomhavenClient.IsClientRunning())
            {
                _btnConnectToServer.Text = "Disconnect";
                _btnShowCampaign.Enabled = true;
                _btnShowCampaign.Alpha = 1;
            }
            else
            {
                _btnConnectToServer.Text = "Connect";
                _btnShowCampaign.Enabled = false;
                _btnShowCampaign.Alpha = 0.5f;
            }
           
        }

        private void UpdateGUIServerIsRunning()
        {
            if (GloomhavenServer.IsServerRunning())
            {
                _serveripaddress.Text = GloomhavenServer.Instance.GetLocalIpAddress();
                _serverstatus.Text = "Online";
                _serverstatus.SetTextColor(Color.DarkOliveGreen);
                _btnStartServer.Text = "Stop Server";
            }
            else
            {
                _serveripaddress.Text = "";
                _serverstatus.Text = "Offline";
                _serverstatus.SetTextColor(Color.DarkOrange);
                _btnStartServer.Text = "Start Server";
            }
        }

        private void _btnShowCampaign_Click(object sender, EventArgs e)
        {
            if (!GloomhavenClient.IsClientRunning()) return;
            
            Task.Run(async () =>
            {
                GloomhavenClient.Instance.SendRequest(PAYLOADTYPES.CAMPAIGNLISTREQUEST);

                var resonse = await GloomhavenClient.Instance.Recieve();

                if (resonse.PayloadType == PAYLOADTYPES.CAMPAIGNLISTRESPONSE)
                {
                    var campaignList = JsonConvert.DeserializeObject<List<DL_VIEW_Campaign>>(resonse.JSONPayload);

                    RunOnUiThread(() =>
                    {
                        try
                        {
                            // Show dialog with selectable scenarios and radio buttons
                            var view = LayoutInflater.Inflate(Resource.Layout.alertdialog_listview, null);
                            var listview = view.FindViewById<ListView>(Resource.Id.listView);
                            listview.ItemsCanFocus = true;
                            listview.ChoiceMode = ChoiceMode.Single;

                            var adapter = new SelectableCampaignAdapter(this, campaignList);
                            listview.Adapter = adapter;

                            new CustomDialogBuilder(this, Resource.Style.MyDialogTheme)
                                .SetCustomView(view)
                                .SetTitle("Select campaign from server")
                                .SetPositiveButton("Download", (senderAlert, args) =>
                                {
                                    var selectedCampaign = adapter.GetSelected();
                                    if (selectedCampaign == null) return;
                                                             
                                    LoadCampaignAsync(selectedCampaign.CampaignId);                                
                                })
                                .Show();
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            throw;
                        }
                    });
                }
            });            
        }

        private async void LoadCampaignAsync(int selectedCampaignId)
        {
            if (!GloomhavenClient.IsClientRunning()) return;

            GloomhavenClient.Instance.SendCampaignRequest(selectedCampaignId);

            var resonse = await GloomhavenClient.Instance.Recieve();

            if (resonse.PayloadType == PAYLOADTYPES.CAMPAIGNDATA_RESPOMSE)
            {
                var campaignData = JsonConvert.DeserializeObject<CampaignExchangeData>(resonse.JSONPayload);

                if (campaignData == null) return;

                var campBuilder = new CampaignBuilder(campaignData);
                campBuilder.BuildCampaign();

                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, "Canpaign loaded", ToastLength.Long).Show();
                });
            }       
        }

        private void GetGuid()
        {
            var guid = GloomhavenSettingsRepository.Get().FirstOrDefault(x => x.Name == "DeviceGuid");

            if (guid == null)
            {
                _guid = Guid.NewGuid();
                GloomhavenSettingsRepository.InsertOrReplace(new Shared.Data.Entities.DL_GlommhavenSettings() { Name = "DeviceGuid", Value = _guid.ToString() });
            }
            else
            {
                Guid.TryParse(guid.Value, out _guid);
            }
        }

        private void _btnConnectToServer_Click(object sender, EventArgs e)
        {
            var ipaddress = _txtServerIP.Text;
            var username = _txtUsername.Text;

            if (!GloomhavenClient.IsClientRunning())
            {
                if (string.IsNullOrEmpty(ipaddress))
                {
                    Toast.MakeText(this, "Enter a valid IP Address", ToastLength.Short).Show();
                    return;
                }

                if(string.IsNullOrEmpty(username))
                {
                    Toast.MakeText(this, "Enter an username", ToastLength.Short).Show();
                    return;
                }

                GCTContext.Settings.Username = username;

                GloomhavenClient.ClientGuid = _guid;
                GloomhavenClient.ServerIPAddress = ipaddress;
                
                var message = "";

                Task.Run(async () =>
                {
                    try
                    {
                        await GloomhavenClient.Instance.Connect();
                    }
                    catch
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, "Can't connect to server", ToastLength.Short).Show();                            
                        });

                        GloomhavenClient.StopClient();
                        return;
                    }

                    GloomhavenClient.Instance.SendClientConfig(username, DatabaseUpdateHelper.Dbversion.ToString());

                    var resonse = await GloomhavenClient.Instance.Recieve();

                    if (resonse.PayloadType == PAYLOADTYPES.SERVERCONFIG)
                    {
                        var serverconfig = JsonConvert.DeserializeObject<ServerConfig>(resonse.JSONPayload);
                        
                        RunOnUiThread(() =>
                        {
                            try
                            {
                                message = $"Connected to Server {ipaddress} - {serverconfig.DBVersion}.";
                                GloomhavenClient.Instance.ServerGuid = serverconfig.ServerGuid;
                                UpdateConnectedToServer();
                                ShowConnectedToServerNotification();
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception);
                                throw;
                            }
                            Toast.MakeText(this, message, ToastLength.Long).Show();
                        });
                    }                   
                });                
            }
            else
            {
                Client_Disconnect(ipaddress);
            }
        }

        private void ShowConnectedToServerNotification()
        {
            // Build the notification:
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                .SetAutoCancel(false)
                .SetContentTitle("Connected to Server")
                .SetSmallIcon(Resource.Drawable.ic_ability_retaliate)
                .SetContentText($"You are connected to Server {GloomhavenClient.ServerIPAddress}")
                .SetOngoing(true);

            // Finally, publish the notification:
            NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(ClientConnectedNotificationId, builder.Build());
        }

        private void Client_Disconnect(string ipaddress)
        {
            var message = "";
            Task.Run(async () =>
            {
                var r = await GloomhavenClient.Instance.DisConnect();
                GloomhavenClient.StopClient();

                RunOnUiThread(() =>
                {
                    if (r)
                    {
                        try
                        {
                            message = $"Disconnected from Server {ipaddress}.";
                            _btnConnectToServer.Text = "Connect";
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            throw;
                        }
                    }
                    else
                    {
                        message = $"Disconnect from Server {ipaddress} failed.";
                    }

                    Toast.MakeText(this, message, ToastLength.Long).Show();

                    UpdateConnectedToServer();

                    RemoveConnectedToServerNotification();
                });
            });
        }

        private void RemoveConnectedToServerNotification()
        {
            NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(ClientConnectedNotificationId);
        }

        private void _btnStartServer_Click(object sender, EventArgs e)
        {
            if(!GloomhavenServer.IsServerRunning())
            {
                if(_guid != null)
                {
                    Toast.MakeText(this, "Starting Server...", ToastLength.Short).Show();
                    
                    GloomhavenServer.Serverguid = _guid;
 
                    GloomhavenServer.Instance.ClientConnected += ShowClientConnected;

                    var campaigns = CampaignRepository.Get(recursive: false);

                    foreach (var c in campaigns)
                    {
                        //c.HostingDeviceGuid = _guid.ToString();

                        CampaignRepository.InsertOrReplace(c, false);
                    }

                    Task.Run(() =>
                    {
                        try
                        {
                            GloomhavenServer.Instance.Start();
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(this, "Server started.", ToastLength.Short).Show();
                                UpdateGUIServerIsRunning();

                                ShowNotifactionServerRunning();
                            });
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex);
                            throw;
                        }                        
                    });                   
                }  
            }
            else
            {
                if (GloomhavenServer.IsServerRunning())
                {
                    Toast.MakeText(this, "Stopping Server...", ToastLength.Short).Show();
                    Task.Run(() => GloomhavenServer.Instance.Stop()).Wait();

                    Toast.MakeText(this, "Server stopped.", ToastLength.Short).Show();

                    GloomhavenServer.StopServer();
                    UpdateGUIServerIsRunning();

                    HideServerIsRunningNotification();
                }                
            }                     
        }

        private void HideServerIsRunningNotification()
        {
            NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(ServerStartedNotificationId);
        }

        private void ShowNotifactionServerRunning()
        {
            // Build the notification:
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                .SetAutoCancel(false)                   
                .SetContentTitle("Server is running")                        
                .SetSmallIcon(Resource.Drawable.ic_ability_range)  
                .SetContentText("You are hosting Gloomhaven")
                .SetOngoing(true); 

            // Finally, publish the notification:
            NotificationManager notificationManager =(NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(ServerStartedNotificationId, builder.Build());
        }

        private async Task ShowClientConnected(object sender,ClientConnectedEventArgs e)
        {
            await Task.Run(() =>
            {
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, e.Message, ToastLength.Short).Show();
                });
            });
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var id = item.ItemId;
            if (id != Android.Resource.Id.Home) return base.OnOptionsItemSelected(item);
            Finish();
            return true;
        }
    }
}