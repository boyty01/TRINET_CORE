using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TRINET_CORE.Database;
using TRINET_CORE.Modules.Wiz;

namespace TRINET_CORE.Modules.Nanoleaf
{

    public class NanoleafNewResponse 
    {
        public string AuthToken { get; set; }
    }

    public class NanoleafModule : ModuleBase, IDeviceControllerInterface
    {

        public int DefaultNanoleafPort = 16021;
        public HttpClient NanoClient { get; set; } = new HttpClient();

        public ICollection<NanoleafPanel> Shapes  { get; private set; } = [];
        private IServiceScopeFactory ServiceScopeFactory { get; set; }
        private bool Initialised = false;

        public NanoleafModule(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Init();
        }

        private async void Init()
        {
            await RefreshDevices();
            Initialised = true;
        }

        public Task<string> SendDeviceApiRequest(Device device, string request)
        {
            throw new NotImplementedException();
        }


        public async Task RefreshDevices()
        {
            Shapes.Clear();
            IServiceScope scope = ServiceScopeFactory.CreateScope();
            TrinetDatabase db = scope.ServiceProvider.GetRequiredService<TrinetDatabase>();
            List<Device> nanoleafPanels = await db.Devices.Where(d => d.DeviceManufacturer == ETrinetDeviceManufacturer.NANOLEAF).ToListAsync();

            nanoleafPanels.ForEach(async u =>
            {
                if (u.DeviceType == ETrinetDeviceType.LIGHT_PANEL)
                {
                    DeviceAuthorisationData? authDevice = await db.DeviceAuthorisationData.Where(d => d.DeviceId == u.Id).FirstOrDefaultAsync();
                   
                    if(authDevice is null)
                    {
                        Console.WriteLine($"Error. Authorised device id: {u.Id} is missing authorisation data.");
                        return;
                    }
                    AuthorisedDevice finalAuthDevice = new AuthorisedDevice(u, authDevice);
                    Shapes.Add(new NanoleafPanel(finalAuthDevice));
                }
            });

            scope.Dispose();
        }

        /**
         * Attempt to pair with a nanoleaf device and procure an auth token.
         */
        public async Task<bool> PairDevice(Device device)
        {
            NanoClient.BaseAddress = new Uri(device.NetworkAddress ?? "");
            var response = await NanoClient.PostAsync("/api/v1/new", JsonContent.Create<object>(new object()));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var rContent = await response.Content.ReadAsStringAsync();
                NanoleafNewResponse? auth = JsonSerializer.Deserialize<NanoleafNewResponse>(rContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });
                
                if (auth is null)
                {
                    throw new Exception("Error deserializing Nanoleaf pairing response. Chances are the serializer is misconfigured to handle snake case.");
                }

                IServiceScope scope = ServiceScopeFactory.CreateScope();
                TrinetDatabase db = scope.ServiceProvider.GetRequiredService<TrinetDatabase>();

                db.DeviceAuthorisationData.Add(new DeviceAuthorisationData { DeviceId = device.Id, AuthToken = auth.AuthToken });
                await db.SaveChangesAsync();
                scope.Dispose();

                return true;
            }
            return false;
        }

    }

}
