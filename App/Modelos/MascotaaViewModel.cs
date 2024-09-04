using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using FrontEndHealthPets.Modelos;
using Microsoft.Maui.Controls;
using FrontEndHealthPets.Paginas.FlyPaginas;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using FrontEndHealthPets.Entidades;
using FrontEndHealthPets.Entidades.response;
using FrontEndHealthPets.Entidades.Entitys;



namespace FrontEndHealthPets.Modelos
{
    public class MascotasViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<PerfilMascota> perfilMascotas;
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:44348/api";

        public ObservableCollection<PerfilMascota> PerfilMascotas
        {
            get => perfilMascotas;
            set
            {
                perfilMascotas = value;
                OnPropertyChanged(nameof(PerfilMascotas));
            }
        }

        public ICommand VerDetallesCommand { get; }

        public MascotasViewModel()
        {
            PerfilMascotas = new ObservableCollection<PerfilMascota>();
            VerDetallesCommand = new Command<PerfilMascota>(OnVerDetalles);

            _httpClient = new HttpClient(); // Inicializa HttpClient

            // Cargar las mascotas al iniciar el ViewModel
            CargarMascotasRegistradas();
        }

        public async void CargarMascotasRegistradas()
        {
            PerfilMascotas.Clear(); // Limpia la colección antes de agregar nuevas mascotas
            try
            {
                int id_usuario = (int)Sesion.id_usuario; // Suponiendo que tienes el id_usuario almacenado en la sesión
                var mascotas = await ObtenerMascotasDelUsuarioAsync(id_usuario);

                // Crea un HashSet para almacenar los Id_Mascota únicos
                HashSet<int> idMascotasUnicos = new HashSet<int>();

                foreach (var mascota in mascotas)
                {
                    // Verifica si el Id_Mascota ya está en el HashSet
                    if (!idMascotasUnicos.Contains(mascota.Id_Mascota))
                    {
                        // Agrega el Id_Mascota al HashSet
                        idMascotasUnicos.Add(mascota.Id_Mascota);

                        // Crea el perfil de la mascota y agrégalo a la colección
                        var perfilMascota = new PerfilMascota
                        {
                            Id_Mascota = mascota.Id_Mascota,
                            Name = mascota.Nombre,
                            Especie = mascota.especie,
                            Raza = mascota.raza,
                            Fecha_Nacimiento = mascota.Fecha_Nacimiento,
                            Fecha_Proximo_Baheiro = mascota.Fecha_Proximo_Baheiro,
                            ImageSource = await ObtenerImagenDeMascotaAsync(mascota.Id_Mascota)
                        };
                        // Verifica si la mascota ya está en la colección antes de agregarla
                        if (!PerfilMascotas.Any(pm => pm.Id_Mascota == perfilMascota.Id_Mascota))
                        {
                            PerfilMascotas.Add(perfilMascota);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async Task<ImageSource> ObtenerImagenDeMascotaAsync(int id_mascota)
        {
            try
            {
                var fotos = await ObtenerFotosDeMascotaAsync(id_mascota);
                var fotoPrincipal = fotos.FirstOrDefault(); // Asumiendo que quieres la primera foto

                if (fotoPrincipal != null)
                {
                    return ImageSource.FromStream(() => new MemoryStream(fotoPrincipal.Foto));
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener la imagen de la mascota: {ex.Message}");
                return null;
            }
        }
        private async Task<List<Registro_Mascota>> ObtenerMascotasDelUsuarioAsync(int id_usuario)
        {
            try
            {
                var requestUrl = $"{ApiUrl}/Lista_Mascotas/Obtener_Lista_Mascotas?id_usuario={id_usuario}";
                var response = await _httpClient.GetAsync(requestUrl);

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Res_Lista_mascotas>(jsonString);
                Debug.WriteLine("Respuesta JSON de la API: " + jsonString);

                if (result.resultado)
                {
                    return result.ListaMascotas;
                }
                else
                {
                    Debug.WriteLine($"Error en la API: {result.error}");
                    return new List<Registro_Mascota>();
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HttpRequestException: {ex.Message}");
                return new List<Registro_Mascota>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return new List<Registro_Mascota>();
            }
        }

        private async void OnVerDetalles(PerfilMascota mascota)
        {
            if (mascota != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new DetallesMascotaPage(mascota));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        }


        /// <summary>
        /// ////////////////////
        /// </summary>
        /// <param </param>
        /// <returns></returns>

        private async Task<List<FotosMascota>> ObtenerFotosDeMascotaAsync(int id_mascota)
        {
            try
            {
                var requestUrl = $"{ApiUrl}/Lista_Fotos_Mascotas/Obtener_Lista_Fotos_Mascotas?id_mascota={id_mascota}";
                var response = await _httpClient.GetAsync(requestUrl);

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Res_Lista_Fotos>(jsonString);

                if (result.resultado)
                {
                    return result.Lista_Fotos; // Suponiendo que `ListaFotos` es una lista de `FotosMascota`
                }
                else
                {
                    Debug.WriteLine($"Error en la API: {result.error}");
                    return new List<FotosMascota>();
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HttpRequestException: {ex.Message}");
                return new List<FotosMascota>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return new List<FotosMascota>();
            }
        }


    }


}