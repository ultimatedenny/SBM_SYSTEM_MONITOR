using System.IdentityModel.Tokens.Jwt;
using System.Text;
//using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
//using SBM_SYSTEM_MONITOR.App_Start;
using Microsoft.Extensions.DependencyInjection;

namespace SBM_SYSTEM_MONITOR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var issuer = "SBMIT";
            var audience = "SBMIT";
            var secret = "0nQJNTz7IPk/NZ3BuYnNHehDvobbNB2GrDzwIxx4QkM=";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key
            };

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                TokenValidationParameters = tokenValidationParameters
            });
        }
    }
}