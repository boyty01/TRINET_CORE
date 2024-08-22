using TRINET_CORE.Database;

namespace TRINET_CORE.Routes
{
    public static class RoomRoute
    {

        public static WebApplication MountRoomRoutes(WebApplication app)
        {
            app.MapPost("/rooms", async (TrinetDatabase db, Room room) =>
            {
                await db.Rooms.AddAsync(room);
                await db.SaveChangesAsync();
                return Results.Created($"/rooms/{room.Id}", room);
            });

            app.MapGet("/rooms/{id}", async (TrinetDatabase db, Guid id) =>
            {
                return await db.Rooms.FindAsync(id);

            });

            app.MapDelete("/rooms/{id}", async (TrinetDatabase db, Guid id) =>
            {
                var Room = await db.Rooms.FindAsync(id);
                if (Room == null) return Results.NotFound();
                db.Rooms.Remove(Room);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            return app;
        }

    }
}
