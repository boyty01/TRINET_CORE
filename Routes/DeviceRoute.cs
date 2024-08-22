using TRINET_CORE.Database;

namespace TRINET_CORE.Routes
{
    public class DeviceRoute
    {
        public static WebApplication MountDeviceRoutes(WebApplication app)
        {
            app.MapGet("/devices/{id}", async (TrinetDatabase db, Guid id) =>
            {

            });
           
           app.MapGet("/devices/", () => DeviceDB.GetDevices());
           app.MapPost("devices/", (Device device) => LocationDB.CreateDevice(device));
            //     app.MapPut("/locations", (Location location) => LocationDB.UpdateLocation(location));
            //      app.MapDelete("/locations/{id}", (Guid id) => LocationDB.RemoveLocation(id));
            //     app.MapGet("/locations/{loc_id}/rooms/{room_id}", (Guid loc_id, Guid room_id) => RoomDB.GetRoomAtLocation(loc_id, room_id));
            //     app.MapGet("/locations/{loc_id}/rooms", (Guid loc_id) => RoomDB.GetRoomsAtLocation(loc_id));

            return app;
        }
    }
}
