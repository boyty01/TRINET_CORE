using TRINET_CORE.Database;
using TRINET_CORE.Modules.Nanoleaf;
using Microsoft.EntityFrameworkCore;
namespace TRINET_CORE.Modules.Nanoleaf
{
    public class NanoleafRoute
    {

        public static WebApplication MountNanoleafRoutes(WebApplication app, WebApplicationBuilder builder)
        {

            app.MapPost("/nanoleaf/pair", async (TrinetDatabase db, NanoleafModule module, DeviceRequest request) =>
            {
                try
                {
                    var device = await db.Devices.FirstOrDefaultAsync(u => u.Id == request.DeviceId);

                    if (device != null) 
                    {
                        await module.PairDevice(device);
                        return Results.Created($"devices/{device.Id}", device);
                    }

                    return Results.NotFound();
                }
                catch (Exception ex)
                {
                    return Results.Problem();
                }
            });

            return app;
        }

    }
}
