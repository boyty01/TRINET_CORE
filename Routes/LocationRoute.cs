using Microsoft.EntityFrameworkCore;
using TRINET_CORE.Database;

namespace TRINET_CORE.Routes
{
    public static class LocationRoute
    {

        public static WebApplication MountLocationRoutes(WebApplication app)
        {

            app.MapGet("/locations/{id}", async (TrinetDatabase db, Guid id) =>
            {
                return await db.Locations.Include(i => i.Rooms).FirstAsync(y => y.Id == id);
            });


            app.MapGet("/locations", async (TrinetDatabase db) => 
            { 
                return await db.Locations.Include(i=> i.Rooms).ToListAsync(); 
            });


            app.MapPost("/locations", async (TrinetDatabase db, Location location) =>
            {
                await db.Locations.AddAsync(location);
                await db.SaveChangesAsync();
                return Results.Created($"/locations/{location.Id}", location);
            });


            app.MapPut("/locations/{id}", async (TrinetDatabase db, Location updatelocation, Guid id) =>
            {
                var location = await db.Locations.FindAsync(id);
                if (location is null) return Results.NotFound();
                location.Name = updatelocation.Name;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });


            app.MapDelete("/locations/{id}", async (TrinetDatabase db, Guid id) =>
            {
                var location = await db.Locations.FindAsync(id);
                if (location is null) return Results.NotFound();
                db.Locations.Remove(location);
                await db.SaveChangesAsync();
                return Results.Ok();
            });


            app.MapGet("/locations/{loc_id}/rooms/{room_id}", async (TrinetDatabase db, Guid loc_id, Guid room_id) =>
            {                
                return await db.Rooms.Include(u=> u.Devices).FirstAsync(u => u.LocationId == loc_id && u.Id == room_id);
            });


            return app;
        }

    }
}
