using Microsoft.IdentityModel.Tokens;
using Owin;
using System.Text;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;

[assembly: OwinStartup(typeof(Api_Mascotas.Startup))]

namespace Api_Mascotas
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.Use(async (context, next) =>
            {
                // Log the request
                System.Diagnostics.Debug.WriteLine($"Request: {context.Request.Uri}");

                await next.Invoke();

                // Log the response
                System.Diagnostics.Debug.WriteLine($"Response: {context.Response.StatusCode}");
            });

            var key = Encoding.ASCII.GetBytes("YourNewSecureKeyThatIsAtLeast32BytesLongAndSecure1234");

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://localhost:44348",
                    ValidAudience = "http://localhost:44348",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }
            });
        }
    }
}