using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                var record = await db.Users.FindAsync(user.Username);
                if (record == null) return Results.NotFound();

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
                            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                            new Claim(JwtRegisteredClaimNames.Email, user.Username),
                            new Claim(JwtRegisteredClaimNames.Jti,
                            Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Role, user.UserAccessLevel.ToString())
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
                    return Results.Ok(stringToken);
                }

                return Results.Unauthorized();
            });



            app.MapPost("/user/register", async (TrinetDatabase db, User user) =>
            {
                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                string Hash = passwordHasher.HashPassword(user, user.Password);
                user.Password = Hash;
                try
                {
                    await db.Users.AddAsync(user);
                    User SafeUser = new() { Username = user.Username, Password = "", UserAccessLevel = user.UserAccessLevel };
                    return Results.Created();
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
