using TRINET_CORE.Database;

namespace TRINET_CORE.Modules
{
    public interface IDeviceControllerInterface
    {

        public Task<string> SendDeviceApiRequest(Device device, string request);


    }
}