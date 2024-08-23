using Microsoft.EntityFrameworkCore;


namespace TRINET_CORE.Database
{
    public enum ETrinetDeviceType
    {
        UNDEFINED,
        LIGHT_BULB,
        LIGHT_PANEL,
        TELEVISION,
        DOOR_LOCK,
        DOORBELL,
        WEB_CAMERA,
        WASHING_MACHINE,
        MICROWAVE,
        FRIDGE,
        KETTLE
    }

    public enum ETrinetDeviceManufacturer
    {
        NONE,
        SAMSUNG,
        LG,
        WIZ,
        NANOLEAF
    }


    public class Location
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public ICollection<Room> Rooms { get; } = new List<Room>();
    }



    public class Room
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid LocationId { get; set; }

        public ICollection<Device> Devices { get; } = new List<Device>();

    }


    public class Device
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public string? InternalName { get; set; }
        public string? NetworkAddress { get; set; }
        public ETrinetDeviceType DeviceType { get; set; } = ETrinetDeviceType.UNDEFINED;
        public ETrinetDeviceManufacturer DeviceManufacturer { get; set; } = ETrinetDeviceManufacturer.NONE;

        public Guid RoomId { get; set; }
    }


    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    
    }

    public class TrinetDatabase : DbContext
    {

        public TrinetDatabase(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                .HasMany(e => e.Rooms)
                .WithOne()
                .HasForeignKey(e => e.LocationId)
                .HasPrincipalKey(e => e.Id)
                .IsRequired();
        }

        public DbSet<Location> Locations { get; set; } = null!;

        public DbSet<Room> Rooms { get; set; } = null!;

        public DbSet<Device> Devices { get; set; } = null!;
    }

}