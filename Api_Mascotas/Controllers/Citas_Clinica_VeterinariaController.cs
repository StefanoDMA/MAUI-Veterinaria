using Log_Mascotas.Entidades;
using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using Log_Mascotas.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Api_Mascotas.Controllers
{
    public class Citas_Clinica_VeterinariaController: ApiController
    {

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Ingresar_Datos_CLinica/Ingresar_Citas_Datos_CLinica")]


        public Res_Clinica_Veterinaria IngresarDatosCLinica(Req_Clinica_Veterinaria req)
        {
            var logAgregarDoctor = new Log_AgregarDoctor();
            return new Log_Clinica_Veterinaria(logAgregarDoctor).IngresarClinicaVeterinaria(req);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Lista_Clinica_Veterinaria/Obtener_Lista_Clinica_Veterinaria")]

        public Res_Lista_Clinica_Veterinaria ObtenerListaClinicaVeterinaria(
            )
        {
            var logAgregarDoctor = new Log_AgregarDoctor();
            return new Log_Clinica_Veterinaria(logAgregarDoctor).ListaClinicasVeterinarias(null);
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Ingresar_Citas_Veterinarias/Ingresar_Citas_Veterinarias")]

        public Res_Citas_Clinica_Veterinaria_Mascotas IngresarCitasVeterinarias(Req_Citas_Clinica_Veterinaria_Mascotas req)
        {
            return new Log_Agregar_Citas_Clinica_Veterinaria_Mascotas().IngresarCitasClinicaVeterinaria(req);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Ingresar_Doctor_Veterinario/Ingresar_Doctor_Veterinario")]

        public Res_AgregarDoctor ObtenerDoctor(Req_AgregarDoctor req)
        {
            return new Log_AgregarDoctor().IngresarDoctor(req);

        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Lista_Clinica_Veterinaria/Obtener_Lista_Clinica_Veterinaria")]

        public Res_Lista_de_Doctores_Clinica Listadedoctoresporclinica(int Id_Clinica_Veterinaria)
        {
            return new Log_Lista_de_Doctores_Por_Clinica().ObtenerDoctoresPorClinica(Id_Clinica_Veterinaria);
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Lista_Doctores/Obtener_Lista_Doctores")]

        public  Res_Lista_Doctor ObtenerListaDoctores(int id_Doctor )

        {
           
            return new Log_Lista_Doctores().ListaDoctores(id_Doctor);
        }



        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Lista_Citas_Veterinarias/Obtener_Lista_Citas_Veterinarias")]

        public Res_Lista_Citas_Veterinaria_mascotas ObtenerListaCitasVeterinarias(int id_mascota)
        {
            return new Log_Agregar_Citas_Clinica_Veterinaria_Mascotas().IngresarListaCitasVeterinarias( id_mascota );
        }


    }
}