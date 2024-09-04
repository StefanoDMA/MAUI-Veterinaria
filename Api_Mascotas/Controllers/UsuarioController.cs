using Log_Mascotas.Entidades;
using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using Log_Mascotas.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;

namespace Api_Mascotas.Controllers
{
    public class UsuarioController : ApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Registro_Usuario/IngresarUsuario")]

        public Res_Usuario IngresarUsuario(Req_Usuario req)
        {
            return new LogIngresarUsuario().IngresarUsuario(req);
        }


        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/Nueva_Password/RecuperarPassword")]

        public Res_RecuperarPassword RecuperarPassword(Req_RecuperarPassword req)
        {
            return new Log_RecuperarPassword().RecuperarPassword(req);
        }
        //aacacacaca
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("api/Usuarios/EliminarUsuario/{id}")]
      
        [Authorize]


        public IHttpActionResult EliminarUsuario(int idUsuario, [FromUri] string correoElectronico)
        {
            if (string.IsNullOrEmpty(correoElectronico))
            {
                return BadRequest("El correo electrónico es obligatorio.");
            }

            var logEliminarUsuario = new Log_EliminarUsuario();
            var resultado = logEliminarUsuario.EliminarUsuario(idUsuario, correoElectronico);

            if (resultado.resultado)
            {
                return Ok(resultado.error);
            }
            else
            {
                return BadRequest(resultado.error);
            }
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/Usuarios/ActualizarUsuario/{id}")]
        [Authorize]
        public Res_Actualizar_Usuario ActualizarUsuario(int Id_Usuario, Req_Actualizar_Usuario req)
        {
            req.Id_Usuario = Id_Usuario;
            return new Log_Actualizar_Usuario().Actualizar_Usuario(req);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/help/about")]

        public string about()
        {
            return "Api de mascotas";
        }
    }

}