using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TRINET_CORE.Database;

namespace TRINET_CORE
{
    public static class AuthConfig
    {

        private static ConfigurationManager _configuration = new();
        public static int TokenExpirationMinutes { get; } = 30;
        public static void Init(WebApplicationBuilder builder)
        {
            _configuration = builder.Configuration;

            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = GetBaseTokenValidationParameters();
            });
        }


        public static TokenValidationParameters GetBaseTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "0x00")),
                ClockSkew = new TimeSpan(0, 0, 5)
            };
        }


        public static TokenValidationParameters GetRefreshTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "0x00")),
                ValidateLifetime = false
            };
        }


        public static SecurityToken CreateJwtToken(User record)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "0x00");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, record.Username),
                    new Claim(JwtRegisteredClaimNames.Email, record.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, record.UserAccessLevel.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(TokenExpirationMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature),

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);
        }


    }

}
