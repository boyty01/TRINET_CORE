using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using TRINET_CORE.Database;
using TRINET_CORE.Routes;


var builder = WebApplication.CreateBuilder(args);

// Authentication
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
// Make sure the pw hash iteration is higher than the weirdly low default from NET v7 and lower. 
builder.Services.Configure<PasswordHasherOptions>(opt => opt.IterationCount = 600_000);


// data store
var connectionString = builder.Configuration.GetConnectionString("TrinetDatabase") ?? "Data Source=TrinetDatabase.db";
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<TrinetDatabase>(connectionString); //options => options.UseInMemoryDatabase("items"));

// swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TriNet Core API", Description = "Main TriNet API", Version = "v1" });
});



var app = builder.Build();

// enable swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TriNet API V1");
    });
}

// mount routes
LocationRoute.MountLocationRoutes(app);
RoomRoute.MountRoomRoutes(app);
DeviceRoute.MountDeviceRoutes(app);
UserRoute.MountUserRoutes(app);

app.UseAuthentication();
app.UseAuthorization();
app.Run();