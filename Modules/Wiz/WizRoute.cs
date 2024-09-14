using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TRINET_CORE.Database;
using TRINET_CORE.Modules.Wiz;

namespace TRINET_CORE.Modules.Wiz
{
    public class WizRoute
    {


        public static WebApplication MountWizRoutes(WebApplication app, WebApplicationBuilder builder)
        {
            app.MapPost("/wiz/command", async (TrinetDatabase db, WizModule module, DeviceRequest request) =>
            {
                Console.WriteLine("Command received");
                Device? device = await db.Devices.FirstOrDefaultAsync(d => d.Id == request.DeviceId);
                if (device == null)
                {
                    return Results.NotFound();
                }
                Console.WriteLine($"forwarding request {request.Request}");
                string result = await module.SendDeviceApiRequest(device, request.Request);
                Console.WriteLine("returning " + result);
                return Results.Ok(result);
            });

 

            return app;
        }

        
    }
}
