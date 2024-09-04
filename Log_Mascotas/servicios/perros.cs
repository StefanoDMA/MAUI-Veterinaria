using System;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MauiVistas.Services
{
    public class DogApiService
    {
        private readonly HttpClient _httpClient;

        public DogApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Breed[]> GetBreedsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://dog.ceo/api/breeds/list/all");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var breeds = JsonConvert.DeserializeObject<Breed[]>(json);
                return breeds;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener las razas de perros: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error al deserializar las razas de perros: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error desconocido: {ex.Message}");
                throw;
            }
        }

        public async Task<string[]> GetImagesAsync(string breed)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://dog.ceo/api/breed/{breed}/images");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var images = JsonConvert.DeserializeObject<string[]>(json);
                return images;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener las imágenes de la raza {breed}: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error al deserializar las imágenes de la raza {breed}: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error desconocido: {ex.Message}");
                throw;
            }
        }
        public class Breed
        {
            public string Name { get; set; }
        }
    }
}