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

            // Asociar los eventos de selecci�n
            MascotaPicker.SelectedIndexChanged += MascotaPicker_SelectedIndexChanged;
            BaheiroPicker.SelectedIndexChanged += BaheiroPicker_SelectedIndexChanged;
            Debug.WriteLine("Eventos de selecci�n asociados");
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

            // Actualiza el ItemsSource del Picker expl�citamente
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

            // Actualiza el ItemsSource del Picker expl�citamente
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

                response.EnsureSuccessStatusCode(); // Lanza una excepci�n si la respuesta no es exitosa

                var jsonString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta JSON: {jsonString}");
                var result = JsonConvert.DeserializeObject<Res_Lista_mascotas>(jsonString);

                if (result.resultado)
                {
                    Debug.WriteLine("Mascotas obtenidas con �xito");
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
                Debug.WriteLine($"Excepci�n en ObtenerMascotasAsync: {ex.Message}");
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

                response.EnsureSuccessStatusCode(); // Lanza una excepci�n si la respuesta no es exitosa

                var jsonString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta JSON: {jsonString}");
                var result = JsonConvert.DeserializeObject<Res_Lista_Baheiros>(jsonString);

                if (result.resultado)
                {
                    Debug.WriteLine("Baheiros obtenidos con �xito");
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
                await DisplayAlert("Error", $"No se pudo obtener la lista de ba�os: {ex.Message}", "OK");
                Debug.WriteLine($"Excepci�n en ObtenerBaheirosAsync: {ex.Message}");
                return new List<Baheiro>();
            }
        }

        private void MascotaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Cambio en selecci�n de mascota");
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
            Debug.WriteLine("Cambio en selecci�n de baheiro");
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
                    await DisplayAlert("Error", "Seleccione un ba�o", "Aceptar");
                    Debug.WriteLine("No se ha seleccionado un baheiro");
                    return;
                }

                if (MascotaPicker.SelectedIndex == -1)
                {
                    await DisplayAlert("Error", "Seleccione una mascota", "Aceptar");
                    Debug.WriteLine("No se ha seleccionado una mascota");
                    return;
                }

                // Validar fecha y hora de ba�o
                if (DatePickerBa�o.Date < DateTime.Today)
                {
                    await DisplayAlert("Error", "Seleccione una fecha de ba�o v�lida", "Aceptar");
                    Debug.WriteLine("Fecha de ba�o inv�lida");
                    return;
                }

                // Validar fecha y hora del pr�ximo ba�o
                if (DatePickerProximoBa�o.Date < DateTime.Today)
                {
                    await DisplayAlert("Error", "Seleccione una fecha de pr�ximo ba�o v�lida", "Aceptar");
                    Debug.WriteLine("Fecha de pr�ximo ba�o inv�lida");
                    return;
                }

                if (string.IsNullOrEmpty(EditorNotas.Text))
                {
                    await DisplayAlert("Error", "Ingrese notas", "Aceptar");
                    Debug.WriteLine("Notas vac�as");
                    return;
                }

                // Declarar y asignar las variables de fecha y hora combinadas
                DateTime fechaYHoraBa�o = DatePickerBa�o.Date.Add(TimePickerBa�o.Time);
                DateTime fechaYHoraProximoBa�o = DatePickerProximoBa�o.Date.Add(TimePickerProximoBa�o.Time);

                Req_Baheiro_Mascotas req = new Req_Baheiro_Mascotas
                {
                    baheiroMascotas = new BaheiroMascotas
                    {
                        Id_Mascota = mascotaSeleccionada.Id_Mascota, // Utiliza la mascota seleccionada
                        Id_Baheiro = baheiroSeleccionado.Id_Baheiro, // Utiliza el ba�o seleccionado
                        Fecha_y_hora_Baheiro = fechaYHoraBa�o, // Asigna la fecha y hora combinadas
                        Fecha_y_hora_proximo_Baheiro = fechaYHoraProximoBa�o, // Asigna la fecha y hora combinadas
                        Notas = EditorNotas.Text
                    }
                };

                Debug.WriteLine("Serializando la solicitud a JSON");
                var jsoncontent = new StringContent(JsonConvert.SerializeObject(req), System.Text.Encoding.UTF8, "application/json");
                Debug.WriteLine("Enviando la solicitud al servidor");

                var response = await _httpClient.PostAsync(LaURL + "/Ingresar_Baherios_Mascotas/Ingresar_Baherios_Mascotas", jsoncontent);

                if (response.IsSuccessStatusCode) // saber si el API est� vivo
                {
                    Debug.WriteLine("Respuesta exitosa del servidor");

                    var responsecontent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Contenido de la respuesta: {responsecontent}");

                    Res_Baheiro_Mascotas res = JsonConvert.DeserializeObject<Res_Baheiro_Mascotas>(responsecontent);

                    if (res.resultado)
                    {
                        Debug.WriteLine("El registro se realiz� correctamente");
                        await DisplayAlert("�xito", "Ba�o agregado correctamente", "Aceptar");
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
                    Debug.WriteLine("Error de conexi�n: no se pudo conectar al servidor");
                    await DisplayAlert("Error de conexi�n", "Ocurri� un error de conexi�n", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepci�n: {ex.Message}");
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}