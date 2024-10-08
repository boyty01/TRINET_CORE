﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TRINET_CORE.Database;
using TRINET_CORE.Modules.Wiz;

namespace TRINET_CORE.Routes
{
    public static class UserRoute
    {

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

        private enum ETokenRefreshFailureReason
        {
            Unauthorised,
            Unexpired
        }
        private class TokenRefreshFailureReason()
        {
            public ETokenRefreshFailureReason Reason;
        }
        public static WebApplication MountUserRoutes(WebApplication app, WebApplicationBuilder builder)
        {

            app.MapGet("/user/validate", async () =>
            {
                return Results.Ok();
            }).RequireAuthorization();


            /**
             * Login to an existing account. Generates a JWT Token
             */
            app.MapPost("/user/login", async (TrinetDatabase db, WizModule wiz, LoginUser user) =>
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
                            RefreshToken = refreshToken,
                            LocationId = record.LocationId
                        });
                    }

                    Console.WriteLine("Failed auth");
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

                if (user is null || user.RefreshToken != refreshAuth.RefreshToken) 
                {
                    return Results.Unauthorized();
                }

                // if refresh isn't due but we're authenticated then just return OK status to prevent client from assuming it needs to reauthenticate.
                if (user.RefreshTokenExpiry > DateTime.UtcNow)
                {
                    return Results.Accepted();
                }

                var token = AuthConfig.CreateJwtToken(user);

                Console.WriteLine("Token Refreshed.");
                return Results.Ok(new LoginResponse
                {
                    JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    RefreshToken = refreshAuth.RefreshToken,
                    LocationId = user.LocationId
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

            }).RequireAuthorization(policy => policy.RequireRole(EUserAccessLevel.ADMIN.ToString()));

            return app;
        }
    }
}
