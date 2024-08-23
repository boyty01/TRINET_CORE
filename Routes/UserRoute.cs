using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TRINET_CORE.Database;

namespace TRINET_CORE.Routes
{
    public static class UserRoute
    {

        private static ConfigurationManager _configuration = new();



        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var generator = RandomNumberGenerator.Create();

            generator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }


        private static ClaimsPrincipal? GetPrinicpalFromExpiredToken(string? token)
        {
            var validation = AuthConfig.GetRefreshTokenValidationParameters();

            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }

        public static WebApplication MountUserRoutes(WebApplication app, WebApplicationBuilder builder)
        {


            /**
             * Login to an existing account. Generates a JWT Token
             */
            app.MapPost("/user/login", async (TrinetDatabase db, LoginUser user) =>
            {
                try
                {
                    var All = await db.Users.ToListAsync();
                    var record = await db.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
                    if (record == null) return Results.Unauthorized();


                    PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                    PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(record, record.Password, user.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        var refreshToken = GenerateRefreshToken();
                        record.RefreshToken = refreshToken;
                        record.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(AuthConfig.TokenExpirationMinutes);

                        db.Users.Update(record);
                        await db.SaveChangesAsync();

                        var token = AuthConfig.CreateJwtToken(record);
                        return Results.Ok(new LoginResponse
                        {
                            JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                            Expiration = token.ValidTo,
                            RefreshToken = refreshToken
                        });
                    }

                    return Results.Unauthorized();
                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }
            });


            app.MapPost("/user/refresh_token", async (TrinetDatabase db, RefreshAuth refreshAuth) =>
            {
                var principal = GetPrinicpalFromExpiredToken(refreshAuth.AccessToken);

                if (principal?.Identity?.Name is null)
                {
                    Console.WriteLine("No Identity Found");
                    return Results.Unauthorized();
                }

                var principalName = principal.Identity.Name;
                var user = await db.Users.FirstOrDefaultAsync(u => u.Username == principalName);

                if(user is null) Console.WriteLine("No User record");

                if (user is not null && user.RefreshToken != refreshAuth.RefreshToken) Console.WriteLine("Refresh token is invalid");

                if (user is not null && user.RefreshTokenExpiry > DateTime.UtcNow) Console.WriteLine("Token hasn't expired");

                if (user is null || user.RefreshToken != refreshAuth.RefreshToken || user.RefreshTokenExpiry > DateTime.UtcNow)
                {
                    return Results.Unauthorized();
                }

                var token = AuthConfig.CreateJwtToken(user);

                return Results.Ok(new LoginResponse
                {
                    JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    RefreshToken = refreshAuth.RefreshToken
                });

            });


            /**
             * Register a new user. Admin Only
             */
            app.MapPost("/user/register", async (TrinetDatabase db, LoginUser user) =>
            {
                User NewUser = new();
                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                string Hash = passwordHasher.HashPassword(NewUser, user.Password);
                NewUser.Id = Guid.NewGuid();
                NewUser.Username = user.Username;
                NewUser.Password = Hash;
                try
                {


                    await db.Users.AddAsync(NewUser);
                    await db.SaveChangesAsync();
                    return Results.Created();
                }
                catch (Exception)
                {
                    //@TODO.  Super generic & lazy catch all.  Expand it. 
                    return Results.Problem();
                }
            }).RequireAuthorization(policy => policy.RequireRole(EUserAccessLevel.ADMIN.ToString()));


            /**
             * Update user details. Admin only
             */
            app.MapPut("/user/{username}", async (TrinetDatabase db, User user) =>
            {
                try
                {
                    var record = await db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
                    if (record == null) return Results.NotFound();

                    record.Username = user.Username;
                    record.Password = user.Password;
                    record.UserAccessLevel = user.UserAccessLevel;
                    db.Users.Update(record);
                    await db.SaveChangesAsync();
                    return Results.Ok();
                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }

            }).RequireAuthorization(new AuthorizeAttribute() { Roles = EUserAccessLevel.ADMIN.ToString() });

            return app;
        }
    }
}
