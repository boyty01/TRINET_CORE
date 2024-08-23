using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using TRINET_CORE.Database;

namespace TRINET_CORE.Routes
{
    public static class UserRoute
    {


        public static WebApplication MountUserRoutes(WebApplication app, WebApplicationBuilder builder)
        {


            app.MapPost("/user/login", async (TrinetDatabase db, User user) =>
            {
                try
                {
                    var record = await db.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
                    Console.WriteLine(record);
                    if (record == null) return Results.Unauthorized();

                    PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                    PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(record, record.Password, user.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        var issuer = builder.Configuration["Jwt:Issuer"];
                        var audience = builder.Configuration["Jwt:Audience"];
                        var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? "0x00");
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new[]
                            {
                            new Claim("Id", Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, record.Username),
                            new Claim(JwtRegisteredClaimNames.Email, record.Username),
                            new Claim(JwtRegisteredClaimNames.Jti,
                            Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Role, record.UserAccessLevel.ToString())
                        }),
                            Expires = DateTime.UtcNow.AddMinutes(5),
                            Issuer = issuer,
                            Audience = audience,
                            SigningCredentials = new SigningCredentials
                            (new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha512Signature),

                        };
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var jwtToken = tokenHandler.WriteToken(token);
                        var stringToken = tokenHandler.WriteToken(token);
                        return Results.Ok("{\"token\":\"" + stringToken + "\"}");

                    }

                    return Results.Unauthorized();
                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }
            });



            app.MapPost("/user/register", async (TrinetDatabase db, User user) =>
            {
                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                string Hash = passwordHasher.HashPassword(user, user.Password);
                user.Password = Hash;
                try
                {
                    user.Id = Guid.NewGuid();
                    await db.Users.AddAsync(user);
                    await db.SaveChangesAsync();
                    User SafeUser = new() { Id = user.Id, Username = user.Username, Password = "", UserAccessLevel = user.UserAccessLevel };
                    return Results.Created($"/user/{SafeUser.Id}",SafeUser);
                }
                catch (Exception)
                {
                    //@TODO.  Super generic & lazy catch all.  Expand it. 
                    return Results.Problem();
                }
            }).RequireAuthorization(policy => policy.RequireRole(EUserAccessLevel.ADMIN.ToString()));



            app.MapPut("/user/{username}", async (TrinetDatabase db, User user) =>
            {

            }).RequireAuthorization(new AuthorizeAttribute() { Roles = EUserAccessLevel.ADMIN.ToString() });

            return app;
        }
    }
}
