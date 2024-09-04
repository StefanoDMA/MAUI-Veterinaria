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
    public partial class Consultas_Medicamentos : ContentPage
    {
        private readonly HttpClient _httpClient;
        private string LaURL = "https://localhost:44348/api";

        private ObservableCollection<Registro_Mascota> Mascotas { get; set; }
        private ObservableCollection<MedicamentosMascotas> MedicamentosMascotas { get; set; }

        public Consultas_Medicamentos()
        {
            InitializeComponent();
            _httpClient = new HttpClient(); // Inicializa HttpClient
            Mascotas = new ObservableCollection<Registro_Mascota>();
            MedicamentosMascotas = new ObservableCollection<MedicamentosMascotas>();
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
                    await CargarMedicamentosMascota(mascotaSeleccionada.Id_Mascota);

                    // Actualiza el ItemsSource del ListView para mostrar los datos de medicamentos
                    MedicamentosListView.ItemsSource = MedicamentosMascotas;
                }
            }
        }

        // Carga de medicamentos de la mascota
        private async Task CargarMedicamentosMascota(int id_Mascota)
        {
            try
            {
                var requestUrl = $"{LaURL}/Lista_Medicamentos_Mascotas/Obtener_Lista_Medicamentos_Mascotas?id_mascota={id_Mascota}";
                Debug.WriteLine($"Request URL: {requestUrl}");

                var response = await _httpClient.GetAsync(requestUrl);
                Debug.WriteLine($"Response Status Code: {response.StatusCode}");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Response JSON: {jsonString}");

                var result = JsonConvert.DeserializeObject<Res_Lista_Medicamentos_Mascotas>(jsonString);

                if (result.resultado)
                {
                    MedicamentosMascotas.Clear(); // Limpiar la colección antes de agregar nuevos elementos

                    foreach (var medicamento in result.ListaMedicamentosMascotas)
                    {
                        Debug.WriteLine($"Medicamento: {medicamento.Nombre_Medicamento}, " +
                   $"Categoría: {medicamento.categoria}, " +
                   $"Modo de Administración: {medicamento.Modo_De_Administracion}, " +
                   $"Hora de Ingesta: {medicamento.Hora_De_Ingesta}, " +
                   $"Fecha de Inicio: {medicamento.Fecha_Inicio}, " +
                   $"Fecha de Fin: {medicamento.Fecha_Fin}, " +
                   $"Notas: {medicamento.Notas}");
                        MedicamentosMascotas.Add(medicamento);
                    }
                }
                else
                {
                    await DisplayAlert("Error", $"Error en la API: {result.error ?? "Desconocido"}", "OK");
                }
                // Forzar la actualización del ListView (opcional)

            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", $"No se pudo obtener la lista de medicamentos: {ex.Message}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }

        private async void BtGuardarLista_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Ruta del archivo (puedes cambiarla a una ruta específica si es necesario)
                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Medicamentos.xlsx");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Medicamentos");

                    // Agregar encabezados
                    worksheet.Cell(1, 1).Value = "Nombre Medicamento";
                    worksheet.Cell(1, 2).Value = "Categoría";
                    worksheet.Cell(1, 3).Value = "Modo de Administración";
                    worksheet.Cell(1, 4).Value = "Fecha Inicio";
                    worksheet.Cell(1, 5).Value = "Fecha Fin";
                    worksheet.Cell(1, 6).Value = "Notas";

                    // Agregar datos
                    var row = 2;
                    foreach (var medicamento in MedicamentosMascotas)
                    {
                        worksheet.Cell(row, 1).Value = medicamento.Nombre_Medicamento;
                        worksheet.Cell(row, 2).Value = medicamento.categoria;
                        worksheet.Cell(row, 3).Value = medicamento.Modo_De_Administracion;
                        // Formatear fechas como DateOnly
                        worksheet.Cell(row, 4).Value = medicamento.Fecha_Inicio;
                        worksheet.Cell(row, 4).Style.NumberFormat.Format = "dd/MM/yyyy"; // Establecer formato

                        worksheet.Cell(row, 5).Value = medicamento.Fecha_Fin;
                        worksheet.Cell(row, 5).Style.NumberFormat.Format = "dd/MM/yyyy"; // Establecer formato
                        worksheet.Cell(row, 6).Value = medicamento.Notas ?? "No hay notas disponibles";
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

      