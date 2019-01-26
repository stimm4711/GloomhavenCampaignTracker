using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sockets.Plugin;

namespace GloomhavenCampaignTracker.Business.Network
{
    public class Client : NetworkBase
    {
        protected readonly TcpSocketClient _client;
        protected readonly string _ipAddressServer;
        
        public Client(string ipAddressServer, int port) : base(port)
        {
            _ipAddressServer = ipAddressServer;
            _client = new TcpSocketClient();
        }

        public async void Send(DataExchangeProtocoll data)
        {
            try
            {
                var bytes = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(data));
                await _client.WriteStream.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<DataExchangeProtocoll> Recieve()
        {
            try
            {
                int bytesize = 1024 * 1024;
                byte[] buffer = new byte[bytesize];

                var bytesread = await _client.ReadStream.ReadAsync(buffer, 0, bytesize);

                if (bytesread <= 0) return null;

                return GetData(buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> Connect()
        {
            try
            {
                await _client.ConnectAsync(_ipAddressServer, _port);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DisConnect()
        {
            try
            {
                await _client.DisconnectAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
