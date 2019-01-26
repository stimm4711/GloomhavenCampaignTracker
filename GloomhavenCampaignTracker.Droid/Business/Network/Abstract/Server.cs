using System;
using Android.OS;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;

namespace GloomhavenCampaignTracker.Business.Network
{
    public abstract class Server : NetworkBase
    {
        protected TcpSocketListener _listener;

        public Server(int port) : base(port)
        {

        }

        protected abstract void HandleData(DataExchangeProtocoll data, ITcpSocketClient client);

        public async void Start()
        {
            _listener = new TcpSocketListener();

            // when we get connections, read bytes until we get -1 (eof)
            _listener.ConnectionReceived += async (sender, args) =>
            {
                var client = args.SocketClient;

                while (true)
                {
                    try
                    {
                        
                        DataExchangeProtocoll data = await Recieve(client);

                        if (data == null) return; // Client disconnected

                        HandleData(data, client);                      
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            };

            // bind to the listen port across all interfaces
            await _listener.StartListeningAsync(_port);
        }

        public async void Stop()
        {
            await _listener.StopListeningAsync();
        }
    }
}
