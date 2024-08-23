using TRINET_CORE.Database;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace TRINET_CORE.Routes
{
    public static class UserRoute
    {


        public static WebApplication MountUserRoutes(WebApplication app)
        {

            app.MapPost("/user/login", async (TrinetDatabase db, User user) => 
            { 
                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            });


            app.MapPost("/user/register", async (TrinetDatabase db, User user) =>
            {
                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                passwordHasher.HashPassword(user, user.Password);
            });

            return app;
        }       
    }
}
