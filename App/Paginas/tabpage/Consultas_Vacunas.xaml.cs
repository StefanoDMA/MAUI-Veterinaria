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

namespace FrontEndHealthPets.Paginas.tabpage
{
    public partial class Consultas_Vacunas : ContentPage
    {
        private readonly HttpClient _httpClient;
        private string LaURL = "https://localhost:44348/api";

        private ObservableCollection<Registro_Mascota> Mascotas { get; set; }
        private ObservableCollection<Vacunas_Mascotas> VacunasMascotas { get; set; }

        public Consultas_Vacunas()
        {
            InitializeComponent();
            _httpClient = new HttpClient(); // Inicializa HttpClient
            Mascotas = new ObservableCollection<Registro_Mascota>();
            VacunasMascotas = new ObservableCollection<Vacunas_Mascotas>();
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
                    // Cargar los medicamentos de la mascota seleccionada para mostrarlos en pantalla
                    await CargarVacunasMascota(mascotaSeleccionada.Id_Mascota);

                    // Actualiza el ItemsSource del ListView para mostrar los datos de medicamentos
                    VacunasmascotasListView.ItemsSource = VacunasMascotas;
                }
            }
        }

        private async Task CargarVacunasMascota(int id_mascota)
        {
            try
            {
                var requestUrl = $"{LaURL}/Lista_Vacunas_Mascotas/Obtener_Lista_Vacunas_Mascotas?id_mascota={id_mascota}";
                Debug.WriteLine($"Request URL: {requestUrl}");

                var response = await _httpClient.GetAsync(requestUrl);
                Debug.WriteLine($"Response Status Code: {response.StatusCode}");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Response JSON: {jsonString}");

                var result = JsonConvert.DeserializeObject<Res_Lista_Vacunas_Mascotas>(jsonString);

                if (result.resultado)
                {
                    Debug.WriteLine("Vacunas obtenidas correctamente.");
                    VacunasMascotas.Clear();
                    foreach (var vacuna in result.ListarVacunasMascotas)
                    {
                        VacunasMascotas.Add(vacuna);
                    }
                }
                else
                {
                    Debug.WriteLine($"Error en la API: {result.error}");
                    await DisplayAlert("Error", $"Error en la API: {result.error}", "OK");
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HttpRequestException: {ex.Message}");
                await DisplayAlert("Error", $"No se pudo obtener la lista de vacunas: {ex.Message}", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }

        // Maneja el clic en el botón de guardar
        private async void BtGuardarLista_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Ruta del archivo (puedes cambiarla a una ruta específica si es necesario)
                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Vacunas.xlsx");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Vacunas");

                    // Agregar encabezados
                    worksheet.Cell(1, 1).Value = "Vacuna";
                    worksheet.Cell(1, 2).Value = "Descripcion";
                    worksheet.Cell(1, 3).Value = "Dosis";
                    worksheet.Cell(1, 4).Value = "Fecha y hora Aplicacion ";
                    worksheet.Cell(1, 5).Value = "Fecha y hora proxima Aplicacion";
                    worksheet.Cell(1, 6).Value = "Notas";

                    // Agregar datos
                    var row = 2;
                    foreach (var vacuna in VacunasMascotas)
                    {
                        worksheet.Cell(row, 1).Value = vacuna.Nombre_Vacuna;
                        worksheet.Cell(row, 2).Value = vacuna.Descripcion;
                        worksheet.Cell(row, 3).Value = vacuna.Dosis;
                        worksheet.Cell(row, 4).Value = vacuna.Fecha_y_Hora_Aplicacion;
                        worksheet.Cell(row, 4).Style.NumberFormat.Format = "dd/MM/yyyy HH:mm"; // Establecer formato para fecha y hora

                        worksheet.Cell(row, 5).Value = vacuna.Fecha_y_Hora_Proxima_Aplicacion;
                        worksheet.Cell(row, 5).Style.NumberFormat.Format = "dd/MM/yyyy HH:mm"; // Establecer formato para fecha y hora
                        worksheet.Cell(row, 6).Value = vacuna.notas ?? "No hay notas disponibles";
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
