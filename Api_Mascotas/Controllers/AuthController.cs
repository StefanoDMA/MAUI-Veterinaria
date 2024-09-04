using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using Log_Mascotas.Logica;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Api_Mascotas.Controllers
{
    public class AuthController : ApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/login/IngresarLogin")]
        // Método que se encarga de autenticar a un usuario y devolver un token JWT si las credenciales son correctas
        public Res_login IngresarLogin(Req_login req)
        {
            System.Diagnostics.Debug.WriteLine($"Login Request: {req}");

            var resultadoLogin = new LogLogin().Ingresarlogin(req);

            if (!resultadoLogin.resultado || resultadoLogin.registro_Usuario == null)
            {
                resultadoLogin.error = "Unauthorized";
                System.Diagnostics.Debug.WriteLine($"Login Failed: {resultadoLogin.error}");
                return resultadoLogin;
            }

            var token = GenerarToken(resultadoLogin.registro_Usuario.Id_Usuario, resultadoLogin.registro_Usuario.Nombre, resultadoLogin.registro_Usuario.Apellidos);
            resultadoLogin.registro_Usuario.token = token;

            System.Diagnostics.Debug.WriteLine($"Login Successful: Token - {token}");

            return resultadoLogin;
        }
    


    private string GenerarToken(long id_usuario, string nombre, string apellidos)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("YourNewSecureKeyThatIsAtLeast32BytesLongAndSecure1234");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, nombre),
            new Claim(ClaimTypes.Surname, apellidos),
            new Claim(ClaimTypes.NameIdentifier, id_usuario.ToString())
        }),

                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "http://localhost:44348",
                Audience = "http://localhost:44348"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Log the token for debugging purposes
            System.Diagnostics.Debug.WriteLine($"Generated Token: {tokenString}");

            return tokenString;
        }
    }
}

   