using FrontEndHealthPets.Entidades.Request;

using FrontEndHealthPets.Modelo;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace FrontEndHealthPets.Paginas.FlyPaginas
{
    public partial class RazasAnimales : ContentPage
    {
        private readonly DogApiService _dogApiService;
        private Dictionary<string, List<string>> _breeds;

        public RazasAnimales()
        {
            InitializeComponent();
            _dogApiService = new DogApiService(new HttpClient()); // Proporciona un HttpClient aquí
            LoadBreedsAsync();
        }

        private async Task LoadBreedsAsync()
        {
            try
            {
                // Obtener la lista de razas desde el servicio
                var response = await _dogApiService.GetBreedsAsync(); // Sin parámetros
                _breeds = response.Breeds;

                // Convertir las claves del diccionario (nombres de razas principales) en una lista
                var breedList = _breeds.Keys.ToList();

                // Verificar si la lista tiene elementos
                if (breedList.Count > 0)
                {
                    // Asignar la lista al Picker
                    dogSelector.ItemsSource = breedList;
                }
                else
                {
                    // Mostrar mensaje si la lista está vacía
                    messageLabel.Text = "No se encontraron razas de perros.";
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra durante la solicitud
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error", $"No se pudo obtener las razas de perros: {ex.Message}", "OK");
            }
        }

        private async void BtBuscar_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar el botón mientras se hace la solicitud
                buscarFotoButton.IsEnabled = false;

                // Mostrar el indicador de carga
                loadingIndicator.IsVisible = true;

                // Obtener la raza seleccionada del Picker
                var selectedBreed = dogSelector.SelectedItem as string;

                if (!string.IsNullOrEmpty(selectedBreed))
                {
                    // Obtener la imagen correspondiente a la raza seleccionada
                    var response = await _dogApiService.GetImagesAsync(selectedBreed);

                    // Limpiar las imágenes anteriores
                    apiStackLayout.Children.Clear();

                    if (!string.IsNullOrEmpty(response?.Image))
                    {
                        var image = new Image
                        {
                            Source = response.Image,
                            WidthRequest = 300,
                            HeightRequest = 300,
                            Margin = new Thickness(5)
                        };
                        apiStackLayout.Children.Add(image);
                    }
                    else
                    {
                        // Mostrar mensaje si no hay imágenes
                        messageLabel.Text = "No se encontraron imágenes para la raza seleccionada.";
                    }
                }
                else
                {
                    // Mostrar mensaje si no se ha seleccionado ninguna raza
                    messageLabel.Text = "Por favor, seleccione una raza.";
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra al intentar obtener imágenes de la raza seleccionada
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error", "No se pudo procesar la raza seleccionada", "OK");
            }
            finally
            {
                // Rehabilitar el botón y ocultar el indicador de carga
                buscarFotoButton.IsEnabled = true;
                loadingIndicator.IsVisible = false;
            }
        }
    }
}