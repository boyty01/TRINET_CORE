using TRINET_CORE.Database;

namespace TRINET_CORE.Modules.Nanoleaf
{
    public class NanoleafPanel
    {
        public AuthorisedDevice AuthorisedDevice { get; set; }

        public NanoleafPanel(AuthorisedDevice authorisedDevice)
        {
            AuthorisedDevice = authorisedDevice;
        }

    }
}
