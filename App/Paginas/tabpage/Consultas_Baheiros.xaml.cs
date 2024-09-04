using FrontEndHealthPets.Entidades;
using FrontEndHealthPets.Entidades.Entitys;
using FrontEndHealthPets.Entidades.response;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using ClosedXML.Excel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;

namespace FrontEndHealthPets.Paginas.tabpage
{
    public partial class Consultas_Baheiros : ContentPage
    {
        private readonly HttpClient _httpClient;
        private string LaURL = "https://localhost:44348/api";  // Actualiza la URL base de tu API

        private ObservableCollection<Registro_Mascota> Mascotas { get; set; }
        private ObservableCollection<BaheiroMascotas> BaheirosMascotas { get; set; }

        public Consultas_Baheiros()
        {
            InitializeComponent();
            _httpClient = new HttpClient(); // Inicializa HttpClient
            Mascotas = new ObservableCollection<Registro_Mascota>();
            BaheirosMascotas = new ObservableCollection<BaheiroMascotas>();
            BindingContext = this;

            // Cargar las mascotas al iniciar la página
            CargarMascotas();
        }

        // Carga de mascotas
        private async void CargarMascotas()
        {
            int id_usuario = (int)Sesion.id_usuario;
            Debug.WriteLine($"ID del usuario: {id_usuario}");

            var mascotas = await ObtenerMascotasAsync(id_usuario);
            Mascotas.Clear();
            foreach (var mascota in mascotas)
            {
                Mascotas.Add(mascota);
            }

            // Actualiza el ItemsSource del Picker explícitamente
            MascotaPicker.ItemsSource = Mascotas;
        }

        // Obtener lista de mascotas del usuario conectado
        private async Task<List<Registro_Mascota>> ObtenerMascotasAsync(int id_usuario)
        {
            try
            {
                var requestUrl = $"{LaURL}/Lista_Mascotas/Obtener_Lista_Mascotas?id_usuario={id_usuario}";
                Debug.WriteLine($"Request URL: {requestUrl}");

                var response = await _httpClient.GetAsync(requestUrl);
                Debug.WriteLine($"Response Status Code: {response.StatusCode}");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Response JSON: {jsonString}");

                var result = JsonConvert.DeserializeObject<Res_Lista_mascotas>(jsonString);

                if (result.resultado)
                {
                    Debug.WriteLine("Mascotas obtenidas correctamente.");
                    return result.ListaMascotas;
                }
                else
                {
                    Debug.WriteLine($"Error en la API: {result.error}");
                    await DisplayAlert("Error", $"Error en la API: {result.error}", "OK");
                    return new List<Registro_Mascota>();
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HttpRequestException: {ex.Message}");
                await DisplayAlert("Error", $"No se pudo obtener la lista de mascotas: {ex.Message}", "OK");
                return new List<Registro_Mascota>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
                return new List<Registro_Mascota>();
            }
        }

        // Maneja la selección de mascotas en el Picker
        private async void MascotaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MascotaPicker.SelectedIndex != -1)
            {
                var mascotaSeleccionada = (Registro_Mascota)MascotaPicker.SelectedItem;
                Debug.WriteLine($"Mascota seleccionada: {mascotaSeleccionada?.Nombre}");

                if (mascotaSeleccionada != null)
                {
                    // Cargar los baños/consultas de la mascota seleccionada para mostrarlos en pantalla
                    await CargarBaheirosMascota(mascotaSeleccionada.Id_Mascota);

                    // Actualiza el ItemsSource del ListView para mostrar los datos de baños
                    BaheirosListView.ItemsSource = BaheirosMascotas;
                }
            }
        }

        // Carga de baños/consultas de la mascota
        private async Task CargarBaheirosMascota(int id_Mascota)
        {
            try
            {
                var requestUrl = $"{LaURL}/Lista_Baherios_Mascotas/Obtener_Lista_Baherios_Mascotas?id_mascota={id_Mascota}";
                Debug.WriteLine($"Request URL: {requestUrl}");

                var response = await _httpClient.GetAsync(requestUrl);
                Debug.WriteLine($"Response Status Code: {response.StatusCode}");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Response JSON: {jsonString}");

                var result = JsonConvert.DeserializeObject<Res_ListaBaheiros_Mascotas>(jsonString);

                if (result.resultado)
                {
                    Debug.WriteLine("API result successful, clearing and adding items to BaheirosMascotas...");
                    BaheirosMascotas.Clear(); // Limpiar la colección antes de agregar nuevos elementos

                    foreach (var Baheiro in result.ListarBaheirosMascotas)
                    {
                        Debug.WriteLine($"Adding Baheiro: {JsonConvert.SerializeObject(Baheiro)}");
                        BaheirosMascotas.Add(Baheiro);
                    }

                    Debug.WriteLine("Completed adding Baheiros. Total count: " + BaheirosMascotas.Count);
                }
                else
                {
                    Debug.WriteLine($"API result failed with error: {result.error ?? "Desconocido"}");
                    await DisplayAlert("Error", $"Error en la API: {result.error ?? "Desconocido"}", "OK");
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HttpRequestException: {ex.Message}");
                await DisplayAlert("Error", $"No se pudo obtener la lista de baheiros: {ex.Message}", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
        // Guardar la lista de baños/consultas en un archivo Excel
        private async void BtGuardarLista_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Ruta del archivo (puedes cambiarla a una ruta específica si es necesario)
                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Baheiros.xlsx");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Baheiros");

                    // Agregar encabezados
                    worksheet.Cell(1, 1).Value = "Tipo De Baño";
                    worksheet.Cell(1, 2).Value = "Descripcion";
                    worksheet.Cell(1, 3).Value = "fecha ultimo baño";
                    worksheet.Cell(1, 4).Value = "Fecha proximo baño";
                    worksheet.Cell(1, 5).Value = "Notas";
                   

                    // Agregar datos
                    var row = 2;
                    foreach (var baheiro in BaheirosMascotas)
                    {
                        worksheet.Cell(row, 1).Value = baheiro.Nombre_Baheiro;
                        worksheet.Cell(row, 2).Value = baheiro.Descripcion_Baheiro;
                        worksheet.Cell(row, 3).Value = baheiro.Fecha_y_hora_Baheiro;
                        worksheet.Cell(row, 4).Value = baheiro.Fecha_y_hora_Baheiro;
                        worksheet.Cell(row, 5).Value = baheiro.Notas;
                        
                        row++;
                    }

                    // Guardar el archivo
                    workbook.SaveAs(filePath);
                }

                await DisplayAlert("Éxito", "Archivo Excel guardado correctamente.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo guardar el archivo: {ex.Message}", "OK");
            }
        }
    }
}