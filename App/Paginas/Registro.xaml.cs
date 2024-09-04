using FrontEndHealthPets.Entidades;
using FrontEndHealthPets.Entidades.Request;
using FrontEndHealthPets.Entidades.response;
using Newtonsoft.Json;

namespace FrontEndHealthPets.Paginas;

public partial class Registro : ContentPage
{
    string LaURL = "https://localhost:44348/api";
    public Registro()
	{
		InitializeComponent();
	}

    private async void btRegistrar_Clicked(object sender, EventArgs e)
    {

        try
        {
            if (string.IsNullOrEmpty(Nombre.Text))
            {
                DisplayAlert("Error", "Faltan datos", "Aceptar");
                return;
            }
            else
       if (string.IsNullOrEmpty(Apellido.Text))
            {
                DisplayAlert("Error", "Faltan datos", "Aceptar");
                return;
            }
            else
       if (string.IsNullOrEmpty(Email.Text))
            {
                DisplayAlert("Error", "Faltan datos", "Aceptar");
                return;
            }
            else
           if (string.IsNullOrEmpty(Comfirmacion_Email.Text))
            {
                DisplayAlert("Error", "Faltan datos", "Aceptar");
                return;
            }
            else
           if (string.IsNullOrEmpty(Password.Text))
            {
                DisplayAlert("Error", "Faltan datos", "Aceptar");
                return;
            }
            else
         if (string.IsNullOrEmpty(Confirmar_Password.Text))
            {
                DisplayAlert("Error", "Faltan datos", "Aceptar");
                return;
            }
            else
           if (string.IsNullOrEmpty(NumeroVerificacion.Text))
            {
                DisplayAlert("Error", "Faltan datos", "Aceptar");
                return;
            }

            Req_Usuario req = new Req_Usuario();

            req.registro_Usuario = new Registro_Usuario();
            req.registro_Usuario.Nombre = Nombre.Text;
            req.registro_Usuario.Apellidos = Apellido.Text;
            req.registro_Usuario.Correo_Electronico = Email.Text;
            req.registro_Usuario.Confirmacion_Correo_Electronico = Comfirmacion_Email.Text;
            req.registro_Usuario.Password = Password.Text;
            req.registro_Usuario.Confirmar_Password = Confirmar_Password.Text;
            req.registro_Usuario.numero_verificacion = NumeroVerificacion.Text;

            var jsoncontent = new StringContent(JsonConvert.SerializeObject(req), System.Text.Encoding.UTF8, "application/json");
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.PostAsync(LaURL + "/Registro_Usuario/IngresarUsuario", jsoncontent);
            if (response.IsSuccessStatusCode)// saber si el api esta vivo
            {
                // si conecto

                var responsecontent = await response.Content.ReadAsStringAsync();
                Res_Usuario res = new Res_Usuario();

                res = JsonConvert.DeserializeObject<Res_Usuario>(responsecontent);

                if (res.resultado)
                {
                    await DisplayAlert("Exito", "Usuario ingresado correctamente", "Aceptar");
                     Application.Current.MainPage = new NavigationPage(new MainPage());

                }
                else
                {
                    DisplayAlert("Error", res.error, "Aceptar");// Error en backend", "Login incorrecto!", "Aceptar" agregar luego

                }
            }
            else
            {
                //No conectó
                DisplayAlert("Error de conexión", "Ocurrió un error de conexión", "Aceptar");

            }
        }

        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }









    }

    private void btRegresar_Clicked(object sender, EventArgs e)
    {

    }
}