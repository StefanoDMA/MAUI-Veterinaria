using CommunityToolkit.Maui.Views;
using DocumentFormat.OpenXml.Drawing;
using FrontEndHealthPets.Entidades.Entitys;
using FrontEndHealthPets.Entidades.Request;
using FrontEndHealthPets.Entidades.Response;
using FrontEndHealthPets.Paginas;
using FrontEndHealthPets.Paginas.FlyPaginas;
using FrontEndHealthPets.Paginas.tabpage;

using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Net.Http;
using FrontEndHealthPets.Entidades;
using FrontEndHealthPets.Entidades.entitys;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Maui.Graphics.Text;



namespace FrontEndHealthPets
{
    public partial class MainPage : ContentPage
    {
        string laURL = "https://localhost:44348/api";

        public MainPage()
        {
            InitializeComponent();
        }

        private async void btiniciarsecion_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new PagFlyPrincipal());
            try
            {
                // Validación de campos de inicio de sesión
                if (string.IsNullOrWhiteSpace(Correo.Text))
                {
                    await DisplayAlert("Error", "Por favor, ingresa tu usuario o correo electrónico.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Passwoord.Text))
                {
                    await DisplayAlert("Error", "Por favor, ingresa tu contraseña.", "OK");
                    return;
                }

                Req_Login req = new Req_Login
                {
                    Correo_Electronico = Correo.Text,
                    Contrasena = Passwoord.Text
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(req), System.Text.Encoding.UTF8, "application/json");

                using (HttpClient httpClient = new HttpClient())
                {
                    var responseTask = httpClient.PostAsync($"{laURL}/login/IngresarLogin", jsonContent);
                    var response = await responseTask;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Res_Login res = JsonConvert.DeserializeObject<Res_Login>(responseContent);


                        if (res.resultado)
                        {

                            Sesion.id_usuario = res.Registro_Usuario.Id_Usuario;
                            Sesion.nombre = res.Registro_Usuario.Nombre;
                            Sesion.apellidos = res.Registro_Usuario.Apellidos;
                            Sesion.Correo_Electronico = res.Registro_Usuario.Correo_Electronico;
                            Sesion.Password = res.Registro_Usuario.Password;
                            Sesion.token = res.Registro_Usuario.token;

                            Debug.WriteLine($"Correo_Electronico: {Sesion.Correo_Electronico}");
                            Debug.WriteLine($"Password: {Sesion.Password}");

                            await Navigation.PushAsync(new PagFlyPrincipal());
                        }
                        else
                        {
                            await DisplayAlert("Error en backend", "Login incorrecto!", "Aceptar");
                        }
                    }
                    else
                    {
                        await DisplayAlert($"Error de conexión", "Ocurrió un error de conexión", "Aceptar");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error de aplicación", "Reinstale la aplicación. Detalle: " + ex.Message, "Aceptar");
            }

        }

        public class RecuperarPasswordPopup : Popup
        {
            private static readonly HttpClient httpClient = new HttpClient();
            private string laURL;

            public RecuperarPasswordPopup(string url)
            {
                laURL = url;


                var emailEntry = new Entry { Placeholder = "Ingrese su correo electrónico", Keyboard = Keyboard.Email, TextColor = Colors.Black };
                var nuevaContrasenaEntry = new Entry { Placeholder = "Ingrese nueva contraseña", IsPassword = true , TextColor = Colors.Black };
                var confirmarContrasenaEntry = new Entry { Placeholder = "Confirme su contraseña", IsPassword = true , TextColor = Colors.Black };
                var enviarButton = new Button { Text = "Enviar", BackgroundColor = Colors.Purple, TextColor = Colors.White };
               
                enviarButton.Clicked += async (sender, e) =>
                {
                    
                    var email = emailEntry.Text;
                    var nuevaContrasena = nuevaContrasenaEntry.Text;
                    var confirmarContrasena = confirmarContrasenaEntry.Text;

                    if (string.IsNullOrEmpty(email))
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "El correo electrónico es obligatorio.", "OK");
                        return;
                    }

                    if (string.IsNullOrEmpty(nuevaContrasena) || string.IsNullOrEmpty(confirmarContrasena))
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Las contraseñas son obligatorias.", "OK");
                        return;
                    }

                    if (nuevaContrasena != confirmarContrasena)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
                        return;
                    }

                    Req_RecuperarPassword req = new Req_RecuperarPassword
                    {
                        recuperarPassword = new RecuperarPassword
                        {
                            CorreoElectronico = email,
                            NuevoPassword = nuevaContrasena,
                            ConfirmarPassword = confirmarContrasena
                        }
                    };

                    try
                    {
                        var requestUrl = $"{laURL}/Nueva_Password/RecuperarPassword";
                        Debug.WriteLine($"Request URL: {requestUrl}");

                        if (string.IsNullOrEmpty(laURL))
                        {
                            Debug.WriteLine("Error: La URL está vacía o no es válida.");
                            await Application.Current.MainPage.DisplayAlert("Error", "La URL no es válida.", "OK");
                            return;
                        }

                       
                       

                        var jsonContent = JsonConvert.SerializeObject(req);
                        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                        Debug.WriteLine("Enviando la solicitud PUT...");
                        var response = await httpClient.PutAsync(requestUrl, content);
                        Debug.WriteLine($"Response Status Code: {response.StatusCode}");

                        if (!response.IsSuccessStatusCode)
                        {
                            Debug.WriteLine($"Error: La solicitud falló con el código de estado {response.StatusCode}");
                            var errorContent = await response.Content.ReadAsStringAsync();
                            Debug.WriteLine($"Contenido de la respuesta de error: {errorContent}");
                            await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo actualizar la información. Código de estado: {response.StatusCode}", "OK");
                            return;
                        }

                        var jsonString = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Response JSON: {jsonString}");

                        await Application.Current.MainPage.DisplayAlert("Éxito", "Datos actualizados correctamente.", "OK");
                        Close();
                    }
                    catch (JsonException jsonEx)
                    {
                        Debug.WriteLine($"Error de deserialización: {jsonEx.Message}");
                        await Application.Current.MainPage.DisplayAlert("Error", "Hubo un problema al procesar la respuesta del servidor.", "OK");
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
                nuevaContrasenaEntry,
                confirmarContrasenaEntry,
                enviarButton
            },
                    WidthRequest = 300,
                    HeightRequest = 300
                };
            }
        }





        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            var popup = new RecuperarPasswordPopup(laURL); // Usa la URL definida en MainPage
            await Application.Current.MainPage.ShowPopupAsync(popup);
        }

        private async void btregistrarse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registro());
        }
    }
}
    

    

    
