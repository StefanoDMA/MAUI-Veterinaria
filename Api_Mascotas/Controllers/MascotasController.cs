using Log_Mascotas;
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
    public class MascotasController : ApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Registro_Mascota/IngresarMascota")]

        public Res_Mascota IngresarMascota(Req_Mascota req)
        {
            return new Log_RegistrarMascota().RegistrarMascota(req);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Lista_Mascotas/Obtener_Lista_Mascotas")]

        public Res_Lista_mascotas ObtenerListaMascotas(int id_usuario)
        {
            return new Log_RegistrarMascota().ListaMascotas(id_usuario);
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Foto_Mascota/Ingresar_Fotos_Mascotas")]


        public Res_FotosMascotas IngresarFoto(Req_FotosMascota req)
        {
            return new Log_FotosMascotas().IngresarFotos(req);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Lista_Fotos_Mascotas/Obtener_Lista_Fotos_Mascotas")]

        public Res_Lista_Fotos ObtenerListaFotos(int id_mascota)
        {
            return new Log_FotosMascotas().ListaFotos(id_mascota);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Ingresar_Baheiro/Ingresar_Baheiro")]

        public Res_Baheiro IngresarBaño(Req_Baheiro req)
        {
            return new Log_Baheiro().AgregarBaherio(req);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Lista_Baherios/Obtener_Lista_Baherios")]

        public Res_Lista_Baheiros ObtenerListaBaheiros()
        {
            return new Log_Baheiro().ListaBaheiros(null);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Ingresar_Baherios_Mascotas/Ingresar_Baherios_Mascotas")]

        public Res_Baheiro_Mascotas IngresarBahairoMascota(Req_Baheiro_Mascotas req)
        {
            return new Log_Baheiro_Mascotas().IngresarBaheriosMascotas(req);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Lista_Baherios_Mascotas/Obtener_Lista_Baherios_Mascotas")]

        public Res_ListaBaheiros_Mascotas ObtenerListaBaheiroMascota(int id_mascota) // Areglar metodo
        {
            return new Log_Baheiro_Mascotas().ListaBaheriosMascotas( id_mascota);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Ingresar_Vacunas/ObtenerIngresar_Vacunas")]

        public Res_Vacuna IngresarVacuna(Req_Vacunas req)
        {
            return new Log_Agregar_Vacuna().IngresarVacuna(req);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Lista_Vacunas/Obtener_Lista_Vacunas")]

        public Res_Lista_Vascunas ObtenerListaVacunas()
        {
            return new Log_Agregar_Vacuna().ListaVacunas(null);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Ingresar_Vacunas_Mascotas/Ingresar_Vacunas_Mascotas")]

        public Res_Vacunas_Mascotas IngresarVacunasMascotas(Req_Vacunas_Mascotas req)
        {
            return new Log_Ingresar_Vacunas_Mascotas().IngresarVAcunasMascotas(req);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Lista_Vacunas_Mascotas/Obtener_Lista_Vacunas_Mascotas")]

        public Res_Lista_Vacunas_Mascotas ObtenerListaVacunasMascotas(int id_mascota)
        {
            return new Log_Ingresar_Vacunas_Mascotas().ListaVacunasMascotas( id_mascota);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Ingresar_Medicamentos/Ingresar_Medicamentos")]

        public Res_Medicamentos IngresarMedicamentosMascotas(Req_Medicamentos req)
        {
            return new Log_Agregar_Medicamentos().AgregarMedicamentos(req);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Lista_Medicamentos/Obtener_Lista_Medicamentos")]

        public Res_LIstaMedicamentos ObtenerListaMedicamentos()
        {
            return new Log_Agregar_Medicamentos().ListaMedicamentos(null);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Ingresar_Medicamentos_Mascotas/Ingresar_Medicamentos_Mascotas")]

        public Res_MedicamentosMascotas IngresarMedicamentosMascotas(Req_MedicamentosMascotas req)
        {
            return new Log_MedicamentosMascotas().AgregarMedicamentosMascotas(req);
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Lista_Medicamentos_Mascotas/Obtener_Lista_Medicamentos_Mascotas")]

        public Res_Lista_Medicamentos_Mascotas ObtenerListaMedicamentosMascotas(int id_mascota)
        {
            return new Log_MedicamentosMascotas().ListaMedicamentosMascotas(id_mascota);
        }

 

    }
}