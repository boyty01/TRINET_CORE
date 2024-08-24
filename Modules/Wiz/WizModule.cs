using Microsoft.EntityFrameworkCore;
using System.Net;
using TRINET_CORE.Database;

namespace TRINET_CORE.Modules.Wiz

{

    public enum EBulbRequestCommand
    {
        STATUS,
        POWER,
        COLOUR,
        COLOUR_DIM,
        DIM
    }



    public class WizModule : ModuleBase, IDeviceControllerInterface
    {
        private IServiceScopeFactory ServiceScopeFactory;
        public bool Initialised = false;
        public List<WizBulb> WizBulbs = [];
        public int DefaultBulbCommsPort = 38899;


        public WizModule(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Init();
        }


        public async void Init()
        {
            await RefreshDevices();
            Initialised = true;
        }


        /*Refresh all device controller instances */
        public async Task RefreshDevices()
        {
            WizBulbs.Clear();
            IServiceScope scope = ServiceScopeFactory.CreateScope();
            TrinetDatabase db = scope.ServiceProvider.GetRequiredService<TrinetDatabase>();
            List<Device> wizDevices = await db.Devices.Where(d => d.DeviceManufacturer == ETrinetDeviceManufacturer.WIZ).ToListAsync();

            wizDevices.ForEach(u =>
            {
                if (u.DeviceType == ETrinetDeviceType.LIGHT_BULB)
                {
                    WizBulbs.Add(new WizBulb(u.NetworkAddress ?? "", DefaultBulbCommsPort, u.Name ?? ""));
                }
            });
            scope.Dispose();
        }

        private WizBulb? GetBulbFromDevice(Device device)
        {
            return WizBulbs.Find(b => b == device);
        }


        public TrinetModuleRequestResult SendDeviceApiRequest(Device device, string request)
        {
            if (device.DeviceType == ETrinetDeviceType.LIGHT_BULB)
            {
                return BulbRequest(device, request);
            }

            return new TrinetModuleRequestResult
            {
                Message = "Unsupported device type.",
                Status = ERequestStatus.FAILED
            };
        }


        public async Task<string> GetDeviceStatus(Device device)
        {
            if (device.NetworkAddress is null) throw new ArgumentNullException("Requested device status with null network address");

            IPEndPoint IP = new(IPAddress.Parse(device.NetworkAddress), DefaultBulbCommsPort);

            return "";
        }


        public TrinetModuleRequestResult BulbRequest(Device device, string request)
        {

            return new TrinetModuleRequestResult
            {
                Message = "Sent",
                Status = ERequestStatus.SENT
            };
        }


    }
}
