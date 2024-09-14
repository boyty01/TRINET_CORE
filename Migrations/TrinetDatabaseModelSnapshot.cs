﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TRINET_CORE.Database;

#nullable disable

namespace TRINET_CORE.Migrations
{
    [DbContext(typeof(TrinetDatabase))]
    partial class TrinetDatabaseModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("TRINET_CORE.Database.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("DeviceManufacturer")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeviceType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InternalName")
                        .HasColumnType("TEXT");

                    b.Property<string>("MacAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("NetworkAddress")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("TRINET_CORE.Database.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Locations");

                    b.HasData(
                        new
                        {
                            Id = new Guid("b20ec704-a3e3-493a-952e-9ee06eeffeb5"),
                            Name = "Default"
                        });
                });

            modelBuilder.Entity("TRINET_CORE.Database.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("TRINET_CORE.Database.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("PasswordResetRequired")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RefreshTokenExpiry")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserAccessLevel")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("cc83b1c3-20b2-4ae4-b81f-aeb2da3c0515"),
                            LocationId = new Guid("b20ec704-a3e3-493a-952e-9ee06eeffeb5"),
                            Password = "AQAAAAIAAYagAAAAEJlUNwBNFK7izjkdcqDFs7fnkJw54tpm38asXEA6+oqM+l6unRPwpA5o+FLwOfNbuw==",
                            PasswordResetRequired = true,
                            RefreshTokenExpiry = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserAccessLevel = 4,
                            Username = "Admin"
                        });
                });

            modelBuilder.Entity("TRINET_CORE.Database.Device", b =>
                {
                    b.HasOne("TRINET_CORE.Database.Room", null)
                        .WithMany("Devices")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TRINET_CORE.Database.Room", b =>
                {
                    b.HasOne("TRINET_CORE.Database.Location", null)
                        .WithMany("Rooms")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TRINET_CORE.Database.Location", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("TRINET_CORE.Database.Room", b =>
                {
                    b.Navigation("Devices");
                });
#pragma warning restore 612, 618
        }
    }
}
