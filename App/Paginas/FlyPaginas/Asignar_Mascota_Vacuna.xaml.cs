using FrontEndHealthPets.Entidades;
using FrontEndHealthPets.Entidades.Entitys;
using FrontEndHealthPets.Entidades.response;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Paginas.FlyPaginas
{
    public partial class Asignar_Mascota_Vacuna : ContentPage
    {
        string LaURL = "https://localhost:44348/api";
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "https://localhost:44348/api";

        private ObservableCollection<Entidades.Vacunas> Vacunas { get; set; }
        private ObservableCollection<Registro_Mascota> Mascotas { get; set; }

        public Asignar_Mascota_Vacuna()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            Vacunas = new ObservableCollection<Entidades.Vacunas>();
            Mascotas = new ObservableCollection<Registro_Mascota>();
            BindingContext = this;
            CargarVacunas();
            CargarMascotas();
        }

        private async void CargarVacunas()
        {
            var vacunas = await ObtenerVacunasAsync();
            Vacunas.Clear();
            foreach (var vacuna in vacunas)
            {
                Vacunas.Add(vacuna);
            }
            vacunaMascotaPicker.ItemsSource = Vacunas;
        }

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
            MascotaPicker.ItemsSource = Mascotas;
        }

        private async Task<List<Entidades.Vacunas>> ObtenerVacunasAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"{ApiBaseUrl}/Lista_Vacunas/Obtener_Lista_Vacunas");
                var result = JsonConvert.DeserializeObject<Res_Lista_Vascunas>(response);

                if (result.resultado)
                {
                    return result.Lista_Vacunas;
                }
                else
                {
                    await DisplayAlert("Error", $"Error en la API: {result.error}", "OK");
                    return new List<Entidades.Vacunas>();
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", $"No se pudo obtener la lista de vacunas: {ex.Message}", "OK");
                return new List<Entidades.Vacunas>();
            }
        }

        private async Task<List<Registro_Mascota>> ObtenerMascotasAsync(int id_usuario)
        {
            try
            {
                var requestUrl = $"{ApiBaseUrl}/Lista_Mascotas/Obtener_Lista_Mascotas?id_usuario={id_usuario}";
                var response = await _httpClient.GetStringAsync(requestUrl);
                var result = JsonConvert.DeserializeObject<Res_Lista_mascotas>(response);

                if (result.resultado)
                {
                    return result.ListaMascotas;
                }
                else
                {
                    await DisplayAlert("Error", $"Error en la API: {result.error}", "OK");
                    return new List<Registro_Mascota>();
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", $"No se pudo obtener la lista de mascotas: {ex.Message}", "OK");
                return new List<Registro_Mascota>();
            }
        }

        private void vacunaMascotaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (vacunaMascotaPicker.SelectedIndex != -1)
            {
                var vacunaSeleccionada = (Entidades.Vacunas)vacunaMascotaPicker.SelectedItem;
                if (vacunaSeleccionada != null)
                {
                    Debug.WriteLine("No se ha seleccionado ningún vacuna.");
                    DescripcionLabel.Text = vacunaSeleccionada.Decripcion;
                    FechaDeVencimientoLabel.Text = vacunaSeleccionada.FechaVencimiento.ToString();

                }
            }
        }

        private void MascotaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MascotaPicker.SelectedIndex != -1)
            {
                Debug.WriteLine("No se ha seleccionado ninguna mascota.");
                var mascotaSeleccionada = (Registro_Mascota)MascotaPicker.SelectedItem;
                if (mascotaSeleccionada != null)
                {
                    EspecieLabel.Text = mascotaSeleccionada.especie;
                    RazaLabel.Text = mascotaSeleccionada.raza;
                }
            }
        }



        private async void BtGUardar_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Botón BtGUardar_Clicked presionado.");
            try
            {
                Debug.WriteLine("Iniciando BtGUardar_Clicked");

                Debug.WriteLine($"SelectedIndex de MascotaPicker: {MascotaPicker.SelectedIndex}");
                Debug.WriteLine($"SelectedIndex de vacunaMascotaPicker: {vacunaMascotaPicker.SelectedIndex}");

                var mascotaSeleccionada = (Registro_Mascota)MascotaPicker.SelectedItem;
                var vacunaSeleccionado = (Entidades.Vacunas)vacunaMascotaPicker.SelectedItem;




                if (MascotaPicker.SelectedIndex == -1)
                {
                    await DisplayAlert("Error", "Seleccione una mascota", "Aceptar");
                    Debug.WriteLine("Error: No se seleccionó ninguna mascota.");
                    return;
                }
                else
                // Validar selección de mascota
                if (vacunaMascotaPicker.SelectedIndex == -1)
                {
                    await DisplayAlert("Error", "Seleccione un medicamento", "Aceptar");
                    Debug.WriteLine("Error: No se seleccionó ningún medicamento.");
                    return;
                }
                else

        // Convertir el valor seleccionado de DosisPicker a entero
                     if (DosisPicker.SelectedItem != null)
                   {
                    if (int.TryParse(DosisPicker.SelectedItem.ToString(), out int dosisSeleccionada))
                    {
                        Debug.WriteLine($"Dosis seleccionada: {dosisSeleccionada}");
                        // Aquí puedes usar dosisSeleccionada según sea necesario
                    }
                    else
                    {
                        await DisplayAlert("Error", "Selección de dosis inválida", "Aceptar");
                        Debug.WriteLine("Error: No se pudo convertir la dosis seleccionada.");
                        return;
                    }


                    var fechaHoraAplicacion = DatePickerVacuna.Date.Add(TimePickerVacuna.Time);
                    if (fechaHoraAplicacion < DateTime.Now)
                    {
                        await DisplayAlert("Error", "La fecha y hora de aplicación no pueden ser en el pasado.", "Aceptar");
                        Debug.WriteLine("Error: La fecha y hora de aplicación son inválidas.");
                        return;
                    }

                    var fechaHoraProximaAplicacion = DatePickerProxiVacuna.Date.Add(TimePickerProximaVacuna.Time);
                    if (fechaHoraProximaAplicacion < fechaHoraAplicacion)
                    {
                        await DisplayAlert("Error", "La fecha y hora de la próxima aplicación deben ser posteriores a la fecha y hora de la aplicación actual.", "Aceptar");
                        Debug.WriteLine("Error: La fecha y hora de la próxima aplicación son inválidas.");
                        return;
                    }else

                    if (string.IsNullOrWhiteSpace(EditorComentarios.Text))
                    {
                        await DisplayAlert("Error", "Ingrese comentarios adicionales", "Aceptar");
                        Debug.WriteLine("Error: Comentarios adicionales vacíos.");
                        return;
                    }




                    Req_Vacunas_Mascotas req = new Req_Vacunas_Mascotas
                    {
                        vacunas_Mascotas = new Vacunas_Mascotas
                        {
                            Id_Mascota = mascotaSeleccionada.Id_Mascota,
                            Id_Vacuna = vacunaSeleccionado.Id_Vacuna,
                            Dosis = dosisSeleccionada,
                            Fecha_y_Hora_Aplicacion = fechaHoraAplicacion,
                            Fecha_y_Hora_Proxima_Aplicacion = fechaHoraProximaAplicacion,
                            notas = EditorComentarios.Text
                        }
                    };

                    Debug.WriteLine("Serializando la solicitud a JSON");

                    var jsoncontent = new StringContent(JsonConvert.SerializeObject(req), System.Text.Encoding.UTF8, "application/json");
                    HttpClient httpClient = new HttpClient();

                    Debug.WriteLine("Enviando la solicitud al servidor");

                    var response = await httpClient.PostAsync(LaURL + "/Ingresar_Vacunas_Mascotas/Ingresar_Vacunas_Mascotas", jsoncontent);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Respuesta exitosa del servidor");

                        var responsecontent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Contenido de la respuesta: {responsecontent}");

                        Res_Vacunas_Mascotas res = JsonConvert.DeserializeObject<Res_Vacunas_Mascotas>(responsecontent);

                        if (res.resultado)
                        {
                            Debug.WriteLine("El medicamento se registró correctamente");

                            await DisplayAlert("Éxito", "Vacuna asignada correctamente", "Aceptar");
                            await Navigation.PushAsync(new RazasAnimales());
                        }
                        else
                        {
                            Debug.WriteLine($"Error en el backend: {res.error}");
                            await DisplayAlert("Error", res.error, "Aceptar");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Error de conexión: no se pudo conectar al servidor");
                        await DisplayAlert("Error de conexión", "Ocurrió un error de conexión", "Aceptar");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción: {ex.Message}");
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}