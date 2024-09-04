
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using Log_Mascotas.Logica;
using MauiVistas.Services;

namespace Api_Mascotas.Controllers
{

    public class PerrosController : ApiController
    {
        private readonly DogApiService _dogApiService;


        public PerrosController()
        {
            _dogApiService = new DogApiService(new HttpClient());
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Perros/GetBreeds")]

        public async Task<Res_ObtenerBreeds> ListaRazas()
        {
            // Crear una instancia vacía de Req_ObtenerBreeds ya que no se necesitan parámetros
            var req = new Req_ObtenerBreeds();
            return await new Log_DogApiService(_dogApiService).MostrarBreeds(req);
        }
        
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Perros/GetImages/{breed}")]

        public async Task<Res_ObtenerImages> ListaImagenes(string breed)
        {
            try
            {
                Debug.WriteLine($"Received request for breed: {breed}"); // Debugging
                var req = new Req_ObtenerImages { Breed = breed };
                Debug.WriteLine($"Calling MostrarImages with breed: {req.Breed}"); // Debugging

                var response = await _dogApiService.MostrarImages(req);

                Debug.WriteLine("Successfully retrieved images."); // Debugging
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ListaImagenes: {ex.Message}"); // Debugging
                // Puedes devolver un estado HTTP 500 o un error específico si lo prefieres
                throw; // Vuelve a lanzar la excepción para que el cliente reciba el error
            }
        }
        
    }
}
