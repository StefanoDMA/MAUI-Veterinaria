using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using Newtonsoft.Json;

namespace MauiVistas.Services
{
    public class DogApiService
    {
        private readonly HttpClient _httpClient;

        public DogApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método GetBreedsAsync
        public async Task<Res_ObtenerBreeds> GetBreedsAsync(Req_ObtenerBreeds request)
        {
            try
            {
                Debug.WriteLine("Starting GetBreedsAsync"); // Debugging
                var response = await _httpClient.GetAsync("https://dog.ceo/api/breeds/list/all");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("JSON Response for Breeds: " + json); // Debugging

                var data = JsonConvert.DeserializeObject<BreedResponse>(json);

                if (data == null)
                {
                    Debug.WriteLine("Deserialization returned null for Breeds data."); // Debugging
                }
                else
                {
                    Debug.WriteLine("Successfully deserialized Breeds data."); // Debugging
                }

                return new Res_ObtenerBreeds
                {
                    Breeds = data?.Message ?? new Dictionary<string, List<string>>() // Handle null case
                };
            }
            catch (JsonSerializationException jsonEx)
            {
                Debug.WriteLine($"JSON Serialization Error in GetBreedsAsync: {jsonEx.Message}"); // Debugging
                throw;
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"HTTP Request Error in GetBreedsAsync: {httpEx.Message}"); // Debugging
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General Error in GetBreedsAsync: {ex.Message}"); // Debugging
                throw;
            }
        }

        // Método GetImagesAsync
        public async Task<Res_ObtenerImages> GetImagesAsync(Req_ObtenerImages request)
        {
            try
            {
                Debug.WriteLine($"Starting GetImagesAsync for breed: {request.Breed}"); // Debugging
                var response = await _httpClient.GetAsync($"https://dog.ceo/api/breed/{request.Breed}/images/random");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("JSON Response for Images: " + json); // Debugging

                var data = JsonConvert.DeserializeObject<ImageResponse>(json);

                if (data == null)
                {
                    Debug.WriteLine("Deserialization returned null for Images data."); // Debugging
                }
                else
                {
                    Debug.WriteLine("Successfully deserialized Images data."); // Debugging
                }
                // Dado que `data.Message` es una sola cadena, la agregamos a una lista
                return new Res_ObtenerImages
                {
                    Images = new List<string> { data.Message } // Agregar el único mensaje a la lista
                };
            }
            catch (JsonSerializationException jsonEx)
            {
                Debug.WriteLine($"JSON Serialization Error in GetImagesAsync: {jsonEx.Message}"); // Debugging
                throw;
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"HTTP Request Error in GetImagesAsync: {httpEx.Message}"); // Debugging
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General Error in GetImagesAsync: {ex.Message}"); // Debugging
                throw;
            }
        }

        // Método MostrarImages, que llama internamente a GetImagesAsync
        public async Task<Res_ObtenerImages> MostrarImages(Req_ObtenerImages req)
        {
            try
            {
                Debug.WriteLine($"Calling GetImagesAsync for breed: {req.Breed}"); // Debugging
                var response = await GetImagesAsync(req);
                Debug.WriteLine("Successfully retrieved images."); // Debugging
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener las imágenes: {ex.Message}"); // Debugging
                return new Res_ObtenerImages { Images = new List<string>() };
            }
        }

        // Clase interna BreedResponse para manejar la respuesta de razas
        public class BreedResponse
        {
            [JsonProperty("message")]
            public Dictionary<string, List<string>> Message { get; set; }
        }

        // Clase interna ImageResponse para manejar la respuesta de imágenes
        public class ImageResponse
        {
            [JsonProperty("message")]
            public string Message { get; set; }  // Cambiado de string[] a string
        }
    }
}
