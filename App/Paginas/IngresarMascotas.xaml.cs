namespace FrontEndHealthPets.Paginas;

using FrontEndHealthPets.Entidades;
using FrontEndHealthPets.Entidades.Entitys;
using FrontEndHealthPets.Entidades.Request;
using FrontEndHealthPets.Entidades.response;
using FrontEndHealthPets.Paginas.FlyPaginas;
using FrontEndHealthPets.Modelos;
using Newtonsoft.Json;
using System.Diagnostics;

public partial class IngresarMascotas : ContentPage
{
    string LaURL = "https://localhost:44348/api";
    private MemoryStream imagenSeleccionadaMemoryStream; // Usar MemoryStream aquí
    public IngresarMascotas()
    {
        InitializeComponent();
        BindingContext = new MascotasViewModel(); // Asignar el ViewModel si no lo has hecho ya
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            Debug.WriteLine("Abriendo la galería para seleccionar una imagen...");

            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona una foto de perfil",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                Debug.WriteLine("Imagen seleccionada: " + result.FileName);

                // Obtener el stream de la imagen seleccionada
                using (var stream = await result.OpenReadAsync())
                {
                    // Copiar el stream a un MemoryStream
                    imagenSeleccionadaMemoryStream = new MemoryStream();
                    await stream.CopyToAsync(imagenSeleccionadaMemoryStream);
                    imagenSeleccionadaMemoryStream.Position = 0; // Restablecer la posición del stream a 0
                    Debug.WriteLine("Stream convertido a MemoryStream y posición restablecida.");

                    // Establecer la imagen en el control Image
                    imgPerfil.Source = ImageSource.FromStream(() => imagenSeleccionadaMemoryStream);
                    Debug.WriteLine("Imagen cargada en el control Image.");
                }
            }
            else
            {
                Debug.WriteLine("No se seleccionó ninguna imagen.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error al cargar la imagen: " + ex.Message);
            await DisplayAlert("Error", "No se pudo cargar la imagen: " + ex.Message, "OK");
        }
    }

    private async void btRegistrar_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Validar Fecha de Nacimiento
            DateTime fechaNacimiento = Fecha_Nacimiento.Date;

            if (string.IsNullOrEmpty(Nombre.Text))
            {
                Debug.WriteLine("Error: Nombre vacío");
                await DisplayAlert("Error", "Nombre vacío", "Aceptar");
                return;
            }
            else if (string.IsNullOrEmpty(Especie.Text))
            {
                Debug.WriteLine("Error: Especie vacía");
                await DisplayAlert("Error", "Especie vacía", "Aceptar");
                return;
            }
            else if (string.IsNullOrEmpty(Raza.Text))
            {
                Debug.WriteLine("Error: Raza vacía");
                await DisplayAlert("Error", "Por favor ingrese la raza de la mascota", "Aceptar");
                return;
            }
            else if (fechaNacimiento == DateTime.MinValue)
            {
                await DisplayAlert("Error", "Por favor, selecciona una fecha de nacimiento válida.", "OK");
                return;
            }

            Req_Mascota req = new Req_Mascota
            {
                Registro_Mascota = new Registro_Mascota
                {
                    id_Usuario = Sesion.id_usuario,
                    Nombre = Nombre.Text,
                    especie = Especie.Text,
                    raza = Raza.Text,
                    Fecha_Nacimiento = fechaNacimiento
                }
            };

            var json = JsonConvert.SerializeObject(req);
            Debug.WriteLine("JSON enviado: " + json);

            var jsoncontent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.PostAsync(LaURL + "/Registro_Mascota/IngresarMascota", jsoncontent);
            Debug.WriteLine("Código de estado: " + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("Contenido de la respuesta: " + responseContent);

                Res_Mascota res = JsonConvert.DeserializeObject<Res_Mascota>(responseContent);

                if (res.resultado)
                {
                    await DisplayAlert("Éxito", "Mascota registrada con éxito", "Aceptar");

                    // Verificar si hay imagen seleccionada
                    if (imagenSeleccionadaMemoryStream != null)
                    {
                        Debug.WriteLine("Imagen seleccionada, procediendo a subirla.");

                        byte[] imagenBytes = imagenSeleccionadaMemoryStream.ToArray();

                        if (imagenBytes.Length == 0)
                        {
                            await DisplayAlert("Error", "La imagen seleccionada está vacía.", "Aceptar");
                            return;
                        }

                        Debug.WriteLine("Imagen convertida a bytes. Tamaño de la imagen en bytes: " + imagenBytes.Length);

                        // Preparar solicitud de foto
                        var reqFoto = new Req_FotosMascota
                        {
                            FotosMascotas = new FotosMascota
                            {
                                Foto = imagenBytes,
                                Id_Mascota = res.Id_Mascota,
                                Descripcion = "Foto de perfil"
                            }
                        };
                        Debug.WriteLine("Solicitud de foto preparada. ID de la mascota: " + reqFoto.FotosMascotas.Id_Mascota);

                        // Enviar la foto al servidor
                        var resultadoFoto = await EnviarFotoAlServidor(reqFoto);
                        Debug.WriteLine("Resultado de la subida de la foto: " + resultadoFoto.resultado);

                        if (!resultadoFoto.resultado)
                        {
                            await DisplayAlert("Error", resultadoFoto.error, "Aceptar");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No se seleccionó una imagen.");
                    }

                    // Navegar a la página MisMascotas
                    Debug.WriteLine("Navegando a la página MisMascotas.");
                    await Navigation.PushAsync(new MisMascotas { BindingContext = this.BindingContext });
                }
                else
                {
                    Debug.WriteLine("Error al registrar la mascota: " + res.error);
                    await DisplayAlert("Error", res.error, "Aceptar");
                }
            }
            else
            {
                Debug.WriteLine("Error de conexión con el servidor.");
                await DisplayAlert("Error de conexión", "Ocurrió un error de conexión", "Aceptar");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Excepción capturada: " + ex.Message);
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }

        private async Task<Res_FotosMascotas> EnviarFotoAlServidor(Req_FotosMascota req)
    {
        var jsoncontent = new StringContent(JsonConvert.SerializeObject(req), System.Text.Encoding.UTF8, "application/json");
        Debug.WriteLine("Enviando foto al servidor.");

        HttpClient httpClient = new HttpClient();
        var response = await httpClient.PostAsync(LaURL + "/Foto_Mascota/Ingresar_Fotos_Mascotas", jsoncontent);

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("Contenido de la respuesta de la foto: " + responseContent);
            return JsonConvert.DeserializeObject<Res_FotosMascotas>(responseContent);
        }
        else
        {
            Debug.WriteLine("Error al enviar la foto al servidor.");
            return new Res_FotosMascotas { resultado = false, error = "Error en el servidor" };
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Botón de navegación presionado.");
        Navigation.PushAsync(new MisMascotas());
    }

}