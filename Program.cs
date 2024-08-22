using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using TRINET_CORE.Database;
using TRINET_CORE.Routes;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var connectionString = builder.Configuration.GetConnectionString("TrinetDatabase") ?? "Data Source=TrinetDatabase.db";
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSqlite<TrinetDatabase>(connectionString); //options => options.UseInMemoryDatabase("items"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TriNet Core API", Description = "Main TriNet API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TriNet API V1");
    });
}

LocationRoute.MountLocationRoutes(app);
RoomRoute.MountRoomRoutes(app);
DeviceRoute.MountDeviceRoutes(app);
app.Run();