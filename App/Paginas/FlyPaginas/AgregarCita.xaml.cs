using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;
using FrontEndHealthPets.Entidades;
using FrontEndHealthPets.Entidades.Entitys;
using FrontEndHealthPets.Entidades.Request;
using FrontEndHealthPets.Entidades.response;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Paginas.FlyPaginas
{
    public partial class AgregarCita : ContentPage
    {
        private readonly HttpClient _httpClient;
        private readonly string LaURL = "https://localhost:44348/api";
        public ObservableCollection<Clinica_Veterinaria> listaClinicas;
        public ObservableCollection<Doctor> listaDoctores;
        public ObservableCollection<Registro_Mascota> Mascotas { get; set; }
        private int _Id_Clinica_Veterinaria;
        public AgregarCita()
        {
            InitializeComponent();

            _httpClient = new HttpClient();
            listaClinicas = new ObservableCollection<Clinica_Veterinaria>();
            listaDoctores = new ObservableCollection<Doctor>();
            Mascotas = new ObservableCollection<Registro_Mascota>();
            _Id_Clinica_Veterinaria = 0;
             
             BindingContext = this;

            Debug.WriteLine("Inicialización completa. Cargando datos...");
            CargarDatosClinicas();
            
            CargarDatosDoctores(_Id_Clinica_Veterinaria);
            CargarMascotas();
        }

        // Método para cargar datos de clínicas
        private async void CargarDatosClinicas()
        {
            Debug.WriteLine("Cargando datos de clínicas...");
            var clinicas = await ObtenerClinicasAsync();

            listaClinicas.Clear();
            foreach (var clinica in clinicas)
            {
                Debug.WriteLine($"ID Clínica: {clinica.Id_Clinica_Veterinaria}, Dirección: {clinica.Direccion}, Teléfono: {clinica.Telefono}");
                listaClinicas.Add(clinica);
            }

            pickerClinicas.ItemsSource = listaClinicas;
            Debug.WriteLine("Datos de clínicas cargados.");
        }

        // Método para cargar datos de doctores
        private async void CargarDatosDoctores(int Id_Clinica_Veterinaria)
        {
            Debug.WriteLine($"Cargando datos de doctores para clínica ID: {Id_Clinica_Veterinaria}...");
            var doctores = await ObtenerDoctoresPorClinicaAsync(Id_Clinica_Veterinaria);

            listaDoctores.Clear();
            foreach (var doctor in doctores)
            {
                Debug.WriteLine($"ID Doctor: {doctor.Id_Doctor}, Nombre: {doctor.Nombre}");
                listaDoctores.Add(doctor);
            }

            pickerDoctores.ItemsSource = listaDoctores;
            Debug.WriteLine("Datos de doctores cargados.");
        }

        // Método para cargar datos de mascotas
        private async void CargarMascotas()
        {
            int id_usuario = (int)Sesion.id_usuario;
            Debug.WriteLine($"ID del usuario: {id_usuario}");

            var mascotas = await ObtenerMascotasAsync(id_usuario);
            Mascotas.Clear();
            foreach (var mascota in mascotas)
            {
                Debug.WriteLine($"ID Mascota: {mascota.Id_Mascota}, Nombre: {mascota.Nombre}");
                Mascotas.Add(mascota);
            }
            MascotaPicker.ItemsSource = Mascotas;
            Debug.WriteLine("Datos de mascotas cargados.");
        }

        private async Task<List<Registro_Mascota>> ObtenerMascotasAsync(int id_usuario)
        {
            try
            {
                Debug.WriteLine($"ID del usuario: {id_usuario}");
                Debug.WriteLine($"URL de solicitud: {LaURL}/Lista_Mascotas/Obtener_Lista_Mascotas?id_usuario={id_usuario}");

                var requestUrl = $"{LaURL}/Lista_Mascotas/Obtener_Lista_Mascotas?id_usuario={id_usuario}";
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

                var response = await _httpClient.SendAsync(request);
                Debug.WriteLine($"Código de estado de respuesta: {response.StatusCode}");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta JSON: {jsonString}");
                var result = JsonConvert.DeserializeObject<Res_Lista_mascotas>(jsonString);

                if (result.resultado)
                {
                    Debug.WriteLine($"Número de mascotas obtenidas: {result.ListaMascotas.Count}");
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
                Debug.WriteLine($"Error de solicitud HTTP: {ex.Message}");
                await DisplayAlert("Error", $"No se pudo obtener la lista de mascotas: {ex.Message}", "OK");
                return new List<Registro_Mascota>();
            }
        }

        private async Task<List<Entidades.Clinica_Veterinaria>> ObtenerClinicasAsync()
        {
            try
            {
                Debug.WriteLine($"URL de solicitud: {LaURL}/Lista_Clinica_Veterinaria/Obtener_Lista_Clinica_Veterinaria");

                var request = new HttpRequestMessage(HttpMethod.Get, LaURL + "/Lista_Clinica_Veterinaria/Obtener_Lista_Clinica_Veterinaria");
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta JSON: {jsonString}");

                var result = JsonConvert.DeserializeObject<Res_Lista_Clinica_Veterinaria>(jsonString);

                if (result.resultado)
                {
                    // Imprime la lista de clínicas para ver los datos deserializados
                    foreach (var clinica in result.Lista_Clinica_Veterinaria)
                    {

                        Debug.WriteLine($"ID Clínica deserializado: {clinica.Id_Clinica_Veterinaria}");
                    }
                    return result.Lista_Clinica_Veterinaria;
                }
                else
                {
                    await DisplayAlert("Error", $"Error en la API: {result.error}", "OK");
                    return new List<Entidades.Clinica_Veterinaria>();
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", $"No se pudo obtener la lista de clínicas: {ex.Message}", "OK");
                return new List<Entidades.Clinica_Veterinaria>();
            }
        }

        private async Task<List<Doctor>> ObtenerDoctoresPorClinicaAsync(int Id_Clinica_Veterinaria)
        {
            try
            {
                Debug.WriteLine($"URL de solicitud: {LaURL}/Lista_Doctores/Obtener_Doctores_Por_Clinica?idClinica={Id_Clinica_Veterinaria}");

                var requestUrl = $"{LaURL}/Lista_Clinica_Veterinaria/Obtener_Lista_Clinica_Veterinaria?Id_Clinica_Veterinaria={Id_Clinica_Veterinaria}";
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

                var response = await _httpClient.SendAsync(request);
                Debug.WriteLine($"Código de estado de respuesta: {response.StatusCode}");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta JSON: {jsonString}");
                var result = JsonConvert.DeserializeObject<Res_Lista_de_Doctores_Clinica>(jsonString);

                if (result.resultado)
                {
                    Debug.WriteLine($"Número de doctores obtenidos: {result.listaDoctoresporclinica.Count}");
                    return result.listaDoctoresporclinica;

                }
                else
                {
                    return new List<Doctor>();

                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Error de solicitud HTTP: {ex.Message}");
                await DisplayAlert("Error", $"No se pudo obtener la lista de doctores: {ex.Message}", "OK");
                return new List<Doctor>();
            }
        }
        

        // Método para manejar selección de clínica
        private void PickerClinicas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerClinicas.SelectedIndex != -1)
            {
                var clinicaSeleccionada = (Entidades.Clinica_Veterinaria)pickerClinicas.SelectedItem;
                if (clinicaSeleccionada != null)
                {
                    Debug.WriteLine($"ID Clínica Seleccionada: {clinicaSeleccionada.Id_Clinica_Veterinaria}");
                    lblDireccionClinica.Text = clinicaSeleccionada.Direccion ?? "Dirección no disponible";
                    lblTelefonoClinica.Text = clinicaSeleccionada.Telefono ?? "Teléfono no disponible";
                    

                    // Ahora que tienes el Id de la clínica seleccionada, puedes usarlo para cargar los doctores
                    _Id_Clinica_Veterinaria = clinicaSeleccionada.Id_Clinica_Veterinaria;
                    CargarDatosDoctores(_Id_Clinica_Veterinaria);
                }
                else
                {
                    Debug.WriteLine("Clínica seleccionada es null.");
                    lblDireccionClinica.Text = "Dirección no disponible";
                    lblTelefonoClinica.Text = "Teléfono no disponible";
                }
            }
            else
            {
                Debug.WriteLine("Ninguna clínica seleccionada.");
            }
        }

        // Método para manejar selección de doctor
        private void PickerDoctores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerDoctores.SelectedIndex != -1)
            {
                var doctorSeleccionado = (Doctor)pickerDoctores.SelectedItem;
                if (doctorSeleccionado != null)
                {
                    Debug.WriteLine($"ID Doctor Seleccionado: {doctorSeleccionado.Id_Doctor}");
                    lblTelefonoDoctor.Text = doctorSeleccionado.Telefono;
                    lblCorreoDoctor.Text = doctorSeleccionado.Correo_Electronico;
                }
                else
                {
                    Debug.WriteLine("Doctor seleccionado es null.");
                    lblTelefonoDoctor.Text = "Teléfono no disponible";
                    lblCorreoDoctor.Text = "Correo no disponible";
                }
            }
            else
            {
                Debug.WriteLine("Ningún doctor seleccionado.");
            }
        }

        // Método para manejar selección de mascota
        private void MascotaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Implementar según sea necesario
        }

        // Método para agregar una cita
        private async void BtCita_Clicked(object sender, EventArgs e)
        {
            try
            {
                var mascotaSeleccionada = (Registro_Mascota)MascotaPicker.SelectedItem;
                var clinicaSeleccionada = (Clinica_Veterinaria)pickerClinicas.SelectedItem;
                var doctorSeleccionado = (Doctor)pickerDoctores.SelectedItem;

                if (mascotaSeleccionada == null || clinicaSeleccionada == null || doctorSeleccionado == null)
                {
                    Debug.WriteLine("Faltan datos: mascota, clínica o doctor no seleccionados.");
                    await DisplayAlert("Error", "Por favor seleccione una mascota, clínica y doctor", "OK");
                    return;
                }

                DateTime fechaYHoraCita = DatePickerCita.Date.Add(TimePickerCita.Time);

                Debug.WriteLine($"ID Mascota: {mascotaSeleccionada.Id_Mascota}, ID Clínica: {clinicaSeleccionada.Id_Clinica_Veterinaria}, ID Doctor: {doctorSeleccionado.Id_Doctor}");
                Debug.WriteLine($"Fecha y Hora de Cita: {fechaYHoraCita}, Notas: {notasEditor.Text}, Descripción: {descripcionEditor.Text}");

                Req_Citas_Clinica_Veterinaria_Mascotas req = new Req_Citas_Clinica_Veterinaria_Mascotas
                {
                    citas_Clinica_Veterinaria_Mascotas = new Citas_Clinica_Veterinaria_Mascotas
                    {
                        Id_Mascota = mascotaSeleccionada.Id_Mascota,
                        Id_Clinica = clinicaSeleccionada.Id_Clinica_Veterinaria,
                        id_doctor = doctorSeleccionado.Id_Doctor,
                        Fecha_y_hora_Cita = fechaYHoraCita,
                        Notas = notasEditor.Text,
                        Direccion = lblDireccionClinica.Text,
                        Telefono = lblTelefonoClinica.Text,
                        Descripcion = descripcionEditor.Text,
                    }
                };

                var jsoncontent = new StringContent(JsonConvert.SerializeObject(req), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(LaURL + "/Ingresar_Citas_Veterinarias/Ingresar_Citas_Veterinarias", jsoncontent);

                Debug.WriteLine($"Código de estado de respuesta: {response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var responsecontent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Respuesta JSON: {responsecontent}");
                    Res_Citas_Clinica_Veterinaria_Mascotas res = JsonConvert.DeserializeObject<Res_Citas_Clinica_Veterinaria_Mascotas>(responsecontent);

                    if (res.resultado)
                    {
                        Debug.WriteLine("Cita registrada exitosamente.");
                        await DisplayAlert("Éxito", "Cita registrada exitosamente", "Aceptar");
                        await Navigation.PushAsync(new RazasAnimales());
                    }
                    else
                    {
                        Debug.WriteLine($"Error al registrar cita: {res.error}");
                        await DisplayAlert("Error", res.error, "Aceptar");
                    }
                }
                else
                {
                    Debug.WriteLine("Error de conexión al registrar cita.");
                    await DisplayAlert("Error de conexión", "Ocurrió un error de conexión", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al agregar cita: {ex.Message}");
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
