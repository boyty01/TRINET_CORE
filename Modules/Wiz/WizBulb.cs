using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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


    // empty params
    public class WizBulbRequestParamsBase { }


    // set pilot params

    public class SetPilotParams : WizBulbRequestParamsBase
    {
        public bool? State { get; set; }
        public int? SceneId { get; set; }
        public int? R { get; set; }
        public int? G { get; set; }
        public int? B { get; set; }
        public int? C { get; set; }
        public int? W { get; set; }
        public int? Temp { get; set; }
        public int? Dimming { get; set; }
    }


    // set state params
    public class SetStateParams : WizBulbRequestParamsBase
    {
        public bool State { get; set; }
    }


    // container for request body from Trinet client
    public class WizRequestBase
    {
        public EWizBulbRequestMethod Method { get; set; }
        public string Request { get; set; } = null!;

        public override string ToString()
        {
            return $"{Method} {Request} ";
        }
    }


    // set pilot request 
    public class SetPilotRequest
    {
        public int? Id { get; set; }
        public string Method { get; set; } = null!;
        public WizBulbRequestParamsBase? Params { get; set; }

    }


    // prepared get pilot request. 
    public class GetPilotRequest
    {
        public string Method { get; set; } = EWizBulbRequestMethod.getPilot.ToString();
        public WizBulbRequestParamsBase Params { get; set; } = new WizBulbRequestParamsBase();

    }


    // get pilot response 
    public class GetPilotResponse
    {
        public string Method { get; set; }
        public string Env { get; set; } 
        public GetPilotResponseResult Result { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this).ToString();
        }
    }


    // get pilot response Result
    public class GetPilotResponseResult
    {
        public string? Mac { get; set; }
        public int? Rssi { get; set; }
        public bool? State { get; set; }
        public int? SceneId { get; set; }
        public int? R { get; set; }
        public int? G { get; set; }
        public int? B { get; set; }
        public int? C { get; set; }
        public int? W { get; set; }
        public int? Temp { get; set; }
        public int? Dimming { get; set; }
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


        /**
         * Main entry
         */
        public async Task<string> HandleRequest(string request)
        {

            return await SendRaw(request);

            Console.WriteLine("Handling request");
            Console.WriteLine($"Deserialising {request}");
            var obj = JsonSerializer.Deserialize<WizRequestBase>(request, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            Console.WriteLine(obj?.ToString());
            if (obj is null) return "{\"error\":\"Bad request. Bad Payload.\"}";
           
            // Get state
            if (obj.Method == EWizBulbRequestMethod.getPilot)
            {
                return await GetState();
            }

            // send request command. SetPilot and SetState 
            if (obj.Method == EWizBulbRequestMethod.setPilot || obj.Method == EWizBulbRequestMethod.setState)
            {
                Console.WriteLine(obj.Request);
                var JsonRequest = JsonSerializer.Deserialize<SetPilotRequest>(obj.Request, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                Console.WriteLine("set pilot command");
                if (JsonRequest is null) return "{\"error\":\"Bad request. Bad Payload. \"}";

                return await SendRequest(JsonRequest);
            }

            return "{\"error\":\"Bad request. Unknown method.\"}";

        }

        private async Task<string> SendRaw(string request)
        {
            Console.WriteLine($"Sending {request}");
            UdpClient client = new UdpClient();
            byte[] cBytes = System.Text.Encoding.UTF8.GetBytes(request);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(NetworkAddress), Port);
            await client.SendAsync(cBytes, ep);
            Console.WriteLine("Sent");
            // get response as string
            var buffer = await client.ReceiveAsync();
            Console.WriteLine("Received response");
            var asString = System.Text.Encoding.UTF8.GetString(buffer.Buffer, 0, buffer.Buffer.Length);
            Console.WriteLine(asString);
            client.Close();
            return asString;
        }

        /** 
         * send a getPilot command.
         */
        private async Task<string> GetState()
        {
            // serialize
            var command = new GetPilotRequest();  
            var cConv = JsonSerializer.Serialize(command, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase});         
            byte[] cBytes = System.Text.Encoding.UTF8.GetBytes(cConv);

            // send request
            UdpClient client = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(NetworkAddress), Port);            
            await client.SendAsync(cBytes, ep);

            // get response as string
            var buffer = await client.ReceiveAsync();
            var asString = System.Text.Encoding.UTF8.GetString(buffer.Buffer, 0, buffer.Buffer.Length);
            client.Close();
            return asString;
        }



        /**
         * Send a set state / pilot command.
         */
        private async Task<string> SendRequest(SetPilotRequest request)
        {
            byte[] cBytes = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));
            try
            {
                UdpClient client = new UdpClient(NetworkAddress, Port);
                client.Client.ReceiveTimeout = 200;
                await client.SendAsync(cBytes);
                client.Close();
                return "OK";
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

 
    }

}
