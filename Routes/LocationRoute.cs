using Microsoft.EntityFrameworkCore;
using TRINET_CORE.Database;

namespace TRINET_CORE.Routes
{
    public static class LocationRoute
    {

        public static WebApplication MountLocationRoutes(WebApplication app)
        {


            /** 
             * Get all locations. Does not include dependent data. 
             */
            app.MapGet("/locations", async (TrinetDatabase db) =>
            {
                return await db.Locations.ToListAsync();
            });


            /**
             * Add a new location
             */
            app.MapPost("/locations", async (TrinetDatabase db, Location location) =>
            {
                await db.Locations.AddAsync(location);
                await db.SaveChangesAsync();
                return Results.Created($"/locations/{location.Id}", location);
            });


            /** 
             * Get location info by Id. Does not include dependent data. 
             */
            app.MapGet("/locations/{loc_id}", async (TrinetDatabase db, Guid loc_id) =>
            {
                return await db.Locations.FirstAsync(y => y.Id == loc_id);
            });


            /**
             * Update an existing location
             */
            app.MapPut("/locations/{loc_id}", async (TrinetDatabase db, Location updatelocation, Guid loc_id) =>
            {
                var location = await db.Locations.FindAsync(loc_id);
                if (location is null) return Results.NotFound();
                location.Name = updatelocation.Name;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });


            /**
             * Delete a location. CASCADING DELETE - Dependent room and device data will be lost.
             */
            app.MapDelete("/locations/{loc_id}", async (TrinetDatabase db, Guid loc_id) =>
            {
                var location = await db.Locations.FindAsync(loc_id);
                if (location is null) return Results.NotFound();
                db.Locations.Remove(location);
                await db.SaveChangesAsync();
                return Results.Ok();
            });


            /**
             * Get a location by id and all associated dependends (single depth)
             */
            app.MapGet("/locations/{loc_id}/rooms", async (TrinetDatabase db, Guid loc_id) =>
            {
                return await db.Locations.Include(u => u.Rooms).Where(l => l.Id == loc_id).ToListAsync();
            });



            /** 
             * Get a specific room associated with the given location. Does not include Room dependents (devices)
             */
            app.MapGet("/locations/{loc_id}/rooms/{room_id}", async (TrinetDatabase db, Guid loc_id, Guid room_id) =>
            {
                return await db.Rooms.Include(u => u.Devices).FirstAsync(u => u.LocationId == loc_id && u.Id == room_id);
            });



            /**
             * Get a specific room associated with the given location.
             */
            app.MapGet("locations/{loc_id}/rooms/{room_id}/devices", async (TrinetDatabase db, Guid loc_id, Guid room_id) =>
            {
                return await db.Rooms.Include(u => u.Devices).FirstOrDefaultAsync(u => u.Id == loc_id && u.Id == room_id);
            });



            return app;
        }

    }
}
