using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using TRINET_CORE.Database;
using TRINET_CORE.Modules.Wiz;
using TRINET_CORE.Routes;


var builder = WebApplication.CreateBuilder(args);


// Authentication
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
TRINET_CORE.AuthConfig.Init(builder);

builder.Services.AddAuthorization();
// Make sure the pw hash iteration is higher than the weirdly low default.
builder.Services.Configure<PasswordHasherOptions>(opt => opt.IterationCount = 600_000);


// data store
var connectionString = builder.Configuration.GetConnectionString("TrinetDatabase") ?? "Data Source=TrinetDatabase.db";
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<TrinetDatabase>(connectionString);
builder.Services.AddSingleton<WizModule>();

// swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TriNet Core API", Description = "Main TriNet API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header in the format `Bearer {key}` "
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string [] {}
        }
    });
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
UserRoute.MountUserRoutes(app, builder);


app.UseAuthentication();
app.UseAuthorization();
app.Run();