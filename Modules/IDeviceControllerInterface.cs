using TRINET_CORE.Database;

namespace TRINET_CORE.Modules
{
    public interface IDeviceControllerInterface
    {

        public TrinetModuleRequestResult SendDeviceApiRequest(Device device, string request);


        public Task<string> GetDeviceStatus(Device device);
    }
}