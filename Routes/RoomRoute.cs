using Microsoft.EntityFrameworkCore;
using TRINET_CORE.Database;

namespace TRINET_CORE.Routes
{
    public static class RoomRoute
    {

        public static WebApplication MountRoomRoutes(WebApplication app)
        {

            /**
             * Create a new room
             */
            app.MapPost("/rooms", async (TrinetDatabase db, Room room) =>
            {
                await db.Rooms.AddAsync(room);
                await db.SaveChangesAsync();
                return Results.Created($"/rooms/{room.Id}", room);
            });


            /**
             * Update an existing room
             */
            app.MapPut("/rooms/{room_id}", async (TrinetDatabase db, Room room, Guid room_id) =>
            {
                var ExistingRoom = await db.Rooms.FindAsync(room_id);
                if (ExistingRoom == null) return Results.NotFound();

                ExistingRoom.Name = room.Name;
                ExistingRoom.LocationId = room_id;
                await db.SaveChangesAsync();
                return Results.Ok();
            });


            /**
             * Delete an existing room. CASCADING DELETE. This will delete all associated devices.
             */
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
