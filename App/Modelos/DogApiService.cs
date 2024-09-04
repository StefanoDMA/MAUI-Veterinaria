using FrontEndHealthPets.Entidades.response;
using FrontEndHealthPets.Entidades.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Modelo
{
    class DogApiService
    {
        private readonly HttpClient _httpClient;

        public DogApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Res_ObtenerBreeds> GetBreedsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://dog.ceo/api/breeds/list/all");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("JSON Response: " + json); // Debugging

                // Deserializar en la clase intermedia
                var breedResponse = JsonConvert.DeserializeObject<BreedResponse>(json);

                // Mapear a Res_ObtenerBreeds
                return new Res_ObtenerBreeds
                {
                    Breeds = breedResponse?.Message ?? new Dictionary<string, List<string>>(),
                    Status = breedResponse?.Status
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<Res_ObtenerImages> GetImagesAsync(string breed)
        {
            try
            {
                Console.WriteLine($"Starting GetImagesAsync for breed: {breed}"); // Debugging: Inicia la solicitud

                var response = await _httpClient.GetAsync($"https://dog.ceo/api/breed/{breed}/images/random");
                Console.WriteLine($"HTTP Response Status Code: {response.StatusCode}"); // Debugging: Código de estado HTTP

                response.EnsureSuccessStatusCode();
                Console.WriteLine("Response ensured success."); // Debugging: Respuesta asegurada como exitosa

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("JSON Response: " + json); // Debugging: Respuesta JSON

                var imageResponse = JsonConvert.DeserializeObject<Res_ObtenerImages>(json);
                Console.WriteLine("Deserialization result: " + (imageResponse != null ? "Success" : "Failure")); // Debugging: Resultado de la deserialización

                if (imageResponse == null)
                {
                    Console.WriteLine("Res_ObtenerImages is null."); // Debugging: Si Res_ObtenerImages es null
                }
                else
                {
                    Console.WriteLine($"Image Response Image: {imageResponse.Image}"); // Debugging: Imágen obtenida
                }

                return imageResponse;
            }
            catch (JsonSerializationException jsonEx)
            {
                Console.WriteLine($"JSON Serialization Error: {jsonEx.Message}"); // Debugging: Error de serialización JSON
                throw;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Request Error: {httpEx.Message}"); // Debugging: Error en la solicitud HTTP
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}"); // Debugging: Error general
                throw;
            }
        }
    }
}