using Microsoft.AspNetCore.Identity;
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

    public enum EUserAccessLevel
    {
        NONE,
        USER,
        CPANEL,
        BOT,
        ADMIN
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
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public EUserAccessLevel UserAccessLevel { get; set; } = EUserAccessLevel.NONE;

        public bool PasswordResetRequired { get; set; } = false;

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }

    }

    public class LoginUser
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class LoginResponse
    {
        public required string JwtToken { get; set; }
        public DateTime Expiration { get; set; }
        public required string RefreshToken { get; set; }

    }


    public class RefreshAuth
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }

    public class DeviceRequest
    {
        public required Guid DeviceId { get; set; }
        public string Request { get; set; } = null!;
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

            var hasher = new PasswordHasher<User>();


            modelBuilder.Entity<User>()
                .HasIndex(e => e.Username).IsUnique();


            // setup default admin
            modelBuilder.Entity<User>()
                .HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Admin",
                    Password = hasher.HashPassword(new User(), "%Administrator%"),
                    UserAccessLevel = EUserAccessLevel.ADMIN,
                    PasswordResetRequired = true
                });

        }


        public DbSet<Location> Locations { get; set; } = null!;

        public DbSet<Room> Rooms { get; set; } = null!;

        public DbSet<Device> Devices { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;
    }

}