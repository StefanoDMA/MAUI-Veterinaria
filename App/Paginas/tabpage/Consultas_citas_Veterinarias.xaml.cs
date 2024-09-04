using ClosedXML.Excel;
using FrontEndHealthPets.Entidades;
using FrontEndHealthPets.Entidades.Entitys;
using FrontEndHealthPets.Entidades.response;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FrontEndHealthPets.Paginas.tabpage;

public partial class Consultas_citas_Veterinarias : ContentPage
{
    private readonly HttpClient _httpClient;
    private string LaURL = "https://localhost:44348/api";

    private ObservableCollection<Registro_Mascota> Mascotas { get; set; }
    private ObservableCollection<Citas_Clinica_Veterinaria_Mascotas> CitaClinicasVeterinaria { get; set; }
    public Consultas_citas_Veterinarias()
    {
        InitializeComponent();
        _httpClient = new HttpClient(); // Inicializa HttpClient
        Mascotas = new ObservableCollection<Registro_Mascota>();
        CitaClinicasVeterinaria = new ObservableCollection<Citas_Clinica_Veterinaria_Mascotas>();
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

    private async void MascotaPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (MascotaPicker.SelectedIndex != -1)
        {
            var mascotaSeleccionada = (Registro_Mascota)MascotaPicker.SelectedItem;
            Debug.WriteLine($"Mascota seleccionada: {mascotaSeleccionada?.Nombre}");

            if (mascotaSeleccionada != null)
            {

                await CargarCitasVeteriaMascota(mascotaSeleccionada.Id_Mascota);

                // Actualiza el ItemsSource del ListView para mostrar los datos de Citasclinicaveterinaria
                CitasClinicamascotasListView.ItemsSource = CitaClinicasVeterinaria;
            }
        }
    }
    //
    // Carga de medicamentos de la mascota
    private async Task CargarCitasVeteriaMascota(int id_Mascota)
    {
        try
        {
            // Método para obtener el id_doctor
            var requestUrl = $"{LaURL}/Lista_Citas_Veterinarias/Obtener_Lista_Citas_Veterinarias?id_mascota={id_Mascota}";
            Debug.WriteLine($"Request URL: {requestUrl}");

            var response = await _httpClient.GetAsync(requestUrl);
            Debug.WriteLine($"Response Status Code: {response.StatusCode}");

            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response JSON: {jsonString}");
            Debug.WriteLine("jsonString: " + jsonString);
            var result = JsonConvert.DeserializeObject<Res_Lista_Citas_Veterinaria_mascotas>(jsonString);

            if (result.resultado)
            {
                CitaClinicasVeterinaria.Clear(); // Limpiar la colección antes de agregar nuevos elementos

                foreach (var CitasVeterinarias in result.Lista_Citas_Veterinaria_Mascotas)
                {

                    Debug.WriteLine("Detalles de la cita veterinaria:");
                    Debug.WriteLine($"Nombre_Clinica: {CitasVeterinarias.Nombre_Clinica}");
                    Debug.WriteLine($"Direccion: {CitasVeterinarias.Direccion}");
                    Debug.WriteLine($"Telefono: {CitasVeterinarias.Telefono}");
                    Debug.WriteLine($"Nombre_Doctor: {CitasVeterinarias.Nombre_Doctor}");
                    Debug.WriteLine($"Fecha_y_hora_Cita: {CitasVeterinarias.Fecha_y_hora_Cita}");
                    Debug.WriteLine($"Notas: {CitasVeterinarias.Notas}");
                    Debug.WriteLine($"Nombre_Clinica: {CitasVeterinarias.Nombre_Clinica}, " +
               $"Nombre_Clinica:  {CitasVeterinarias.Direccion} " +
               $"Telefono: {CitasVeterinarias.Telefono}, " +
               $"Nombre_Doctor: {CitasVeterinarias.Nombre_Doctor}, " +
               $"Fecha_y_hora_Cita: {CitasVeterinarias.Fecha_y_hora_Cita}, " +
               $"Notas: {CitasVeterinarias.Notas}");
                    CitaClinicasVeterinaria.Add(CitasVeterinarias);
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
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CitasVeterinarias.xlsx");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Citas Veterinarias");

                // Agregar encabezados
                worksheet.Cell(1, 1).Value = "Nombre Clínica";
                worksheet.Cell(1, 2).Value = "Dirección";
                worksheet.Cell(1, 3).Value = "Teléfono";
                worksheet.Cell(1, 4).Value = "Nombre Doctor";
                worksheet.Cell(1, 5).Value = "Fecha y Hora Cita";
                worksheet.Cell(1, 6).Value = "Notas";

                // Agregar datos
                var row = 2;
                foreach (var cita in CitaClinicasVeterinaria)
                {
                    worksheet.Cell(row, 1).Value = cita.Nombre_Clinica;
                    worksheet.Cell(row, 2).Value = cita.Direccion;
                    worksheet.Cell(row, 3).Value = cita.Telefono;
                    worksheet.Cell(row, 4).Value = cita.Nombre_Doctor;

                    // Formatear fecha y hora
                    worksheet.Cell(row, 5).Value = cita.Fecha_y_hora_Cita;
                    worksheet.Cell(row, 5).Style.NumberFormat.Format = "dd/MM/yyyy HH:mm";

                    worksheet.Cell(row, 6).Value = cita.Notas ?? "No hay notas disponibles";
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