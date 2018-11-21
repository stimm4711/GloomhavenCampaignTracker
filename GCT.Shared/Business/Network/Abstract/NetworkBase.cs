using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sockets.Plugin.Abstractions;

namespace GloomhavenCampaignTracker.Shared.Business.Network
{
    public class NetworkBase
    {
        protected int _port;

        public NetworkBase(int port)
        {
            _port = port;
        }

        public async void Send(DataExchangeProtocoll data, ITcpSocketClient client)
        {
            try
            {
                var bytes = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(data));
                await client.WriteStream.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async void SendNullResponse(ITcpSocketClient client)
        {
            try
            {
                var bytes = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(new DataExchangeProtocoll() { JSONPayload = "", PayloadLength = 0, PayloadType = PAYLOADTYPES.NULL_RESPONSE }));
                await client.WriteStream.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<DataExchangeProtocoll> Recieve(ITcpSocketClient client)
        {
            try
            {
                int bytesize = 1024*1024;
                byte[] buffer = new byte[bytesize];

                var bytesread = await client.ReadStream.ReadAsync(buffer, 0, bytesize);

                if (bytesread <= 0) return null;

                return GetData(buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected DataExchangeProtocoll GetData(byte[] buffer)
        {
            var message = Encoding.Unicode.GetString(buffer);
            var data = JsonConvert.DeserializeObject<DataExchangeProtocoll>(message);
            return data;
        }

        public string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
