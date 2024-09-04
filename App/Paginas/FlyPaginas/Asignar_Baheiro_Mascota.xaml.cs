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
    public partial class Asignar_Baheiro_Mascota : ContentPage
    {
        private readonly HttpClient _httpClient;
        private string LaURL = "https://localhost:44348/api";
        private ObservableCollection<Registro_Mascota> Mascotas { get; set; }
        private ObservableCollection<Baheiro> Baheiros { get; set; }

        public Asignar_Baheiro_Mascota()
        {
            InitializeComponent();
            _httpClient = new HttpClient(); // Inicializa HttpClient
            Debug.WriteLine("HttpClient inicializado");

            Mascotas = new ObservableCollection<Registro_Mascota>();
            Baheiros = new ObservableCollection<Baheiro>();

            BindingContext = this;
            Debug.WriteLine("BindingContext establecido");

            CargarMascotas();
            CargarBaheiros();

            // Asociar los eventos de selección
            MascotaPicker.SelectedIndexChanged += MascotaPicker_SelectedIndexChanged;
            BaheiroPicker.SelectedIndexChanged += BaheiroPicker_SelectedIndexChanged;
            Debug.WriteLine("Eventos de selección asociados");
        }

        private async void CargarMascotas()
        {
            Debug.WriteLine("Cargando mascotas...");
            int id_usuario = (int)Sesion.id_usuario;
            Debug.WriteLine($"ID de usuario: {id_usuario}");

            var mascotas = await ObtenerMascotasAsync(id_usuario);
            Mascotas.Clear();
            foreach (var mascota in mascotas)
            {
                Debug.WriteLine($"Mascota cargada: {mascota.Nombre}");
                Mascotas.Add(mascota);
            }

            // Actualiza el ItemsSource del Picker explícitamente
            MascotaPicker.ItemsSource = Mascotas;
            Debug.WriteLine("Mascotas cargadas y ItemsSource actualizado");
        }

        private async void CargarBaheiros()
        {
            Debug.WriteLine("Cargando baheiros...");
            var baheiros = await ObtenerBaheirosAsync();
            Baheiros.Clear();
            foreach (var baheiro in baheiros)
            {
                Debug.WriteLine($"Baheiro cargado: {baheiro.Descripcion}");
                Baheiros.Add(baheiro);
            }

            // Actualiza el ItemsSource del Picker explícitamente
            BaheiroPicker.ItemsSource = Baheiros;
            Debug.WriteLine("Baheiros cargados y ItemsSource actualizado");
        }

        private async Task<List<Registro_Mascota>> ObtenerMascotasAsync(int id_usuario)
        {
            try
            {
                var requestUrl = $"{LaURL}/Lista_Mascotas/Obtener_Lista_Mascotas?id_usuario={id_usuario}";
                Debug.WriteLine($"Solicitando mascotas desde: {requestUrl}");
                var response = await _httpClient.GetAsync(requestUrl);

                response.EnsureSuccessStatusCode(); // Lanza una excepción si la respuesta no es exitosa

                var jsonString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta JSON: {jsonString}");
                var result = JsonConvert.DeserializeObject<Res_Lista_mascotas>(jsonString);

                if (result.resultado)
                {
                    Debug.WriteLine("Mascotas obtenidas con éxito");
                    return result.ListaMascotas;
                }
                else
                {
                    await DisplayAlert("Error", $"Error en la API: {result.error}", "OK");
                    Debug.WriteLine($"Error en la API: {result.error}");
                    return new List<Registro_Mascota>();
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", $"No se pudo obtener la lista de mascotas: {ex.Message}", "OK");
                Debug.WriteLine($"Excepción en ObtenerMascotasAsync: {ex.Message}");
                return new List<Registro_Mascota>();
            }
        }

        private async Task<List<Baheiro>> ObtenerBaheirosAsync()
        {
            try
            {
                // Crear la solicitud sin encabezado Content-Type
                var request = new HttpRequestMessage(HttpMethod.Get, LaURL + "/Lista_Baherios/Obtener_Lista_Baherios");
                Debug.WriteLine($"Solicitando baheiros desde: {request.RequestUri}");

                // Enviar la solicitud
                var response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode(); // Lanza una excepción si la respuesta no es exitosa

                var jsonString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta JSON: {jsonString}");
                var result = JsonConvert.DeserializeObject<Res_Lista_Baheiros>(jsonString);

                if (result.resultado)
                {
                    Debug.WriteLine("Baheiros obtenidos con éxito");
                    return result.ListaBaheiros;
                }
                else
                {
                    await DisplayAlert("Error", $"Error en la API: {result.error}", "OK");
                    Debug.WriteLine($"Error en la API: {result.error}");
                    return new List<Baheiro>();
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", $"No se pudo obtener la lista de baños: {ex.Message}", "OK");
                Debug.WriteLine($"Excepción en ObtenerBaheirosAsync: {ex.Message}");
                return new List<Baheiro>();
            }
        }

        private void MascotaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Cambio en selección de mascota");
            if (MascotaPicker.SelectedIndex != -1)
            {
                var mascotaSeleccionada = (Registro_Mascota)MascotaPicker.SelectedItem;
                if (mascotaSeleccionada != null)
                {
                    Debug.WriteLine($"Mascota seleccionada: {mascotaSeleccionada.Nombre}");
                    EspecieLabel.Text = mascotaSeleccionada.especie;
                    RazaLabel.Text = mascotaSeleccionada.raza;
                }
            }
        }

        private void BaheiroPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Cambio en selección de baheiro");
            if (BaheiroPicker.SelectedIndex != -1)
            {
                var baheiroSeleccionado = (Baheiro)BaheiroPicker.SelectedItem;
                if (baheiroSeleccionado != null)
                {
                    Debug.WriteLine($"Baheiro seleccionado: {baheiroSeleccionado.Descripcion}");
                    DescripcionLabel.Text = baheiroSeleccionado.Descripcion;
                }
            }
        }

        private async void BtGUardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("Guardando datos...");
                var baheiroSeleccionado = (Baheiro)BaheiroPicker.SelectedItem;
                var mascotaSeleccionada = (Registro_Mascota)MascotaPicker.SelectedItem;

                if (BaheiroPicker.SelectedIndex == -1)
                {
                    await DisplayAlert("Error", "Seleccione un baño", "Aceptar");
                    Debug.WriteLine("No se ha seleccionado un baheiro");
                    return;
                }

                if (MascotaPicker.SelectedIndex == -1)
                {
                    await DisplayAlert("Error", "Seleccione una mascota", "Aceptar");
                    Debug.WriteLine("No se ha seleccionado una mascota");
                    return;
                }

                // Validar fecha y hora de baño
                if (DatePickerBaño.Date < DateTime.Today)
                {
                    await DisplayAlert("Error", "Seleccione una fecha de baño válida", "Aceptar");
                    Debug.WriteLine("Fecha de baño inválida");
                    return;
                }

                // Validar fecha y hora del próximo baño
                if (DatePickerProximoBaño.Date < DateTime.Today)
                {
                    await DisplayAlert("Error", "Seleccione una fecha de próximo baño válida", "Aceptar");
                    Debug.WriteLine("Fecha de próximo baño inválida");
                    return;
                }

                if (string.IsNullOrEmpty(EditorNotas.Text))
                {
                    await DisplayAlert("Error", "Ingrese notas", "Aceptar");
                    Debug.WriteLine("Notas vacías");
                    return;
                }

                // Declarar y asignar las variables de fecha y hora combinadas
                DateTime fechaYHoraBaño = DatePickerBaño.Date.Add(TimePickerBaño.Time);
                DateTime fechaYHoraProximoBaño = DatePickerProximoBaño.Date.Add(TimePickerProximoBaño.Time);

                Req_Baheiro_Mascotas req = new Req_Baheiro_Mascotas
                {
                    baheiroMascotas = new BaheiroMascotas
                    {
                        Id_Mascota = mascotaSeleccionada.Id_Mascota, // Utiliza la mascota seleccionada
                        Id_Baheiro = baheiroSeleccionado.Id_Baheiro, // Utiliza el baño seleccionado
                        Fecha_y_hora_Baheiro = fechaYHoraBaño, // Asigna la fecha y hora combinadas
                        Fecha_y_hora_proximo_Baheiro = fechaYHoraProximoBaño, // Asigna la fecha y hora combinadas
                        Notas = EditorNotas.Text
                    }
                };

                Debug.WriteLine("Serializando la solicitud a JSON");
                var jsoncontent = new StringContent(JsonConvert.SerializeObject(req), System.Text.Encoding.UTF8, "application/json");
                Debug.WriteLine("Enviando la solicitud al servidor");

                var response = await _httpClient.PostAsync(LaURL + "/Ingresar_Baherios_Mascotas/Ingresar_Baherios_Mascotas", jsoncontent);

                if (response.IsSuccessStatusCode) // saber si el API está vivo
                {
                    Debug.WriteLine("Respuesta exitosa del servidor");

                    var responsecontent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Contenido de la respuesta: {responsecontent}");

                    Res_Baheiro_Mascotas res = JsonConvert.DeserializeObject<Res_Baheiro_Mascotas>(responsecontent);

                    if (res.resultado)
                    {
                        Debug.WriteLine("El registro se realizó correctamente");
                        await DisplayAlert("Éxito", "Baño agregado correctamente", "Aceptar");
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción: {ex.Message}");
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}