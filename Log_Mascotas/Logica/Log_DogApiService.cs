using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using MauiVistas.Services;

namespace Log_Mascotas.Logica
{
    public class Log_DogApiService
    {
        private readonly DogApiService _dogApiService;

        public Log_DogApiService(DogApiService dogApiService)
        {
            _dogApiService = dogApiService;
        }

        public async Task<Res_ObtenerBreeds> MostrarBreeds(Req_ObtenerBreeds req)
        {
            try
            {
                Debug.WriteLine("Calling GetBreedsAsync"); // Debugging
                var response = await _dogApiService.GetBreedsAsync(req);
                Debug.WriteLine("Successfully retrieved breeds."); // Debugging
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener las razas: {ex.Message}"); // Debugging
                return new Res_ObtenerBreeds { Breeds = new Dictionary<string, List<string>>() };
            }
        }

        public async Task<Res_ObtenerImages> MostrarImages(Req_ObtenerImages req)
        {
            try
            {
                Debug.WriteLine($"Calling GetImagesAsync for breed: {req.Breed}"); // Debugging
                var response = await _dogApiService.GetImagesAsync(req);
                Debug.WriteLine("Successfully retrieved images."); // Debugging
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener las imágenes: {ex.Message}"); // Debugging
                return new Res_ObtenerImages { Images = new List<string>() };
            }
        }
    }
}
