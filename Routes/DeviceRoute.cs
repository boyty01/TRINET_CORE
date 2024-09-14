using TRINET_CORE.Database;

namespace TRINET_CORE.Routes
{
    public class DeviceRoute
    {
        public static WebApplication MountDeviceRoutes(WebApplication app)
        {

            /**
             * Create a new device.
             */
            app.MapPost("/devices", async (TrinetDatabase db, Device device) =>
            {
                try
                {
                    db.Devices.Add(device);
                    await db.SaveChangesAsync();
                    return Results.Created($"/devices/{device.Id}", device);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });


            /**
             * Update existing device record.
             */
            app.MapPut("/devices/{device_id}", async (TrinetDatabase db, Device device, Guid device_id) =>
            {
                var ExistingDevice = await db.Devices.FindAsync(device_id);
                if (ExistingDevice == null) return Results.NotFound();

                ExistingDevice.Name = device.Name;
                ExistingDevice.NetworkAddress = device.NetworkAddress;
                ExistingDevice.RoomId = device.RoomId;
                await db.SaveChangesAsync();
                return Results.Ok();
            });


            /**
             * Delete the device with the specified id.
             */
            app.MapDelete("/devices/{device_id}", async (TrinetDatabase db, Guid device_id) =>
            {
                var device = await db.Devices.FindAsync(device_id);
                if (device == null) return Results.NotFound();

                db.Devices.Remove(device);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            /**
             * add authorisation data to a device
            */
            app.MapPost("/devices/{device_id}/auth", async (TrinetDatabase db, Guid device_id, AuthorisedDevice device) =>
            {
                try
                {
                    if(await db.Devices.FindAsync(device_id) is null)
                    {
                        return Results.NotFound();
                    }
                        
                    db.DeviceAuthorisationData.Add(device.AuthorisationData);
                    await db.SaveChangesAsync();
                    return Results.Created($"/devices/{device.Device.Id}", device.Device);

                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }

            });

            return app;

        }
    }
}
