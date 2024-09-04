using FrontEndHealthPets.Entidades.Entitys;
using FrontEndHealthPets.Modelos;
using System.Diagnostics;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;
using System.Net.Http.Headers;
using CommunityToolkit.Maui.Views;
using FrontEndHealthPets.Entidades;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using FrontEndHealthPets.Entidades.Request;
using FrontEndHealthPets.Entidades.response;
using FrontEndHealthPets.Entidades.entitys;
using Microsoft.Maui.Graphics.Text;


namespace FrontEndHealthPets.Paginas.FlyPaginas;
public partial class perfilu : ContentPage
{
    private readonly HttpClient _httpClient;
    public static string LaURL { get; } = "https://localhost:44348/api";
    private UsuarioViewModel viewModel;
    public perfilu()
    {
        InitializeComponent();
        _httpClient = new HttpClient();
        viewModel = new UsuarioViewModel
        {
            Nombre = Sesion.nombre,
            Apellido = Sesion.apellidos,
            CorreoElectronico = Sesion.Correo_Electronico, // Asegúrate de tener este dato almacenado en la sesión si lo necesitas
            Password = Sesion.Password // Asegúrate de tener este dato almacenado en la sesión si lo necesitas
        };


        // Debugging
        Debug.WriteLine($"Nombre: {Sesion.nombre}");
        Debug.WriteLine($"Apellido: {Sesion.apellidos}");
        Debug.WriteLine($"Correo: {Sesion.Correo_Electronico}");
        Debug.WriteLine($"Password: {Sesion.Password}");


        BindingContext = viewModel;
        Debug.WriteLine($"BindingContext: {BindingContext}");
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            // Abre la galería para seleccionar una imagen
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona una foto de perfil",
                FileTypes = FilePickerFileType.Images // Filtra solo imágenes
            });

            if (result != null)
            {
                // Muestra la imagen seleccionada en el control Image
                using var stream = await result.OpenReadAsync();
                imgPerfil.Source = ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo cargar la imagen: " + ex.Message, "OK");
        }
    }

    private void btRegresar_Clicked(object sender, EventArgs e)
    {

    }

    private async void BtActualizarNombre_Clicked(object sender, EventArgs e)
    {
        var viewModel = (UsuarioViewModel)this.BindingContext; // Usa UsuarioViewModel aquí
        if (viewModel != null)
        {
            var popup = new EditarDatosPopup(viewModel, "Nombre");
            await this.ShowPopupAsync(popup);
        }
    }

    private async void BtActualizarApeliido_Clicked(object sender, EventArgs e)
    {
        var viewModel = (UsuarioViewModel)this.BindingContext; // Usa UsuarioViewModel aquí
        if (viewModel != null)
        {
            var popup = new EditarDatosPopup(viewModel, "Apellidos");
            await this.ShowPopupAsync(popup);
        }
    }


    private async void BtActualizarCooreo_Clicked(object sender, EventArgs e)
    {
        var viewModel = (UsuarioViewModel)this.BindingContext; // Usa UsuarioViewModel aquí
        if (viewModel != null)
        {
            var popup = new EditarDatosPopup(viewModel, "Correo");
            await this.ShowPopupAsync(popup);
        }
    }



    private async void BtActualizarPassword_Clicked(object sender, EventArgs e)
    {
        var viewModel = (UsuarioViewModel)this.BindingContext; // Usa UsuarioViewModel aquí
        if (viewModel != null)
        {
            var popup = new EditarDatosPopup(viewModel, "Password");
            await this.ShowPopupAsync(popup);
        }
    }

    public class EditarDatosPopup : Popup
    {
        private readonly UsuarioViewModel _viewModel;
        private readonly string _tipoEdicion;

        public EditarDatosPopup(UsuarioViewModel viewModel, string tipoEdicion)
        {
            _viewModel = viewModel;
            _tipoEdicion = tipoEdicion;

            var entry1 = new Entry { IsPassword = _tipoEdicion == "Password" };
            Entry entry2 = null; // Define it as null and initialize later if needed.

            var button = new Button { Text = "Aceptar", BackgroundColor = Colors.Purple, TextColor = Colors.White };

            string labelText, placeholder1, placeholder2 = null;

            // Adjust placeholders and label based on the type of edition.
            switch (_tipoEdicion)
            {
                case "Nombre":
                    labelText = "Actualizar Nombre";
                    placeholder1 = "Nombre";
                    break;

                case "Apellidos":
                    labelText = "Actualizar Apellidos";
                    placeholder1 = "Apellidos";
                    break;

                case "Correo":
                    labelText = "Actualizar Correo Electrónico";
                    placeholder1 = "Correo Electrónico";
                    placeholder2 = "Confirmar Correo Electrónico";
                    break;

                case "Password":
                    labelText = "Actualizar Contraseña";
                    placeholder1 = "Nueva Contraseña";
                    placeholder2 = "Confirmar Contraseña";
                    break;

                default:
                    throw new ArgumentException("Tipo de edición no reconocido");
            }

            entry1.Placeholder = placeholder1;

            var stackLayout = new StackLayout
            {
                Padding = new Thickness(20),
                Children =
            {
                new Label { Text = labelText, FontSize = 20, FontAttributes = FontAttributes.Bold },
                entry1,
                button
            }
            };

            // Only add the second entry if it is needed.
            if (placeholder2 != null)
            {
                entry2 = new Entry { IsPassword = _tipoEdicion == "Password" };
                entry2.Placeholder = placeholder2;
                stackLayout.Children.Insert(2, entry2);
            }

            Content = stackLayout;

            // Button click logic
            button.Clicked += async (sender, e) =>
            {
                var valor1 = entry1.Text;
                var valor2 = entry2?.Text; // Use null propagation.

                // Add checks for required fields
                if (string.IsNullOrEmpty(valor1) || (entry2 != null && string.IsNullOrEmpty(valor2)))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
                    return;
                }

                if (entry2 != null && valor1 != valor2)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Los valores no coinciden.", "OK");
                    return;
                }

                // Send the update request
                await EnviarActualizacion(_tipoEdicion, valor1, valor2);
            };
        }

        private async Task EnviarActualizacion(string tipoEdicion, string valor1, string valor2)
        {
            // Preparar el objeto a enviar
            var req = new Req_Actualizar_Usuario
            {
                Id_Usuario = (int)Sesion.id_usuario,
                Nombre = tipoEdicion == "Nombre" ? valor1 : null,
                Apellidos = tipoEdicion == "Apellidos" ? valor1 : null,
                Correo_Electronico = tipoEdicion == "Correo" ? valor1 : null,
                Confirmacion_Correo_Electronico = tipoEdicion == "Correo" ? valor2 : null,
                Password = tipoEdicion == "Password" ? valor1 : null,
                Confirmar_Password = tipoEdicion == "Password" ? valor2 : null
            };

            try
            {
                // Construir la URL con el parámetro de consulta
                var requestUrl = $"{LaURL}/Usuarios/ActualizarUsuario/{Sesion.id_usuario}?Id_Usuario={Sesion.id_usuario}";

                // Verificar si la URL es válida
                if (string.IsNullOrEmpty(LaURL))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "La URL no es válida.", "OK");
                    return;
                }

                // Verificar si el ID de usuario es válido
                if (Sesion.id_usuario == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "El ID de usuario no es válido.", "OK");
                    return;
                }

                using var httpClient = new HttpClient();

                // Añadir el token Bearer a los encabezados
                string token = Sesion.token;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PutAsJsonAsync(requestUrl, req);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo actualizar la información. Código de estado: {response.StatusCode}", "OK");
                    return;
                }

                var jsonString = await response.Content.ReadAsStringAsync();

                try
                {
                    var result = JsonConvert.DeserializeObject<Res_Actualizar_Usuario>(jsonString);

                    // Actualizar el ViewModel y la Sesion con los nuevos valores
                    switch (tipoEdicion)
                    {
                        case "Nombre":
                            _viewModel.Nombre = valor1;
                            Sesion.nombre = _viewModel.Nombre;
                            break;

                        case "Apellidos":
                            _viewModel.Apellido = valor1;
                            Sesion.apellidos = _viewModel.Apellido;
                            break;

                        case "Correo":
                            _viewModel.CorreoElectronico = valor1;
                            Sesion.Correo_Electronico = _viewModel.CorreoElectronico;
                            break;

                        case "Password":
                            _viewModel.Password = valor1;
                            Sesion.Password = _viewModel.Password;
                            break;
                    }

                    await Application.Current.MainPage.DisplayAlert("Éxito", "Datos actualizados correctamente.", "OK");
                    Close();
                }
                catch (JsonException jsonEx)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Hubo un problema al procesar la respuesta del servidor.", "OK");
                }
            }
            catch (HttpRequestException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al actualizar los datos: {ex.Message}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error inesperado: {ex.Message}", "OK");
            }
        }
    }





    private void Btcerrarsesion_Clicked(object sender, EventArgs e)
    {
        CerrarSesion();
    }

    private async void CerrarSesion()
    {
        // Cerrar la sesión y limpiar los datos de sesión.
        Sesion.CerrarSesion();

        // Reemplazar la MainPage con la página de inicio de sesión, lo que evita problemas de navegación.
        Application.Current.MainPage = new NavigationPage(new MainPage());

        // Mostrar un mensaje de confirmación.
        await Application.Current.MainPage.DisplayAlert("Cerrar sesión", "Has cerrado sesión correctamente.", "OK");
    }





    public class EliminarsesionPopup : Popup
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private string laURL;

        public EliminarsesionPopup(string url)
        {
            laURL = url;

            var emailEntry = new Entry
            {
                Placeholder = "Ingrese su correo electrónico",
                Keyboard = Keyboard.Email,
                TextColor = Colors.Black
            };

            var enviarButton = new Button
            {
                Text = "Eliminar",
                BackgroundColor = Colors.Purple,
                TextColor = Colors.White
            };

            enviarButton.Clicked += async (sender, e) =>
            {
                var email = emailEntry.Text;

                if (string.IsNullOrEmpty(email))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "El correo electrónico es obligatorio.", "OK");
                    return;
                }

                if (Sesion.id_usuario == null || Sesion.id_usuario <= 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se encontró un ID de usuario válido en la sesión.", "OK");
                    return;
                }

                // Eliminar la solicitud de cuerpo (req) para DELETE
                Debug.WriteLine($"Id_Usuario: {Sesion.id_usuario}, Correo Electrónico: {email}");

                try
                {
                    if (string.IsNullOrEmpty(laURL))
                    {
                        Debug.WriteLine("Error: La URL está vacía o no es válida.");
                        await Application.Current.MainPage.DisplayAlert("Error", "La URL no es válida.", "OK");
                        return;
                    }

                    var requestUrl = $"{laURL}/Usuarios/EliminarUsuario/{Sesion.id_usuario}?idUsuario={Sesion.id_usuario}&correoElectronico={Uri.EscapeDataString(email)}";
                    Debug.WriteLine($"Request URL: {requestUrl}");

                    // Añadir el token Bearer a los encabezados
                    string token = Sesion.token; // Asumiendo que el token está guardado en Sesion
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    Debug.WriteLine($"Bearer Token: {token}");
                    Debug.WriteLine($"Authorization Header: {httpClient.DefaultRequestHeaders.Authorization}");

                    Debug.WriteLine("Enviando la solicitud DELETE...");
                    var response = await httpClient.DeleteAsync(requestUrl);
                    Debug.WriteLine($"Response Status Code: {response.StatusCode}");

                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine($"Error: La solicitud falló con el código de estado {response.StatusCode}");
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Contenido de la respuesta de error: {errorContent}");
                        await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo actualizar la información. Código de estado: {response.StatusCode}", "OK");
                        return;
                    }

                    // Leer el contenido de la respuesta
                    var jsonString = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response JSON: {jsonString}");

                    // Mostrar el contenido de la respuesta en una alerta
                    await Application.Current.MainPage.DisplayAlert("Éxito", jsonString, "OK");

                    Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Excepción inesperada: {ex.Message}");
                    await Application.Current.MainPage.DisplayAlert("Error", $"Error inesperado: {ex.Message}", "OK");
                }
            };

            Content = new StackLayout
            {
                Padding = 20,
                Spacing = 15,
                Children =
            {
                emailEntry,
                enviarButton
            },
                WidthRequest = 300,
                HeightRequest = 200
            };
        }
    }

    private async void bteliminar_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Mostrar el popup
            var popup = new EliminarsesionPopup(LaURL);
            await Application.Current.MainPage.ShowPopupAsync(popup);

           

            // Redirigir a la página principal
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
        catch (Exception ex)
        {
            // Manejar o registrar la excepción
            Console.WriteLine($"Error mostrando popup: {ex.Message}");
        }
    }


}












