using System.Net.Sockets;
using System.Text.Json;
using TRINET_CORE.Database;

namespace TRINET_CORE.Modules.Wiz
{

    public enum EWizBulbRequestMethod
    {
        setPilot,
        getPilot,
        setState
    }

    public class WizBulbRequestParamsBase { }

    public class WizBulbColourRequestParams : WizBulbRequestParamsBase
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int Dimming { get; set; }
    }

    public class WizStateRequest : WizBulbRequestParamsBase
    {
        public bool State { get; set; }
    }

    public class WizSetRequest
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public WizBulbRequestParamsBase Params { get; set; }

    }

    public class WizGetStatusRequest
    {
        public string Method { get; set; } = EWizBulbRequestMethod.getPilot.ToString();
        public WizBulbRequestParamsBase Params { get; set; }

    }

    public class WizStatusResponse
    {
        public string State { get; set; }  

    }

    public class WizBulb
    {
        private string NetworkAddress;
        private int Port;
        private string? Name;

        public WizBulb(string _NetworkAddress, int _Port, string _Name = "")
        {
            NetworkAddress = _NetworkAddress;
            Port = _Port;
            Name = _Name;
        }

        public override bool Equals(object? obj)
        {
            if (obj is WizBulb)
            {
                return this == obj;
            }

            if (obj is Device)
            {
                return this == (Device)obj;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(WizBulb bulb, Device device)
        {
            return bulb.NetworkAddress == device.NetworkAddress;
        }

        public static bool operator !=(WizBulb bulb, Device device)
        {
            return !(bulb.NetworkAddress == device.NetworkAddress);
        }

        public void HandleRequest(string request)
        {

        }

        public async Task<WizStatusResponse?> GetState()
        {
            var command = new WizGetStatusRequest();           
            byte[] cBytes = System.Text.Encoding.UTF8.GetBytes
                (
                    JsonSerializer.Serialize
                    (
                        command, new JsonSerializerOptions
                        {
                            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
                        }
                    )
                );

            UdpClient client = new UdpClient(NetworkAddress, Port);
            client.Client.ReceiveTimeout = 200;
            await client.SendAsync(cBytes);
            var buffer = await client.ReceiveAsync();
            WizStatusResponse? response = JsonSerializer.Deserialize<WizStatusResponse>(buffer.ToString()?? "");
            client.Close();
            return response;
        }



        // Send an change RGBA command
        public async void RequestColourWithDimming(WizStateRequest request)
        {
            byte[] cBytes = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));
            try
            {
                UdpClient client = new UdpClient(NetworkAddress, Port);
                client.Client.ReceiveTimeout = 200;
                await client.SendAsync(cBytes);
                client.Close();
                return;
            }
            catch (Exception ex)
            {
                // log. 
            }
        }
    }

}
